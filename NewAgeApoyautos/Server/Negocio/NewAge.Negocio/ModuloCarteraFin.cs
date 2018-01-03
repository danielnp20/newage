using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Reportes;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.Librerias.ExceptionHandler;
using SentenceTransformer;
using System.Data;


namespace NewAge.Negocio
{
    public class ModuloCarteraFin : ModuloBase
    {
        #region Variables

        #region DALs

        private DAL_CarteraFin _dal_CarteraFin = null;
        private DAL_ccCreditoDocu _dal_ccCreditoDocu = null;
        private DAL_ccCreditoPagos _dal_ccCreditoPagos = null;
        private DAL_ccCreditoPlanPagos _dal_ccCreditoPlanPagos = null;
        private DAL_ccNominaDeta _dal_ccNominaDeta = null;
        private DAL_ccNominaPreliminar _dal_ccNominaPreliminar = null;
        private DAL_ccPolizaEstado _dal_ccPolizaEstado = null;
        private DAL_ccSolicitudDocu _dal_ccSolicitudDocu = null;
        private DAL_ccSolicitudCtasExtra _dal_ccSolicitudCtasExtra = null;
        private DAL_ccSolicitudPlanPagos _dal_ccSolicitudPlanPagos = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_ReportesCartera _dal_ReportesCartera = null;

        private DAL_Cartera _dal_Cartera = null;
        private DAL_ccCierreDia _dal_ccCierreDia = null;
        private DAL_ccCierreMes _dal_ccCierreMes = null;
        private DAL_ccCreditoComponentes _dal_ccCreditoComponentes = null;
        private DAL_ccCJHistorico _dal_ccCJHistorico = null;

        #endregion

        #region Modulos

        private ModuloGlobal _moduloGlobal = null;
        private ModuloCartera _moduloCartera = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloCuentasXPagar _moduloCxP = null;
        private DAL_ccSolicitudDocu _dalsolDocu = null;

        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Cartera
        /// </summary>
        /// <param name="conn"></param>
        public ModuloCarteraFin(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Cierres

        /// <summary>
        /// Realiza el proceso de cierre mensual
        /// </summary>
        public DTO_TxResult Proceso_CierreMesCarteraFin(int documentID, DateTime periodo, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_coComprobante coComp = null;
            DTO_glDocumentoControl ctrl = null;
            try
            {
                #region 1. Variables

                //Variables Generales
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCierreDia = (DAL_ccCierreDia)base.GetInstance(typeof(DAL_ccCierreDia), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCierreMes = (DAL_ccCierreMes)base.GetInstance(typeof(DAL_ccCierreMes), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Cartera = (DAL_Cartera)base.GetInstance(typeof(DAL_Cartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCJHistorico = (DAL_ccCJHistorico)base.GetInstance(typeof(DAL_ccCJHistorico), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPagos = (DAL_ccCreditoPagos)base.GetInstance(typeof(DAL_ccCreditoPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                // Variables por defecto
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string proyDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string ctoCostoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string concSaldoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);
                string codCarteraPropia = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);

                // Variables de operacion
                string af = this.GetAreaFuncionalByUser();
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string coDocID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DocuCierreMensual);
                DTO_coDocumento coDoc = new DTO_coDocumento();

                //Cuentas
                string ctaUtilidadVentaID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaUtilidadCesion);
                string ctaIngresosAmortizacionDerechosID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaIngAmortDerVenCar);

                // Periodo documento
                DateTime fechaDoc = DateTime.Now;
                if (fechaDoc.Year != periodo.Year || fechaDoc.Month != periodo.Month)
                    fechaDoc = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                #endregion
                #region 2. Validaciones

                //Valida las cuentas
                if (string.IsNullOrWhiteSpace(ctaUtilidadVentaID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CuentaUtilidadCesion + "&&" + string.Empty;

                    return result;
                }

                //Valida las cuentas
                if (string.IsNullOrWhiteSpace(ctaIngresosAmortizacionDerechosID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CuentaIngAmortDerVenCar + "&&" + string.Empty;

                    return result;
                }

                //Valida el coDocumento
                if (string.IsNullOrWhiteSpace(coDocID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_DocuCierreMensual + "&&" + string.Empty;

                    return result;
                }
                else
                    coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocID, true, false);

                //Valida que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;

                    return result;
                }

                //Valida que haya cerrado todo el mes
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                EstadoAjuste estadoCtrl = this._moduloContabilidad.HasDocument(documentID, periodo, coComp.BalanceTipoID.Value);
                if (estadoCtrl == EstadoAjuste.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_DocumentoAprobado;
                    return result;
                }

                string diaCierre = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                int maxDia = DateTime.DaysInMonth(periodo.Year, periodo.Month);

                if (diaCierre != maxDia.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cc_CerrarDias + "&&" + diaCierre;

                    return result;
                }

                #endregion
                #region 3. Actualiza la info de la tabla cierre Mes

                #region Cierra el mes

                List<DTO_ccCierreDia> cierres = this._dal_ccCierreDia.DAL_ccCierreDia_GetForCierreMes(periodo);
                foreach (DTO_ccCierreDia dia in cierres)
                    _dal_ccCierreMes.DAL_ccCierreMes_Add(dia);

                #endregion
                #region Actualiza el dia de cierre en glControl
                string EmpNro = this.Empresa.NumeroControl.Value;
                string _modId = ((int)ModulesPrefix.cc).ToString();

                if (_modId.Length == 1)
                    _modId = "0" + _modId;

                string keyControl = EmpNro + _modId + AppControl.cc_DiaUltimoCierre;
                DTO_glControl diaCierreControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                diaCierreControl.Data.Value = "0";
                this._moduloGlobal.glControl_Update(diaCierreControl);
                #endregion

                batchProgress[tupProgress] = 10;

                #endregion
                #region 4. Procesa el resumen de todos los movimientos de los creditos

                object r = this._dal_ccCierreMes.DAL_ccCierreMesCarteraFin_Procesar(periodo);
                if (r.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)r;
                    return result;
                }

                List<DTO_ccCierreMesCartera> cierresCartera = (List<DTO_ccCierreMesCartera>)r;
                batchProgress[tupProgress] = 25;

                #endregion
                if (false)
                {
                    #region 5. Carga la información del cobro jurídico

                    // Fecha de cierre
                    DateTime fechaCierre = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                    //Tasa de usura
                    DTO_glDatosMensuales datosAnuales = (DTO_glDatosMensuales)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDatosAnuales, periodo.ToString(FormatString.ControlDate), true, false);
                    decimal tasaUsura = datosAnuales == null ? 0 : datosAnuales.Tasa2.Value.Value;

                    //Tasa de mora
                    string componenteMoraId = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
                    DTO_ccCarteraComponente componenteMora = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, componenteMoraId, true, false);
                    decimal tasaMora = componenteMora == null ? 0 : componenteMora.PorcentajeID.Value.Value;
                    if (tasaMora > tasaUsura && tasaUsura > 0)
                        tasaMora = tasaUsura;

                    #region Clase deuda principal (1)
                    List<DTO_ccCJHistorico> historicosPrincipal = this._dal_ccCJHistorico.DAL_ccCJHistorico_GetForCierreMensual_CJ(fechaCierre, (byte)ClaseDeuda.Principal);
                    foreach (DTO_ccCJHistorico hist in historicosPrincipal)
                    {
                        int numDias = (fechaCierre - hist.FechaMvto.Value.Value).Days;
                        decimal vlrInteres = Convert.ToInt32(hist.SaldoCapital.Value.Value / 30 * numDias * tasaMora / 100);

                        DTO_ccCJHistorico newHistorico = new DTO_ccCJHistorico();
                        newHistorico.NumeroDoc.Value = hist.NumeroDoc.Value;
                        newHistorico.PeriodoID.Value = periodo;
                        newHistorico.FechaMvto.Value = fechaCierre;
                        newHistorico.FechaInicial.Value = hist.FechaMvto.Value;
                        newHistorico.FechaFinal.Value = fechaCierre;
                        newHistorico.Dias.Value = numDias;
                        newHistorico.PorInteres.Value = tasaMora;
                        newHistorico.MvtoInteres.Value = vlrInteres;
                        newHistorico.SaldoCapital.Value = hist.SaldoCapital.Value;
                        newHistorico.SaldoInteres.Value = hist.SaldoInteres.Value.Value + vlrInteres;
                        newHistorico.ClaseDeuda.Value = (byte)ClaseDeuda.Principal;
                        newHistorico.EstadoDeuda.Value = hist.EstadoCarteraCliente.Value;
                        newHistorico.TipoMvto.Value = (byte)TipoMovimiento_CJHistorico.InteresMora;
                        newHistorico.Observacion.Value = "Interes Mora";

                        this._dal_ccCJHistorico.DAL_ccCJHistorico_Add(newHistorico);
                    }
                    #endregion
                    #region Clase deuda adicional (2)
                    List<DTO_ccCJHistorico> historicosAdicional = this._dal_ccCJHistorico.DAL_ccCJHistorico_GetForCierreMensual_CJ(fechaCierre, (byte)ClaseDeuda.Adicional);
                    foreach (DTO_ccCJHistorico hist in historicosAdicional)
                    {
                        int numDias = (fechaCierre - hist.FechaMvto.Value.Value).Days;
                        decimal vlrInteres = Convert.ToInt32(hist.SaldoCapital.Value.Value * numDias * tasaMora / 100);

                        DTO_ccCJHistorico newHistorico = new DTO_ccCJHistorico();
                        newHistorico.NumeroDoc.Value = hist.NumeroDoc.Value;
                        newHistorico.PeriodoID.Value = periodo;
                        newHistorico.FechaMvto.Value = fechaCierre;
                        newHistorico.FechaInicial.Value = hist.FechaMvto.Value;
                        newHistorico.FechaFinal.Value = fechaCierre;
                        newHistorico.Dias.Value = numDias;
                        newHistorico.PorInteres.Value = tasaMora;
                        newHistorico.MvtoInteres.Value = vlrInteres;
                        newHistorico.SaldoCapital.Value = hist.SaldoCapital.Value;
                        newHistorico.SaldoInteres.Value = hist.SaldoInteres.Value.Value + vlrInteres;
                        newHistorico.ClaseDeuda.Value = (byte)ClaseDeuda.Adicional;
                        newHistorico.EstadoDeuda.Value = hist.EstadoCarteraCliente.Value;
                        newHistorico.TipoMvto.Value = (byte)TipoMovimiento_CJHistorico.InteresMora;
                        newHistorico.Observacion.Value = "Interes Mora";

                        this._dal_ccCJHistorico.DAL_ccCJHistorico_Add(newHistorico);
                    }
                    #endregion

                    #endregion
                    #region 6. Comprobantes

                    List<DTO_ccCierreMesCartera> credCarteraCedida = cierresCartera.Where(cr => !string.IsNullOrWhiteSpace(cr.CompradorCarteraID.Value) && cr.CompradorCarteraID.Value != codCarteraPropia).ToList();
                    if (credCarteraCedida.Count > 0)
                    {
                        #region Variables
                        DTO_ccCompradorCartera comprador;
                        DTO_glDocumentoControl ctrlCredito = null;
                        Dictionary<string, DTO_ccCompradorCartera> cacheCompradores = new Dictionary<string, DTO_ccCompradorCartera>();

                        DTO_coPlanCuenta ctaDB = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaUtilidadVentaID, true, false);
                        DTO_glConceptoSaldo cSaldoDB = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaDB.ConceptoSaldoID.Value, true, false);

                        DTO_coPlanCuenta ctaCR = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaIngresosAmortizacionDerechosID, true, false);
                        DTO_glConceptoSaldo cSaldoCR = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaCR.ConceptoSaldoID.Value, true, false);


                        DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                        DTO_Comprobante comprobante = new DTO_Comprobante();
                        #endregion
                        #region Crea el documento del cierre (glDocumentoControl)

                        ctrl = new DTO_glDocumentoControl();
                        ctrl.DocumentoNro.Value = 0;
                        ctrl.DocumentoID.Value = documentID;
                        ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                        ctrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                        ctrl.Fecha.Value = DateTime.Now;
                        ctrl.FechaDoc.Value = fechaDoc;
                        ctrl.PeriodoDoc.Value = periodo;
                        ctrl.PeriodoUltMov.Value = periodo;
                        ctrl.AreaFuncionalID.Value = af;
                        ctrl.PrefijoID.Value = prefijoDef;
                        ctrl.ProyectoID.Value = proyDef;
                        ctrl.CentroCostoID.Value = ctoCostoDef;
                        ctrl.LugarGeograficoID.Value = lugGeoDef;
                        ctrl.LineaPresupuestoID.Value = linPresDef;
                        ctrl.TerceroID.Value = terceroDef;
                        ctrl.MonedaID.Value = mdaLoc;
                        ctrl.TasaCambioCONT.Value = 0;
                        ctrl.TasaCambioDOCU.Value = 0;
                        ctrl.Descripcion.Value = "CIERRE MENSUAL " + periodo.ToString(FormatString.ControlDate);
                        ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        ctrl.seUsuarioID.Value = this.UserId;
                        ctrl.Valor.Value = 0;
                        ctrl.Iva.Value = 0;

                        resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                        if (resultGLDC.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(resultGLDC);
                            return result;
                        }

                        ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        comprobante.Header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                        comprobante.Header.ComprobanteNro.Value = 0;

                        #endregion
                        #region Carga el header del comprobante

                        comprobante.Header.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                        comprobante.Header.PeriodoID.Value = periodo;
                        comprobante.Header.EmpresaID.Value = this.Empresa.ID.Value;
                        comprobante.Header.TasaCambioBase.Value = 0;
                        comprobante.Header.TasaCambioOtr.Value = 0;
                        comprobante.Header.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                        comprobante.Header.MdaTransacc.Value = mdaLoc;
                        comprobante.Header.ComprobanteNro.Value = 0;
                        comprobante.Header.Fecha.Value = fechaDoc;

                        #endregion
                        #region Carga los registros del comprobante

                        foreach (DTO_ccCierreMesCartera cr in credCarteraCedida)
                        {
                            // Carga el comprador
                            if (cacheCompradores.ContainsKey(cr.CompradorCarteraID.Value))
                            {
                                comprador = cacheCompradores[cr.CompradorCarteraID.Value];
                            }
                            else
                            {
                                comprador = (DTO_ccCompradorCartera)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCompradorCartera, cr.CompradorCarteraID.Value, true, false);
                                cacheCompradores[cr.CompradorCarteraID.Value] = comprador;
                            }

                            // Solo aplica para compradores con tipo de control de recursos disponibles
                            if (comprador.TipoControlRecursos.Value == (byte)TipoControlRecursos.RecursosDisponibles)
                            {
                                //Revisa si tiene pagos en el periodo
                                List<DTO_ccCreditoPagos> pagos = this._dal_ccCreditoPagos.DAL_ccCreditoPagos_GetPagosByPeriodo(periodo, cr.NumeroDoc.Value.Value);
                                if (pagos.Count > 0)
                                {
                                    //Carga la lista de creditos
                                    ctrlCredito = this._moduloGlobal.glDocumentoControl_GetByID(cr.NumeroDoc.Value.Value);

                                    //Carga los valores
                                    decimal vlrDerechosCesion = pagos.Sum(p => p.VlrDerechosCesion.Value.Value);
                                    if (pagos.First().DocumentoAnula != null && pagos.First().DocumentoAnula.Value.HasValue)
                                        vlrDerechosCesion *= -1;

                                    #region DB

                                    DTO_ComprobanteFooter detDB = new DTO_ComprobanteFooter();
                                    detDB.CuentaID.Value = ctaUtilidadVentaID;
                                    detDB.ConceptoSaldoID.Value = cSaldoDB.ID.Value;
                                    detDB.IdentificadorTR.Value = this.GetIdentificadorTR(ctrlCredito, cSaldoDB);

                                    detDB.vlrMdaLoc.Value = vlrDerechosCesion;
                                    detDB.vlrMdaExt.Value = ctrl.TasaCambioDOCU.Value == 0 ? 0 : Math.Round(detDB.vlrMdaLoc.Value.Value / ctrl.TasaCambioDOCU.Value.Value, 0);
                                    detDB.vlrMdaOtr.Value = detDB.vlrMdaLoc.Value;

                                    detDB.TerceroID.Value = ctrlCredito.TerceroID.Value;
                                    detDB.ProyectoID.Value = ctrlCredito.ProyectoID.Value;
                                    detDB.CentroCostoID.Value = ctrlCredito.CentroCostoID.Value;
                                    detDB.LineaPresupuestoID.Value = ctrlCredito.LineaPresupuestoID.Value;
                                    detDB.LugarGeograficoID.Value = ctrlCredito.LugarGeograficoID.Value;
                                    detDB.PrefijoCOM.Value = ctrlCredito.PrefijoID.Value;
                                    detDB.DocumentoCOM.Value = ctrlCredito.DocumentoTercero.Value;

                                    detDB.vlrBaseML.Value = 0;
                                    detDB.vlrBaseME.Value = 0;
                                    detDB.ConceptoCargoID.Value = concCargoDef;
                                    detDB.TasaCambio.Value = ctrl.TasaCambioDOCU.Value;

                                    detDB.Descriptivo.Value = "CONT. CIERRE MENSUAL";

                                    comprobante.Footer.Add(detDB);
                                    #endregion
                                    #region CR
                                    DTO_ComprobanteFooter detCR = ObjectCopier.Clone(detDB);

                                    detCR.CuentaID.Value = ctaCR.ID.Value;
                                    detCR.ConceptoSaldoID.Value = cSaldoCR.ID.Value;
                                    detCR.IdentificadorTR.Value = this.GetIdentificadorTR(ctrlCredito, cSaldoCR);

                                    detCR.vlrMdaLoc.Value = detDB.vlrMdaLoc.Value;
                                    detCR.vlrMdaExt.Value = detDB.vlrMdaExt.Value;
                                    detCR.vlrMdaOtr.Value = detDB.vlrMdaOtr.Value;

                                    comprobante.Footer.Add(detDB);

                                    #endregion

                                }
                            }
                        }

                        #endregion
                    }

                    #endregion 
                }
                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_CierreMes");

                return result;
            }
            finally
            {
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Asigna consecutivos

                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (ctrl.NumeroDoc.Value != null && ctrl.NumeroDoc.Value != 0)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                            ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                        }

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
        /// Realiza el proceso de amortizacion mensual
        /// </summary>
        public DTO_TxResult Proceso_AmortizacionMensual(int documentID, DateTime periodo, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_coComprobante coCompAmortizDerPagos = null;
            DTO_glDocumentoControl ctrlAmortizDerPagos = null;
            DTO_coComprobante coCompAmortizDereRecomp = null;
            DTO_glDocumentoControl ctrlAmortizDereRecomp = null;
            try
            {
                #region Variables

                //Variables Generales
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_CarteraFin = (DAL_CarteraFin)base.GetInstance(typeof(DAL_CarteraFin), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                // Variables por defecto
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string proyDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string ctoCostoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string af = this.GetAreaFuncionalByUser();
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                DateTime fechaDoc = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                DTO_Comprobante compAmortizDerPagos = new DTO_Comprobante();
                DTO_Comprobante compAmortizDerRecompra = new DTO_Comprobante();
                object res = null;
                #endregion                
                #region Comprobantes
                //Comprobante de Amortizacion Derechos Pagos
                #region Crea el documento de Amortizacion Derechos Pagos (glDocumentoControl)
                ctrlAmortizDerPagos = new DTO_glDocumentoControl();
                ctrlAmortizDerPagos.DocumentoNro.Value = 0;
                ctrlAmortizDerPagos.DocumentoID.Value = AppDocuments.AcuerdoPago;
                ctrlAmortizDerPagos.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                ctrlAmortizDerPagos.ComprobanteID.Value = string.Empty;
                ctrlAmortizDerPagos.Fecha.Value = DateTime.Now;
                ctrlAmortizDerPagos.FechaDoc.Value = fechaDoc;
                ctrlAmortizDerPagos.PeriodoDoc.Value = periodo;
                ctrlAmortizDerPagos.PeriodoUltMov.Value = periodo;
                ctrlAmortizDerPagos.AreaFuncionalID.Value = af;
                ctrlAmortizDerPagos.PrefijoID.Value = prefijoDef;
                ctrlAmortizDerPagos.ProyectoID.Value = proyDef;
                ctrlAmortizDerPagos.CentroCostoID.Value = ctoCostoDef;
                ctrlAmortizDerPagos.LugarGeograficoID.Value = lugGeoDef;
                ctrlAmortizDerPagos.LineaPresupuestoID.Value = linPresDef;
                ctrlAmortizDerPagos.TerceroID.Value = terceroDef;
                ctrlAmortizDerPagos.MonedaID.Value = mdaLoc;
                ctrlAmortizDerPagos.TasaCambioCONT.Value = 0;
                ctrlAmortizDerPagos.TasaCambioDOCU.Value = 0;
                ctrlAmortizDerPagos.Descripcion.Value = "AMORTIZACION DER. PAGOS MENSUAL " + periodo.ToString(FormatString.ControlDate);
                ctrlAmortizDerPagos.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrlAmortizDerPagos.seUsuarioID.Value = this.UserId;
                ctrlAmortizDerPagos.Valor.Value = 0;
                ctrlAmortizDerPagos.Iva.Value = 0;

                resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrlAmortizDerPagos, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }

                ctrlAmortizDerPagos.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                #endregion
                #region Crear Comprobante Amortizacion Derechos Pagos
                res = this._dal_CarteraFin.DAL_CarteraFin_GetComprobanteAmortizaDerechos(ctrlAmortizDerPagos.NumeroDoc.Value.Value,periodo,true);
                if (res.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)res;
                    return result;
                }
                else
                    compAmortizDerPagos = (DTO_Comprobante)res;

                compAmortizDerPagos.coDocumentoID = ctrlAmortizDerPagos.DocumentoID.Value.Value.ToString();
                compAmortizDerPagos.PrefijoID = ctrlAmortizDerPagos.PrefijoID.Value.ToString();
                compAmortizDerPagos.DocumentoNro = ctrlAmortizDerPagos.DocumentoNro.Value.Value;
                compAmortizDerPagos.CuentaID = ctrlAmortizDerPagos.CuentaID.Value;
                compAmortizDerPagos.TerceroID = ctrlAmortizDerPagos.TerceroID.Value;
                compAmortizDerPagos.ProyectoID = ctrlAmortizDerPagos.ProyectoID.Value;
                compAmortizDerPagos.CentroCostoID = ctrlAmortizDerPagos.CentroCostoID.Value;
                compAmortizDerPagos.LineaPresupuestoID = ctrlAmortizDerPagos.LineaPresupuestoID.Value;
                compAmortizDerPagos.LugarGeograficoID = ctrlAmortizDerPagos.LugarGeograficoID.Value;
                #endregion
                #region Guardar comprobante Amortizacion Derechos Pagos
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, compAmortizDerPagos, compAmortizDerPagos.Header.PeriodoID.Value.Value, ModulesPrefix.cc, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;
                coCompAmortizDerPagos = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compAmortizDerPagos.Header.ComprobanteID.Value, true, false);

                #endregion

                //Comprobante de Amortizacion Derechos Recompra
                #region Crea el documento de Amortizacion Derechos Recompra (glDocumentoControl)
                ctrlAmortizDereRecomp = new DTO_glDocumentoControl();
                ctrlAmortizDereRecomp.DocumentoNro.Value = 0;
                ctrlAmortizDereRecomp.DocumentoID.Value = AppDocuments.AcuerdoPago;
                ctrlAmortizDereRecomp.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                ctrlAmortizDereRecomp.ComprobanteID.Value = string.Empty;
                ctrlAmortizDereRecomp.Fecha.Value = DateTime.Now;
                ctrlAmortizDereRecomp.FechaDoc.Value = fechaDoc;
                ctrlAmortizDereRecomp.PeriodoDoc.Value = periodo;
                ctrlAmortizDereRecomp.PeriodoUltMov.Value = periodo;
                ctrlAmortizDereRecomp.AreaFuncionalID.Value = af;
                ctrlAmortizDereRecomp.PrefijoID.Value = prefijoDef;
                ctrlAmortizDereRecomp.ProyectoID.Value = proyDef;
                ctrlAmortizDereRecomp.CentroCostoID.Value = ctoCostoDef;
                ctrlAmortizDereRecomp.LugarGeograficoID.Value = lugGeoDef;
                ctrlAmortizDereRecomp.LineaPresupuestoID.Value = linPresDef;
                ctrlAmortizDereRecomp.TerceroID.Value = terceroDef;
                ctrlAmortizDereRecomp.MonedaID.Value = mdaLoc;
                ctrlAmortizDereRecomp.TasaCambioCONT.Value = 0;
                ctrlAmortizDereRecomp.TasaCambioDOCU.Value = 0;
                ctrlAmortizDereRecomp.Descripcion.Value = "AMORTIZACION DER. RECOMPRA MENSUAL " + periodo.ToString(FormatString.ControlDate);
                ctrlAmortizDereRecomp.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrlAmortizDereRecomp.seUsuarioID.Value = this.UserId;
                ctrlAmortizDereRecomp.Valor.Value = 0;
                ctrlAmortizDereRecomp.Iva.Value = 0;

                resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrlAmortizDereRecomp, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }

                ctrlAmortizDereRecomp.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                #endregion
                #region Crear Comprobante Amortizacion Derechos Recompra
                res = this._dal_CarteraFin.DAL_CarteraFin_GetComprobanteAmortizaDerechos(ctrlAmortizDerPagos.NumeroDoc.Value.Value, periodo, false);
                if (res.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)res;
                    return result;
                }
                else
                    compAmortizDerRecompra = (DTO_Comprobante)res;

                compAmortizDerRecompra.coDocumentoID = ctrlAmortizDereRecomp.DocumentoID.Value.Value.ToString();
                compAmortizDerRecompra.PrefijoID = ctrlAmortizDereRecomp.PrefijoID.Value.ToString();
                compAmortizDerRecompra.DocumentoNro = ctrlAmortizDereRecomp.DocumentoNro.Value.Value;
                compAmortizDerRecompra.CuentaID = ctrlAmortizDereRecomp.CuentaID.Value;
                compAmortizDerRecompra.TerceroID = ctrlAmortizDereRecomp.TerceroID.Value;
                compAmortizDerRecompra.ProyectoID = ctrlAmortizDereRecomp.ProyectoID.Value;
                compAmortizDerRecompra.CentroCostoID = ctrlAmortizDereRecomp.CentroCostoID.Value;
                compAmortizDerRecompra.LineaPresupuestoID = ctrlAmortizDereRecomp.LineaPresupuestoID.Value;
                compAmortizDerRecompra.LugarGeograficoID = ctrlAmortizDereRecomp.LugarGeograficoID.Value;
                #endregion
                #region Guardar comprobante Amortizacion Derechos Recompra
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, compAmortizDerRecompra, compAmortizDerRecompra.Header.PeriodoID.Value.Value, ModulesPrefix.cc, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;

                coCompAmortizDereRecomp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compAmortizDerRecompra.Header.ComprobanteID.Value, true, false);

                #endregion

                #endregion
                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_CierreMes");

                return result;
            }
            finally
            {
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Asigna consecutivos
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (ctrlAmortizDerPagos.NumeroDoc.Value != null && ctrlAmortizDerPagos.NumeroDoc.Value != 0)
                        {
                            ctrlAmortizDerPagos.ComprobanteID.Value = coCompAmortizDerPagos.ID.Value;
                            ctrlAmortizDerPagos.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrlAmortizDerPagos.PrefijoID.Value);
                            ctrlAmortizDerPagos.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompAmortizDerPagos, ctrlAmortizDerPagos.PrefijoID.Value, ctrlAmortizDerPagos.PeriodoDoc.Value.Value, ctrlAmortizDerPagos.DocumentoNro.Value.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrlAmortizDerPagos, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrlAmortizDerPagos.NumeroDoc.Value.Value, ctrlAmortizDerPagos.ComprobanteIDNro.Value.Value, false);
                        }
                        if (ctrlAmortizDereRecomp.NumeroDoc.Value != null && ctrlAmortizDereRecomp.NumeroDoc.Value != 0)
                        {
                            ctrlAmortizDereRecomp.ComprobanteID.Value = coCompAmortizDereRecomp.ID.Value;
                            ctrlAmortizDereRecomp.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrlAmortizDereRecomp.PrefijoID.Value);
                            ctrlAmortizDereRecomp.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompAmortizDereRecomp, ctrlAmortizDereRecomp.PrefijoID.Value, ctrlAmortizDereRecomp.PeriodoDoc.Value.Value, ctrlAmortizDereRecomp.DocumentoNro.Value.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrlAmortizDereRecomp, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrlAmortizDereRecomp.NumeroDoc.Value.Value, ctrlAmortizDereRecomp.ComprobanteIDNro.Value.Value, false);
                        }

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

        #region Polizas

        #region Funciones Privadas

        #region Poliza Estado

        /// <summary>
        /// Trae la info de un credito por su libranza
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranzaID">Identificador de la libranza</param>
        /// <returns><Retorna la info de un credito/returns>
        private DTO_TxResult PolizaEstado_Add(DTO_ccPolizaEstado poliza)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result.ExtraField = this._dal_ccPolizaEstado.DAL_ccPolizaEstado_Add(poliza);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PolizaEstado_Add");
                return result;
            }
        }

        #endregion

        #region Plan pagos y comprobante renovación

        /// <summary>
        /// Realiza el proceso de liquidacion del credito
        /// </summary>
        /// <param name="edad">Edad del cliente. No se usa en el simulador</param>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="plazo">Plazo de pago</param>
        /// <param name="fechaLiquida">Fecha de liquidacion</param>
        /// <param name="fechaCuota1">Fecha de la primera cuota</param>
        /// <param name="valorSolicitado">Valor solicitado por el cliente</param>
        /// <param name="vlrGiro">Valor a girar al cliente</param>
        /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        private DTO_SerializedObject CrearPlanPagosPoliza(string lineaCredID, int valorSolicitado, int vlrGiro, int plazo, DateTime fechaLiquida,
            DateTime fechaCuota1, decimal interes, int vlrCuotaPol)
        {
            DTO_TxResult result = new DTO_TxResult();
            DTO_PlanDePagos planPagos = new DTO_PlanDePagos();

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                //Variables para resultados
                List<DTO_ccSolicitudComponentes> componentes = new List<DTO_ccSolicitudComponentes>();
                List<DTO_ccSolicitudComponentes> componentesUsuario = new List<DTO_ccSolicitudComponentes>();
                List<DTO_ccSolicitudComponentes> componentesAll = new List<DTO_ccSolicitudComponentes>();
                List<DTO_Cuota> cuotas = new List<DTO_Cuota>();

                //Variables de control
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                string compIntSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);

                //Variables de calculos
                string ctaIVAID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaIVAComponentes);
                DTO_coPlanCuenta ctaIVA;
                decimal porcIVA = 0;
                int SMMLV = 0;
                decimal tasaTotal = 0;
                if (interes != 0)
                    tasaTotal = interes;
                decimal vlrCompra = 0;
                int vlrCuota = 0;
                int capitalTotal = 0;
                int saldoParcial = valorSolicitado;
                //int ultimoDia = 1;
                //if (fechaLiquida.Month == 2)
                //    ultimoDia = 28;
                //else
                //    ultimoDia = 30;
                DateTime periodo = new DateTime(fechaLiquida.Year, fechaLiquida.Month, 1);
                DateTime fechaCuota = fechaCuota1;// new DateTime(fechaCuota1.Year, fechaCuota1.Month, ultimoDia);
                DateTime fechaLiquidacion = fechaLiquida.Date;
                //Variables
                string descSeguro = string.Empty;
                DTO_MasterBasic basic;

                //Variables de cache
                DTO_ccCarteraComponente carteraComponenteDTO = new DTO_ccCarteraComponente();
                Dictionary<string, bool> dict_PagosAnticipados = new Dictionary<string, bool>();
                Dictionary<string, bool> dict_MayorValor = new Dictionary<string, bool>();
                Dictionary<string, decimal> dict_TasasComponentes = new Dictionary<string, decimal>();
                Dictionary<string, Int16> dict_TiposValor = new Dictionary<string, Int16>();
                Dictionary<string, bool> dict_ComponentesFijos = new Dictionary<string, bool>();
                Dictionary<string, bool> dict_FactorValorCredito = new Dictionary<string, bool>();
                Dictionary<string, bool> dict_SaldoPromediado = new Dictionary<string, bool>();
                Dictionary<string, int> dict_TotalComponentesSaldo = new Dictionary<string, int>();
                Dictionary<string, DTO_ccCarteraComponente> cacheCarteraComponentes = new Dictionary<string, DTO_ccCarteraComponente>();
                Dictionary<Tuple<string, string>, DTO_ccLineaComponente> cacheLineaCred = new Dictionary<Tuple<string, string>, DTO_ccLineaComponente>();
                Dictionary<Tuple<string, int>, DTO_ccComponenteEdad> cacheEdad = new Dictionary<Tuple<string, int>, DTO_ccComponenteEdad>();
                Dictionary<Tuple<string, string, int>, DTO_ccLineaComponenteMonto> cacheMontos = new Dictionary<Tuple<string, string, int>, DTO_ccLineaComponenteMonto>();
                Dictionary<Tuple<string, string, int, int>, DTO_ccLineaComponentePlazo> cachePlazos = new Dictionary<Tuple<string, string, int, int>, DTO_ccLineaComponentePlazo>();

                #endregion
                #region Validaciones

                #region Valida el SMMLV
                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDatosAnuales, periodo.Year.ToString(), true, false);
                if (basic == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidSMMLV;
                    return result;
                }
                else
                {
                    try
                    {
                        DTO_glDatosAnuales datos = (DTO_glDatosAnuales)basic;
                        SMMLV = Convert.ToInt32(datos.Valor11.Value.Value);
                    }
                    catch (Exception)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_InvalidSMMLV;
                        return result;
                    }
                }
                #endregion
                #region Valida la cuenta de IVA

                //Valida la cuenta de IVA
                if (string.IsNullOrWhiteSpace(ctaIVAID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CuentaIVAComponentes + "&&" + string.Empty;

                    return result;
                }

                ctaIVA = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaIVAID, true, false);
                porcIVA = ctaIVA.ImpuestoPorc.Value != null && ctaIVA.ImpuestoPorc.Value.HasValue ? ctaIVA.ImpuestoPorc.Value.Value : 0;

                #endregion

                #endregion
                #region Trae la lista de componentes
                int maxComp = 0;
                string maxCompID = string.Empty;
                List<DTO_ccCarteraComponente> comps = this._moduloGlobal.ccCarteraComponente_GetByLineaCredito(lineaCredID);

                comps = comps.FindAll(x => x.TipoComponente.Value.Value != (byte)TipoComponente.MayorValor).ToList();
                foreach (DTO_ccCarteraComponente c in comps)
                {
                    if (c.ID.Value == compSeguro || c.ID.Value == compIntSeguro)
                    {
                        DTO_ccSolicitudComponentes anexo = new DTO_ccSolicitudComponentes();
                        anexo.ComponenteCarteraID.Value = c.ID.Value;
                        anexo.Descripcion.Value = c.Descriptivo.Value;
                        anexo.CuotaValor.Value = 0;
                        anexo.TotalValor.Value = 0;

                        componentes.Add(anexo);

                        //Revisa si es el máximo componente
                        if (maxComp == 0 || c.NumeroComp.Value.Value > maxComp)
                        {
                            maxComp = c.NumeroComp.Value.Value;
                            maxCompID = c.ID.Value;
                        }

                        DTO_ccCarteraComponente cT = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, c.ID.Value, true, false);
                        cacheCarteraComponentes.Add(c.ID.Value, cT);
                    }
                }

                #endregion
                #region Calcula los valores de todos los componentes
               
                int i = 1;
                bool isValid = true;
                result.Details = new List<DTO_TxResultDetail>();
                int vlrCuotaPrestamo = vlrCuotaPol == 0 ? Evaluador.GetCuotaCreditoCartera(valorSolicitado, plazo, tasaTotal) : vlrCuotaPol;
                decimal vlrBase = 0;

                #region Calcula los valores de los componentes de la linea del credito
                foreach (DTO_ccSolicitudComponentes comp in componentes)
                {
                    i++;
                    comp.CompInvisibleInd.Value = false;
                    if (comp.ComponenteCarteraID.Value != compSeguro)
                    {
                        decimal tasaSaldo = 0;
                        decimal? tasaFija = null;
                        if (comp.ComponenteCarteraID.Value == compIntSeguro)
                            tasaFija = interes;

                        bool reCalculaIntAnt = false;
                        DTO_TxResult resultLiquida = this._moduloCartera.LiquidarComponente(lineaCredID, comp, valorSolicitado, vlrGiro, plazo, 0, SMMLV, ref vlrBase,
                            out tasaSaldo, porcIVA, dict_PagosAnticipados, dict_MayorValor, dict_FactorValorCredito, dict_ComponentesFijos, dict_SaldoPromediado, 
                            dict_TasasComponentes, dict_TiposValor,cacheLineaCred, cacheEdad, cacheMontos, cachePlazos, 0, tasaFija, null, ref reCalculaIntAnt);

                        if (resultLiquida.Result == ResultValue.NOK)
                        {
                            isValid = false;
                            result.Result = ResultValue.NOK;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = i;
                            rd.Message = resultLiquida.ResultMessage;
                            result.Details.Add(rd);
                        }
                        else
                        {
                            if (tasaSaldo != 0 && vlrCuotaPrestamo == 0)
                                tasaTotal += tasaSaldo;
                        }
                    }
                }

                if (!isValid)
                    return result;

                #endregion
                //Agrega los componentes de la linea del credito a la lista de los nuevos componentes
                componentesAll.AddRange(componentes);
                #endregion
                #region Llena los valores de las cuotas
                //Trae el valor de la cuota
                if (vlrCuotaPrestamo == 0)
                    vlrCuota = Evaluador.GetCuotaCreditoCartera(valorSolicitado, plazo, tasaTotal);
                else
                    vlrCuota = vlrCuotaPrestamo;

                saldoParcial = valorSolicitado;
                if (vlrCuota != 0)
                {
                    for (i = 1; i <= plazo; ++i)
                    {

                        int saldoXCapital = 0;
                        DTO_Cuota cuota = new DTO_Cuota();
                        List<string> componentesCuota = new List<string>();
                        List<int> valoresComponentesCuota = new List<int>();

                        cuota.NumCuota = i;
                        #region Calculo del capital para la ultima cuota
                        if (i == plazo)
                        {
                            cuota.Capital = saldoParcial;
                            capitalTotal += cuota.Capital;
                        }
                        #endregion
                        #region Calculo del valor de todas las cuotas excepto la ultima
                        if (i != plazo)
                        {
                            foreach (DTO_ccSolicitudComponentes comp in componentes)
                            {
                                if (comp.ComponenteCarteraID.Value == compIntSeguro)
                                {
                                    int temp = 0;
                                    decimal tasa = dict_TasasComponentes[comp.ComponenteCarteraID.Value];
                                    #region Asigna el valor del interes a la cuota
                                    if (!dict_ComponentesFijos[comp.ComponenteCarteraID.Value])
                                    {
                                        cuota.Intereses = Convert.ToInt32(tasa * saldoParcial / 100);
                                        saldoXCapital += cuota.Intereses;
                                    }
                                    temp = cuota.Intereses;
                                    #endregion
                                    #region Calculo de saldos y valores de componentes fijos y variables
                                    if (!dict_ComponentesFijos[comp.ComponenteCarteraID.Value])
                                    {
                                        if (i == 1)
                                            dict_TotalComponentesSaldo.Add(comp.ComponenteCarteraID.Value, temp);
                                        else
                                            dict_TotalComponentesSaldo[comp.ComponenteCarteraID.Value] += temp;
                                    }
                                    #endregion
                                }
                            }
                        }

                        #endregion
                        #region Calculo el valor de los componentes para la ultima cuota
                        if (i == plazo)
                        {
                            //Asigna la info del seguro
                            decimal tasa = dict_TasasComponentes[compIntSeguro];
                            DTO_ccSolicitudComponentes comp = componentes.Where(x => x.ComponenteCarteraID.Value == compIntSeguro).First();

                            cuota.Intereses = vlrCuota - cuota.Capital;
                            dict_TotalComponentesSaldo[compIntSeguro] += cuota.Intereses;
                            componentesUsuario.Insert(0, comp);
                        }
                        #endregion
                        #region Calculo del capital para todas las cuotas menos la primera
                        if (i != plazo)
                        {
                            // Asignan los valores de los componentes de anticipo
                            cuota.Capital = vlrCuota - saldoXCapital;
                            capitalTotal += cuota.Capital;
                        }
                        #endregion
                        #region Asigna la fecha de la cuota
                        if (i == 1)
                        {
                            #region Cuota1
                            fechaCuota = fechaCuota1;
                            cuota.Fecha = fechaCuota;
                            #endregion
                        }
                        else
                        {
                            //if (fechaCuota.Month == 2)
                            //{
                            //    cuota.Fecha = fechaCuota.AddMonths(i - 1);
                            //    if (cuota.Fecha.Month != 2)
                            //        cuota.Fecha = cuota.Fecha.AddDays(2);
                            //}
                            //else
                                cuota.Fecha = fechaCuota.AddMonths(i - 1);
                        }
                        #endregion

                        cuota.Componentes = componentesCuota;
                        cuota.ValoresComponentes = valoresComponentesCuota;
                        cuota.ValorCuota = cuota.Capital + cuota.Intereses;

                        if (vlrCuota != 0 && (cuota.Capital <= 0 || cuota.Intereses < 0))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Cc_PlanPagosNeg;
                            return result;
                        }

                        cuotas.Add(cuota);
                        saldoParcial -= cuota.Capital;
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cc_Cuota0Poliza;
                    return result;
                }

                #endregion
                #region Agrega el capital y calcula los valores de los saldos
                foreach (DTO_ccSolicitudComponentes comp in componentes)
                {
                    if (comp.ComponenteCarteraID.Value == compSeguro)
                    {
                        comp.TotalValor.Value = capitalTotal;
                        comp.CuotaValor.Value = Convert.ToInt32(comp.TotalValor.Value.Value / plazo);
                        componentesUsuario.Insert(0, comp);
                    }
                    else
                    {
                        comp.TotalValor.Value = dict_TotalComponentesSaldo[comp.ComponenteCarteraID.Value];
                        comp.CuotaValor.Value = Convert.ToInt32(comp.TotalValor.Value.Value / plazo);
                        comp.Porcentaje.Value = dict_TasasComponentes[comp.ComponenteCarteraID.Value];
                    }
                }

                #endregion
                #region Asigna las listas al plan de pagos
                //Organiza los componentes de usuario por componente de cartera
                planPagos.Tasas = dict_TasasComponentes;
                planPagos.ComponentesAll = (from c in componentesAll orderby c.ComponenteCarteraID.Value select c).ToList();
                planPagos.ComponentesUsuario = (from c in componentesUsuario orderby c.ComponenteCarteraID.Value select c).ToList();
                planPagos.Cuotas = cuotas;

                decimal cuotaLibranza = 0;
                foreach (DTO_ccSolicitudComponentes c in planPagos.ComponentesUsuario)
                    cuotaLibranza += c.TotalValor.Value.Value;

                #endregion
                #region Asigna Valores calculados de Campos Extra
                planPagos.VlrPrestamoPoliza = (int)valorSolicitado;
                planPagos.VlrCompra = Convert.ToInt32(vlrCompra);
                planPagos.VlrGiro = valorSolicitado - planPagos.VlrCompra - planPagos.VlrDescuento;
                //planPagos.VlrPoliza = Convert.ToInt32(cuotaLibranza);
                planPagos.VlrCuota = vlrCuota;// Convert.ToInt32(planPagos.VlrPoliza / plazo);
                planPagos.TasaTotal = tasaTotal;
                #endregion

                return planPagos;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetLiquidacionCartera");
                return result;
            }
        }

        /// <summary>
        /// Contabiliza la liquidacion de un credito
        /// </summary>
        /// <param name="ctrlCredito">Documento para contabilizar</param>
        /// <param name="coDoc">Documento contable de la contrapartida</param>
        /// <param name="ctaContraOrden">Contrapartida cuentas de orden</param>
        /// <param name="terceroAsesor">Tercero del asesor</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        private object GenerarComprobanteRenovacionPoliza(DTO_glDocumentoControl ctrlCredito, string claseCredito, DTO_coDocumento coDoc, string poliza, 
            string ctaContraOrden, string terceroAsesor, decimal vlrFinancia, decimal vlrSeguro, decimal vlrInteresSeguro, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Variables generales
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_ccCreditoPlanPagos = (DAL_ccCreditoPlanPagos)base.GetInstance(typeof(DAL_ccCreditoPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            try
            {
                #region Variables

                DTO_Comprobante comprobante = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> footerTemp = new List<DTO_ComprobanteFooter>();

                decimal tc = 0;
                bool distribuyeSeguro = false;
                string cliente = ctrlCredito.TerceroID.Value;

                //Componentes
                string componenteSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                string componenteInteresSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);

                //Variables por defecto
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string concCargoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                //Otros
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                DateTime fecha = DateTime.Now;
                if (DateTime.Now > periodo)
                {
                    int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    fecha = new DateTime(periodo.Year, periodo.Month, day);
                }

                DTO_coPlanCuenta cta;
                DTO_glConceptoSaldo cSaldo;

                DTO_ccCreditoComponentes seguroComp = new DTO_ccCreditoComponentes();
                DTO_ccCarteraComponente seguroCarteraComp = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, componenteSeguro, true, false);
                DTO_ccComponenteCuenta seguroCompCta = new DTO_ccComponenteCuenta();

                DTO_ccCreditoComponentes interesSeguroComp = new DTO_ccCreditoComponentes();
                DTO_ccCarteraComponente interesSeguroCarteraComp = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, componenteInteresSeguro, true, false);
                DTO_ccComponenteCuenta interesSeguroCompCta = new DTO_ccComponenteCuenta();

                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, DTO_glConceptoSaldo> cacheSaldos = new Dictionary<string, DTO_glConceptoSaldo>();

                //Trae la tasa de cambio
                if (this.Multimoneda())
                    tc = this._moduloGlobal.TasaDeCambio_Get(mdaLoc, periodo);

                #endregion
                #region Carga la lista de cuentas

                DAL_MasterComplex dalComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalComplex.DocumentID = AppMasters.ccComponenteCuenta;

                Dictionary<string, string> pks = new Dictionary<string, string>();
                pks.Add("ComponenteCarteraID", "");
                pks.Add("TipoEstado", ((int)TipoEstadoCartera.Propia).ToString());
                pks.Add("ClaseCredito", claseCredito);

                //Seguro
                pks["ComponenteCarteraID"] = componenteSeguro;
                DTO_MasterComplex complex = this.GetMasterComplexDTO(AppMasters.ccComponenteCuenta, pks, true);
                if (complex != null)
                {
                    seguroCompCta = (DTO_ccComponenteCuenta)complex;
                    #region Carga la cuentaID
                    if (!string.IsNullOrWhiteSpace(seguroCompCta.CuentaID.Value))
                    {
                        if (!cacheCtas.ContainsKey(seguroCompCta.CuentaID.Value))
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, seguroCompCta.CuentaID.Value, true, false);
                            cacheCtas.Add(seguroCompCta.CuentaID.Value, cta);

                            if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                            {
                                cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                            }
                        }
                    }
                    #endregion
                    #region Carga la cuenta de tercero (CtaRecursosTerceros)
                    if (!string.IsNullOrWhiteSpace(seguroCompCta.CtaRecursosTerceros.Value))
                    {
                        if (!cacheCtas.ContainsKey(seguroCompCta.CtaRecursosTerceros.Value))
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, seguroCompCta.CtaRecursosTerceros.Value, true, false);
                            cacheCtas.Add(seguroCompCta.CtaRecursosTerceros.Value, cta);

                            if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                            {
                                cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                            }
                        }
                    }
                    #endregion
                    #region Carga la cuenta de ingreso (CuentaIngreso)
                    if (!string.IsNullOrWhiteSpace(seguroCompCta.CuentaIngreso.Value))
                    {
                        if (!cacheCtas.ContainsKey(seguroCompCta.CuentaIngreso.Value))
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, seguroCompCta.CuentaIngreso.Value, true, false);
                            cacheCtas.Add(seguroCompCta.CuentaIngreso.Value, cta);

                            if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                            {
                                cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                            }
                        }
                    }
                    #endregion
                }

                //Interes Seguro
                pks["ComponenteCarteraID"] = componenteInteresSeguro;
                complex = this.GetMasterComplexDTO(AppMasters.ccComponenteCuenta, pks, true);
                if (complex != null)
                {
                    interesSeguroCompCta = (DTO_ccComponenteCuenta)complex;
                    #region Carga la cuentaID
                    if (!string.IsNullOrWhiteSpace(interesSeguroCompCta.CuentaID.Value))
                    {
                        if (!cacheCtas.ContainsKey(interesSeguroCompCta.CuentaID.Value))
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, interesSeguroCompCta.CuentaID.Value, true, false);
                            cacheCtas.Add(interesSeguroCompCta.CuentaID.Value, cta);

                            if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                            {
                                cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                            }
                        }
                    }
                    #endregion
                    #region Carga la cuenta de tercero (CtaRecursosTerceros)
                    if (!string.IsNullOrWhiteSpace(interesSeguroCompCta.CtaRecursosTerceros.Value))
                    {
                        if (!cacheCtas.ContainsKey(interesSeguroCompCta.CtaRecursosTerceros.Value))
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, interesSeguroCompCta.CtaRecursosTerceros.Value, true, false);
                            cacheCtas.Add(interesSeguroCompCta.CtaRecursosTerceros.Value, cta);

                            if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                            {
                                cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                            }
                        }
                    }
                    #endregion
                    #region Carga la cuenta de ingreso (CuentaIngreso)
                    if (!string.IsNullOrWhiteSpace(interesSeguroCompCta.CuentaIngreso.Value))
                    {
                        if (!cacheCtas.ContainsKey(interesSeguroCompCta.CuentaIngreso.Value))
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, interesSeguroCompCta.CuentaIngreso.Value, true, false);
                            cacheCtas.Add(interesSeguroCompCta.CuentaIngreso.Value, cta);

                            if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                            {
                                cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                            }
                        }
                    }
                    #endregion
                }

                //Agrega la cuenta de contrapartida a la lista de cuentas
                if (!cacheCtas.ContainsKey(ctaContraOrden))
                {
                    cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaContraOrden, true, false);
                    cacheCtas.Add(ctaContraOrden, cta);

                    if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                    {
                        cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                        cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                    }
                }

                #endregion
                #region Carga la información de la distribucion de seguro

                if (seguroCarteraComp.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota
                    && seguroCompCta.CuentaControl.Value.Value == (byte)CuentaControl.Balance
                    && !string.IsNullOrWhiteSpace(seguroCompCta.CuentaDistribuye.Value))
                {
                    distribuyeSeguro = true;

                    if (string.IsNullOrWhiteSpace(seguroCompCta.CtaRecursosTerceros.Value))
                    {
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.Message = DictionaryMessages.Err_Cc_InvalidCtaRecursosTerceros + "&&" + componenteSeguro +
                                "&&" + seguroCompCta.TipoEstado.Value.Value.ToString() + "&&" + seguroCompCta.ClaseCredito.Value;

                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                        return result;
                    }

                    if (!cacheCtas.ContainsKey(seguroCompCta.CuentaDistribuye.Value))
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, seguroCompCta.CuentaDistribuye.Value, true, false);
                        cacheCtas.Add(seguroCompCta.CuentaDistribuye.Value, cta);

                        if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                        {
                            cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                            cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                        }
                    }
                }

                #endregion
                #region Carga los registros del seguro

                if (seguroCompCta.AsesorInd.Value.Value)
                    ctrlCredito.TerceroID.Value = terceroAsesor;
                else if (!String.IsNullOrWhiteSpace(seguroCompCta.TerceroID.Value) && seguroCompCta.CuentaControl.Value != 2)
                    ctrlCredito.TerceroID.Value = seguroCompCta.TerceroID.Value;
                else
                    ctrlCredito.TerceroID.Value = cliente;

                if (!string.IsNullOrWhiteSpace(seguroCompCta.CuentaID.Value))
                {
                    cta = cacheCtas[seguroCompCta.CuentaID.Value];
                    cSaldo = cacheSaldos[cta.ConceptoSaldoID.Value];
                    DTO_ComprobanteFooter f = this.CrearComprobanteFooter(ctrlCredito, cta, cSaldo, tc, concCargoXdef, lgXdef, lineaXdef, vlrFinancia, 0, false);
                    f.IdentificadorTR.Value = ctrlCredito.DocumentoPadre.Value; //Asigna el numDoc del credito
                    f.DatoAdd9.Value = AuxiliarDatoAdd9.Propia.ToString();
                    footerTemp.Add(f);
                }

                #endregion
                #region Carga los registros del interes del seguro

                if (interesSeguroCompCta.AsesorInd.Value.Value)
                    ctrlCredito.TerceroID.Value = terceroAsesor;
                else if (!String.IsNullOrWhiteSpace(interesSeguroCompCta.TerceroID.Value) && interesSeguroCompCta.CuentaControl.Value != 2)
                    ctrlCredito.TerceroID.Value = interesSeguroCompCta.TerceroID.Value;
                else
                    ctrlCredito.TerceroID.Value = cliente;

                if (!string.IsNullOrWhiteSpace(interesSeguroCompCta.CuentaID.Value))
                {
                    cta = cacheCtas[interesSeguroCompCta.CuentaID.Value];
                    cSaldo = cacheSaldos[cta.ConceptoSaldoID.Value];
                    DTO_ComprobanteFooter f = this.CrearComprobanteFooter(ctrlCredito, cta, cSaldo, tc, concCargoXdef, lgXdef, lineaXdef, vlrInteresSeguro, 0, false);
                    f.IdentificadorTR.Value = ctrlCredito.DocumentoPadre.Value; //Asigna el numDoc del credito
                    f.DatoAdd9.Value = AuxiliarDatoAdd9.Propia.ToString();
                    footerTemp.Add(f);
                }

                #endregion
                #region Carga la distribución del seguro

                if (distribuyeSeguro)
                {
                    
                    decimal valRestante = Math.Abs(vlrSeguro - vlrFinancia);

                    //Valor seguro
                    cta = cacheCtas[seguroCompCta.CtaRecursosTerceros.Value];
                    cSaldo = cacheSaldos[cta.ConceptoSaldoID.Value];
                    DTO_ComprobanteFooter f = this.CrearComprobanteFooter(ctrlCredito, cta, cSaldo, tc, concCargoXdef, lgXdef, lineaXdef, (vlrFinancia - valRestante) * -1, 0, false);
                    footerTemp.Add(f);

                    //Valor de distribución
                    DTO_coPlanCuenta ctaDistribuye = cacheCtas[seguroCompCta.CuentaDistribuye.Value];
                    DTO_glConceptoSaldo cSaldoDistribuye = cacheSaldos[ctaDistribuye.ConceptoSaldoID.Value];
                    DTO_ComprobanteFooter fDistribuye = this.CrearComprobanteFooter(ctrlCredito, ctaDistribuye, cSaldoDistribuye, tc, concCargoXdef, lgXdef, lineaXdef, valRestante * -1, 0, false);
                    footerTemp.Add(fDistribuye);
                }

                #endregion
                #region Ordena los registros y agrega las contras
                List<DTO_ComprobanteFooter> fOrden = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> fBalance = new List<DTO_ComprobanteFooter>();

                #region Carga los detalles de orden y balance
                decimal vlrOrdenML = 0;
                decimal vlrBalanceML = 0;
                decimal vlrTotalML = 0;

                decimal vlrOrdenME = 0;
                decimal vlrBalanceME = 0;
                decimal vlrTotalME = 0;

                foreach (DTO_ComprobanteFooter det in footerTemp)
                {
                    cta = cacheCtas[det.CuentaID.Value];
                    det.vlrMdaExt.Value = tc != 0 ? Math.Round(det.vlrMdaLoc.Value.Value / tc, 2) : 0;
                    vlrTotalML += det.vlrMdaLoc.Value.Value;
                    vlrTotalME += det.vlrMdaExt.Value.Value;

                    if (cta.Tipo.Value == ((int)TipoCuenta.Orden).ToString())
                    {
                        vlrOrdenML += det.vlrMdaLoc.Value.Value;
                        vlrOrdenME += det.vlrMdaExt.Value.Value;
                        det.Descriptivo.Value = "RENOVACIÓN PÓLIZA " + poliza;
                        fOrden.Add(det);
                    }
                    else
                    {
                        vlrBalanceML += det.vlrMdaLoc.Value.Value;
                        vlrBalanceME += det.vlrMdaExt.Value.Value;
                        det.Descriptivo.Value = "RENOVACIÓN PÓLIZA " + poliza;
                        fBalance.Add(det);
                    }
                }
                #endregion
                #region Agrega la contra de balance
                if (fBalance.Count > 0)
                {
                    footer.AddRange(fBalance);

                    //Carga la info de la cuenta
                    if (!cacheCtas.ContainsKey(coDoc.CuentaLOC.Value))
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, coDoc.CuentaLOC.Value, true, false);
                        cacheCtas.Add(coDoc.CuentaLOC.Value, cta);
                    }
                    else
                        cta = cacheCtas[coDoc.CuentaLOC.Value];

                    if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                    {
                        cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                        cacheSaldos.Add(cta.ConceptoSaldoID.Value, cSaldo);
                    }
                    else
                        cSaldo = cacheSaldos[cta.ConceptoSaldoID.Value];

                    //Este registro sobra, cambiado el 29 Ago 
                    //DTO_ComprobanteFooter bal = this.CrearComprobanteFooter(ctrlCredito, cta, cSaldo, tc, concCargoXdef, lgXdef, lineaXdef, vlrBalanceML * -1, vlrBalanceME * -1, true);
                    //bal.Descriptivo.Value = "RENOVACIÓN PÓLIZA " + poliza;     
                    //footer.Add(bal);
                }
                #endregion
                #region Agrega la contra de orden
                if (fOrden.Count > 0)
                {
                    footer.AddRange(fOrden);

                    if (!cacheCtas.ContainsKey(ctaContraOrden))
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaContraOrden, true, false);
                    else
                        cta = cacheCtas[ctaContraOrden];

                    if (!cacheSaldos.ContainsKey(cta.ConceptoSaldoID.Value))
                        cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                    else
                        cSaldo = cacheSaldos[cta.ConceptoSaldoID.Value];

                    DTO_ComprobanteFooter orden = this.CrearComprobanteFooter(ctrlCredito, cta, cSaldo, tc, concCargoXdef, lgXdef, lineaXdef, vlrOrdenML * -1, vlrOrdenME * -1, true);
                    orden.Descriptivo.Value = "RENOVACIÓN PÓLIZA " + poliza;                   
                    footer.Add(orden);
                }
                #endregion

                #endregion
                #region Carga el cabezote
                if (footer.Count > 0)
                {
                    //De donde sale el comprobanteID
                    header.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                    header.ComprobanteNro.Value = 0;
                    header.Fecha.Value = fecha;
                    header.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                    header.MdaTransacc.Value = mdaLoc;
                    header.NumeroDoc.Value = ctrlCredito.NumeroDoc.Value;
                    header.PeriodoID.Value = periodo;
                    header.TasaCambioBase.Value = tc;
                    header.TasaCambioOtr.Value = tc;

                    comprobante.Header = header;
                    comprobante.Footer = footer;

                    return comprobante;
                }
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ContabilizaLiquidacion");

                return result;
            }
            finally
            {
                if (!insideAnotherTx)
                    throw new Exception("ContabilizaLiquidacion - Los consecutivos deben ser generados por la transaccion padre");
            }
        }

        #endregion

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Actualiza las polizas
        /// </summary>
        /// <param name="isSolicitud">Indicador si se esta buscando una solisitud o un credito</param>
        /// <param name="libranzaID">Identificador de la libranza</param>
        /// <returns><Retorna la info de un credito/returns>
        public void PolizaEstado_Upd(DTO_ccPolizaEstado poliza, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccPolizaEstado.DAL_ccPolizaEstado_Upd(poliza);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PolizaEstado_Upd");
                throw ex;
            }
            finally
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Elimina la poliza pedida
        /// </summary>
        /// <param name="terceroID">tercero de la poliza</param>
        /// <param name="poliza">Nro de la poliza</param>
        /// <returns></returns>
        public void PolizaEstado_Delete(string terceroID, string poliza, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccPolizaEstado.DAL_ccPolizaEstado_Delete(terceroID,poliza);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PolizaEstado_Delete");
                throw ex;
            }
            finally
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Obtiene las polizas
        /// </summary>
        /// <param name="poliza">filtro</param>
        /// <returns><Retorna la info filtrada/returns>
        public List<DTO_ccPolizaEstado> PolizaEstado_GetByParameter(DTO_ccPolizaEstado poliza)
        {
            this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_ccPolizaEstado.DAL_ccPolizaEstado_GetByParameter(poliza);
        }

        /// <summary>
        /// Renueva una póliza
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <ctrlCredito>Documento control relacionado con el crédito</ctrlCredito>
        /// <creditoPoliza>Credito con las actualizaciones de la poliza</creditoPoliza>
        /// <cuota1Pol>Primera cuota del crédito asignado a la poliza</cuota1Pol>
        /// <cuotasPoliza>Plan de pagos por cada cuota para la poliza</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RenovacionPoliza(int documentID, int? numDocSolicitud,  DTO_ccCreditoDocu credito, DTO_PlanDePagos planPagos, DTO_ccPolizaEstado poliza, 
            bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            try
            {
                this._dal_ccSolicitudDocu = (DAL_ccSolicitudDocu)base.GetInstance(typeof(DAL_ccSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccSolicitudPlanPagos = (DAL_ccSolicitudPlanPagos)base.GetInstance(typeof(DAL_ccSolicitudPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (!numDocSolicitud.HasValue)
                {
                    #region Crea el glDocumentoControl para la poliza

                    DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));

                    // Copia del documento
                    DTO_glDocumentoControl ctrlCredito = this._moduloGlobal.glDocumentoControl_GetByID(credito.NumeroDoc.Value.Value);
                    ctrl = ObjectCopier.Clone(ctrlCredito);

                    ctrl.DocumentoNro.Value = 0;
                    ctrl.DocumentoID.Value = documentID;
                    ctrl.NumeroDoc.Value = 0;
                    ctrl.PeriodoDoc.Value = periodo;
                    ctrl.PeriodoUltMov.Value = periodo;
                    ctrl.Fecha.Value = DateTime.Now;
                    ctrl.FechaDoc.Value = credito.FechaLiqSeguro.Value;
                    ctrl.Observacion.Value = "Renovación Póliza " + credito.Poliza.Value;
                    ctrl.Descripcion.Value = "Renovación Póliza " + credito.Poliza.Value;
                    ctrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                    ctrl.Valor.Value = credito.VlrFinanciaSeguro.Value.Value;
                    ctrl.DocumentoPadre.Value = ctrlCredito.NumeroDoc.Value;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }
                    int numDocNew = Convert.ToInt32(resultGLDC.Key);
                    ctrl.NumeroDoc.Value = numDocNew;
                    #endregion
                    #region Crea la solicitud

                    //Copia de la solicitud
                    DTO_ccSolicitudDocu solicitudNew = new DTO_ccSolicitudDocu();
                    DTO_coTercero cliente = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, credito.ClienteID.Value, true, false);

                    //Info del crédito
                    solicitudNew.NumeroDoc.Value = credito.NumeroDoc.Value;
                    solicitudNew.ClienteRadica.Value = credito.ClienteID.Value;
                    solicitudNew.ClienteID.Value = credito.ClienteID.Value;
                    solicitudNew.ApellidoPri.Value = cliente.ApellidoPri.Value;
                    solicitudNew.ApellidoSdo.Value = cliente.ApellidoSdo.Value;
                    solicitudNew.NombrePri.Value = cliente.NombrePri.Value;
                    solicitudNew.NombreSdo.Value = cliente.NombreSdo.Value;
                    solicitudNew.Libranza.Value = credito.Libranza.Value;
                    solicitudNew.LineaCreditoID.Value = credito.LineaCreditoID.Value;
                    solicitudNew.AsesorID.Value = credito.AsesorID.Value;
                    solicitudNew.ConcesionarioID.Value = credito.ConcesionarioID.Value;
                    solicitudNew.AseguradoraID.Value = credito.AseguradoraID.Value;
                    solicitudNew.CooperativaID.Value = credito.CooperativaID.Value;
                    solicitudNew.ZonaID.Value = credito.ZonaID.Value;
                    solicitudNew.Ciudad.Value = credito.Ciudad.Value;
                    solicitudNew.TipoCreditoID.Value = credito.TipoCreditoID.Value;
                    solicitudNew.VendedorID.Value = credito.VendedorID.Value;
                    solicitudNew.PagaduriaID.Value = credito.PagaduriaID.Value;
                    solicitudNew.CentroPagoID.Value = credito.CentroPagoID.Value;
                    solicitudNew.IncorporaMesInd.Value = credito.IncorporaMesInd.Value;
                    solicitudNew.IncorporacionTipo.Value = credito.IncorporacionTipo.Value;
                    solicitudNew.NumDocVerificado.Value = credito.NumDocVerificado.Value;
                    solicitudNew.NumDocOpera.Value = credito.NumDocOpera.Value;
                    solicitudNew.PeriodoPago.Value = credito.PeriodoPago.Value;
                    solicitudNew.NumDocCompra.Value = credito.NumDocCompra.Value;
                    solicitudNew.PorComponente1.Value = credito.PorComponente1.Value;
                    solicitudNew.PorComponente2.Value = credito.PorComponente2.Value;
                    solicitudNew.PorComponente3.Value = credito.PorComponente3.Value;
                    solicitudNew.PagoVentanillaInd.Value = credito.PagoVentanillaInd.Value;
                    solicitudNew.Codeudor1.Value = credito.Codeudor1.Value;
                    solicitudNew.Codeudor2.Value = credito.Codeudor2.Value;
                    solicitudNew.Codeudor3.Value = credito.Codeudor3.Value;
                    solicitudNew.Codeudor4.Value = credito.Codeudor4.Value;
                    solicitudNew.Codeudor5.Value = credito.Codeudor5.Value;
                    solicitudNew.TipoCredito.Value = credito.TipoCredito.Value;
                    solicitudNew.FechaCuota1.Value = credito.FechaCuota1.Value;
                    solicitudNew.CompraCarteraInd.Value = credito.CompraCarteraInd.Value;
                    solicitudNew.TasaEfectivaCredito.Value = credito.TasaEfectivaCredito.Value;
                    solicitudNew.FechaVto.Value = credito.FechaVto.Value;
                    solicitudNew.PorInteres.Value = credito.PorInteres.Value;
                    solicitudNew.PorSeguro.Value = credito.PorSeguro.Value;
                    solicitudNew.VlrSolicitado.Value = 0; //credito.VlrSolicitado.Value;
                    solicitudNew.VlrAdicional.Value = 0; //credito.VlrAdicional.Value;
                    solicitudNew.VlrPrestamo.Value = 0; //credito.VlrPrestamo.Value;
                    solicitudNew.VlrLibranza.Value = 0; //credito.VlrLibranza.Value;
                    solicitudNew.VlrCompra.Value = 0; //credito.VlrCompra.Value;
                    solicitudNew.VlrDescuento.Value = 0; //credito.VlrDescuento.Value;
                    solicitudNew.VlrGiro.Value = 0; //credito.VlrGiro.Value;
                    solicitudNew.Plazo.Value = credito.Plazo.Value;
                    solicitudNew.VlrCuota.Value = credito.VlrCuota.Value;
                    solicitudNew.VlrCupoDisponible.Value = credito.VlrCupoDisponible.Value;
                    solicitudNew.Solicitud.Value = credito.Solicitud.Value;
                    solicitudNew.Pagare.Value = credito.Pagare.Value;
                    solicitudNew.ConcesionarioID.Value = credito.ConcesionarioID.Value;
                    solicitudNew.PlazoSeguro.Value = credito.PlazoSeguro.Value;
                    solicitudNew.IncorporacionPreviaInd.Value = false;

                    //Info de póliza
                    solicitudNew.NumeroDoc.Value = numDocNew;
                    solicitudNew.DocSeguro.Value = numDocNew;
                    solicitudNew.Poliza.Value = credito.Poliza.Value;
                    solicitudNew.FechaLiqSeguro.Value = credito.FechaLiqSeguro.Value;
                    solicitudNew.FechaVigenciaINI.Value = credito.FechaVigenciaINI.Value;
                    solicitudNew.FechaVigenciaFIN.Value = credito.FechaVigenciaFIN.Value;
                    solicitudNew.PorSeguro.Value = credito.PorSeguro.Value;
                    solicitudNew.Cuota1Seguro.Value = credito.Cuota1Seguro.Value;
                    solicitudNew.VlrPoliza.Value = credito.VlrPoliza.Value;
                    solicitudNew.VlrCuotaSeguro.Value = credito.VlrCuotaSeguro.Value;
                    solicitudNew.VlrFinanciaSeguro.Value = credito.VlrFinanciaSeguro.Value;
                    solicitudNew.Observacion.Value = "Renovación Póliza " + credito.Poliza.Value;

                    this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_Add(solicitudNew);

                    #endregion
                    #region Crea el plan de pagos

                    this._dal_ccSolicitudPlanPagos.DAL_ccSolicitudPlanPagos_Add(planPagos, ctrl.NumeroDoc.Value.Value);

                    #endregion
                }
                else
                {
                    #region Actualiza el glDocumentoControl para la poliza

                    // Copia del documento
                    ctrl = this._moduloGlobal.glDocumentoControl_GetByID(numDocSolicitud.Value);
                    ctrl.FechaDoc.Value = credito.FechaLiqSeguro.Value;
                    ctrl.Valor.Value = credito.VlrFinanciaSeguro.Value.Value;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Update(ctrl, true, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }

                    #endregion
                    #region Actualiza la solicitud

                    //Copia de la solicitud
                    DTO_ccSolicitudDocu solicitudNew = this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_GetByNumeroDoc(numDocSolicitud.Value);

                    solicitudNew.Poliza.Value = credito.Poliza.Value;
                    solicitudNew.FechaLiqSeguro.Value = credito.FechaLiqSeguro.Value;
                    solicitudNew.FechaVigenciaINI.Value = credito.FechaVigenciaINI.Value;
                    solicitudNew.FechaVigenciaFIN.Value = credito.FechaVigenciaFIN.Value;
                    solicitudNew.PorSeguro.Value = credito.PorSeguro.Value;
                    solicitudNew.Cuota1Seguro.Value = credito.Cuota1Seguro.Value;
                    solicitudNew.VlrPoliza.Value = credito.VlrPoliza.Value;
                    solicitudNew.VlrCuotaSeguro.Value = credito.VlrCuotaSeguro.Value;
                    solicitudNew.VlrFinanciaSeguro.Value = credito.VlrFinanciaSeguro.Value;

                    this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_Update(solicitudNew);

                    #endregion
                    #region Actualiza el plan de pagos

                    //Borra el plan de pagos
                    this._dal_ccSolicitudPlanPagos.DAL_ccSolicitudPlanPagos_Delete(ctrl.NumeroDoc.Value.Value);

                    //Asigna el nuevo plan de pagos
                    this._dal_ccSolicitudPlanPagos.DAL_ccSolicitudPlanPagos_Add(planPagos, ctrl.NumeroDoc.Value.Value);

                    #endregion
                }

                #region Actualiza la póliza

                this._dal_ccPolizaEstado.DAL_ccPolizaEstado_Upd(poliza);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DigitacionCredito_Add");
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

                        if (!numDocSolicitud.HasValue)
                        {
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, false, false, false);
                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Renueva una póliza de cobro Juridico
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <poliza>Poliza a renovar</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CobroJuridicoRenovacionPoliza_Add(int documentID, DTO_ccPolizaEstado poliza, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            DTO_Comprobante comprobante = null;
            DTO_coComprobante dtoComp = null;
            try
            {
                this._moduloCartera = (ModuloCartera)base.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoDocu = (DAL_ccCreditoDocu)base.GetInstance(typeof(DAL_ccCreditoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPlanPagos = (DAL_ccCreditoPlanPagos)base.GetInstance(typeof(DAL_ccCreditoPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                string defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string defConceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));

                DTO_glDocumentoControl ctrlCredito = this._moduloGlobal.glDocumentoControl_GetByID(poliza.NumDocCredito.Value.Value);
                DTO_ccCreditoDocu cred = this._dal_ccCreditoDocu.DAL_ccCreditoDocu_GetByID(poliza.NumDocCredito.Value.Value);
                List<DTO_ccCreditoPlanPagos> planPagos = this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_GetByNumDoc(poliza.NumDocCredito.Value.Value);
                DTO_ccLineaCredito lineaCred = (DTO_ccLineaCredito)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccLineaCredito, cred.LineaCreditoID.Value, true, false);
                string coDocRenovacion = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DocContableRenovacionPol);
                DTO_coDocumento dtoCoDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocRenovacion, true, false);

                #region Obtiene y Valida las cuentas y el comprobante
                string tipoestado = string.Empty;
                if (cred.EstadoDeuda.Value == (byte)TipoEstadoCartera.CobroJuridico)
                    tipoestado = "3";
                else if (cred.EstadoDeuda.Value == (byte)TipoEstadoCartera.AcuerdoPago)
                    tipoestado = "4";
                else if (cred.EstadoDeuda.Value == (byte)TipoEstadoCartera.AcuerdoPagoIncumplido)
                    tipoestado = "5";
                Dictionary<string, string> pks = new Dictionary<string, string>();
                pks.Add("ComponenteCarteraID", compSeguro);
                pks.Add("TipoEstado", tipoestado);
                pks.Add("ClaseCredito", lineaCred.ClaseCredito.Value);
                DTO_ccComponenteCuenta dtoCtaCompon = (DTO_ccComponenteCuenta)this.GetMasterComplexDTO(AppMasters.ccComponenteCuenta, pks, true);
                if (dtoCtaCompon == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cf_ComponenteSegNotExist;
                    return result;
                }
                if (string.IsNullOrEmpty(dtoCtaCompon.CtaRecursosTerceros.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CuentaInvalid;
                    return result;
                }

                //Revisa el estado del crédito
                if (string.IsNullOrEmpty(coDocRenovacion)|| dtoCoDoc == null )
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_DocContableRenovacionPol;
                    return result;
                }

                //Valida que tenga comprobante
                if (string.IsNullOrWhiteSpace(dtoCoDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }
                else
                    dtoComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, dtoCoDoc.ComprobanteID.Value, true, false);

                #endregion
                #region Crea el glDocumentoControl de Renovacion
                // Copia del documento
                ctrl = ObjectCopier.Clone(ctrlCredito);
                ctrl.ComprobanteID.Value = dtoCoDoc.ComprobanteID.Value;
                ctrl.ComprobanteIDNro.Value = null;
                ctrl.DocumentoNro.Value = null;                
                ctrl.TerceroID.Value = poliza.TerceroID.Value;
                ctrl.DocumentoTercero.Value = string.Empty;
                ctrl.DocumentoNro.Value = 0;
                ctrl.DocumentoID.Value = AppDocuments.RenovacionPoliza;
                ctrl.NumeroDoc.Value = 0;
                ctrl.PeriodoDoc.Value = periodo;
                ctrl.PeriodoUltMov.Value = periodo;
                ctrl.Fecha.Value = DateTime.Now;
                ctrl.FechaDoc.Value = poliza.FechaMvto.Value;
                ctrl.Observacion.Value = "Renovación Póliza  " + poliza.Poliza.Value + "(Cobro Juridico)";
                ctrl.Descripcion.Value = "Renovación Póliza " + poliza.Poliza.Value + "(Cobro Juridico)";
                ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrl.Valor.Value = poliza.VlrPoliza.Value.Value;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }
                int numDocNew = Convert.ToInt32(resultGLDC.Key);
                ctrl.NumeroDoc.Value = numDocNew;
                #endregion
                #region Actualiza la póliza
                poliza.NumeroDocLiquida.Value = numDocNew;
                this._dal_ccPolizaEstado.DAL_ccPolizaEstado_Upd(poliza);

                #endregion
                #region Actualiza el plan de pagos
                foreach (DTO_ccCreditoPlanPagos pp in planPagos)
                {
                    pp.VlrSeguro.Value += poliza.VlrPoliza.Value;
                    this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_Update(pp);
                }
              
                #endregion
                #region Agrega a CarteraMVto
                DTO_ccCarteraMvto mvto = new DTO_ccCarteraMvto();
                mvto.NumCredito.Value = poliza.NumDocCredito.Value;
                mvto.NumeroDoc.Value = numDocNew;
                mvto.ComponenteCarteraID.Value = compSeguro;
                mvto.Tasa.Value = 0;
                mvto.VlrAbono.Value = 0;
                mvto.VlrComponente.Value = poliza.VlrPoliza.Value;
                this._moduloCartera.ccCarteraMvto_Add(mvto);
                
                #endregion
                #region Genera la info detalle del comprobante
                comprobante = new DTO_Comprobante();
                //Debido
                DTO_coPlanCuenta ctaDeb = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, dtoCtaCompon.CuentaID.Value, true, false);
                DTO_glConceptoSaldo cSaldoDeb = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaDeb.ConceptoSaldoID.Value, true, false);
                DTO_ComprobanteFooter comp1 = this.CrearComprobanteFooter(ctrl, ctaDeb, cSaldoDeb, 0, defConceptoCargo, defLugarGeografico, defLineaPresupuesto, poliza.VlrPoliza.Value.Value, 0, false);
                comp1.Descriptivo.Value = "COBRO JURIDICO RENOVACION POL ";
                comprobante.Footer.Add(comp1);

                //Credito-Contrapartida
                DTO_coPlanCuenta ctaCred = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, dtoCtaCompon.CtaRecursosTerceros.Value, true, false);
                DTO_glConceptoSaldo cSaldoCred = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaCred.ConceptoSaldoID.Value, true, false);
                DTO_ComprobanteFooter comp2 = this.CrearComprobanteFooter(ctrl, ctaCred, cSaldoCred, 0, defConceptoCargo, defLugarGeografico, defLineaPresupuesto, poliza.VlrPoliza.Value.Value*-1, 0,true);
                comp2.Descriptivo.Value = "COBRO JURIDICO RENOVACION POL ";
                comprobante.Footer.Add(comp2);

                #endregion
                #region Contabiliza el comprobante
                #region Carga el header del comprobante
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                comprobante.Header.NumeroDoc.Value = numDocNew;
                comprobante.Header.ComprobanteID.Value = dtoCoDoc.ComprobanteID.Value;
                comprobante.Header.ComprobanteNro.Value = 0;
                comprobante.Header.Fecha.Value = ctrl.FechaDoc.Value;
                comprobante.Header.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                comprobante.Header.MdaTransacc.Value = ctrl.MonedaID.Value;
                comprobante.Header.PeriodoID.Value = ctrl.PeriodoDoc.Value;
                comprobante.Header.TasaCambioBase.Value = ctrl.TasaCambioCONT.Value;
                comprobante.Header.TasaCambioOtr.Value = ctrl.TasaCambioCONT.Value;
                #endregion
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comprobante, ctrl.PeriodoDoc.Value.Value, ModulesPrefix.cc, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    return result;
                }
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DigitacionCredito_Add");
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

                        if (ctrl.NumeroDoc.Value != null && ctrl.NumeroDoc.Value != 0)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.RenovacionPoliza, ctrl.PrefijoID.Value);
                            ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(dtoComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }



        /// <summary>
        /// Registra o actualizas las polizas de cartera
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="polizaEstado">Encabezado</param>
        /// <param name="detallePoliza">Detalle</param>
        /// <param name="insideAnotherTx">Indica es otra transaccion</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult RegistroPoliza(int documentID, DTO_ccPolizaEstado polizaEstado, byte tipoMvto, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                DTO_ccPolizaEstado filter = new DTO_ccPolizaEstado();
                filter.TerceroID.Value = polizaEstado.TerceroID.Value;
                filter.Poliza.Value = polizaEstado.Poliza.Value;
                filter.FechaVigenciaINI.Value = polizaEstado.FechaVigenciaINI.Value;
                List<DTO_ccPolizaEstado> res = this.PolizaEstado_GetByParameter(filter);
                if (res.Count == 0)
                    result = this.PolizaEstado_Add(polizaEstado);
                else
                {
                    polizaEstado.Consecutivo.Value = res.First().Consecutivo.Value;
                    this.PolizaEstado_Upd(polizaEstado, true);
                }
              
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "RegistroPoliza");
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
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Obtiene las polizas de cartera para pagos
        /// </summary>
        /// <returns>Lista</returns>
        public List<DTO_ccPolizaEstado> Poliza_GetForPagos()
        {
            this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_ccPolizaEstado> results = this._dal_ccPolizaEstado.DAL_ccPolizaEstado_GetForPagos();

            return results;
        }

        /// <summary>
        /// Realiza los pagos de poliza con autorizacion de Giro(CxP)
        /// </summary>
        /// <param name="documentID">doc actual</param>
        /// <param name="pagos">lista de pagos</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult PagoPolizasCartera(int documentID,DateTime fechaDoc, List<DTO_ccPolizaEstado> pagos, string aseguradora, bool isAnotherTx, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            #region Variables
            string conceptoCxPPagoTercero = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CxPPagosAnticiposySaldoFavor);
            string componenteSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
            string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string lineaPresupuestalxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            string lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            string conceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            string proyXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            string centroCtoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            string prefijoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            string periodo = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);

            decimal valorTotal = pagos.FindAll(x => x.PagarInd.Value.Value).Sum(x => x.VlrPoliza.Value.Value);
            DTO_Comprobante _comprobante = new DTO_Comprobante();
            DTO_glDocumentoControl ctrlCxP = new DTO_glDocumentoControl();
            List<DTO_ccPolizaEstado> polizas = new List<DTO_ccPolizaEstado>();
            DTO_coComprobante dtoComprob = null;
            string cuentaAseg = string.Empty;

            #endregion

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCxP = (ModuloCuentasXPagar)base.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Validaciones
                #region Valida el componenteSeguro
                if (string.IsNullOrWhiteSpace(componenteSeguro))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cf_ComponenteSegNotExist;
                    return result;
                }
                Dictionary<string, string> pks = new Dictionary<string, string>();
                pks.Add("ComponenteCarteraID", componenteSeguro);
                pks.Add("TipoEstado", "1");
                DTO_ccComponenteCuenta dtoCtaCompon = (DTO_ccComponenteCuenta)this.GetMasterComplexDTO(AppMasters.ccComponenteCuenta, pks, true);
                if (dtoCtaCompon == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cf_ComponenteSegNotExist;
                    return result;
                }
                if (string.IsNullOrEmpty(dtoCtaCompon.CtaRecursosTerceros.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CuentaInvalid;
                    return result;
                }
                #endregion
                #region Valida el concepto CxP
                if (string.IsNullOrWhiteSpace(conceptoCxPPagoTercero))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }
                DTO_cpConceptoCXP concCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, conceptoCxPPagoTercero, true, false);
                if (concCxP == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, concCxP.coDocumentoID.Value, true, false);
                //Revisa que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }
                //Revisa que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }
                #endregion
                #region Valida la cuenta del coDoc segun la moneda
                if ((string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                    return result;
                }
                else
                {
                    //Valida que la cuenta sea de Componente Tercero
                    cuentaAseg = !string.IsNullOrEmpty(coDoc.CuentaLOC.Value) ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuentaAseg, true, false);
                    DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                }
                #endregion
                #region Valida Periodo
                DateTime periodoMod = DateTime.Now;
                if (!string.IsNullOrEmpty(periodo))
                    periodoMod = Convert.ToDateTime(periodo);
                #endregion
                #endregion
                dtoComprob = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                DTO_ccAseguradora aseg = (DTO_ccAseguradora)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAseguradora, aseguradora, true, false);
                string terceroAseg = aseg.TerceroID.Value;
                ctrlCxP = new DTO_glDocumentoControl();
                _comprobante = new DTO_Comprobante();
                #region Ctrl CxP
                ctrlCxP.NumeroDoc.Value = 0;
                ctrlCxP.DocumentoID.Value = AppDocuments.CausarFacturas;
                ctrlCxP.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                ctrlCxP.DocumentoNro.Value = 0;
                ctrlCxP.PeriodoDoc.Value = periodoMod;
                ctrlCxP.FechaDoc.Value = fechaDoc;
                ctrlCxP.PeriodoUltMov.Value = fechaDoc;
                ctrlCxP.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                ctrlCxP.PrefijoID.Value = prefijoxDef;
                ctrlCxP.Observacion.Value = "Pago Pólizas";
                ctrlCxP.Descripcion.Value = "Causacion Facturas";
                ctrlCxP.MonedaID.Value = monedaLocal;
                ctrlCxP.TasaCambioCONT.Value = 0;
                ctrlCxP.TasaCambioDOCU.Value = 0;
                ctrlCxP.ProyectoID.Value = proyXDef;
                ctrlCxP.CentroCostoID.Value = centroCtoxDef;
                ctrlCxP.LineaPresupuestoID.Value = lineaPresupuestalxDef;
                ctrlCxP.LugarGeograficoID.Value = lugarGeografico;
                ctrlCxP.TerceroID.Value = terceroAseg;
                ctrlCxP.DocumentoTercero.Value = fechaDoc.ToShortDateString();
                ctrlCxP.CuentaID.Value = cuentaAseg;
                ctrlCxP.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrlCxP.seUsuarioID.Value = this.UserId;
                ctrlCxP.Valor.Value = valorTotal;
                #endregion    

                DTO_coPlanCuenta ctaRecursoTercero = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, dtoCtaCompon.CtaRecursosTerceros.Value, true, false);
                DTO_glConceptoSaldo conSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaRecursoTercero.ConceptoSaldoID.Value, true, false);

                foreach (DTO_ccPolizaEstado pag in pagos.FindAll(x=>x.PagarInd.Value.Value))
                {
                    #region Variables Iniciales
                    DTO_ccPolizaEstado pol = this._dal_ccPolizaEstado.DAL_ccPolizaEstado_GetByConsecutivo(pag.Consecutivo.Value.Value);
                    if (!pol.ValorRevoca.Value.HasValue) pol.ValorRevoca.Value = 0;
                    if (!pol.VlrPoliza.Value.HasValue) pol.VlrPoliza.Value = 0;
                    DTO_glDocumentoControl ctrlDoc = this._moduloGlobal.glDocumentoControl_GetByID(pol.NumDocCredito.Value != null ? pol.NumDocCredito.Value.Value : pol.NumDocSolicitud.Value.Value);
                    ctrlDoc.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                    #endregion
                    #region Carga el footer
                    //ctrlDoc.CuentaID.Value = dtoCtaCompon.CtaRecursosTerceros.Value;
                    DTO_ComprobanteFooter compFooterTemp = this.CrearComprobanteFooter(ctrlCxP, ctaRecursoTercero, conSaldo, 0, conceptoCargo, lugarGeografico, lineaPresupuestalxDef, pag.VlrPoliza.Value.Value, 0, false);
                    compFooterTemp.Descriptivo.Value = "Componente Seguro Cartera ";
                    compFooterTemp.TerceroID.Value = pag.TerceroID.Value;
                    _comprobante.Footer.Add(compFooterTemp);
                    polizas.Add(pol);
                    #endregion
                }
                _comprobante.Footer = _comprobante.Footer;
                #region Crea la CxP    
                object res = this._moduloCxP.CuentasXPagar_Generar(ctrlCxP,conceptoCxPPagoTercero,valorTotal,_comprobante.Footer,ModulesPrefix.cc,false);
                if (res.GetType() == typeof(DTO_TxResult))
                {
                    result.Result = ResultValue.NOK;
                    return result;
                }
                else
                    ctrlCxP = (DTO_glDocumentoControl)res;
                #endregion
                #region Actualiza documentos complementario con el numDoc de la CxP (ccPolizaEstado/ccPolizaDetalle)
                foreach (DTO_ccPolizaEstado pol in polizas)
                {
                    if ((!pol.NumeroDocPago.Value.HasValue || pol.NumeroDocPago.Value == 0) && pol.VlrPoliza.Value != 0)
                        pol.NumeroDocPago.Value = ctrlCxP.NumeroDoc.Value;
                    if ((!pol.NumDocRevoca.Value.HasValue || pol.NumDocRevoca.Value == 0) && pol.ValorRevoca.Value != 0)
                        pol.NumDocPagoRevoca.Value = ctrlCxP.NumeroDoc.Value;
                    this.PolizaEstado_Upd(pol, true);
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PagoPolizasCartera");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!isAnotherTx)
                    {
                        #region Genera el consecutivo de CxP
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloCxP._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(dtoComprob, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !isAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Trae la ultima poliza de un crédito en cobro jurídico
        /// </summary>
        /// <param name="numDocCredito">Identificador único del crédito</param>
        /// <returns>Información de la póliza</returns>
        public DTO_ccPolizaEstado PolizaEstado_GetLastPoliza(int numDocCredito, int libranza)
        {
            this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_ccSolicitudDocu = (DAL_ccSolicitudDocu)base.GetInstance(typeof(DAL_ccSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_ccPolizaEstado poliza = this._dal_ccPolizaEstado.DAL_ccPolizaEstado_GetLastPoliza(numDocCredito);
            if(poliza != null)
            {
                //Trae la información de la renovación de póliza
                DTO_ccSolicitudDocu solRenovacionPoliza = this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_GetByRenovacionPoliza(libranza, poliza.Poliza.Value);
                if(solRenovacionPoliza != null)
                {
                    //Poliza
                    poliza.Solicitud.Value = solRenovacionPoliza.NumeroDoc.Value;
                }
            }

            return poliza;
        }

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        public List<DTO_ccCreditoDocu> GetCreditosForRenovacionPoliza(string cliente)
        {
            try
            {
                this._dal_ccCreditoDocu = (DAL_ccCreditoDocu)base.GetInstance(typeof(DAL_ccCreditoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ccCreditoDocu.DAL_ccCreditoDocu_GetCreditosForRenovacionPoliza(cliente);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetCreditosForRenovacionPoliza");
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Creación Créditos

        #region Funciones Privadas

        /// <summary>
        /// Realiza el proceso de liquidacion de cartera
        /// </summary>
        /// <param name="edad">Edad del cliente. No se usa en el simulador</param>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="plazo">Plazo de pago</param>
        /// <param name="valorPrestamo">Valor del prestamo</param>
        /// <param name="fechaLiquida">Fecha de liquidacion</param>
        /// <param name="traerCuotas">Indica si se debe incluir el plan de pagos</param>
        /// <param name="cuotasExtras">Lista de cuotas extras para le pago del crédito</param>
        /// <param name="porcInteres">Tasa de ineterés</param>
        /// /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        private DTO_SerializedObject CrearPlanPagosCredito(string lineaCredID, int valorSolicitado, int vlrGiro, int plazo, int edad,
            DateTime fechaLiquida, DateTime fechaCuota1, decimal porcInteres, List<DTO_Cuota> cuotasExtras, Dictionary<string, decimal> compsNuevoValor, string tipoCredito, bool excluyeCompInvisibleInd)
        {
            DTO_TxResult result = new DTO_TxResult();
            DTO_PlanDePagos planPagos = new DTO_PlanDePagos();

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                //Variables para resultados
                List<DTO_ccSolicitudComponentes> componentes = new List<DTO_ccSolicitudComponentes>();
                List<DTO_ccSolicitudComponentes> componentesUsuario = new List<DTO_ccSolicitudComponentes>(); 
                List<DTO_ccSolicitudComponentes> componentesAll = new List<DTO_ccSolicitudComponentes>();
                List<DTO_Cuota> cuotas = new List<DTO_Cuota>();

                //Variables de control
                string compCapital = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                string compInteres = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                string compInteresAnti = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresAnticipado);
                string compSeguroAnti = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroAnti);
                string compIntSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);
                string liquidaIntAnticipado = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_LiquiIntAntiVlrCredito);

                //Variables de calculos
                string ctaIVAID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaIVAComponentes);
                DTO_coPlanCuenta ctaIVA;
                decimal porcIVA = 0;
                int SMMLV = 0;
                decimal tasaTotal = 0;
                if (porcInteres != 0)
                    tasaTotal = porcInteres;

                decimal vlrDescuento = 0;
                decimal vlrAdicional = 0;
                decimal vlrCompra = 0;
                decimal sumInteres = 0;
                decimal vlrCredito = 0;
                int vlrCuotaReal = 0;
                int capitalTotal = 0;
                int saldoParcial = valorSolicitado;
                int ultimoDia = 1;
                //if (fechaLiquida.Month == 2)
                //    ultimoDia = 28;
                //else
                //    ultimoDia = 30;
                DateTime periodo = new DateTime(fechaLiquida.Year, fechaLiquida.Month, 1);

                //Calculo los dias de anticipo
                //if (fechaCuota1.Month == 2)
                //    fechaCuota1 = new DateTime(fechaCuota1.Year, fechaCuota1.Month, 28);
                //else
                //    fechaCuota1 = new DateTime(fechaCuota1.Year, fechaCuota1.Month, 30);

                DateTime fechaCuota = new DateTime(fechaCuota1.Year, fechaCuota1.Month, ultimoDia);
                DateTime fechaLiquidacion = fechaLiquida.Date;

                int diasAnticipo = 0;
                if (fechaCuota.Month == fechaLiquidacion.Month)
                    liquidaIntAnticipado = "0";
                else
                {
                    DateTime ultimoMes = fechaCuota1.AddMonths(-1);
                    DateTime fechaCalculoAnticipo = ultimoMes; //new DateTime(ultimoMes.Year, ultimoMes.Month, fechaCuota1.Day);
                    diasAnticipo = fechaCalculoAnticipo.Subtract(fechaLiquidacion).Days;
                }

                //Variables
                string descSeguro = string.Empty;
                DTO_MasterBasic basic;

                //Variables de cache
                DTO_ccCarteraComponente carteraComponenteDTO = new DTO_ccCarteraComponente();
                Dictionary<string, bool> dict_PagosAnticipados = new Dictionary<string, bool>();
                Dictionary<string, bool> dict_MayorValor = new Dictionary<string, bool>();
                Dictionary<string, decimal> dict_TasasComponentes = new Dictionary<string, decimal>();
                Dictionary<string, Int16> dict_TiposValor = new Dictionary<string, Int16>();
                Dictionary<string, bool> dict_ComponentesFijos = new Dictionary<string, bool>();
                Dictionary<string, bool> dict_FactorValorCredito = new Dictionary<string, bool>();
                Dictionary<string, bool> dict_SaldoPromediado = new Dictionary<string, bool>();
                Dictionary<string, int> dict_TotalComponentesSaldo = new Dictionary<string, int>();
                Dictionary<string, DTO_ccCarteraComponente> cacheCarteraComponentes = new Dictionary<string, DTO_ccCarteraComponente>();
                Dictionary<Tuple<string, string>, DTO_ccLineaComponente> cacheLineaCred = new Dictionary<Tuple<string, string>, DTO_ccLineaComponente>();
                Dictionary<Tuple<string, int>, DTO_ccComponenteEdad> cacheEdad = new Dictionary<Tuple<string, int>, DTO_ccComponenteEdad>();
                Dictionary<Tuple<string, string, int>, DTO_ccLineaComponenteMonto> cacheMontos = new Dictionary<Tuple<string, string, int>, DTO_ccLineaComponenteMonto>();
                Dictionary<Tuple<string, string, int, int>, DTO_ccLineaComponentePlazo> cachePlazos = new Dictionary<Tuple<string, string, int, int>, DTO_ccLineaComponentePlazo>();
                #endregion
                #region Validaciones

                #region Validaciones glControl
                if (string.IsNullOrWhiteSpace(compCapital))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ComponenteCapital + "&&" + string.Empty;
                    return result;
                }
                if (string.IsNullOrWhiteSpace(compInteres))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ComponenteInteresCorriente + "&&" + string.Empty;
                    return result;
                }
                #endregion
                #region Valida el SMMLV
                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDatosAnuales, periodo.Year.ToString(), true, false);
                if (basic == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidSMMLV;
                    return result;
                }
                else
                {
                    try
                    {
                        DTO_glDatosAnuales datos = (DTO_glDatosAnuales)basic;
                        SMMLV = Convert.ToInt32(datos.Valor11.Value.Value);
                    }
                    catch (Exception)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_InvalidSMMLV;
                        return result;
                    }
                }
                #endregion
                #region Valida la cuenta de IVA

                //Valida la cuenta de IVA
                if (string.IsNullOrWhiteSpace(ctaIVAID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CuentaIVAComponentes + "&&" + string.Empty;

                    return result;
                }

                ctaIVA = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaIVAID, true, false);
                porcIVA = ctaIVA.ImpuestoPorc.Value != null && ctaIVA.ImpuestoPorc.Value.HasValue ? ctaIVA.ImpuestoPorc.Value.Value : 0;

                #endregion

                #endregion
                #region Trae la lista de componentes
                int maxComp = 0;
                string maxCompID = string.Empty;
                List<DTO_ccCarteraComponente> comps = this._moduloGlobal.ccCarteraComponente_GetByLineaCredito(lineaCredID);
                DTO_ccTipoCredito dtoTipoCred = (DTO_ccTipoCredito)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, tipoCredito, true, false);
                #region Excluye Componentes por Tipo de Credito
                List<string> compBorrar = new List<string>();
                foreach (var c in comps)
                {
                    if (c.TipoCreditoInd.Value.Value && dtoTipoCred != null && !dtoTipoCred.CancelaGarInd.Value.Value)
                    {
                        compBorrar.Add(c.ID.Value);
                    }
                }
                foreach (string c in compBorrar)
                    comps.RemoveAll(x => x.ID.Value == c);        
                #endregion       
                #region Excluye Componentes de Mayor Valor (Invisibles)
                if (excluyeCompInvisibleInd)
                    comps = comps.FindAll(x => x.TipoComponente.Value.Value != (byte)TipoComponente.MayorValor).ToList();
             
                #endregion       
                foreach (DTO_ccCarteraComponente c in comps)
                {
                    if (c.ID.Value != compIntSeguro)
                    {
                        DTO_ccSolicitudComponentes anexo = new DTO_ccSolicitudComponentes();
                        anexo.ComponenteCarteraID.Value = c.ID.Value;
                        anexo.Descripcion.Value = c.Descriptivo.Value;
                        anexo.CuotaValor.Value = 0;
                        anexo.TotalValor.Value = 0;

                        componentes.Add(anexo);

                        //Revisa si es el máximo componente
                        if (maxComp == 0 || c.NumeroComp.Value.Value > maxComp)
                        {
                            maxComp = c.NumeroComp.Value.Value;
                            maxCompID = c.ID.Value;
                        }

                        DTO_ccCarteraComponente cT = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, c.ID.Value, true, false);
                        cacheCarteraComponentes.Add(c.ID.Value, cT);
                    }
                }
                #endregion
                #region Calcula los valores de todos los componentes
                int i = 1;
                bool isValid = true;
                result.Details = new List<DTO_TxResultDetail>();
                int vlrCuotaPrestamo = Evaluador.GetCuotaCreditoCartera(valorSolicitado, plazo, tasaTotal);
                decimal vlrBase = 0;

                #region Calcula los valores de los componentes de la linea del credito y agrega los componentes de pago anticipado (Interes, seguro y aportes)
                foreach (DTO_ccSolicitudComponentes comp in componentes)
                {
                    i++;
                    comp.CompInvisibleInd.Value = false;
                    if (comp.ComponenteCarteraID.Value != compCapital)
                    {
                        decimal tasaSaldo = 0;
                        decimal? tasaFija = null; 
                        if (comp.ComponenteCarteraID.Value == compInteres)
                            tasaFija = porcInteres;

                        bool reCalculaIntAnt = false;
                        DTO_TxResult resultLiquida = this._moduloCartera.LiquidarComponente(lineaCredID, comp, valorSolicitado, vlrGiro, plazo, edad, SMMLV, ref vlrBase,
                            out tasaSaldo, porcIVA, dict_PagosAnticipados, dict_MayorValor, dict_FactorValorCredito, dict_ComponentesFijos, dict_SaldoPromediado, 
                            dict_TasasComponentes, dict_TiposValor, cacheLineaCred, cacheEdad, cacheMontos, cachePlazos, diasAnticipo, tasaFija, null, ref reCalculaIntAnt);

                        //Valida si asigna el nuevo valor digitado del componente adicional
                        if (compsNuevoValor.ContainsKey(comp.ComponenteCarteraID.Value) && comp.TotalValor.Value.HasValue)
                        {
                            vlrBase -= comp.TotalValor.Value.Value;
                            comp.TotalValor.Value = compsNuevoValor[comp.ComponenteCarteraID.Value];
                            vlrBase += comp.TotalValor.Value.Value;
                        }

                        if (resultLiquida.Result == ResultValue.NOK)
                        {
                            isValid = false;
                            result.Result = ResultValue.NOK;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = i;
                            rd.Message = resultLiquida.ResultMessage;
                            result.Details.Add(rd);
                        }
                        else
                        {
                            if (tasaSaldo != 0 && vlrCuotaPrestamo == 0)
                                tasaTotal += tasaSaldo;

                            if (dict_PagosAnticipados[comp.ComponenteCarteraID.Value])
                            {
                                vlrDescuento += comp.TotalValor.Value.Value;
                                comp.CompInvisibleInd.Value = true;
                            }

                            if (dict_MayorValor[comp.ComponenteCarteraID.Value])
                            {
                                vlrAdicional += comp.TotalValor.Value.Value;
                                comp.CompInvisibleInd.Value = true;
                            }

                            if (cacheCarteraComponentes[comp.ComponenteCarteraID.Value].RecAnticipadoInd.Value.Value)
                            {

                                //Crea el nuevo componente
                                DTO_ccSolicitudComponentes cTemp = new DTO_ccSolicitudComponentes();
                                cTemp.CuotaValor.Value = 0;
                                cTemp.TotalValor.Value = comp.CuotaValor.Value;
                                cTemp.CompInvisibleInd.Value = true;

                                #region Componente Interes Anticipado
                                if (comp.ComponenteCarteraID.Value == compInteres && liquidaIntAnticipado == "1")
                                {
                                    //Valida que el componente existan en glControl
                                    if (string.IsNullOrWhiteSpace(compInteresAnti))
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ComponenteInteresAnticipado + "&&" + string.Empty;
                                        return result;
                                    }

                                    carteraComponenteDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, compInteresAnti, true, false);
                                    cTemp.ComponenteCarteraID.Value = carteraComponenteDTO.ID.Value;
                                    cTemp.Descripcion.Value = carteraComponenteDTO.Descriptivo.Value;
                                    componentesAll.Add(cTemp);
                                }
                                #endregion
                                #region Componente Seguro Anticipado
                                else if (comp.ComponenteCarteraID.Value == compSeguro)
                                {
                                    //Valida que el componente existan en glControl
                                    if (string.IsNullOrWhiteSpace(compSeguroAnti))
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ComponenteInteresAnticipado + "&&" + string.Empty;
                                        return result;
                                    }
                                    carteraComponenteDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, compSeguroAnti, true, false);
                                    cTemp.ComponenteCarteraID.Value = carteraComponenteDTO.ID.Value;
                                    cTemp.Descripcion.Value = carteraComponenteDTO.Descriptivo.Value;
                                    //cTemp.Porcentaje.Value = tasa;
                                    componentesAll.Add(cTemp);
                                }
                                #endregion
                            }
                        }
                    }
                }

                if (!isValid)
                    return result;

                #endregion
                #region Calcula los valores de los componentes anticipados
                foreach (DTO_ccSolicitudComponentes comp in componentesAll)
                {
                    i++;

                    decimal tasaSaldo = 0;
                    DTO_TxResult resultLiquida = null;
                    if (comp.ComponenteCarteraID.Value == compInteresAnti)
                    {
                        #region Calcula el interes anticipado (si existe)

                        //Calcula la tasa por defecto del interes anticipado
                        bool reCalculaIntAnt = false;
                        decimal interesCte = dict_TasasComponentes[compInteres];
                        resultLiquida = this._moduloCartera.LiquidarComponente(lineaCredID, comp, valorSolicitado, vlrGiro, plazo, edad, SMMLV, ref vlrBase, out tasaSaldo,
                            porcIVA, dict_PagosAnticipados, dict_MayorValor, dict_FactorValorCredito, dict_ComponentesFijos, dict_SaldoPromediado, dict_TasasComponentes, dict_TiposValor,
                            cacheLineaCred, cacheEdad, cacheMontos, cachePlazos, diasAnticipo, null, interesCte, ref reCalculaIntAnt, true);

                        if (resultLiquida.Result == ResultValue.OK && reCalculaIntAnt)
                        {
                            #region Re calcula el interes anticipado en base a el interes corriente

                            //La tasa de saldo es la del interes efectiva
                            decimal intAnticipado = 0;
                            decimal intEfectivo = dict_TasasComponentes[compInteresAnti] / 100;
                            if (dict_FactorValorCredito[compInteresAnti])
                            {
                                //Tasa Efectiva Mensual
                                intAnticipado = intEfectivo / (1 - intEfectivo);
                                intAnticipado = intAnticipado * (Convert.ToDecimal(diasAnticipo) / 30);
                            }
                            else
                            {
                                double iBase = Convert.ToDouble(1 + intEfectivo);
                                double exp = (double)diasAnticipo / 30;
                                decimal intVencido = Convert.ToDecimal(Math.Pow(iBase, exp)) - 1;
                                intAnticipado = intVencido / (1 - intVencido);
                            }

                            //Calcula el valor del componente
                            intAnticipado *= 100;
                            resultLiquida = this._moduloCartera.LiquidarComponente(lineaCredID, comp, valorSolicitado, vlrGiro, plazo, edad, SMMLV, ref vlrBase, out tasaSaldo,
                                porcIVA, dict_PagosAnticipados, dict_MayorValor, dict_FactorValorCredito, dict_ComponentesFijos, dict_SaldoPromediado, dict_TasasComponentes, 
                                dict_TiposValor, cacheLineaCred, cacheEdad, cacheMontos, cachePlazos, diasAnticipo, null, intAnticipado, ref reCalculaIntAnt, true);
                          
                            #endregion
                        }

                        if (resultLiquida.Result == ResultValue.NOK)
                        {
                            result.Result = ResultValue.NOK;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = i;
                            rd.Message = resultLiquida.ResultMessage;
                            result.Details.Add(rd);

                            return result;
                        }
                        #endregion
                    }
                    else
                    {
                        bool reCalculaIntAnt = false;
                        resultLiquida = this._moduloCartera.LiquidarComponente(lineaCredID, comp, valorSolicitado, vlrGiro, plazo, edad, SMMLV, ref vlrBase, out tasaSaldo,
                            porcIVA, dict_PagosAnticipados, dict_MayorValor, dict_FactorValorCredito, dict_ComponentesFijos, dict_SaldoPromediado, dict_TasasComponentes, 
                            dict_TiposValor, cacheLineaCred, cacheEdad, cacheMontos, cachePlazos, diasAnticipo, null, null, ref reCalculaIntAnt);
                    }

                    if (resultLiquida.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;

                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.line = i;
                        rd.Message = resultLiquida.ResultMessage;
                        result.Details.Add(rd);

                        return result;
                    }

                    if (tasaSaldo != 0 && vlrCuotaPrestamo == 0)
                        tasaTotal += tasaSaldo;

                    if (dict_PagosAnticipados[comp.ComponenteCarteraID.Value])
                        vlrDescuento += comp.TotalValor.Value.Value;

                    if (dict_MayorValor[comp.ComponenteCarteraID.Value])
                        vlrAdicional += comp.TotalValor.Value.Value;
                }
                #endregion
                //Agrega los componentes de la linea del credito a la lista de los nuevos componentes
                componentesAll.AddRange(componentes);
                #endregion
                #region Calcula el porcentaje de interes total
                foreach (DTO_ccSolicitudComponentes comp in componentesAll)
                {
                    if (comp.ComponenteCarteraID.Value != compCapital)
                    {
                        comp.Porcentaje.Value = dict_TasasComponentes[comp.ComponenteCarteraID.Value] / 100;
                        if (dict_MayorValor[comp.ComponenteCarteraID.Value] && dict_FactorValorCredito[comp.ComponenteCarteraID.Value])
                            sumInteres += dict_TasasComponentes[comp.ComponenteCarteraID.Value] / 100;
                    }
                }
                #endregion
                #region Calcula el valor del crédito
                vlrBase = vlrBase + valorSolicitado;
                vlrCredito = Math.Round(vlrBase / (1 - sumInteres));
                #endregion
                #region Asigna los valores a los componentes de mayor valor y factor valor de crédito
                //Asigna el componente fijo de resta al final
                DTO_ccSolicitudComponentes compMax_DTO = componentesAll.Where(x => x.ComponenteCarteraID.Value == maxCompID).First();
                componentesAll.Remove(compMax_DTO);
                componentesAll.Add(compMax_DTO);

                decimal vlrTemp = 0;
                bool found = false;
                foreach (DTO_ccSolicitudComponentes comp in componentesAll)
                {
                    if (comp.ComponenteCarteraID.Value != compCapital && dict_FactorValorCredito[comp.ComponenteCarteraID.Value])
                    {
                        if (dict_MayorValor[comp.ComponenteCarteraID.Value])
                        {
                            #region Valores anticipados
                            if (maxCompID == comp.ComponenteCarteraID.Value)
                            {
                                if (found)
                                    comp.TotalValor.Value = vlrCredito - vlrBase - vlrTemp;
                                else
                                    comp.TotalValor.Value = Math.Round(vlrCredito * comp.Porcentaje.Value.Value);
                            }
                            else
                            {
                                found = true;
                                comp.TotalValor.Value = Math.Round(vlrCredito * comp.Porcentaje.Value.Value);
                                vlrTemp += comp.TotalValor.Value.Value;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Valor credito
                            decimal valorComp = Math.Round(vlrCredito * comp.Porcentaje.Value.Value);
                            if (dict_TiposValor[comp.ComponenteCarteraID.Value] == (Int16)TipoValor.Cuota)
                            {
                                comp.CuotaValor.Value = valorComp;
                                comp.TotalValor.Value = valorComp * plazo;
                            }
                            else if (dict_TiposValor[comp.ComponenteCarteraID.Value] == (Int16)TipoValor.Valor)
                            {
                                comp.CuotaValor.Value = valorComp / plazo;
                                comp.TotalValor.Value = valorComp;
                            }
                            else
                                comp.TotalValor.Value = valorComp;
                            #endregion
                        }
                    }
                }
                #endregion
                #region Calcula el valor de la cuota

                int vlrAd = Convert.ToInt32(vlrCredito);
                saldoParcial = vlrAd;
                if (vlrCuotaPrestamo == 0 || vlrAd != valorSolicitado)
                    vlrCuotaReal = Evaluador.GetCuotaCreditoCartera(vlrAd, plazo, tasaTotal);
                else
                    vlrCuotaReal = vlrCuotaPrestamo;

                #endregion
                #region Carga la info de las cuotas extras

                int numCuota = 0;
                int cuotasExtrasNum = cuotasExtras.Sum(x => x.NumCuota);
                decimal sumExtra = cuotasExtras.Sum(x => x.ValorCuota);
                decimal sumTotal = vlrCuotaReal * plazo;
                List<int> vlrCuotas = new List<int>();
                foreach (DTO_Cuota cuota in cuotasExtras)
                {
                    for (int j = 0; j < cuota.NumCuota; ++j)
                    {
                        vlrCuotas.Add(cuota.ValorCuota);
                        sumTotal -= cuota.ValorCuota;
                        numCuota++;
                    }
                }

                int cuotasFaltantes = plazo - numCuota;
                if (cuotasFaltantes > 0 && cuotasExtrasNum == 0)
                {
                    int vlrCuotaAjuste = cuotasExtras.Count > 0 ? Convert.ToInt32((sumTotal - sumExtra) / cuotasFaltantes) : vlrCuotaReal;
                    for (int j = numCuota; j < plazo; ++j)
                    {
                        vlrCuotas.Add(vlrCuotaAjuste);
                        numCuota++;
                    }
                }

                #endregion
                #region Llena los valores de las cuotas

                if (vlrCuotaReal != 0)
                {
                    for (i = 1; i <= plazo; ++i)
                    {
                        int vlrCuota = vlrCuotas[i - 1];

                        int saldoXCapital = 0;
                        decimal vlrRadAnticipo = 0;
                        DTO_Cuota cuota = new DTO_Cuota();
                        List<string> componentesCuota = new List<string>();
                        List<int> valoresComponentesCuota = new List<int>();

                        cuota.NumCuota = i;
                        #region Calculo del capital para la ultima cuota
                        if (i == plazo)
                        {
                            cuota.Capital = saldoParcial;
                            capitalTotal += cuota.Capital;
                        }
                        #endregion
                        #region Calculo del valor de todas las cuotas excepto la ultima
                        if (i != plazo)
                        {
                            foreach (DTO_ccSolicitudComponentes comp in componentes)
                            {
                                DTO_ccCarteraComponente dto = cacheCarteraComponentes[comp.ComponenteCarteraID.Value];
                                if (comp.ComponenteCarteraID.Value != compCapital &&
                                    !dict_PagosAnticipados[comp.ComponenteCarteraID.Value] &&
                                    !dict_MayorValor[comp.ComponenteCarteraID.Value])
                                {
                                    int temp = 0;
                                    decimal tasa = dict_TasasComponentes[comp.ComponenteCarteraID.Value];
                                    #region Asigna los valores de la cuota
                                    if (comp.ComponenteCarteraID.Value == compInteres)
                                    {
                                        #region Intereses
                                        cuota.Intereses = Convert.ToInt32(tasa * saldoParcial / 100);
                                        saldoXCapital += cuota.Intereses;
                                        temp = cuota.Intereses;
                                        #endregion
                                    }
                                    else if (comp.ComponenteCarteraID.Value == compSeguro)
                                    {
                                        #region Seguro
                                        descSeguro = comp.Descripcion.Value;
                                        if (!dict_ComponentesFijos[comp.ComponenteCarteraID.Value])
                                        {
                                            cuota.Seguro = Convert.ToInt32(tasa * saldoParcial / 100);
                                            saldoXCapital += cuota.Seguro;
                                        }
                                        else
                                            cuota.Seguro = (int)comp.CuotaValor.Value.Value;

                                        temp = cuota.Seguro;
                                        #endregion
                                    }
                                    else if (!dict_ComponentesFijos[comp.ComponenteCarteraID.Value])
                                    {
                                        #region Componentes Variables
                                        componentesCuota.Add(comp.Descripcion.Value);
                                        valoresComponentesCuota.Add(Convert.ToInt32(tasa * saldoParcial / 100));
                                        saldoXCapital += Convert.ToInt32(tasa * saldoParcial / 100);
                                        temp = Convert.ToInt32(tasa * saldoParcial / 100);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Componentes fijos
                                        componentesCuota.Add(comp.Descripcion.Value);
                                        if(dto.TipoLiquida.Value == (byte)TipoLiquidacionCartera.FactorSaldoPromediado)
                                        {
                                            valoresComponentesCuota.Add(Convert.ToInt32(tasa * saldoParcial / 100));
                                            temp = Convert.ToInt32(tasa * saldoParcial / 100);
                                        }                                         
                                        else
                                            valoresComponentesCuota.Add(Convert.ToInt32(comp.CuotaValor.Value.Value));
                                        #endregion
                                    }
                                    #endregion
                                    #region Calculo de saldos y valores de componentes fijos y variables
                                    if (!dict_ComponentesFijos[comp.ComponenteCarteraID.Value])
                                    {
                                        if (i == 1)
                                            dict_TotalComponentesSaldo.Add(comp.ComponenteCarteraID.Value, temp);
                                        else
                                            dict_TotalComponentesSaldo[comp.ComponenteCarteraID.Value] += temp;
                                    }
                                    else
                                    {
                                        if (dto.TipoLiquida.Value == (byte)TipoLiquidacionCartera.FactorSaldoPromediado)
                                        {
                                            if (i == 1)
                                                dict_TotalComponentesSaldo.Add(comp.ComponenteCarteraID.Value, temp);
                                            else
                                                dict_TotalComponentesSaldo[comp.ComponenteCarteraID.Value] += temp;
                                        }
                                    }
                                    #endregion
                                    #region Revisa si es un componente con codigo indicador de anticipo radicado
                                    DTO_ccCarteraComponente c = cacheCarteraComponentes[comp.ComponenteCarteraID.Value];
                                    if (c.RecAnticipadoInd.Value.Value)
                                        vlrRadAnticipo += comp.TotalValor.Value.Value;
                                    #endregion
                                }
                            }
                        }

                        #endregion
                        #region Calculo el valor de los componentes para la ultima cuota

                        if (i == plazo)
                        {
                            //Asigna la info del seguro
                            decimal tasa = dict_TasasComponentes[compSeguro];
                            DTO_ccSolicitudComponentes comp = componentes.Where(x => x.ComponenteCarteraID.Value == compSeguro).First();
                            if (!dict_ComponentesFijos[compSeguro])
                            {
                                cuota.Seguro = Convert.ToInt32(tasa * saldoParcial / 100);
                                dict_TotalComponentesSaldo[compSeguro] += cuota.Seguro;
                            }
                            else
                                cuota.Seguro = (int)comp.CuotaValor.Value.Value;

                            componentesUsuario.Insert(0, comp);

                            //Componentes Variables
                            int vlrVariableFinal = 0;
                            #region Asigna los valores de la cuota
                            foreach (string tComp in dict_ComponentesFijos.Keys)
                            {
                                if (tComp != compSeguro && tComp != compInteres && tComp != compCapital && !dict_MayorValor[tComp] && !dict_PagosAnticipados[tComp])
                                {
                                    comp = componentesAll.Where(x => x.ComponenteCarteraID.Value == tComp).First();
                                    if (!dict_ComponentesFijos[tComp])
                                    {
                                        componentesCuota.Add(comp.Descripcion.Value);
                                        int cTemp = Convert.ToInt32(dict_TasasComponentes[tComp] * saldoParcial / 100);
                                        valoresComponentesCuota.Add(cTemp);
                                        dict_TotalComponentesSaldo[tComp] += cTemp;

                                        vlrVariableFinal += cTemp;
                                    }
                                    else
                                    {
                                        componentesCuota.Add(comp.Descripcion.Value);
                                        valoresComponentesCuota.Add(Convert.ToInt32(comp.CuotaValor.Value.Value));
                                    }
                                    componentesUsuario.Add(comp);
                                }
                            }
                            #endregion

                            //Asigna la info del interes
                            if (dict_ComponentesFijos[compSeguro])
                                cuota.Intereses = vlrCuota - saldoParcial - vlrVariableFinal;
                            else
                                cuota.Intereses = vlrCuota - saldoParcial - cuota.Seguro - vlrVariableFinal;

                            dict_TotalComponentesSaldo[compInteres] += cuota.Intereses;
                            comp = componentes.Where(x => x.ComponenteCarteraID.Value == compInteres).First();
                            componentesUsuario.Insert(1, comp);
                        }

                        #endregion
                        #region Calculo del capital para todas las cuotas menos la ùltima
                        if (i != plazo)
                        {
                            // Asignan los valores de los componentes de anticipo
                            cuota.Capital = vlrCuota - saldoXCapital;
                            capitalTotal += cuota.Capital;
                        }
                        #endregion
                        #region Asigna la fecha de la cuota
                        if (i == 1)
                        {
                            #region Cuota1
                            fechaCuota = fechaCuota1;
                            cuota.Fecha = fechaCuota;
                            #endregion
                        }
                        else
                        {
                            //if (fechaCuota.Month == 2)
                            //{
                            //    cuota.Fecha = fechaCuota.AddMonths(i - 1);
                            //    if (cuota.Fecha.Month != 2)
                            //        cuota.Fecha = cuota.Fecha.AddDays(2);
                            //}
                            //else
                                cuota.Fecha = fechaCuota.AddMonths(i - 1);
                        }
                        #endregion

                        cuota.Componentes = componentesCuota;
                        cuota.ValoresComponentes = valoresComponentesCuota;

                        if (vlrCuota != 0 && (cuota.Capital <= 0 || cuota.Intereses <= 0))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Cc_PlanPagosNeg;
                            return result;
                        }

                        cuota.ValorCuota = (from c in valoresComponentesCuota select c).Sum() + cuota.Capital + cuota.Intereses + cuota.Seguro;
                        cuotas.Add(cuota);

                        saldoParcial -= cuota.Capital;
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cc_Cuota0;
                    return result;
                }

                #endregion
                #region Agrega el capital y calcula los valores de los saldos
                foreach (DTO_ccSolicitudComponentes comp in componentes)
                {
                    DTO_ccCarteraComponente dto = cacheCarteraComponentes[comp.ComponenteCarteraID.Value];
                    if (comp.ComponenteCarteraID.Value == compCapital)
                    {
                        comp.TotalValor.Value = capitalTotal;
                        comp.CuotaValor.Value = Convert.ToInt32(comp.TotalValor.Value.Value / plazo);
                        componentesUsuario.Insert(0, comp);
                    }
                    else if (!dict_MayorValor[comp.ComponenteCarteraID.Value])
                    {
                        if (comp.ComponenteCarteraID.Value == compInteres || !dict_ComponentesFijos[comp.ComponenteCarteraID.Value])
                        {
                            comp.TotalValor.Value = dict_TotalComponentesSaldo[comp.ComponenteCarteraID.Value];
                            comp.CuotaValor.Value = Convert.ToInt32(comp.TotalValor.Value.Value / plazo);
                            comp.Porcentaje.Value = dict_TasasComponentes[comp.ComponenteCarteraID.Value];
                        }
                        else if (dict_ComponentesFijos[comp.ComponenteCarteraID.Value] && dto.TipoLiquida.Value == (byte)TipoLiquidacionCartera.FactorSaldoPromediado) //Promedia valor Cuota para componentes de Calculo Promedio de Saldo
                        {
                            comp.TotalValor.Value = dict_TotalComponentesSaldo[comp.ComponenteCarteraID.Value];
                            comp.CuotaValor.Value = Math.Round(comp.TotalValor.Value.Value / plazo, 0);
                            // Recalcula Valores de Cuota para componentes que se promedian por saldos Capital
                            comp.TotalValor.Value = comp.CuotaValor.Value * plazo; //Se reasigna el valor por redondeo anterior
                            foreach (DTO_Cuota cuota in cuotas)
                            {
                                for (int j = 0; j < cuota.Componentes.Count; j++)
                                {
                                    if (cuota.Componentes[j].Equals(dto.Descriptivo.Value))
                                    {
                                        cuota.ValoresComponentes[j] = (int)comp.CuotaValor.Value;
                                    }
                                }
                                cuota.ValorCuota = (from c in cuota.ValoresComponentes select c).Sum() + cuota.Capital + cuota.Intereses + cuota.Seguro;
                            }
                        }
                    }                  
                }
                #endregion                
                #region Asigna las listas al plan de pagos
                //Organiza los componentes de usuario por componente de cartera
                dict_ComponentesFijos[compSeguro] = false;
                planPagos.ComponentesFijos = dict_ComponentesFijos;
                planPagos.Tasas = dict_TasasComponentes;
                planPagos.ComponentesAll = (from c in componentesAll orderby c.ComponenteCarteraID.Value select c).ToList();
                planPagos.ComponentesUsuario = (from c in componentesUsuario orderby c.ComponenteCarteraID.Value select c).ToList();
                planPagos.Cuotas = cuotas;
                planPagos.CuotasExtras = cuotasExtras;

                decimal cuotaLibranza = 0;
                foreach (DTO_ccSolicitudComponentes c in planPagos.ComponentesUsuario)
                    cuotaLibranza += c.TotalValor.Value.Value;
                #endregion
                #region Asigna Valores calculados de Campos Extra
                planPagos.VlrAdicional = Convert.ToInt32(vlrCredito - valorSolicitado);
                planPagos.VlrDescuento = Convert.ToInt32(vlrDescuento);
                planPagos.VlrPrestamo = (int)vlrCredito;
                planPagos.VlrCompra = Convert.ToInt32(vlrCompra);
                planPagos.VlrGiro = valorSolicitado - planPagos.VlrCompra - planPagos.VlrDescuento;
                planPagos.VlrLibranza = Convert.ToInt32(cuotaLibranza);
                planPagos.VlrCuota = Convert.ToInt32(planPagos.VlrLibranza / plazo);
                planPagos.TasaTotal = tasaTotal;
                #endregion

                return planPagos;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetLiquidacionCartera");
                return result;
            }
        }

        /// <summary>
        /// Si el campo observacion en el cabezote tiene texto agrega en glDocumentoControl la nueva
        ///observacion guardando el historial de la misma.
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="sol"></param>
        /// <param name="docCtrl"></param>
        private DTO_TxResult AprobarSolicitud(int documentID, string actFlujoID, DTO_ccSolicitudDocu sol, List<DTO_ccSolicitudAnexo> anexos, bool insideAnotherTx)
        {
            //if (!insideAnotherTx)
            //    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_ccPolizaEstado = (DAL_ccPolizaEstado)base.GetInstance(typeof(DAL_ccPolizaEstado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_TxResult result = new DTO_TxResult();
            try
            {
                #region Variables

                string conceptoCXPID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConceptoCxPCredito);
                DTO_coDocumento coDoc = new DTO_coDocumento();
                string coDocID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoDocumentoCredito);
                string ctaBalanceID;
                string numDocCredito;

                #endregion
                #region Validaciones
        
                //Valida el concepto de CxP
                if (string.IsNullOrWhiteSpace(conceptoCXPID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ConceptoCxPCredito + "&&" + string.Empty;

                    return result;
                }

                //Valida el documento para las contrapartidas de las cuemtas de balance
                if (string.IsNullOrWhiteSpace(coDocID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CodigoDocumentoCredito + "&&" + string.Empty;

                    return result;
                }
                else
                    coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocID, false, false);

                //Valida que el documento asociado tenga cuenta local
                if (string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDocID;

                    return result;
                }

                coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocID, true, false);
                ctaBalanceID = coDoc.CuentaLOC.Value;
                #endregion
                #region Genera la liquidación

                result = this._moduloCartera.LiquidacionCredito_Aprobar(documentID, actFlujoID, sol, false);
                if (result.Result == ResultValue.NOK)
                    return result;

                numDocCredito = result.ExtraField;
                #endregion
                #region Genera la aprobacion de giro

                List<DTO_ccCreditoDocu> creds = new List<DTO_ccCreditoDocu>();
                DTO_ccCreditoDocu cred = this._moduloCartera.GetCreditoByNumeroDoc(Convert.ToInt32(numDocCredito));
                creds.Add(cred);

                if (cred.VlrCapital.Value.HasValue && cred.VlrCapital.Value != 0)
                {
                    List<string> actividadGiro = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.AprobacionGiros);
                    result = this._moduloCartera.Giro_Credito_Aprobar(AppDocuments.AprobacionGiros, actividadGiro[0], creds, ctaBalanceID, conceptoCXPID, false, false);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }

                #endregion
                #region Actualiza la póliza

                if (!string.IsNullOrWhiteSpace(cred.Poliza.Value))
                {                  
                    //ccPolizaEstado
                    DTO_ccPolizaEstado polizaEst = this._dal_ccPolizaEstado.DAL_ccPolizaEstado_GetForSolicitud(sol.ClienteID.Value, sol.NumeroDoc.Value.Value);
                    polizaEst.NumDocCredito.Value = cred.NumeroDoc.Value;

                    if (!polizaEst.NumeroDocLiquida.Value.HasValue)
                        polizaEst.NumeroDocLiquida.Value = cred.NumeroDoc.Value;

                    this.PolizaEstado_Upd(polizaEst, false);
                }

                #endregion

                result.ExtraField = numDocCredito;
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Viablidad_Aprobar");
                return result;
            }
            //finally
            //{
            //    if (result.Result == ResultValue.OK)
            //    {
            //        if (!insideAnotherTx)
            //        {
            //            this._mySqlConnectionTx.Commit();
            //        }
            //    }
            //    else if (base._mySqlConnectionTx != null && !insideAnotherTx)
            //        this._mySqlConnectionTx.Rollback();
            //}
        }

        /// <summary>
        /// Si el campo observacion en el cabezote tiene texto agrega en glDocumentoControl la nueva
        ///observacion guardando el historial de la misma.
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="sol"></param>
        /// <param name="docCtrl"></param>
        private DTO_TxResult RechazarSolicitud(int documentID, string actFlujoID, DTO_ccSolicitudDocu sol, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_ccSolicitudDocu = (DAL_ccSolicitudDocu)base.GetInstance(typeof(DAL_ccSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                #region Actualiza la observacion del glDocumentoCtrl
                
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(sol.NumeroDoc.Value.Value);
                ctrl.Observacion.Value = sol.Observacion.Value;
                this._moduloGlobal.glDocumentoControl_Update(ctrl, false, true);

                #endregion
                #region Actualiza el / los flujo
                //Trae la actividad inicial del flujo(Primer paso del flujo)
                List<string> actividadInicial = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.SolicitudLibranza);
                string actInicial = actividadInicial[0].Trim();

                //Digitación Crédito
                List<string> actividades = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.DigitacionCreditoFinanciera);
                string actDigCredito = actividades[0].Trim();

                if (sol.ActividadFlujoReversion.Value == actInicial)
                {
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, sol.Observacion.Value, true);
                    result = this.ActualizarReversionFlujo(documentID, sol.NumeroDoc.Value.Value, actFlujoID, sol.ActividadFlujoReversion.Value, sol.Observacion.Value);
                }
                if (!string.IsNullOrEmpty(sol.ActividadFlujoReversion.Value))
                    result = this.ActualizarReversionFlujo(documentID, sol.NumeroDoc.Value.Value, actFlujoID, sol.ActividadFlujoReversion.Value, sol.Observacion.Value);
                else
                    result = this.AsignarFlujo(documentID, sol.NumeroDoc.Value.Value, actDigCredito, true, sol.Observacion.Value);

                if (result.Result == ResultValue.NOK)
                    return result;

                ////Flujo de la aprobacion
                //result = this.AsignarFlujo(documentID, sol.NumeroDoc.Value.Value, actFlujoID, true, sol.Observacion.Value);
                //if (result.Result == ResultValue.NOK)
                //    return result;

                //Estado documento
                //this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.Cerrado, sol.Observacion.Value, true);

                #endregion
                #region Actualiza la solicitud
                if (string.IsNullOrWhiteSpace(sol.Observacion.Value))
                {
                    sol.Observacion.Value = sol.ObservacionRechazo.Value;
                    this._dal_ccSolicitudDocu.DAL_ccSolicitudDocu_Update(sol);
                }

                #endregion
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudCredito_Rechazar");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Renueva una póliza
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <ctrlCredito>Documento control relacionado con el crédito</ctrlCredito>
        /// <creditoPoliza>Credito con las actualizaciones de la poliza</creditoPoliza>
        /// <cuota1Pol>Primera cuota del crédito asignado a la poliza</cuota1Pol>
        /// <cuotasPoliza>Plan de pagos por cada cuota para la poliza</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        private DTO_TxResult Aprobar_RenovacionPoliza(int documentID, DTO_ccSolicitudDocu sol, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            DTO_Comprobante comprobante = null;
            DTO_coComprobante comp = null;
            try
            {
                this._dal_ccCreditoDocu = (DAL_ccCreditoDocu)base.GetInstance(typeof(DAL_ccCreditoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPlanPagos = (DAL_ccCreditoPlanPagos)base.GetInstance(typeof(DAL_ccCreditoPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccSolicitudPlanPagos = (DAL_ccSolicitudPlanPagos)base.GetInstance(typeof(DAL_ccSolicitudPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCartera = (ModuloCartera)base.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                DTO_coDocumento coDoc = new DTO_coDocumento();
                string coDocID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DocContableRenovacionPol);
                string ctaContraOrden = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaOrdenContraCarterPropia);
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                string compIntSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);

                ctrl = this._moduloGlobal.glDocumentoControl_GetByID(sol.NumeroDoc.Value.Value);
                DTO_ccCreditoDocu credito = this._moduloCartera.GetCreditoByNumeroDoc(ctrl.DocumentoPadre.Value.Value);
                DTO_ccPolizaEstado poliza = this.PolizaEstado_GetLastPoliza(ctrl.DocumentoPadre.Value.Value, credito.Libranza.Value.Value);
                DTO_ccLineaCredito lineaCred = (DTO_ccLineaCredito)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccLineaCredito, sol.LineaCreditoID.Value, true, false);
                List<DTO_ccCreditoPlanPagos> creditoPlanPagos = this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_GetByNumDoc(ctrl.DocumentoPadre.Value.Value);
                List<DTO_ccSolicitudPlanPagos> solicitidPlanPagos = this._dal_ccSolicitudPlanPagos.DAL_ccSolicitudPlanPagos_GetByNumDoc(sol.NumeroDoc.Value.Value);

                #endregion
                #region Validaciones

                //Valida el coDocumento
                if (string.IsNullOrWhiteSpace(coDocID))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CodigoDocumentoCredito + "&&" + string.Empty;

                    return result;
                }
                else
                    coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocID, true, false);

                //Valida que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;

                    return result;
                }

                //Valida que el documento asociado tenga cuenta local
                if (string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDocID;

                    return result;
                }

                //Valida la cuenta de contraP
                if (string.IsNullOrWhiteSpace(ctaContraOrden))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_CuentaOrdenContraCarterPropia + "&&" + string.Empty;

                    return result;
                }

                #endregion
                #region Actualiza el crédito

                //Poliza
                credito.Poliza.Value = sol.Poliza.Value;
                credito.PlazoSeguro.Value = sol.PlazoSeguro.Value;
                credito.VlrPoliza.Value = sol.VlrPoliza.Value;
                credito.FechaLiqSeguro.Value = sol.FechaLiqSeguro.Value;
                credito.FechaVigenciaINI.Value = sol.FechaVigenciaINI.Value;
                credito.FechaVigenciaFIN.Value = sol.FechaVigenciaFIN.Value;
                credito.PorSeguro.Value = sol.PorSeguro.Value;
                credito.Cuota1Seguro.Value = sol.Cuota1Seguro.Value;
                credito.VlrCuotaSeguro.Value = sol.VlrCuotaSeguro.Value;
                credito.VlrFinanciaSeguro.Value = sol.VlrFinanciaSeguro.Value;

                this._dal_ccCreditoDocu.DAL_ccCreditoDocu_Update(credito);
                #endregion
                #region Asigna el nuevo flujo y actualiza el estado de la solicitud

                if (documentID == AppDocuments.AprobacionSolicitudFin)
                {
                    List<string> actividades = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                    this.DeshabilitarAlarma(sol.NumeroDoc.Value.Value, actividades[0]);
                }

                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, sol.Observacion.Value, true);
                
                //Actualiza el documentoID deol documento control
                ctrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                ctrl.ComprobanteIDNro.Value = 0;
                ctrl.DocumentoID.Value = AppDocuments.RenovacionPoliza;
                ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                this._moduloGlobal.glDocumentoControl_Update(ctrl, false, true);

                #endregion
                #region Actualiza el plan de pagos del credito con la informacion de la nueva poliza

                int cuota1Pol = poliza.Cuota1Financia.Value.Value;
                decimal vlrFinancia = sol.VlrFinanciaSeguro.Value.Value;
                decimal vlrSeguroTotal = 0;
                decimal vlrIntSeguroTotal = 0;
                for (int i = 0; i < poliza.PlazoFinancia.Value.Value; ++i)
                {
                    DTO_ccSolicitudPlanPagos cuota = solicitidPlanPagos[i];

                    creditoPlanPagos[i + cuota1Pol - 1].VlrSeguro.Value = cuota.VlrCapital.Value;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrOtro1.Value += cuota.VlrInteres.Value;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrSaldoSeguro.Value = vlrFinancia;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrCuota.Value += (cuota.VlrCapital.Value + cuota.VlrInteres.Value);

                    this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_Update(creditoPlanPagos[i + cuota1Pol - 1]);

                    vlrFinancia -= cuota.VlrCapital.Value.Value;
                    //Acumula el valor total
                    vlrSeguroTotal += cuota.VlrCapital.Value.Value;
                    vlrIntSeguroTotal += cuota.VlrInteres.Value.Value;
                }

                #endregion
                #region Carga el historico de polizas

                DTO_ccCliente cliente = (DTO_ccCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, credito.ClienteID.Value, true, false);

                poliza.TerceroID.Value = cliente.TerceroID.Value;
                poliza.NumDocCredito.Value = credito.NumeroDoc.Value;
                poliza.NumeroDocLiquida.Value = ctrl.NumeroDoc.Value;

                this.PolizaEstado_Upd(poliza, true);

                #endregion
                #region Genera el comprobante relacionado

                decimal interesPoliza = solicitidPlanPagos.Sum(x => x.VlrInteres.Value.Value);
                DTO_ccAsesor asesor = (DTO_ccAsesor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAsesor, credito.AsesorID.Value, true, false);
                object compTemp = this.GenerarComprobanteRenovacionPoliza(ctrl, lineaCred.ClaseCredito.Value, coDoc, sol.Poliza.Value, ctaContraOrden, asesor.TerceroID.Value,
                    credito.VlrFinanciaSeguro.Value.Value, credito.VlrPoliza.Value.Value, interesPoliza, true);

                if (compTemp.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)compTemp;
                    return result;
                }
                else
                {
                    comprobante = (DTO_Comprobante)compTemp;
                    comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobante.Header.ComprobanteID.Value, true, false);
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, comprobante, ctrl.PeriodoDoc.Value.Value, ModulesPrefix.cc, 0, false);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }

                #endregion
                #region Agrega a CarteraMVto(Seguro - IntSeguro)
                DTO_ccCarteraMvto mvto = new DTO_ccCarteraMvto();
                mvto.NumCredito.Value = poliza.NumDocCredito.Value;
                mvto.NumeroDoc.Value = sol.NumeroDoc.Value.Value;
                mvto.ComponenteCarteraID.Value = compSeguro;
                mvto.Tasa.Value = 0;
                mvto.VlrAbono.Value = 0;
                mvto.VlrComponente.Value = vlrSeguroTotal;
                this._moduloCartera.ccCarteraMvto_Add(mvto);

                //Int Seguro
                mvto = new DTO_ccCarteraMvto();
                mvto.NumCredito.Value = poliza.NumDocCredito.Value;
                mvto.NumeroDoc.Value = sol.NumeroDoc.Value.Value;
                mvto.ComponenteCarteraID.Value = compIntSeguro;
                mvto.Tasa.Value = 0;
                mvto.VlrAbono.Value = 0;
                mvto.VlrComponente.Value = vlrIntSeguroTotal;
                this._moduloCartera.ccCarteraMvto_Add(mvto);
                #endregion
                
                if (result.Result == ResultValue.OK)
                    result.ResultMessage = string.Empty;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DigitacionCredito_Add");
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

                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(ctrl, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                    else if (documentID != AppDocuments.AprobacionSolicitudFin)
                        throw new Exception("ContabilizaLiquidacion - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Si el campo observacion en el cabezote tiene texto agrega en glDocumentoControl la nueva
        ///observacion guardando el historial de la misma.
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="sol"></param>
        /// <param name="docCtrl"></param>
        private DTO_TxResult Rechazar_RenovacionPoliza(int documentID, string actFlujoID, DTO_ccSolicitudDocu sol, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                result = this.RechazarSolicitud(documentID, actFlujoID, sol, true);
                if (result.Result == ResultValue.NOK)
                    return result;

                DTO_ccPolizaEstado poliza = this.PolizaEstado_GetLastPoliza(sol.NumeroDoc.Value.Value, sol.Libranza.Value.Value);
                poliza.AnuladaIND.Value = true;
                this.PolizaEstado_Upd(poliza, true);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Rechazar_RenovacionPoliza");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Realiza el proceso de liquidacion de cartera financiera
        /// </summary>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="valorCredito">Valor solicitado para el credito</param>
        /// <param name="valorPoliza">Valor solicitado para la poliza</param>
        /// <param name="vlrGiro">Valor a girar</param>
        /// <param name="plazoCredito">Plazo del credito solicitado</param>
        /// <param name="plazoPoliza">PLazo de la poliza</param>
        /// <param name="edad"> Edad de la persona que solicita el credito</param>
        /// <param name="fechaLiquida">Fecha en la cual se solicita el credito</param>
        /// <param name="fechaCuota1">Fecha de la primera cuota</param>
        /// <param name="interesCredito">Tasa de interes que se aplica para el credito</param>
        /// <param name="interesPoliza">Tasa de interes que se aplica a la poliza</param>
        /// <param name="cta1Poliza">Cuota en la que debe empezar la poliza</param>
        /// <param name="liquidaAll">Indica si debe liquidar crédito y póliza, o solo la póliza</param>
        /// <param name="cuotasExtras">Lista de cuotas extras para le pago del crédito</param>
        /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        public DTO_SerializedObject GenerarPlanPagosFinanciera(string lineaCredID, int valorCredito, int valorPoliza, int vlrGiro, int plazoCredito, int plazoPoliza,
            int edad, DateTime fechaLiquida, DateTime fechaCuota1, decimal interesCredito, decimal interesPoliza, int cta1Poliza, int vlrCuotaPol, bool liquidaAll,
            List<DTO_Cuota> cuotasExtras, Dictionary<string, decimal> compsNuevoValor, int numDocCredito, string tipoCredito, bool excluyeCompInvisibleInd)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_PlanDePagos planPagosCredito = new DTO_PlanDePagos();
            DTO_PlanDePagos planPagosPoliza = new DTO_PlanDePagos();
            DTO_PlanDePagos planPagosFinal = new DTO_PlanDePagos();

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCartera = (ModuloCartera)base.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccSolicitudCtasExtra = (DAL_ccSolicitudCtasExtra)base.GetInstance(typeof(DAL_ccSolicitudCtasExtra), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                
                DTO_ccSolicitudComponentes componenteSeguro = new DTO_ccSolicitudComponentes();
                DTO_ccSolicitudComponentes componenteInteresSeguro = new DTO_ccSolicitudComponentes();
                string compIntSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
               
                #region Valida que existan los componentes basicos en glControl
                if (string.IsNullOrWhiteSpace(compIntSeguro))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ComponenteInteresSeguro + "&&" + string.Empty;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(compSeguro))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.cc_ComponenteSeguroVida + "&&" + string.Empty;
                    return result;
                }
                #endregion
                #region Calcula el plan de pagos del credito
                if (liquidaAll)
                {
                    object credito = this.CrearPlanPagosCredito(lineaCredID, valorCredito, vlrGiro, plazoCredito, edad, fechaLiquida, fechaCuota1, interesCredito, cuotasExtras, compsNuevoValor,tipoCredito, excluyeCompInvisibleInd);
                    if (credito.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult resultCredito = (DTO_TxResult)credito;
                        return resultCredito;
                    }
                    else
                    {
                        planPagosCredito = (DTO_PlanDePagos)credito;
                        componenteSeguro = planPagosCredito.ComponentesAll.Where(x => x.ComponenteCarteraID.Value == compSeguro).FirstOrDefault();
                        planPagosCredito.ComponentesAll.Remove(componenteSeguro);
                        planPagosCredito.ComponentesUsuario.Remove(componenteSeguro);

                        planPagosFinal.CuotasCredito = planPagosCredito.Cuotas;
                        if (cuotasExtras.Count == 0)
                        {
                            List<DTO_ccSolicitudCtasExtra> cExtras = this._dal_ccSolicitudCtasExtra.DAL_ccSolicitudAnexo_GetByNumeroDoc(numDocCredito);
                            cExtras.ForEach(x => cuotasExtras.Add(new DTO_Cuota() { NumCuota = x.CuotaID.Value.Value, ValorCuota = Convert.ToInt32(x.VlrCuota.Value.Value) }));
                        }

                        planPagosFinal.CuotasExtras = cuotasExtras;
                    }
                }
                #endregion
                #region Calcula el plan de pagos de la poliza
                if (valorPoliza != 0 || !liquidaAll)
                {
                    object poliza = this.CrearPlanPagosPoliza(lineaCredID, valorPoliza, vlrGiro, plazoPoliza, fechaLiquida, fechaCuota1, interesPoliza, vlrCuotaPol);
                    if (poliza.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)poliza;
                        return result;
                    }
                    else
                    {
                        planPagosPoliza = (DTO_PlanDePagos)poliza;
                        componenteInteresSeguro = planPagosPoliza.ComponentesAll.Where(x => x.ComponenteCarteraID.Value == compIntSeguro).FirstOrDefault();

                        planPagosFinal.CuotasCredito = planPagosPoliza.Cuotas;
                    }
                }
                #endregion
                #region Genera el plan de pago definitivo
                #region Agrega al plan de pagos final la información de la poliza
                if (valorPoliza != 0)
                {
                    int cuotaIni = cta1Poliza - 1;
                    int cuotaFin = cuotaIni + plazoPoliza;
                    if (cuotaFin > plazoCredito)
                        cuotaFin = plazoCredito;

                    if (liquidaAll)
                    {                    
                        int j = 0;
                        for (int i = cuotaIni; i < cuotaFin; i++)
                        {
                            planPagosCredito.Cuotas[i].ValorCuota += planPagosPoliza.VlrCuota;
                            planPagosCredito.Cuotas[i].Seguro = planPagosPoliza.Cuotas[j].Capital;

                            //Agrega el interes del seguro
                            planPagosCredito.Cuotas[i].Componentes.Add(componenteInteresSeguro.Descripcion.Value);
                            planPagosCredito.Cuotas[i].ValoresComponentes.Add(planPagosPoliza.Cuotas[j].Intereses);

                            //Para la primera cuota de la poliza
                            if (j == 0)
                                planPagosCredito.ComponentesFijos.Add(compIntSeguro, false);
                            j++;
                        }
                    }
                    else
                    {
                        DTO_ccTipoCredito tipoCred = (DTO_ccTipoCredito)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, tipoCredito, true, false);
                        if (tipoCred != null && tipoCred.TipoCredito.Value == (byte)TipoCredito.PolizaSinCredito)
                        {
                            //Asigna valores solo de Polizas
                            for (int i = cuotaIni; i < cuotaFin; i++)
                            {
                                //Asigna el valor del seguro
                                planPagosPoliza.Cuotas[i].Seguro = planPagosPoliza.Cuotas[i].Capital;

                                //Agrega el interes del seguro
                                planPagosPoliza.Cuotas[i].Componentes.Add(componenteInteresSeguro.Descripcion.Value);
                                planPagosPoliza.Cuotas[i].ValoresComponentes.Add(planPagosPoliza.Cuotas[i].Intereses);

                                //Reasigna los valores del capital e interes
                                planPagosPoliza.Cuotas[i].Capital = 0;
                                planPagosPoliza.Cuotas[i].Intereses = 0;

                                //Para la primera cuota de la poliza
                                if (i == 0)
                                    planPagosCredito.ComponentesFijos.Add(compIntSeguro, false);
                            }
                        }
                        else
                            planPagosCredito.ComponentesFijos.Add(compIntSeguro, false);
                    }                        

                    planPagosFinal.VlrCuotaPoliza = planPagosPoliza.VlrCuota;
                    planPagosFinal.VlrPoliza = planPagosPoliza.VlrGiro;
                    //planPagosFinal.VlrPoliza = planPagosPoliza.VlrGiro;
                    planPagosFinal.VlrPrestamoPoliza = planPagosPoliza.VlrPrestamoPoliza;
                }
                #endregion
                #region Agrega las listas al plan de pagos final
                planPagosFinal.Cuotas = liquidaAll == true ? planPagosCredito.Cuotas : planPagosPoliza.Cuotas;
                planPagosFinal.ComponentesFijos = planPagosCredito.ComponentesFijos;
                planPagosFinal.ComponentesAll.AddRange(planPagosCredito.ComponentesAll);
                planPagosFinal.ComponentesAll.AddRange(planPagosPoliza.ComponentesAll);
                planPagosFinal.ComponentesUsuario.AddRange(planPagosCredito.ComponentesUsuario);
                planPagosFinal.ComponentesUsuario.AddRange(planPagosPoliza.ComponentesUsuario);
                #endregion
                #region Agrega los valores adicionales
                if (liquidaAll)
                {
                    planPagosFinal.VlrAdicional = planPagosCredito.VlrAdicional;
                    planPagosFinal.VlrCompra = planPagosCredito.VlrCompra;
                    planPagosFinal.VlrCuota = planPagosCredito.VlrCuota;
                    planPagosFinal.VlrDescuento = planPagosCredito.VlrDescuento;
                    planPagosFinal.VlrGiro = planPagosCredito.VlrGiro;
                    planPagosFinal.VlrLibranza = planPagosCredito.VlrLibranza;
                    planPagosFinal.VlrPrestamo = planPagosCredito.VlrPrestamo;
                }
                #endregion
                #endregion

                return planPagosFinal;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GenerarPlanPagosFinanciera");
                return result;
            }
        }

        /// <summary>
        /// Actualiza la informacion de la vaibilidad
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="solicitudes">Solicitudes que se estan modificando</param>
        /// <param name="anexos">Anexos de la solicitud</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> SolicitudFin_AprobarRechazar(int documentID, string actFlujoID, List<DTO_ccSolicitudDocu> solicitudes, 
            List<DTO_ccSolicitudAnexo> anexos, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                int i = 0;
                foreach (DTO_ccSolicitudDocu sol in solicitudes)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / solicitudes.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    if (sol.Aprobado.Value.Value)
                    {
                        #region Aprobaciones
                        try
                        {
                            if (sol.TipoCredito.Value == (byte)TipoCredito.PolizaRenueva)
                            {
                                result = this.Aprobar_RenovacionPoliza(documentID, sol, false);
                            }
                            else
                            {
                                result = this.AprobarSolicitud(documentID, actFlujoID, sol, anexos, false);
                            }
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Viabilidad_AprobarRechazar (Aprobar)");
                        }
                        #endregion
                    }
                    else if (sol.Rechazado.Value.Value)
                    {
                        #region Rechazos
                        try
                        {
                            result = this.RechazarSolicitud(documentID, actFlujoID, sol, false);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Viabilidad_AprobarRechazar (Rechazar)");
                        }
                        #endregion
                    }

                    #region Carga resultados
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(Convert.ToInt32(result.ExtraField), false);
                        results.Add(alarma);
                    }
                    #endregion
                }
                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Viabilidad_AprobarRechazar");
                results.Add(result);
                return results;
            }
        }

        /// <summary>
        /// Revierte la renovacion de poliza
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RenovacionPoliza_Revertir(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls,
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
                #region Variables

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoComponentes = (DAL_ccCreditoComponentes)this.GetInstance(typeof(DAL_ccCreditoComponentes), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPagos = (DAL_ccCreditoPagos)base.GetInstance(typeof(DAL_ccCreditoPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPlanPagos = (DAL_ccCreditoPlanPagos)this.GetInstance(typeof(DAL_ccCreditoPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCartera = (ModuloCartera)this.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoDocu = (DAL_ccCreditoDocu)this.GetInstance(typeof(DAL_ccCreditoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccSolicitudPlanPagos = (DAL_ccSolicitudPlanPagos)this.GetInstance(typeof(DAL_ccSolicitudPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables del credito
                DTO_glDocumentoControl ctrlRenovacion = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                DTO_ccCreditoDocu cred = this._moduloCartera.GetCreditoByNumeroDoc(ctrlRenovacion.DocumentoPadre.Value.Value);

                //Variables de la poliza
                DTO_ccPolizaEstado poliza = this.PolizaEstado_GetLastPoliza(cred.NumeroDoc.Value.Value, cred.Libranza.Value.Value);
                List<DTO_ccCreditoPlanPagos> creditoPlanPagos = this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_GetByNumDoc(cred.NumeroDoc.Value.Value);
                List<DTO_ccSolicitudPlanPagos> solicitidPlanPagos = this._dal_ccSolicitudPlanPagos.DAL_ccSolicitudPlanPagos_GetByNumDoc(numeroDoc);

                #endregion                                
                #region Valida que no tenga pagos
                long sumPagos = this._dal_ccCreditoPagos.DAL_ccCreditoPagos_CountByNumDocCredito(numeroDoc, null);
                if (sumPagos > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cc_RevCrPagos + "&&" + cred.Libranza.Value.Value.ToString();
                    return result;
                }
                #endregion
                #region Actualiza el crédito

                cred.Poliza.Value =null;
                cred.PlazoSeguro.Value = 0;
                cred.VlrPoliza.Value = 0;
                cred.FechaLiqSeguro.Value = null;
                cred.FechaVigenciaINI.Value = null;
                cred.FechaVigenciaFIN.Value = null;
                cred.PorSeguro.Value =0;
                cred.Cuota1Seguro.Value = 0;
                cred.VlrCuotaSeguro.Value = 0;
                cred.VlrFinanciaSeguro.Value =0;
                this._dal_ccCreditoDocu.DAL_ccCreditoDocu_Update(cred);
                           
                //Actualiza el documentoID del documento control de la poliza
                ctrlRenovacion.DocumentoPadre.Value = null;
                this._moduloGlobal.glDocumentoControl_Update(ctrlRenovacion, false, true);
                #endregion
                #region Actualiza el plan de pagos del credito con la informacion de la nueva poliza

                int cuota1Pol = poliza.Cuota1Financia.Value.Value;
                for (int i = 0; i < poliza.PlazoFinancia.Value.Value; ++i)
                {
                    DTO_ccSolicitudPlanPagos cuota = solicitidPlanPagos[i];
                    creditoPlanPagos[i + cuota1Pol - 1].VlrSeguro.Value = 0;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrOtro1.Value = 0;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrSaldoSeguro.Value = 0;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrPagadoCuota.Value = 0;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrPagadoExtras.Value = 0;
                    creditoPlanPagos[i + cuota1Pol - 1].VlrCuota.Value -= (cuota.VlrCapital.Value + cuota.VlrInteres.Value);//Recalcular

                    this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_Update(creditoPlanPagos[i + cuota1Pol - 1]);
                }

                #endregion
                #region Actualiza Poliza Estado
                DTO_ccCliente cliente = (DTO_ccCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, cred.ClienteID.Value, true, false);
                //poliza.NumDocCredito.Value =null;
                //poliza.NumeroDocLiquida.Value = null;
                //this.PolizaEstado_Upd(poliza, true);
                this.PolizaEstado_Delete(cliente.TerceroID.Value, poliza.Poliza.Value, true);
                #endregion                
                #region Revierte el documento
                result = this._moduloGlobal.glDocumentoControl_Revertir(documentID, numeroDoc, consecutivoPos, ref ctrls, ref coComps, true);
                if (result.Result == ResultValue.NOK)
                    return result;

                if (!consecutivoPos.HasValue)
                    consecutivoPos = 0;

                #endregion
                #region Revierte los movimientos de la cartera
                this._moduloCartera.ccCarteraMvto_Revertir(cred.NumeroDoc.Value.Value, numeroDoc, ctrls[consecutivoPos.Value].NumeroDoc.Value.Value);
                #endregion
                
                
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "LiquidacionCredito_Revertir");

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

        #endregion

        #region Pagos

        /// <summary>
        /// Valida que la información basica de la migracion nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="fechaNomina">Fecha de los recaudos</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult RecaudosMasivosFin_Validar(int documentID, DateTime fechaNomina, ref List<DTO_ccIncorporacionDeta> data, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            try
            {
                #region Variables

                this._dal_ccNominaDeta = (DAL_ccNominaDeta)base.GetInstance(typeof(DAL_ccNominaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCartera = (ModuloCartera)base.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables de cache
                Dictionary<string, DTO_ccCliente> cacheCodigosEmpleados = new Dictionary<string, DTO_ccCliente>();
                Dictionary<string, DTO_ccCreditoDocu> cacheCreditoClientes = new Dictionary<string, DTO_ccCreditoDocu>();
                Dictionary<string, List<DTO_ccCreditoDocu>> cacheCreditosClientes = new Dictionary<string, List<DTO_ccCreditoDocu>>();
                Dictionary<int, DTO_InfoCredito> cacheSaldoCredito = new Dictionary<int, DTO_InfoCredito>();
                Dictionary<string, int> cacheCreditosClientesCount = new Dictionary<string, int>();
                string compCapital = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                string compInteres = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);

                //Variables temporales
                DTO_ccCliente cliente;
                bool validRow = true;
                int i = 0;
                #endregion
                #region Valida que solo pueda haber una migracion por día
                //bool hasMigraciones = this._dal_ccNominaDeta.DAL_ccNominaDeta_HasMigracionByDay(fechaNomina);
                //if (hasMigraciones)
                //{
                //    result.Result = ResultValue.NOK;
                //    result.ResultMessage = DictionaryMessages.Err_Cf_RecaudosMasivosProccesed; + banco + fecha
                //    return result;
                //}
                #endregion

                #region Valida y Obtiene los creditos del cliente con saldos vencidos
                List<DTO_ccIncorporacionDeta> incorporacionFinal = new List<DTO_ccIncorporacionDeta>();
                List<string> clientes = data.Select(x => x.ClienteID.Value).Distinct().ToList();
                //Recorre los clientes no repetidos
                foreach (string cli in clientes)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = (i * 100) / data.Count;
                    batchProgress[tupProgress] = percent;

                    #region Trae creditos e info del Cliente
                    List<DTO_ccCreditoDocu> creditosCli = this._moduloCartera.GetCreditosPendientesByCliente(cli);
                    cacheCreditosClientes.Add(cli, creditosCli);

                    DTO_ccCliente dtoCliente = (DTO_ccCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, cli, true, false); 
                    #endregion

                    #region Agrega los registros que SI tengan libranza en la importacion
                    foreach (DTO_ccIncorporacionDeta detaxLib in data.FindAll(x => x.ClienteID.Value == cli && x.Libranza.Value != null))
                    {
                        if (dtoCliente.EstadoCartera.Value == (byte)EstadoDeuda.Juridico || dtoCliente.EstadoCartera.Value == (byte)EstadoDeuda.AcuerdoPago ||
                            dtoCliente.EstadoCartera.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                            detaxLib.EstadoClienteInvalid.Value = true;
                        DTO_ccCreditoDocu cred = this._moduloCartera.GetCreditoByLibranza(detaxLib.Libranza.Value.Value);
                        if (cred != null)
                        {
                            cacheCreditoClientes.Add(cli, cred);
                            detaxLib.CanceladoInd.Value = cred.CanceladoInd.Value;
                            detaxLib.EstadoCuentaInd.Value = cred.DocEstadoCuenta.Value != null ? true : false;
                            incorporacionFinal.Add(detaxLib);
                        }
                        else
                            detaxLib.Libranza.Value = null;
                    }  
                    #endregion                         

                    //Agrega los registros que NO tengan libranza incluida validando los creditos con el saldo vencido mas antiguo
                    if (data.Count(x => x.ClienteID.Value == cli && x.Libranza.Value == null) > 0)
                    {
                        //Sumariza valores de los registros importados  por cliente
                        DTO_ccIncorporacionDeta deta = data.Find(x => x.ClienteID.Value == cli && x.Libranza.Value == null);
                        deta.ValorCuota.Value = data.FindAll(x => x.ClienteID.Value == cli).Sum(y => y.ValorCuota.Value);
                        deta.ValorNomina.Value = data.FindAll(x => x.ClienteID.Value == cli).Sum(y => y.ValorNomina.Value);
                        decimal vlrCuotaRestante = deta.ValorCuota.Value.Value;

                        if (dtoCliente.EstadoCartera.Value == (byte)EstadoDeuda.Juridico || dtoCliente.EstadoCartera.Value == (byte)EstadoDeuda.AcuerdoPago ||
                            dtoCliente.EstadoCartera.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                            deta.EstadoClienteInvalid.Value = true;

                        //Valida las libranzas del cliente son mayores a 1                           
                        List<DTO_RecaudoPagos> listPagos = new List<DTO_RecaudoPagos>();
                        #region Trae saldos vencidos por credito y asigna el valor de la cuota
                        foreach (DTO_ccCreditoDocu cred in creditosCli)
                        {                                  
                            //Trae saldo del credito
                            DTO_InfoCredito saldo = this._moduloCartera.GetSaldoCredito(cred.NumeroDoc.Value.Value, fechaNomina,true, true,true);
                            cacheSaldoCredito.Add(cred.Libranza.Value.Value, saldo);
                            //Ordena las Cuotas por fecha mas antigua
                            saldo.PlanPagos = saldo.PlanPagos.OrderBy(y=>y.FechaCuota.Value).ToList();
                            cred.VlrSaldoVencido.Value = 0;
                            cred.VlrSaldoOtros.Value = 0;
                            foreach (DTO_ccCreditoPlanPagos pp in saldo.PlanPagos)
                            {
                                #region Agrega cuotas con saldo del credito 
                                DTO_RecaudoPagos pagos = new DTO_RecaudoPagos();
                                pagos.NumDocCredito.Value = cred.NumeroDoc.Value;
                                pagos.Libranza.Value = cred.Libranza.Value;
                                pagos.CuotaID.Value = pp.CuotaID.Value;
                                pagos.FechaCuota.Value = pp.FechaCuota.Value;
                                pagos.NumDocEstadoCta.Value = cred.DocEstadoCuenta.Value;
                                pagos.CanceladoInd.Value = cred.CanceladoInd.Value;
                                pagos.CuotaID.Value = pp.CuotaID.Value;
                                pagos.VlrPago.Value = saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                pagos.SaldosComponentes = saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value);
                                if (pp.FechaCuota.Value <= fechaNomina)
                                    cred.VlrSaldoVencido.Value += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                else
                                    cred.VlrSaldoOtros.Value += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                listPagos.Add(pagos); 
                                #endregion
                            }
                        }
                        // Ordena las cuota de todos los creditos por Fecha y creacion del credito
                        listPagos = listPagos.OrderBy(x => x.FechaCuota.Value).ThenBy(x => x.NumDocCredito.Value).ToList();

                        #region Crea los recaudos segun los pagos consultados
                        foreach (DTO_RecaudoPagos pag in listPagos)
                        {
                            //Crea y asigna libranza y valor Cuota finales
                            DTO_ccIncorporacionDeta newDeta = ObjectCopier.Clone(deta);
                            newDeta.Libranza.Value = pag.Libranza.Value;
                            newDeta.CanceladoInd.Value = pag.CanceladoInd.Value;
                            newDeta.EstadoCuentaInd.Value = pag.NumDocEstadoCta.Value != null ? true : false;
                            newDeta.ValorCuota.Value = vlrCuotaRestante >= pag.VlrPago.Value.Value ? pag.VlrPago.Value.Value : vlrCuotaRestante;
                            vlrCuotaRestante -= pag.VlrPago.Value.Value;
                            incorporacionFinal.Add(newDeta);

                            //Si el valor de cuota es inferior o igual al restante termina
                            if (vlrCuotaRestante <= 0)
                                break;
                        } 
                        #endregion

                        if (vlrCuotaRestante > 0 && incorporacionFinal.Count > 0)
                            incorporacionFinal.Last().ValorCuota.Value += vlrCuotaRestante;

                        #endregion                                                   
                    } 
                i++;
                }
                data = incorporacionFinal;
                
                #endregion
                i = 0;
                for (i = 1; i <= data.Count; ++i)
                {
                    #region Valida cada una de las lineas
                    int percent = (i * 100) / data.Count;
                    batchProgress[tupProgress] = percent;

                    DTO_ccIncorporacionDeta dto = data[i - 1];
                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                    rd.line = dto.PosicionImportacion.Value.Value;
                    dto.IndInconsistencia.Value = false; //Valida si tiene inconsistencias
                    validRow = true;
                  
                    DTO_ccCreditoDocu credito = null;
                    int indexCuota = 1;
                    bool lastCuotaCredito = false;
                    #region Trae el credito
                    if (dto.Libranza.Value != null)
                    {
                        #region Credito por libranza
                        credito = this._moduloCartera.GetCreditoByLibranza(dto.Libranza.Value.Value);
                        if (credito == null)
                        {
                            validRow = false;
                            rd.Message = DictionaryMessages.Err_Cc_InvalidCredito + "&&" + dto.Libranza.Value.Value.ToString();
                        }
                        else 
                        {
                            #region Trae y valida el saldo del credito contra el recaudo
                            DTO_InfoCredito saldo = null;
                            if (!cacheSaldoCredito.ContainsKey(dto.Libranza.Value.Value))
                            {
                                saldo = this._moduloCartera.GetSaldoCredito(credito.NumeroDoc.Value.Value, fechaNomina, true, true, true);
                                cacheSaldoCredito.Add(dto.Libranza.Value.Value, saldo);
                            }
                            else
                                saldo = cacheSaldoCredito[dto.Libranza.Value.Value];
                          
                            credito.VlrSaldoOtros.Value = 0;
                            credito.VlrSaldoVencido.Value = 0;

                            //OBtiene valores Vencidos y No vencidos
                            foreach (DTO_ccCreditoPlanPagos pp in saldo.PlanPagos)
                            {
                                if (pp.FechaCuota.Value <= fechaNomina)
                                    credito.VlrSaldoVencido.Value += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                else
                                {
                                    if (indexCuota <= 2) //Abona solo 2 cuotas anticipidas
                                    {                                      
                                        credito.VlrSaldoOtros.Value += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                        indexCuota++;
                                        lastCuotaCredito = pp.CuotaID.Value == credito.Plazo.Value ? true : false; 
                                    }
                                }
                               
                            }
                            //Valida que el valor del recaudo no sea mayor a 2 cuotas
                            if (data.FindAll(x=>x.Libranza.Value == dto.Libranza.Value.Value).Sum(y=>y.ValorCuota.Value) > (credito.VlrSaldoOtros.Value + credito.VlrSaldoVencido.Value) && !lastCuotaCredito)                           
                            {
                                validRow = false;
                                rd.Message = "Cliente: " + dto.ClienteID.Value + ", Obligación: " + dto.Libranza.Value.Value.ToString() + ". Paga más de 2 cuotas anticipadas ";
                                if (result.Details.Exists(x => x.Message.Equals(rd.Message)))
                                {
                                    validRow = true;
                                    rd.Message = string.Empty;
                                }                               
                            } 
                            #endregion
                        }
                        #endregion
                    }
                    else if (!string.IsNullOrWhiteSpace(dto.CodEmpleado.Value))
                    {
                        #region Credito por codigo y validacion de cliente
                        if (!cacheCodigosEmpleados.ContainsKey(dto.CodEmpleado.Value))
                        {
                            cliente = this._moduloGlobal.ccCliente_GetClienteByCodigoEmpleado(dto.CodEmpleado.Value);
                            cacheCodigosEmpleados.Add(dto.CodEmpleado.Value, cliente);
                        }
                        else
                            cliente = cacheCodigosEmpleados[dto.CodEmpleado.Value];

                        if (cliente == null)
                        {
                            validRow = false;
                            rd.Message = DictionaryMessages.Err_Cc_InvalidCodEmpleado + "&&" + dto.CodEmpleado.Value;
                        }
                        else
                        {
                            if (cacheCreditosClientesCount.ContainsKey(cliente.ID.Value))
                            {
                                #region Cliente existente
                                if (cacheCreditosClientesCount[cliente.ID.Value] == 0)
                                {
                                    validRow = false;
                                    rd.Message = DictionaryMessages.Err_Cc_NoCredByCodigo + "&&" + dto.CodEmpleado.Value;
                                }                                
                                else
                                {
                                    credito = cacheCreditoClientes[cliente.ID.Value];
                                    dto.Libranza.Value = credito.Libranza.Value;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Nuevo cliente
                                List<DTO_ccCreditoDocu> creditos = cacheCreditosClientes[cliente.ID.Value];
                                if (creditos.Count == 0)
                                {
                                    cacheCreditosClientesCount.Add(cliente.ID.Value, 0);
                                    cacheCreditoClientes.Add(cliente.ID.Value, null);

                                    validRow = false;
                                    rd.Message = DictionaryMessages.Err_Cc_NoCredByCodigo + "&&" + dto.CodEmpleado.Value;
                                }
                                else if (creditos.Count > 1)
                                {
                                    cacheCreditosClientesCount.Add(cliente.ID.Value, creditos.Count);
                                    cacheCreditoClientes.Add(cliente.ID.Value, null);

                                    decimal? vlrSaldoCuotas = 0;
                                    decimal? vlrSaldoVencido = 0;
                                    //Recorre los creditos del cliente trayendo los saldos
                                    foreach (DTO_ccCreditoDocu cred in creditos)
                                    {
                                        #region Trae y valida el saldo del credito contra el recaudo
                                        DTO_InfoCredito saldo = null;
                                        if (!cacheSaldoCredito.ContainsKey(cred.Libranza.Value.Value))
                                        {
                                            saldo = this._moduloCartera.GetSaldoCredito(cred.NumeroDoc.Value.Value, fechaNomina, true, true, true);
                                            cacheSaldoCredito.Add(cred.Libranza.Value.Value, saldo);
                                        }
                                        else
                                            saldo = cacheSaldoCredito[cred.Libranza.Value.Value];

                                        foreach (DTO_ccCreditoPlanPagos pp in saldo.PlanPagos)
                                        {
                                            //OBtiene valores Vencidos
                                            if (pp.FechaCuota.Value <= fechaNomina)
                                                vlrSaldoVencido += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                            //OBtiene valores No vencidos
                                            else
                                            {
                                                if (indexCuota <= 2) //Abona solo 2 cuotas anticipidas
                                                {
                                                    vlrSaldoCuotas += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                                    indexCuota++;
                                                    lastCuotaCredito = pp.CuotaID.Value == credito.Plazo.Value ? true : false;
                                                }
                                            }                                           
                                        }
                                        #endregion
                                    }
                                    //Valida que el valor del recaudo no sea mayor a 2 cuotas
                                    if (data.FindAll(x => x.Libranza.Value == dto.Libranza.Value.Value).Sum(y => y.ValorCuota.Value) > (vlrSaldoCuotas + vlrSaldoVencido) && !lastCuotaCredito)
                                    {
                                        validRow = false;
                                        rd.Message = rd.Message = "Obligación: " + creditos.Last().Libranza.Value.ToString() + ". Paga más de 2 cuotas anticipadas ";
                                        if (result.Details.Exists(x => x.Message.Equals(rd.Message)))
                                        {
                                            validRow = true;
                                            rd.Message = string.Empty;
                                        }
                                    }
                                }
                                else
                                {
                                    credito = creditos[0];
                                    dto.Libranza.Value = credito.Libranza.Value;

                                    cacheCreditosClientesCount.Add(cliente.ID.Value, creditos.Count);
                                    cacheCreditoClientes.Add(cliente.ID.Value, credito);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Credito por cliente
                        if (cacheCreditosClientesCount.ContainsKey(dto.ClienteID.Value))
                        {
                            #region Cliente existente
                            if (cacheCreditosClientesCount[dto.ClienteID.Value] == 0)
                            {
                                validRow = false;
                                rd.Message = DictionaryMessages.Err_Cc_NoCredByCliente + "&&" + dto.ClienteID.Value;
                            }                           
                            else
                            {
                                credito = cacheCreditoClientes[dto.ClienteID.Value];
                                dto.Libranza.Value = credito.Libranza.Value;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Nuevo cliente
                            List<DTO_ccCreditoDocu> creditos = cacheCreditosClientes[dto.ClienteID.Value];
                            if (creditos.Count == 0)
                            {
                                cacheCreditosClientesCount.Add(dto.ClienteID.Value, 0);
                                cacheCreditoClientes.Add(dto.ClienteID.Value, null);

                                validRow = false;
                                rd.Message = DictionaryMessages.Err_Cc_NoCredByCliente + "&&" + dto.ClienteID.Value;
                            }
                            else if (creditos.Count > 1)
                            {
                                cacheCreditosClientesCount.Add(dto.ClienteID.Value, creditos.Count);
                                cacheCreditoClientes.Add(dto.ClienteID.Value, null);

                                decimal? vlrSaldoCuotas = 0;
                                decimal? vlrSaldoVencido = 0;
                                //Recorre los creditos del cliente trayendo los saldos
                                foreach (DTO_ccCreditoDocu cred in creditos)
                                {
                                    #region Trae y valida el saldo del credito contra el recaudo
                                    DTO_InfoCredito saldo = null;
                                    if (!cacheSaldoCredito.ContainsKey(cred.Libranza.Value.Value))
                                    {
                                        saldo = this._moduloCartera.GetSaldoCredito(cred.NumeroDoc.Value.Value, fechaNomina, true, true, true);
                                        cacheSaldoCredito.Add(cred.Libranza.Value.Value, saldo);
                                    }
                                    else
                                        saldo = cacheSaldoCredito[cred.Libranza.Value.Value];

                                    foreach (DTO_ccCreditoPlanPagos pp in saldo.PlanPagos)
                                    {
                                        //OBtiene valores Vencidos
                                        if (pp.FechaCuota.Value <= fechaNomina)
                                            vlrSaldoVencido += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                        //OBtiene valores No vencidos
                                        else 
                                        {
                                            if (indexCuota <= 2) //Abona solo 2 cuotas anticipidas
                                            {
                                                vlrSaldoCuotas += saldo.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value).Sum(x => x.CuotaSaldo.Value);
                                                indexCuota++;
                                                lastCuotaCredito = pp.CuotaID.Value == credito.Plazo.Value ? true : false;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                //Valida que el valor del recaudo no sea mayor a 2 cuotas
                                if (data.FindAll(x => x.Libranza.Value == dto.Libranza.Value.Value).Sum(y => y.ValorCuota.Value) > (vlrSaldoCuotas + vlrSaldoVencido) && !lastCuotaCredito)
                                {
                                    validRow = false;
                                    rd.Message = rd.Message = "Obligación: " + creditos.Last().Libranza.Value.ToString() + ". Paga más de 2 cuotas anticipadas ";
                                    if (result.Details.Exists(x => x.Message.Equals(rd.Message)))
                                    {
                                        validRow = true;
                                        rd.Message = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                credito = creditos[0];
                                dto.Libranza.Value = credito.Libranza.Value;

                                cacheCreditosClientesCount.Add(dto.ClienteID.Value, creditos.Count);
                                cacheCreditoClientes.Add(dto.ClienteID.Value, credito);
                            }
                            #endregion
                        }
                        #endregion
                    }

                    #endregion
                    #region Valida que el cliente corresponda con el del credito
                    if (string.IsNullOrWhiteSpace(rd.Message))
                    {
                        //if (credito != null && !string.IsNullOrWhiteSpace(dto.ClienteID.Value) && credito.ClienteID.Value != dto.ClienteID.Value)
                        //{
                        //    validRow = false;
                        //    rd.Message = DictionaryMessages.Err_Cc_InvalidClienteLibranza;
                        //}
                        //else
                        //    dto.Libranza.Value = credito.Libranza.Value;
                    }
                    #endregion
                    #region Valida que el cliente no este en estado inválido (Jurídico, acuerdo de pago, acuerdo de pago incumplido)
                    if (string.IsNullOrWhiteSpace(rd.Message) && dto.EstadoClienteInvalid.Value.Value)
                    {                      
                        validRow = false;
                        rd.Message = dto.ClienteID.Value + ": Cliente en Estado Jurídico, acuerdo de pago, acuerdo de pago incumplido";
                    }
                    #endregion
                    #region Valida si tiene estado de cuenta
                    if (string.IsNullOrWhiteSpace(rd.Message) && dto.EstadoCuentaInd.Value.Value)
                    {
                        validRow = false;
                        rd.Message = DictionaryMessages.Err_Cc_CreditoConEstadoCuenta + "&&" + credito.Libranza.Value.Value.ToString();
                    }
                    #endregion
                    #region Valida si el credito ya esta cancelado
                    if (string.IsNullOrWhiteSpace(rd.Message) && dto.CanceladoInd.Value.Value)
                    {
                        //ERROR, PERO LO DEJA SEGUIR
                        rd.Message = DictionaryMessages.Err_Cc_CreditoCancelado + "&&" + dto.Libranza.Value.Value.ToString();
                    }
                    #endregion

                    //Asigna las inconsistenciass
                    if (!string.IsNullOrWhiteSpace(rd.Message))
                    {
                        result.Details.Add(rd);
                        dto.IndInconsistencia.Value = true;
                    }

                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "MigracionNomida_Validar");
                return result;
            }
        }

        /// <summary>
        /// Procesa la migracion de nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="data">Información a migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult RecaudosMasivosFin_Procesar(int documentID, DateTime periodo, List<DTO_ccIncorporacionDeta> data, bool isAnotherTx, 
            Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            try
            {
                this._dal_CarteraFin = (DAL_CarteraFin)base.GetInstance(typeof(DAL_CarteraFin), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccNominaPreliminar = (DAL_ccNominaPreliminar)base.GetInstance(typeof(DAL_ccNominaPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_ccNominaPreliminar.DAL_ccNominaPreliminar_Delete();

                string centroPagoID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CentroPagoPorDefecto);
                result = this._dal_CarteraFin.DAL_CarteraFin_RecaudosMasivos_Inconsistencias(centroPagoID, periodo, data);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "RecaudosMasivos_Procesar");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!isAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !isAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna la lista de resultados</returns>
        public DataTable RecaudosMasivos_GetRelacionPagos(int documentID, List<DTO_ccIncorporacionDeta> data, Dictionary<Tuple<int, int>, int> batchProgress, Dictionary<string, string> columnsComp)
        {
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_ccCreditoDocu credito = null;

            try
            {
                DataTable tablaFinal = new DataTable("Componentes");

                #region Variables

                //Dals y módulos
                this._dal_ccCreditoDocu = (DAL_ccCreditoDocu)base.GetInstance(typeof(DAL_ccCreditoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPagos = (DAL_ccCreditoPagos)base.GetInstance(typeof(DAL_ccCreditoPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCreditoPlanPagos = (DAL_ccCreditoPlanPagos)base.GetInstance(typeof(DAL_ccCreditoPlanPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCartera = (ModuloCartera)base.GetInstance(typeof(ModuloCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Info de componentes
                string componenteInteres = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                string componenteUsura = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteUsura);
                string componenteSaldoFavor = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSaldosAFavor);
                string componentePJ = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);

                //Diccionarios de cache
                DTO_ccCarteraComponente compDTO = new DTO_ccCarteraComponente();
                Dictionary<string, DTO_ccCarteraComponente> cacheComponentes = new Dictionary<string, DTO_ccCarteraComponente>();

                //Información de la tabla
                DataTable tablaCreditos = new DataTable();
                tablaCreditos.Columns.Add("NumDoc", typeof(int));
                tablaCreditos.Columns.Add("Credito", typeof(int));
                tablaCreditos.Columns.Add("ClienteID");
                tablaCreditos.Columns.Add("Nombre");
                tablaCreditos.Columns.Add("Cuota", typeof(int));
                tablaCreditos.Columns.Add("ComponenteCarteraID");
                tablaCreditos.Columns.Add("Descriptivo");
                tablaCreditos.Columns.Add("Valor", typeof(int));

                #endregion
                #region Carga la información de los créditos

                for (int i = 0; i < data.Count; ++i)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = (i * 100) / data.Count;
                    batchProgress[tupProgress] = percent;

                    //Pago
                    DTO_ccIncorporacionDeta pago = data[i];

                    #region Variables

                    //Variables de inicio
                    decimal saldoTotal = 0;
                    decimal vlrUsura = 0;
                    decimal valorPagadoCuota = 0;
                    DTO_ccSaldosComponentes saldoAFavor = null;
                    DTO_ccCreditoPlanPagos extraCuota = null;

                    //Lista de componentes
                    List<DTO_ccSaldosComponentes> componentes = new List<DTO_ccSaldosComponentes>();
                    List<DTO_ccCreditoPlanPagos> planPagos = new List<DTO_ccCreditoPlanPagos>();
                    Dictionary<string, decimal> valoresComponentes = new Dictionary<string, decimal>();

                    #endregion
                    #region Trae la info del credito y del cliente

                    credito = this._moduloCartera.GetCreditoByLibranza(pago.Libranza.Value.Value);
                    DTO_InfoCredito infoCredito = this._moduloCartera.GetSaldoCredito(credito.NumeroDoc.Value.Value, pago.FechaNomina.Value.Value, true, true, true);
                    DTO_ccCliente cliente = (DTO_ccCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, credito.ClienteID.Value, true, false);
                    decimal porcHonorarios = 0;

                    string abogadoID = cliente.AbogadoID.Value;
                    if (string.IsNullOrWhiteSpace(abogadoID))
                        abogadoID = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AbogadoCobroPrejuridico);

                    if (!string.IsNullOrWhiteSpace(abogadoID))
                    {
                        DTO_ccAbogado abogado = (DTO_ccAbogado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAbogado, abogadoID, true, false);
                        porcHonorarios = abogado.PorHonorarios.Value.Value;
                    }

                    #endregion
                    #region Calcula el saldo a favor (Actualizado Dic 3 2015) - Daniel Peralta

                    //if (infoCredito.PlanPagos.Count > 0)
                    //{
                    //    componentes = infoCredito.SaldosComponentes.Where(x => x.CuotaID.Value == credito.Plazo.Value).ToList();
                    //    saldoTotal = (from c in componentes select c.TotalSaldo.Value.Value).Sum();
                    //}
                    //else
                    //{
                    //    //extraCuota = this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_GetByNumDoc(credito.NumeroDoc.Value.Value).Last();
                    //    //extraCuota.PagoInd.Value = true;
                    //    //extraCuota.VlrPagadoCuota.Value = saldoAFavor.TotalInicial.Value;

                    //    //infoCredito.PlanPagos.Add(extraCuota);
                    //}

                    //if (pago.ValorCuota.Value.Value > saldoTotal)
                    //{
                    //    saldoAFavor = new DTO_ccSaldosComponentes();
                    //    saldoAFavor.CuotaID.Value = credito.Plazo.Value.Value;
                    //    saldoAFavor.ComponenteCarteraID.Value = componenteSaldoFavor;
                    //    saldoAFavor.ComponenteFijo.Value = false;
                    //    saldoAFavor.PagoTotalInd.Value = true;
                    //    saldoAFavor.CuotaInicial.Value = Math.Abs(saldoTotal - pago.ValorCuota.Value.Value);
                    //    saldoAFavor.TotalInicial.Value = saldoAFavor.CuotaInicial.Value;
                    //    saldoAFavor.CuotaSaldo.Value = saldoAFavor.CuotaInicial.Value;
                    //    saldoAFavor.TotalSaldo.Value = saldoAFavor.CuotaInicial.Value;
                    //    saldoAFavor.AbonoValor.Value = saldoAFavor.CuotaInicial.Value;

                    //    //Asigna el componente a la info general
                    //    infoCredito.SaldosComponentes.Add(saldoAFavor);
                    //}

                    //planPagos = infoCredito.PlanPagos;
                    //List<string> nombresComponentes = infoCredito.SaldosComponentes.Select(x => x.ComponenteCarteraID.Value).Distinct().ToList();

                    #endregion
                    #region Calcula el saldo a favor

                    if (infoCredito.PlanPagos.Count > 0)
                    {
                        int ultimaCuota = (from c in infoCredito.SaldosComponentes select c.CuotaID.Value.Value).OrderBy(x=>x).Last();
                        componentes = infoCredito.SaldosComponentes.Where(x => x.CuotaID.Value == ultimaCuota).ToList();
                        saldoTotal = (from c in componentes select c.TotalSaldo.Value.Value).Sum();
                    }

                    if (infoCredito.PlanPagos.Count == 0 || pago.ValorCuota.Value.Value > saldoTotal)
                    {
                        DTO_ccCarteraComponente compSaldoFavorDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, componenteSaldoFavor, true, false);

                        saldoAFavor = new DTO_ccSaldosComponentes();
                        saldoAFavor.CuotaID.Value = (from c in infoCredito.SaldosComponentes select c.CuotaID.Value.Value).OrderBy(x => x).Last(); 
                        saldoAFavor.ComponenteCarteraID.Value = componenteSaldoFavor;
                        saldoAFavor.ComponenteFijo.Value = false;
                        saldoAFavor.PagoTotalInd.Value = true;
                        saldoAFavor.TipoComponente.Value = compSaldoFavorDTO.TipoComponente.Value;
                        saldoAFavor.CuotaInicial.Value = Math.Abs(saldoTotal - pago.ValorCuota.Value.Value);
                        saldoAFavor.TotalInicial.Value = saldoAFavor.CuotaInicial.Value;
                        saldoAFavor.CuotaSaldo.Value = saldoAFavor.CuotaInicial.Value;
                        saldoAFavor.TotalSaldo.Value = saldoAFavor.CuotaInicial.Value;
                        saldoAFavor.AbonoValor.Value = saldoAFavor.CuotaInicial.Value;

                        //Asigna el componente a la info general
                        infoCredito.SaldosComponentes.Add(saldoAFavor);
                    }

                    if (infoCredito.PlanPagos.Count == 0)
                    {
                        extraCuota = this._dal_ccCreditoPlanPagos.DAL_ccCreditoPlanPagos_GetByNumDoc(credito.NumeroDoc.Value.Value).Last();
                        extraCuota.PagoInd.Value = true;
                        extraCuota.VlrPagadoCuota.Value = saldoAFavor.TotalInicial.Value;

                        infoCredito.PlanPagos.Add(extraCuota);
                    }

                    planPagos = infoCredito.PlanPagos;
                    List<string> nombresComponentes = infoCredito.SaldosComponentes.Select(x => x.ComponenteCarteraID.Value).Distinct().ToList();

                    #endregion

                    #region Calcula los pagos

                    decimal abono = pago.ValorCuota.Value.Value;
                    decimal vlrCuotaInicial = pago.ValorCuota.Value.Value;
                    foreach (DTO_ccCreditoPlanPagos cuota in planPagos)
                    {
                        cuota.PagoInd.Value = false;
                        valorPagadoCuota = 0;
                        if (pago.ValorCuota.Value.Value > 0)
                        {
                            foreach (string comp in nombresComponentes)
                                valoresComponentes[comp] = 0;

                            #region Paga La cuota
                            cuota.PagoInd.Value = true;
                            componentes = infoCredito.SaldosComponentes.Where(x => x.CuotaID.Value == cuota.CuotaID.Value).ToList();
                            decimal saldoCuota = (from c in componentes select c.CuotaSaldo.Value.Value).Sum();
                            vlrUsura = (from c in componentes where c.ComponenteCarteraID.Value == componenteUsura select c.CuotaSaldo.Value.Value).Sum();

                            #region Revisa si la cuota tiene componente prejurídico

                            bool hasPJ_cuota = componentes.Where(c => c.ComponenteCarteraID.Value == componentePJ).Count() > 0 ? true : false;
                            if (hasPJ_cuota && porcHonorarios > 0)
                            {
                                decimal totalPJ = componentes.Where(c => c.ComponenteCarteraID.Value == componentePJ).First().CuotaSaldo.Value.Value;
                                decimal vlrTmp = abono - (Convert.ToInt32(abono / ((porcHonorarios / 100) + 1)));
                                decimal abonoPJ = Math.Round(vlrTmp / 1000, 0) * 1000;
                                abonoPJ = Convert.ToInt32(1000 * Math.Round(Convert.ToDouble(abonoPJ) / 1000));

                                if (abonoPJ > totalPJ)
                                    abonoPJ = Convert.ToInt32(totalPJ);

                                abono -= abonoPJ;
                                componentes.Where(c => c.ComponenteCarteraID.Value == componentePJ).First().AbonoValor.Value = abonoPJ;
                                valoresComponentes[componentePJ] = valoresComponentes[componentePJ] + abonoPJ;
                            }

                            #endregion

                            //Paga el valor de los componentes
                            decimal abonoAct = 0;
                            for (int j = componentes.Count; j > 0; j--)
                            {
                                string compCarteraID = componentes[j - 1].ComponenteCarteraID.Value;

                                #region Asigna los pagos a los componentes de la cuota

                                if (componentes[j - 1].ComponenteCarteraID.Value != componenteUsura && componentes[j - 1].ComponenteCarteraID.Value != componentePJ)
                                {
                                    if (vlrUsura != 0 && componentes[j - 1].ComponenteCarteraID.Value == componenteInteres)
                                    {
                                        #region Componentes dependientes de la usura
                                        decimal interesReal = componentes[j - 1].CuotaSaldo.Value.Value + vlrUsura;

                                        if (abono <= interesReal)
                                        {
                                            componentes[j - 1].AbonoValor.Value = abono;
                                            valoresComponentes[compCarteraID] = valoresComponentes[compCarteraID] + abono;

                                            valorPagadoCuota += abono;
                                            abono = 0;
                                            pago.ValorCuota.Value = abono;

                                            break;
                                        }
                                        else
                                        {
                                            abonoAct = abono - interesReal;
                                            componentes[j - 1].AbonoValor.Value = componentes[j - 1].CuotaSaldo.Value.Value;
                                            valoresComponentes[compCarteraID] = valoresComponentes[compCarteraID] + componentes[j - 1].CuotaSaldo.Value.Value;

                                            // Pago de usura
                                            DTO_ccSaldosComponentes usura = componentes.Where(c => c.ComponenteCarteraID.Value == componenteUsura).First();
                                            usura.AbonoValor.Value = usura.CuotaSaldo.Value;

                                            valorPagadoCuota += componentes[j - 1].CuotaSaldo.Value.Value + usura.CuotaSaldo.Value.Value;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Componentes fijos de pago
                                        if (abono <= componentes[j - 1].CuotaSaldo.Value)
                                        {
                                            componentes[j - 1].AbonoValor.Value = abono;
                                            valorPagadoCuota += abono;
                                            abono = 0;
                                            pago.ValorCuota.Value = abono;

                                            valoresComponentes[compCarteraID] = valoresComponentes[compCarteraID] + componentes[j - 1].AbonoValor.Value.Value;
                                            break;
                                        }
                                        else
                                        {
                                            //vlrDescuentoComp = 

                                            abonoAct = abono - componentes[j - 1].CuotaSaldo.Value.Value;
                                            componentes[j - 1].AbonoValor.Value = componentes[j - 1].CuotaSaldo.Value.Value;
                                            valorPagadoCuota += componentes[j - 1].CuotaSaldo.Value.Value;

                                            valoresComponentes[compCarteraID] = valoresComponentes[compCarteraID] + componentes[j - 1].CuotaSaldo.Value.Value;
                                        }
                                        #endregion
                                    }

                                    abono = abonoAct;
                                    pago.ValorCuota.Value = abono;
                                }
                                #endregion
                            }

                            //Asigna el valor pagado de la cuota
                            cuota.VlrPagadoCuota.Value = valorPagadoCuota;

                            //Si es la ultima cuota y hay saldo a favor lo agrega
                            if (cuota.CuotaID.Value == credito.Plazo.Value && saldoAFavor != null)
                                cuota.VlrPagadoCuota.Value += saldoAFavor.AbonoValor.Value;
                            #endregion
                            #region Asigna la información a la tabla

                            foreach (KeyValuePair<string, decimal> comp in valoresComponentes)
                            {
                                if (cacheComponentes.ContainsKey(comp.Key))
                                    compDTO = cacheComponentes[comp.Key];
                                else
                                {
                                    compDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, comp.Key, true, false);
                                    cacheComponentes.Add(comp.Key, compDTO);
                                }

                                DataRow fila = tablaCreditos.NewRow();
                                fila["NumDoc"] = credito.NumeroDoc.Value.Value.ToString();
                                fila["Credito"] = pago.Libranza.Value.Value.ToString();
                                fila["ClienteID"] = pago.ClienteID.Value;
                                fila["Nombre"] = cliente.Descriptivo.Value;
                                fila["Cuota"] = cuota.CuotaID.Value.Value;
                                fila["ComponenteCarteraID"] = compDTO.ID.Value;
                                fila["Descriptivo"] = compDTO.Descriptivo.Value;
                                fila["Valor"] = Convert.ToInt32(comp.Value);

                                tablaCreditos.Rows.Add(fila);
                            }

                            #endregion
                        }
                    }
                    #endregion
                }

                #endregion
                #region Agega las columnas de la tabla final

                //Trae toda la lista de todos los componentes
                DataTable distinctComps = tablaCreditos.DefaultView.ToTable(true, "ComponenteCarteraID");
                DataColumn[] colsComps = new DataColumn[distinctComps.Rows.Count];
                for (int i = 0; i < distinctComps.Rows.Count; ++i)
                {
                    DataRow row = distinctComps.Rows[i];
                    colsComps[i] = new DataColumn(row["ComponenteCarteraID"].ToString());
                }

                //Agrega las columnas a la tabla final
                //tablaFinal.Columns.Add("NumDoc");
                tablaFinal.Columns.Add("ClienteID", typeof(int));
                tablaFinal.Columns.Add("Nombre");
                tablaFinal.Columns.Add("Credito", typeof(int));
                tablaFinal.Columns.Add("Cuota", typeof(int));
                tablaFinal.Columns.Add("Total", typeof(int));
                tablaFinal.Columns.AddRange(colsComps);

                #endregion
                #region Organiza la info

                //Lista de créditos(Libranza, NumDoc y Cliente)
                DataTable distinctCreditos = tablaCreditos.DefaultView.ToTable(true, "Credito", "NumDoc", "ClienteID", "Nombre", "Cuota");

                //Recorre los créditos
                for (int i = 0; i < distinctCreditos.Rows.Count; ++i)
                {
                    var total = 0;
                    DataRow filaTablaFinal = tablaFinal.NewRow();

                    //Carga en la fila la información del crédito
                    var numDoc = Convert.ToInt32(distinctCreditos.Rows[i]["NumDoc"]);
                    var libranza = Convert.ToInt32(distinctCreditos.Rows[i]["Credito"]);
                    var clienteID = Convert.ToInt32(distinctCreditos.Rows[i]["ClienteID"]);
                    var cuota = Convert.ToInt32(distinctCreditos.Rows[i]["Cuota"]);
                    var nombre = distinctCreditos.Rows[i]["Nombre"];

                    filaTablaFinal["Credito"] = libranza;
                    filaTablaFinal["ClienteID"] = clienteID;
                    filaTablaFinal["Nombre"] = nombre;
                    filaTablaFinal["Cuota"] = cuota;

                    //Recorre los componentes
                    for (int j = 0; j < colsComps.Count(); ++j)
                    {
                        var val =
                        (
                            from DataRow dr in tablaCreditos.Rows
                            where Convert.ToInt32(dr["NumDoc"]) == numDoc &&
                                Convert.ToInt32(dr["Cuota"]) == cuota && 
                                dr["ComponenteCarteraID"].ToString() == colsComps[j].ToString()
                            select  Convert.ToInt32(dr["Valor"])
                        ).FirstOrDefault();

                        filaTablaFinal[colsComps[j].ToString()] = val;
                        if (!string.IsNullOrWhiteSpace(val.ToString()))
                            total += Convert.ToInt32(val);
                    }

                    filaTablaFinal["Total"] = total;                
                    tablaFinal.Rows.Add(filaTablaFinal);
                }

                #endregion
                #region Actualiza los nombres de las columas para los componentes

                for (int i = 5; i < tablaFinal.Columns.Count; ++i)
                {
                    string colName = tablaFinal.Columns[i].ColumnName;
                    DTO_ccCarteraComponente basicDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, colName, true, false);

                    tablaFinal.Columns[i].ColumnName = basicDTO.Descriptivo.Value + "(" + colName + ")";
                    if (columnsComp != null)
                        columnsComp.Add(colName, basicDTO.Descriptivo.Value + "(" + colName + ")");
                }

                #endregion
                #region Ordena la tabla

                if (tablaFinal.Rows.Count == 0)
                    return tablaFinal;

                DataTable results = tablaFinal.AsEnumerable()
                   .OrderBy(r => r.Field<int>("ClienteID"))
                   .ThenBy(r => r.Field<int>("Credito"))
                   .ThenBy(r => r.Field<int>("Cuota"))
                   .CopyToDataTable();

                #endregion

                return results;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "RecaudosMasivos_GetRelacionPagos " + (credito != null? credito.Libranza.Value.ToString() : ""));
                return null;
            }
        }

        #endregion

        #region Cobro Juridico
        /// <summary>
        /// Actualiza la info del cobro juridico
        /// </summary>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <param name="libranza">Identificador de la libranza</param>
        public void ccCJHistorico_RecalcularInteresCJ(DateTime fechaCorte,int libranza, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_ccCJHistorico = (DAL_ccCJHistorico)base.GetInstance(typeof(DAL_ccCJHistorico), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_ccCJHistorico.DAL_ccCJHistorico_RecalcularInteresCJ(fechaCorte,libranza);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ccCJHistorico_RecalcularInteresCJ");
                throw ex;
            }
            finally
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }

        }

        #endregion

        #region Reports

        /// <summary>
        /// Trae cobros juridicos de cartera para informe
        /// </summary>
        /// <param name="claseDeuda">Tipo de deuda del cliente</param>
        /// <returns>Lista de cobros jur</returns>
        public List<DTO_ReporCobroJuridico> Report_CobroJuridicoGet(byte claseDeuda, string cliente, string obligacion)
        {
            List<DTO_ReporCobroJuridico> result = new List<DTO_ReporCobroJuridico>();
            this._dal_ReportesCartera = (DAL_ReportesCartera)base.GetInstance(typeof(DAL_ReportesCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            result = this._dal_ReportesCartera.DAL_ReportesCartera_Cc_CobroJuridicoGet(claseDeuda, cliente, obligacion);
            return result;
        }

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="cliente">Cliente</param>
        /// <param name="libranza">Libranza</param>
        /// <param name="claseDeuda">CLase de deuda</param>
        /// <returns></returns>
        public DataTable Report_Cc_CobroJuridicoToExcel(int documentoID, byte tipoReporte, string cliente, string libranza, byte claseDeuda)
        {
            try
            {
                DataTable result;
                this._dal_ReportesCartera = (DAL_ReportesCartera)this.GetInstance(typeof(DAL_ReportesCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_ReportesCartera.DAL_ReportesCartera_Cc_CobroJuridicoToExcel(documentoID, tipoReporte, cliente, libranza,claseDeuda);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cc_CarteraToExcel");
                throw ex;
            }
        }


        #endregion
    }
}//NameSpace

