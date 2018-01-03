using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Resultados;
using NewAge.ADO;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System.Net;
using System.Configuration;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;
using System.Diagnostics;
using System.Linq;
using NewAge.Negocio.PostSharpAspects;
using System.Data;

namespace NewAge.Negocio
{
    [BussinessExceptionManager]
    public class ModuloNomina : ModuloBase
    {
        #region Variables

        #region DALs

        private DAL_noNovedadesNomina _dal_noNovedadesNomina = null;
        private DAL_noPrestamo _dal_noPrestamo = null;
        private DAL_noNovedadesContrato _dal_noNovedadesContrato = null;
        private DAL_noTraslado _dal_noTraslado = null;
        private DAL_noPlanillaDiariaTrabajo _dal_noPlanillaDiariaTrabajo = null;
        private DAL_noCompensatorio _dal_noCompensatorio = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_MasterComplex _dal_MasterComplex = null;
        private DAL_noLiquidacionPreliminar _dal_noLiquidacionPreliminar = null;
        private DAL_noLiquidacionesDetalle _dal_noLiquidacionesDetalle = null;
        private DAL_noLiquidacionesDocu _dal_noLiquidacionesDocu = null;
        private DAL_noPlanillaAportesDeta _dal_noPlanillaAportesDeta = null;
        private DAL_noEmpleado _dal_noEmpleado = null;
        private DAL_noBeneficiosxEmpleado _dal_noBeneficiosxEmpleado = null;
        private DAL_noProvisionDeta _dal_noProvisionDeta = null;
        private DAL_ReportesNomina _dal_ReportesNomina = null;
        private DAL_noPagoPlanillaAportesDocu _dal_noPagoPlanillaAportesDocu = null;
        private DAL_glControl _dal_glControl = null;

        #endregion

        #region DTOs

        DTO_noConceptoNOM conceptoAuxilioTransporte = null;
        DTO_noConceptoNOM conceptoFondoSalud = null;
        DTO_noConceptoNOM conceptoFondoPension = null;
        DTO_noConceptoNOM conceptoFondoSolidaridad = null;
        DTO_noConceptoNOM conceptoRteFuente = null;
        DTO_noConceptoNOM conceptoVacacionesTiempo = null;
        DTO_noConceptoNOM conceptoVacacionesDinero = null;
        DTO_noConceptoNOM conceptoAnticipoPrimaServicios = null;
        DTO_noConceptoNOM conceptoPrimaServicios = null;
        DTO_glDatosAnuales datosAnuales = null;

        //Guarda el listado de las liquidaciones preliminares del empleado
        List<DTO_noLiquidacionPreliminar> lLiquidacionesEmpleado = null;
        List<DTO_MasterBasic> lReteFuenteBasica = null;
        List<DTO_MasterBasic> lReteFuenteMinima = null;

        #endregion

        #region Modelos

        private ModuloGlobal _moduloGlobal = null;
        private ModuloCuentasXPagar _moduloCXP = null;
        private ModuloContabilidad _moduloContabilidad = null;

        #endregion

        #region Variables Control

        int ultimaQuincenaProcesada = 0;
        int esQuicenal = 0;
        int horasDia = 8;

        string no_ConceptoSueldo = string.Empty;
        string no_ConceptoSalarioIntegral = string.Empty;
        string no_ConceptoSenaLectivo = string.Empty;
        string no_ConceptoSenaProductivo = string.Empty;
        string no_ConceptoPensionJubilacion = string.Empty;
        string no_ConceptoSustitucionPatronal = string.Empty;
        string no_ConceptoAuxilioTransporte = string.Empty;
        string no_ConceptoAjAuxTransporte = string.Empty;
        string no_ConceptoFondoSalud = string.Empty;
        string no_ConceptoFondoPension = string.Empty;
        string no_ConceptoFondoSolidaridad = string.Empty;
        string no_ConceptoRteFuente = string.Empty;
        string no_ConceptoVacacionesTiempo = string.Empty;
        string no_ConceptoVacacionesDinero = string.Empty;
        string no_ConceptoAnticipoPrimaServicios = string.Empty;
        string no_ConceptoPrimaServicios = string.Empty;


        bool no_IndLiquSueldoSubsTransp2Quincena = false;
        bool no_IndLiquRteFuente2Quincena = false;
        bool no_IndLiquAnticipoPrima = false;

        decimal nPorExcento = 0;
        decimal nPorFondos = 0;
        decimal nPorDependi = 0;
        decimal nTopeFondos = 0;
        decimal nTopeExcento = 0;
        decimal nTopeVivien = 0;
        decimal nTopeSalud = 0;
        decimal nTopeDepend = 0;
        decimal nSdoMaxBonos = 0;
        decimal nTopeBonos = 0;

        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Aplicacion
        /// </summary>
        /// <param name="conn">conexion</param>
        public ModuloNomina(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Aprobaciones - Pagos

        #region Aprobacion Nomina

        /// <summary>
        /// Aprueba ó Rechaza una solicitud de Pago de Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>   
        public DTO_TxResult Nomina_AprobarLiquidacion(List<DTO_noNominaPreliminar> liquidaciones, string actividadFlujoID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / liquidaciones.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.PagoNominaAprob);
            batchProgress[tupProgress] = 1;

            foreach (var liq in liquidaciones)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                if (liq.Seleccionar.Value.Value)
                {
                    noLiquidacionBase objLiquidacion = new noLiquidacionNomina(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    objLiquidacion.DocumentoID = liq.DocControl.DocumentoID.Value.Value;
                    objLiquidacion.DocCtrl = liq.DocControl;
                    result = objLiquidacion.Aprobar(liq, actividadFlujoID);
                }
            }
            return result;
        }

        #endregion

        #region Aprobacion Otros

        /// <summary>
        /// Trae las liquidaciones hacia Terceros
        /// </summary>
        /// <param name="documentID">Identificador de Documento</param>
        /// <param name="Periodo">Periodo</param>
        /// <returns></returns>
        public List<DTO_noLiquidacionOtro> Nomina_GetLiquidacionOtros(List<int> documents, DateTime Periodo)
        {
            List<DTO_noLiquidacionOtro> liqOtros = new List<DTO_noLiquidacionOtro>();
            List<DTO_noLiquidacionOtroDetalle> detalle = new List<DTO_noLiquidacionOtroDetalle>();

            foreach (var documentID in documents)
            {
                //Trae las liquidaciones pendientes para el periodo
                var liquidacionesPeriodo = this.Nomina_NominaPreliminarGet("", Periodo, false);
                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, false);

                foreach (var liq in liquidacionesPeriodo.Where(x => x.DocControl.Estado.Value.Value == (byte)EstadoDocControl.Aprobado))
                {
                    foreach (var q in liq.Detalle)
                    {
                        DTO_noConceptoNOM conceptoNOM = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, q.ConceptoNOID.Value, true, false);
                        string terceroID = string.Empty;

                        if (conceptoNOM.TipoTercero.Value == 3)
                        {
                            if (string.IsNullOrEmpty(conceptoNOM.FondoNOID.Value))
                                terceroID = conceptoNOM.TerceroID.Value;
                            else
                            {
                                DTO_noFondo fondo = (DTO_noFondo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noFondo, conceptoNOM.FondoNOID.Value, true, false);
                                terceroID = fondo.TerceroID.Value;
                            }

                            detalle.Add(new DTO_noLiquidacionOtroDetalle()
                            {
                                NumeroDoc = new UDT_Consecutivo() { Value = liq.DocControl.NumeroDoc.Value.Value },
                                Estado = new UDTSQL_smallint() { Value = liq.DocControl.Estado.Value.Value },
                                TerceroID = new UDT_TerceroID() { Value = terceroID },
                                EmpleadoID = new UDT_EmpleadoID() { Value = liq.EmpleadoID.Value },
                                EmpleadoDesc = new UDT_Descriptivo() { Value = liq.NombreEmpleado.Value },
                                ConceptoNOID = new UDT_ConceptoNOID() { Value = q.ConceptoNOID.Value },
                                ConceptoNODesc = new UDT_Descriptivo() { Value = q.ConceptoNODesc.Value },
                                Valor = new UDT_Valor() { Value = q.Valor.Value },
                                DocumentoID = new UDT_BasicID() { Value = documento.ID.Value },
                                DocumentoDesc = new UDT_Descriptivo() { Value = documento.Descriptivo.Value }
                            }
                                        );
                        }
                    }
                }

            }

            foreach (var liq in detalle)
            {
                DTO_noLiquidacionOtro dto = null;
                if (liqOtros.Any(x => x.Tercero.ID.Value == liq.TerceroID.Value))
                    liqOtros.Where(x => x.Tercero.ID.Value == liq.TerceroID.Value).FirstOrDefault().Preliminar.Add(liq);
                else
                {
                    dto = new DTO_noLiquidacionOtro();
                    dto.Tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, liq.TerceroID.Value, true, false);
                    dto.TerceroID.Value = dto.Tercero.ID.Value;
                    dto.TerceroDesc.Value = dto.Tercero.Descriptivo.Value;
                    dto.Valor.Value += liq.Valor.Value;
                    liqOtros.Add(dto);
                }
            }

            return liqOtros;
        }

        /// <summary>
        /// Aprueba una solicitud de Pago a Otros
        /// </summary>
        /// <param name="terceros">listado de terceros</param>
        /// <param name="insideAnotherTx">si viene ó no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_AprobarPagosTerceros(List<DTO_NominaPlanillaContabilizacion> terceros, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();


            DTO_Comprobante comprobante = new DTO_Comprobante();
            DTO_coComprobante comp = null;
            DTO_glDocumentoControl glCtrl = null;
            DTO_glDocumentoControl ctrlCxP = null;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noPlanillaAportesDeta = (DAL_noPlanillaAportesDeta)this.GetInstance(typeof(DAL_noPlanillaAportesDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCXP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);


            try
            {
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string docContable = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_DocumContableLiquidaciones);

                string lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string conceptoCxPcontrol = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPPagoAportes);
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TerceroEmpresaPagaplaniilla);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string cuentaParaFiscales = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CxPParaFiscales);
                DTO_coPlanCuenta ctaParaFiscales = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuentaParaFiscales, true, false);
                DTO_cpConceptoCXP conceptoCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, conceptoCxPcontrol, true, false);
                DTO_coDocumento docContableCXP = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, conceptoCxP.coDocumentoID.Value, true, false);
                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));

                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, DateTime.Now);

                List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
                DTO_ComprobanteFooter footer = null;

                #region Crea el Documento de Pago

                int numeroDoc = 0;
                glCtrl = new DTO_glDocumentoControl();
                glCtrl.Fecha.Value = DateTime.Now;
                glCtrl.DocumentoID.Value = AppDocuments.PagoOtrosAprob;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
                glCtrl.FechaDoc.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
                glCtrl.PrefijoID.Value = prefijDef;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.TasaCambioCONT.Value = tc;
                glCtrl.TasaCambioDOCU.Value = tc;
                glCtrl.TerceroID.Value = terceroPorDefecto;
                glCtrl.DocumentoTercero.Value = string.Empty;
                glCtrl.CuentaID.Value = string.Empty;
                glCtrl.MonedaID.Value = mdaLoc;
                glCtrl.ProyectoID.Value = proyectoXDef;
                glCtrl.CentroCostoID.Value = cCosto;
                glCtrl.LineaPresupuestoID.Value = lineaPres;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                if (result.Result == ResultValue.OK)
                {
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.PagoOtrosAprob, glCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = resultGLDC.Message;
                        return result;
                    }
                    else
                    {
                        numeroDoc = Convert.ToInt32(resultGLDC.Key);
                        glCtrl.NumeroDoc.Value = numeroDoc;
                    }
                }

                #endregion

                #region Agregar Footer Comprobante Planilla Aportes
                foreach (var item in terceros)
                {
                    //Variables de proceso
                    string operacion = string.Empty;
                    string linePresupuestal = string.Empty;
                    string conceptoCargoID = string.Empty;
                    DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                    string concepSLD = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSalud);
                    string concepPEN = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoPension);

                    string claseBYSSENA = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySSena);
                    string claseBYSCAJA = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySCajadeCompensacion);
                    string claseBYSICBF = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySICBF);
                    string claseBYSARP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySArp);
                    string cuentaARP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CuentaARP);

                    switch (item.Liquidacion.Value.Value)
                    {

                        case (byte)TerceroPlanilla.Caja:
                            {
                                item.CuentaID.Value = ctaParaFiscales.ID.Value;
                                break;
                            }
                        case (byte)TerceroPlanilla.SENA:
                            {
                                item.CuentaID.Value = ctaParaFiscales.ID.Value;
                                break;
                            }
                        case (byte)TerceroPlanilla.ICBF:
                            {
                                item.CuentaID.Value = ctaParaFiscales.ID.Value;
                                break;
                            }
                        case (byte)TerceroPlanilla.ARP:
                            {
                                item.CuentaID.Value = cuentaARP;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    #region Crea el detalle comprobante

                    footer = new DTO_ComprobanteFooter();
                    comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docContableCXP.ComprobanteID.Value, true, false);
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, item.CuentaID.Value, true, false);
                    DTO_glConceptoSaldo glConceptoSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, coPlanCta.ConceptoSaldoID.Value, true, false);

                    decimal vlrParcial = item.Valor.Value.Value + item.Valor2.Value.Value;

                    footer = new DTO_ComprobanteFooter();
                    footer.CentroCostoID.Value = glCtrl.CentroCostoID.Value;
                    footer.CuentaID.Value = coPlanCta.ID.Value;
                    footer.ConceptoCargoID.Value = concCargoDef;
                    footer.DocumentoCOM.Value = "0";
                    footer.Descriptivo.Value = glCtrl.Descripcion.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = glCtrl.NumeroDoc.Value;
                    footer.LineaPresupuestoID.Value = item.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = lugarGeografico;
                    footer.PrefijoCOM.Value = glCtrl.PrefijoID.Value;
                    footer.ProyectoID.Value = proyectoXDef;
                    footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    footer.TerceroID.Value = item.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = vlrParcial * tc;
                    footer.vlrMdaLoc.Value = vlrParcial;
                    footer.vlrMdaOtr.Value = vlrParcial;
                    footer.DatoAdd10.Value = glCtrl.NumeroDoc.Value.ToString();
                    footer.Descriptivo.Value = "PAGO PLANILLA NOMINA";
                    lFooter.Add(footer);

                    #endregion
                }

                #endregion

                #region Crea C x P Asociada al Doc

                decimal valorTotal = terceros.Where(y => y.Seleccionar.Value.Value).Sum(x => x.Valor.Value.Value + x.Valor2.Value.Value);

                glCtrl.TerceroID.Value = terceroPorDefecto;
                object obj = this._moduloCXP.CuentasXPagar_Generar(glCtrl, conceptoCxP.ID.Value, valorTotal, lFooter, ModulesPrefix.no, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    return result;
                }
                ctrlCxP = (DTO_glDocumentoControl)obj;
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);

                #endregion

                #region Actualiza el indicador de pago en los documentos

                this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdatePagadoPlanillaInd(true, periodo);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
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
                        base._mySqlConnectionTx.Commit();
                        #region Asigna consecutivos
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        ctrlCxP.ComprobanteID.Value = comp.ID.Value;
                        ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();

            }
        }

        #endregion

        #region Pagos Nomina

        /// <summary>
        /// Realiza el Pago de la Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_PagoNomina(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones, string actividadFlujoID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_Comprobante comprobante = new DTO_Comprobante();
            DTO_coComprobante comp = null;
            DTO_glDocumentoControl glCtrl = null;
            DTO_glDocumentoControl ctrlCxP = null;

            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCXP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);


            string mdaLoc = string.Empty;
            string mdaExt = string.Empty;
            string docContable = string.Empty;
            string lineaPres = string.Empty;
            string cCosto = string.Empty;
            string proyectoXDef = string.Empty;
            string lugarGeografico = string.Empty;
            string conceptoCxPcontrol = string.Empty;
            string terceroPorDefecto = string.Empty;
            string prefijDef = string.Empty;
            string concCargoDef = string.Empty;


            try
            {
                //Valida que existan liquidaciónes para ser procesadas
                if (liquidaciones.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "No existen liquidaciónes";
                    return result;
                }
                else
                {
                    this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (var itemDoc in liquidaciones)
                    {
                        this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdatePagadoInd(itemDoc.NumeroDoc.Value.Value, true);
                    }
                }

                mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                docContable = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_DocumContableLiquidaciones);

                lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                conceptoCxPcontrol = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPagosNomina);
                terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);

                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, DateTime.Now);
                decimal valorTotal = liquidaciones.Sum(x => x.DocLiquidacion.Valor.Value.Value);
                if (valorTotal < 0)
                    valorTotal = 0;

                List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
                DTO_ComprobanteFooter footer = null;

                #region Asigna Valores a la CxP

                int numeroDoc = 0;
                glCtrl = new DTO_glDocumentoControl();
                glCtrl.Fecha.Value = DateTime.Now;
                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.PeriodoDoc.Value = periodo;
                glCtrl.FechaDoc.Value = fechaDoc;
                glCtrl.PrefijoID.Value = prefijDef;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.TasaCambioCONT.Value = tc;
                glCtrl.TasaCambioDOCU.Value = tc;
                glCtrl.TerceroID.Value = terceroPorDefecto;
                glCtrl.DocumentoTercero.Value = string.Empty;
                glCtrl.CuentaID.Value = string.Empty;
                glCtrl.MonedaID.Value = mdaLoc;
                glCtrl.ProyectoID.Value = proyectoXDef;
                glCtrl.CentroCostoID.Value = cCosto;
                glCtrl.LineaPresupuestoID.Value = lineaPres;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.PeriodoUltMov.Value = periodo;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                if (result.Result == ResultValue.OK)
                {
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                    }
                    else
                    {
                        numeroDoc = Convert.ToInt32(resultGLDC.Key);
                        glCtrl.NumeroDoc.Value = numeroDoc;
                    }
                }

                #endregion
                #region Comprobante Footer

                #region Crea el detalle comprobante

                footer = new DTO_ComprobanteFooter();

                DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);
                DTO_coPlanCuenta coPlanCta = coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableNomina.CuentaLOC.Value, true, false);
                DTO_glConceptoSaldo glConceptoSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, coPlanCta.ConceptoSaldoID.Value, true, false);

                footer = this.CrearComprobanteFooter(glCtrl, coPlanCta, glConceptoSaldo, tc, concCargoDef, lugarGeografico, lineaPres, valorTotal, valorTotal * tc, false);
                footer.Descriptivo.Value = "PAGO NOMINA";
                lFooter.Add(footer);

                #endregion

                #endregion
                #region Crea C x P Asociada al Doc

                object obj = this._moduloCXP.CuentasXPagar_Generar(glCtrl, conceptoCxPcontrol, valorTotal, lFooter, ModulesPrefix.no, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    return result;
                }
                ctrlCxP = (DTO_glDocumentoControl)obj;
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
                #endregion

                return result;

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Nomina_PagoNomina");
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
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        ctrlCxP.ComprobanteID.Value = comp.ID.Value;
                        ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #endregion

        #region Aumento Salario

        /// <summary>
        /// Actualiza el Ajuste de Salario en la tabla empleados y crea la novedad de contrato
        /// </summary>
        /// <param name="lSalarios">listado de ajustes en salarios</param>
        /// <param name="insideAnotherTx">verifica si viene de una transacción</param>
        /// <returns>listado de resultados</returns>
        public DTO_TxResult Nomina_UpdSalarioEmpleado(List<DTO_AumentoSalarial> lSalarios, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            //Filtra la lista con los cambios reales de sueldo
            lSalarios = lSalarios.Where(x => x.Sueldo.Value != x.NuevoSueldo.Value).ToList();

            try
            {
                foreach (var salario in lSalarios)
                {

                    string conceptoNovContrato = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoAjSueldo);
                    DTO_noEmpleado empleado = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, salario.EmpleadoID.Value, true, false);
                    empleado.Sueldo.Value = salario.NuevoSueldo.Value;
                    empleado.FechaActSueldo.Value = salario.FechaAumento.Value;
                    //Actualiza salario en Master de Empleados
                    this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_MasterSimple.DAL_MasterSimple_Update(empleado, true);
                    //Crea la novedad de contrato asociada al ajuste de salario

                    List<DTO_noNovedadesContrato> lnovedadContrato = new List<DTO_noNovedadesContrato>();
                    DTO_noNovedadesContrato novedadContrato = new DTO_noNovedadesContrato();

                    novedadContrato.EmpresaID.Value = this.Empresa.ID.Value;
                    novedadContrato.EmpleadoID.Value = empleado.ID.Value;
                    novedadContrato.ContratoNONovID.Value = conceptoNovContrato;
                    novedadContrato.FechaInicial.Value = salario.FechaAumento.Value;
                    novedadContrato.FechaFinal.Value = salario.FechaAumento.Value;
                    novedadContrato.ActivaInd.Value = true;
                    novedadContrato.Valor.Value = salario.Aumento.Value + salario.Ajuste.Value;
                    novedadContrato.Observacion.Value = "CONCEPTO AUMENTO DE SALARIO";
                    novedadContrato.Documento.Value = string.Empty;
                    novedadContrato.ContratoNOID.Value = empleado.ContratoNOID.Value;

                    lnovedadContrato.Add(novedadContrato);

                    this.Nomina_AddNovedadesContrato(lnovedadContrato, true);

                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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

        #region Compensación Flexible

        /// <summary>
        /// Obtiene el listado de beneficios por Empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de beneficios</returns>
        public List<DTO_noBeneficiosxEmpleado> Nomina_GetBeneficioXEmpleado(string empleadoID)
        {
            try
            {
                this._dal_noBeneficiosxEmpleado = (DAL_noBeneficiosxEmpleado)this.GetInstance(typeof(DAL_noBeneficiosxEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_noBeneficiosxEmpleado.DAL_noBeneficiosxEmpleado_Get(empleadoID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_GetBeneficioXEmpleado");
                throw exception;
            }
        }


        /// <summary>
        /// Adiciona un listado de beneficios por empleado
        /// </summary>
        /// <param name="lbeneficios">listado de beneficios</param>
        /// <param name="insideAnotherTx">verifica si viene ó no de una transacción</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_AddBeneficioXEmpleado(List<DTO_noBeneficiosxEmpleado> lbeneficios, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noBeneficiosxEmpleado = (DAL_noBeneficiosxEmpleado)this.GetInstance(typeof(DAL_noBeneficiosxEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var item in lbeneficios)
                {
                    try
                    {
                        List<DTO_noBeneficiosxEmpleado> lbeneficiosBD = this.Nomina_GetBeneficioXEmpleado(item.EmpleadoID.Value);

                        if (lbeneficiosBD.Where(x => x.EmpresaID.Value == item.EmpresaID.Value && x.EmpleadoID.Value == item.EmpleadoID.Value && x.CompFlexibleID.Value == item.CompFlexibleID.Value).Count() > 0)
                            this._dal_noBeneficiosxEmpleado.DAL_noBeneficiosxEmpleado_Upd(item);
                        else
                            this._dal_noBeneficiosxEmpleado.DAL_noBeneficiosxEmpleado_Add(item);

                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail fail = new DTO_TxResultDetail();
                        fail.Key = lbeneficios.IndexOf(item).ToString();
                        fail.Message = ex.Message;
                        result.Details.Add(fail);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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

        #region Compensatorios

        /// <summary>
        /// Obtiene el historico de compensatorios
        /// </summary>
        /// <returns>listado de compensatorios</returns>
        public List<DTO_noCompensatorios> Nomina_GetCompensatorios()
        {
            try
            {
                _dal_noCompensatorio = (DAL_noCompensatorio)this.GetInstance(typeof(DAL_noCompensatorio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_noCompensatorio.DAL_noCompensatorio_GetCompensatorios();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_GetCompensatorios");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la informacion del compensatorio
        /// </summary>
        /// <param name="compesatorio">objeto compensatorio</param>
        /// <returns>true si la operacion es exitosa</returns>
        public DTO_TxResult Nomina_UpdCompensatorio(List<DTO_noCompensatorios> compesatorio, bool insideAnotherTx)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                foreach (var comp in compesatorio)
                {
                    try
                    {
                        this._dal_noCompensatorio.DAL_noCompesatorios_UpdCompensatorio(comp);
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "OK"
                        };
                        result.Details.Add(detalle);
                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "NOK",
                            Key = ex.Message
                        };
                        result.Details.Add(detalle);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_UpdCompensatorio");
                throw exception;
            }
        }

        #endregion

        #region Contabilizacion

        /// <summary>
        /// Obtiene el consolidado de las liquidacion de nomina X periodo
        /// </summary>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <returns></returns>
        public List<DTO_NominaContabilizacion> noLiquidacionesDocu_GetTotal(DateTime periodo)
        {
            try
            {
                DAL_noLiquidacionesDocu dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                var results = dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetTotal(periodo);
                int totalDetalle = 0;
                foreach (var item in results)
                {
                    item.Detalle = dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetTotalDetalle(periodo, item.Liquidacion.Value.Value);
                    totalDetalle++;
                }
                if (totalDetalle > 0)
                {
                    results.FirstOrDefault().Aportes = dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetTotalPlanilaDetalle(periodo, (int)TipoLiquidacion.Pl);
                    results.First().Provisiones = dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetTotalProvisionesDetalle(periodo, (int)TipoLiquidacion.Pr);
                }
                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_SearchEmpleados");
                throw exception;
            }
        }

        #endregion

        #region Empleados


        /// <summary>
        /// Generar un listado de empleados de acuerdo al periodo y el estado de la liquidación
        /// </summary>
        /// <param name="documentoID">ID del documento</param>
        /// <param name="periodo">perido</param>
        /// <param name="estadoLiquidacion">estado de la liquidación</param>
        /// <param name="procesadaInd">indica si el documento ya fue procesada</param>
        /// <param name="estadoEmpleado">estado del empleado</param>
        /// <returns>listado de empleados</returns>
        public List<DTO_noEmpleado> Nomina_noEmpleadoGet(int documentoID, DateTime periodo, byte estadoLiquidacion, bool procesadaInd, byte estadoEmpleado)
        {
            try
            {
                DAL_noEmpleado dal_noEmpleado = (DAL_noEmpleado)this.GetInstance(typeof(DAL_noEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return dal_noEmpleado.DAL_noEmpleadoGet(documentoID, periodo, estadoLiquidacion, procesadaInd, estadoEmpleado);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_noEmpleadoGet");
                throw exception;
            }
        }

        /// <summary>
        /// Lista los empleados segun el estado
        /// </summary>
        /// <param name="activoInd">estado</param>
        /// <param name="empleado">empleado</param>
        /// <returns>lista de empleados</returns>
        public List<DTO_noEmpleado> Nomina_SearchEmpleados(bool activoInd, string empleado)
        {
            try
            {
                DAL_noEmpleado dal_noEmpleado = (DAL_noEmpleado)this.GetInstance(typeof(DAL_noEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return dal_noEmpleado.DAL_noEmpleado_SearchEmpleados(activoInd, empleado);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_SearchEmpleados");
                throw exception;
            }
        }

        /// <summary>
        /// Reincorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        public DTO_TxResult Nomina_ReinCorporacionEmpleado(DTO_noEmpleado empleado, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                //Actualiza la informacion del empleado
                this._dal_noEmpleado = (DAL_noEmpleado)this.GetInstance(typeof(DAL_noEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                empleado.ContratoNOID.Value = this._dal_noEmpleado.DAL_noEmpleado_GetNewContratoID();
                empleado.ActivoInd.Value = true;
                empleado.Estado.Value = (int)EstadoEmpleado.Activo;
                this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple.DocumentID = AppMasters.noEmpleado;
                this._dal_MasterSimple.DAL_MasterSimple_Update(empleado, true);

                string conceptoIngreso = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoIngresoEmpleado);

                //Se crea la novedad de contrato asociada al ingreso del empleado

                List<DTO_noNovedadesContrato> lnovedades = new List<DTO_noNovedadesContrato>();

                DTO_noNovedadesContrato novedadContrato = new DTO_noNovedadesContrato();
                novedadContrato.EmpresaID.Value = this.Empresa.ID.Value;
                novedadContrato.EmpleadoID.Value = empleado.ID.Value;
                novedadContrato.ContratoNONovID.Value = conceptoIngreso;
                novedadContrato.ContratoNOID.Value = empleado.ContratoNOID.Value;
                novedadContrato.FechaInicial.Value = empleado.FechaIngreso.Value;
                novedadContrato.FechaFinal.Value = empleado.FechaIngreso.Value;
                novedadContrato.ActivaInd.Value = true;
                novedadContrato.Valor.Value = empleado.Sueldo.Value;
                novedadContrato.Observacion.Value = "REINCORPORACION";
                novedadContrato.Documento.Value = string.Empty;
                lnovedades.Add(novedadContrato);

                this.Nomina_AddNovedadesContrato(lnovedades, true);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Incorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        public DTO_TxResult Nomina_IncorporacionEmpleado(DTO_noEmpleado empleado, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();



            try
            {

                if (this.noEmpleado_CountTerceroID(empleado.TerceroID.Value) != 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_No_TerceroID;
                    return result;
                }
                //Actualiza la informacion del empleado
                this._dal_noEmpleado = (DAL_noEmpleado)this.GetInstance(typeof(DAL_noEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                empleado.ContratoNOID.Value = this._dal_noEmpleado.DAL_noEmpleado_GetNewContratoID();
                empleado.ActivoInd.Value = true;
                empleado.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple.DocumentID = AppMasters.noEmpleado;
                DTO_TxResultDetail detalleResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(empleado);
                result.Details = new List<DTO_TxResultDetail>();
                result.Details.Add(detalleResult);

                if (detalleResult.Message == ResultValue.OK.ToString())
                {
                    string conceptoIngreso = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoIngresoEmpleado);
                    if (!string.IsNullOrEmpty(conceptoIngreso))
                    {
                        //Se crea la novedad de contrato asociada al ingreso del empleado

                        List<DTO_noNovedadesContrato> lnovedades = new List<DTO_noNovedadesContrato>();

                        DTO_noNovedadesContrato novedadContrato = new DTO_noNovedadesContrato();
                        novedadContrato.EmpresaID.Value = this.Empresa.ID.Value;
                        novedadContrato.EmpleadoID.Value = empleado.ID.Value;
                        novedadContrato.ContratoNONovID.Value = conceptoIngreso;
                        novedadContrato.ContratoNOID.Value = empleado.ContratoNOID.Value;
                        novedadContrato.FechaInicial.Value = empleado.FechaIngreso.Value;
                        novedadContrato.FechaFinal.Value = empleado.FechaIngreso.Value;
                        novedadContrato.ActivaInd.Value = true;
                        novedadContrato.Valor.Value = empleado.Sueldo.Value;
                        novedadContrato.Observacion.Value = "INCORPORACION";
                        novedadContrato.Documento.Value = string.Empty;
                        lnovedades.Add(novedadContrato);

                        result = this.Nomina_AddNovedadesContrato(lnovedades, true);
                        if (result.Result == ResultValue.NOK)
                        {
                            result.ResultMessage = DictionaryMessages.Err_No_existConceptoNovContrato + "&&" + conceptoIngreso;
                        }
                    }
                    else
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + this.Empresa.NumeroControl + ((int)ModulesPrefix.no).ToString() + AppControl.no_ConceptoIngresoEmpleado + "&&" + string.Empty;
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = detalleResult.DetailsFields[0].Message;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Trae el estado de las liquidaciones del empleado del periodo de liquidación en curso
        /// </summary>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Estado Liquidaciones</returns>
        public DTO_noEstadoLiquidaciones Nomina_GetEstadoLiquidaciones(string empleadoID)
        {
            try
            {
                DAL_noEmpleado dal_noEmpleado = (DAL_noEmpleado)this.GetInstance(typeof(DAL_noEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return dal_noEmpleado.DAL_noEmpleadoEstadoLiquidaciones(empleadoID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_GetEstadoLiquidaciones");
                throw exception;
            }
        }

        /// <summary>
        /// dice si un tercero esta repetido o no 
        /// </summary>
        /// <param name="empleadoID">tercero</param>
        /// <returns>un contador con la cantidad de veces que el tercero se repitio</returns>
        public int noEmpleado_CountTerceroID(string tercero)
        {
            try
            {
                DAL_noEmpleado dal_noEmpleado = (DAL_noEmpleado)this.GetInstance(typeof(DAL_noEmpleado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return dal_noEmpleado.DAL_noEmpleado_CountTerceroID(tercero);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_GetEstadoLiquidaciones");
                throw exception;
            }
        }

        #endregion

        #region Liquidacion Detalle

        /// <summary>
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <param name="empleado">empleado</param>
        /// <returns>liquidacion completa</returns>
        public DTO_noNominaDefinitiva Nomina_NominaDefinitivaGet(int documentoId, DateTime periodo, DTO_noEmpleado empleado)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl docControl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(documentoId, empleado.TerceroID.Value, periodo);
                DTO_noNominaDefinitiva liqCompleta = new DTO_noNominaDefinitiva(docControl);
                if (liqCompleta.DocControl != null)
                {

                    liqCompleta.DocLiquidacion = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetByNumeroDoc(liqCompleta.DocControl.NumeroDoc.Value.Value);
                    liqCompleta.Detalle = this._dal_noLiquidacionesDetalle.DAL_noLiquidacionesDetalle_Get(liqCompleta.DocControl.NumeroDoc.Value.Value);
                }

                return liqCompleta;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_LiquidacionCompletaGet");
                throw exception;
            }
        }

        /// <summary>
        ///  Obtiene el detalle para efectos de Pago de Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Nomina</param>
        /// <param name="empleadoId">Identificador de Empleado</param>
        /// <returns>Listado Detalle</returns>
        public List<DTO_noLiquidacionesDetalle> Nomina_GetDetallePago(int documentoID, DateTime periodo, string empleadoId)
        {
            try
            {
                this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_noLiquidacionesDetalle.DAL_noLiquidacionesDetalle_GetDetallePago(documentoID, periodo, empleadoId);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_GetDetallePago");
                throw exception;
            }
        }

        #endregion

        #region Liquidacion Documento

        /// <summary>
        /// Adiciona un documento de liquidacion
        /// </summary>
        /// <param name="dto">documento de liquidación</param>
        /// <param name="insideAnotherTx">verifica si viene o no de una tx</param>
        /// <returns>objeto resultado</returns>
        public DTO_TxResult Nomina_AddLiquidacionDocu(DTO_noLiquidacionesDocu dto, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_Add(dto);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Retorna el documento asociado al numero de documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>documento de liquidación</returns>
        internal DTO_noLiquidacionesDocu noLiquidacionesDocu_Get(DateTime periodo, DTO_noEmpleado empleado)
        {
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_noLiquidacionesDocu documento = new DTO_noLiquidacionesDocu();

            //Documento Nomina
            DTO_glDocumentoControl docControl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(AppDocuments.Nomina, empleado.TerceroID.Value, periodo);
            if (docControl != null)
                documento = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetByNumeroDoc(docControl.NumeroDoc.Value.Value);

            return documento;
        }

        /// <summary>
        /// Retorna el documento asociado al numero de documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>documento de liquidación</returns>
        public List<DTO_noLiquidacionesDocu> Nomina_GetLiquidacionesDocu(int documentoID, DateTime periodo, DTO_noEmpleado empleado)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_noLiquidacionesDocu> documentos = new List<DTO_noLiquidacionesDocu>();

                //Documento Nomina
                List<DTO_glDocumentoControl> ldoc_control = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(documentoID, empleado.TerceroID.Value).Where(x => x.Estado.Value != (Int16)EstadoDocControl.Anulado).ToList();
                if (ldoc_control != null && ldoc_control.Count > 0)
                {
                    foreach (var doc_Control in ldoc_control)
                    {
                        DTO_noLiquidacionesDocu doc = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetByNumeroDoc(doc_Control.NumeroDoc.Value.Value);
                        if (doc != null)
                        {
                            documentos.Add(doc);
                        }
                    }
                }
                return documentos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_noLiquidacionesDocu_GetLiquidaciones");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <returns>listado de liquidaciones aprobadas para generar pago</returns>
        public List<DTO_noPagoLiquidaciones> Nomina_NominaPagosGet(string actividadFlujoID, DateTime periodo)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple.DocumentID = AppMasters.noEmpleado;

                List<DTO_noPagoLiquidaciones> liqCompleta = new List<DTO_noPagoLiquidaciones>();
                List<DTO_noLiquidacionesDocu> documentos = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetDocumentsByActivity(periodo,EstadoDocControl.Aprobado, actividadFlujoID).Where(x => !x.PagadoInd.Value.Value).ToList();
                DTO_glDocumentoControl _docControl = null;
                DTO_noPagoLiquidaciones _docLiq = null;

                if (documentos != null && documentos.Count > 0)
                {
                    List<DTO_glConsultaFiltro> lfiltros = null;

                    foreach (var doc in documentos)
                    {
                        try
                        {
                            _docControl = this._moduloGlobal.glDocumentoControl_GetByID(doc.NumeroDoc.Value.Value);

                            lfiltros = new List<DTO_glConsultaFiltro>();
                            lfiltros.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "ContratoNOID",
                                ValorFiltro = doc.ContratoNOID.Value.ToString(),
                                OperadorFiltro = OperadorFiltro.Igual,
                                OperadorSentencia = "AND"
                            });

                            _docLiq = new DTO_noPagoLiquidaciones(_docControl);
                            _docLiq.Empleado = (DTO_noEmpleado)this._dal_MasterSimple.DAL_MasterSimple_GetPaged(1, 1, null, lfiltros, true).FirstOrDefault();
                            _docLiq.EmpleadoID.Value = _docLiq.Empleado.ID.Value;
                            _docLiq.TerceroID.Value = _docLiq.Empleado.TerceroID.Value;
                            _docLiq.TerceroDesc.Value = _docLiq.Empleado.TerceroDesc.Value;
                            _docLiq.NombreEmpleado.Value = _docLiq.Empleado.Descriptivo.Value;
                            _docLiq.BancoID.Value = _docLiq.Empleado.BancoID.Value;
                            _docLiq.BancoDesc.Value = _docLiq.Empleado.BancoDesc.Value;
                            _docLiq.TipoCuenta.Value = _docLiq.Empleado.TipoCuenta.Value;
                            _docLiq.CuentaAbono.Value = _docLiq.Empleado.CuentaAbono.Value;
                            _docLiq.DocLiquidacion = doc;
                            _docLiq.Valor.Value = this._dal_noLiquidacionesDetalle.DAL_noLiquidacionesDetalle_GetValorByEmpleado(_docControl.DocumentoID.Value.Value, periodo, _docLiq.Empleado.ContratoNOID.Value.Value);

                            liqCompleta.Add(_docLiq);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }

                return liqCompleta;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_LiquidacionPreliminarCompletaGet");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el periodo de vacaciones a liquidar
        /// </summary>
        /// <param name="empleadoId">identificador del empleado</param>
        /// <param name="estado">estado de la liquidacion</param>
        public List<DTO_noLiquidacionVacacionesDeta> Nomina_GetPeriodoVacaciones(string empleadoId, bool estado)
        {
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetPeriodoVacaciones(empleadoId, estado);
        }

        #endregion

        #region Liquidacion Preliminar

        /// <summary>
        /// Obtiene listado de detalle liquidacion preliminar (Prenomina)
        /// </summary>
        /// <returns>Listado de detalles liquidacion</returns>
        public List<DTO_noLiquidacionPreliminar> Nomina_LiquidacionPreliminarGetAll(int documentoId, DateTime periodo, DTO_noEmpleado empleado)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_noLiquidacionPreliminar> ldetalle = new List<DTO_noLiquidacionPreliminar>();

                //Documentos 
                DTO_glDocumentoControl docControl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(documentoId, empleado.TerceroID.Value, periodo);
                if (docControl != null)
                {
                    this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetByNumeroDoc(docControl.NumeroDoc.Value.Value);
                    if (docControl.Estado.Value == (byte)EstadoDocControl.ParaAprobacion)
                        ldetalle = this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_GetAll(docControl.NumeroDoc.Value.Value);

                    //Si ya se aprobo la nomina devuelve el detalle de la tabla final
                    if (docControl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        var detalle = _dal_noLiquidacionesDetalle.DAL_noLiquidacionesDetalle_Get(docControl.NumeroDoc.Value.Value);
                        ldetalle = (from c in detalle.AsEnumerable()
                                    select new DTO_noLiquidacionPreliminar()
                                    {
                                        EmpresaID = c.EmpresaID,
                                        NumeroDoc = c.NumeroDoc,
                                        ConceptoNOID = c.ConceptoNOID,
                                        Dias = c.Dias,
                                        Base = c.Base,
                                        Valor = c.Valor,
                                        OrigenConcepto = c.OrigenConcepto,
                                        ConceptoNODesc = c.ConceptoNODesc
                                    }
                                    ).ToList();
                    }

                }

                return ldetalle;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_LiquidacionPreliminarGetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Adiona registro de detalle en Liquidacion Preliminar
        /// </summary>
        /// <param name="detalle">detalle liquidacion</param>
        /// <param name="insideAnotherTx">si viene ó no de una transacción</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_LiquidacionPreliminarAdd(DTO_noLiquidacionPreliminar detalle, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_Add(detalle);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <returns>liquidacion completa</returns>
        public List<DTO_noNominaPreliminar> Nomina_NominaPreliminarGet(string actividadFlujoID, DateTime periodo, bool isPreliminar)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple.DocumentID = AppMasters.noEmpleado;

                List<DTO_noNominaPreliminar> liqCompleta = new List<DTO_noNominaPreliminar>();
                List<DTO_noLiquidacionesDocu> documentos = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetDocumentsByActivity(periodo,EstadoDocControl.ParaAprobacion, actividadFlujoID).Where(x => !x.PagadoInd.Value.Value).ToList();
                DTO_noNominaPreliminar _docNomPreliminar = null;
                DTO_glDocumentoControl _docControl = null;

                if (documentos != null && documentos.Count > 0)
                {
                    List<DTO_glConsultaFiltro> lfiltros = null;
                    foreach (var doc in documentos)
                    {
                        try
                        {
                            _docControl = this._moduloGlobal.glDocumentoControl_GetByID(doc.NumeroDoc.Value.Value);

                            lfiltros = new List<DTO_glConsultaFiltro>();
                            lfiltros.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "ContratoNOID",
                                ValorFiltro = doc.ContratoNOID.Value.ToString(),
                                OperadorFiltro = OperadorFiltro.Igual,
                                OperadorSentencia = "AND"
                            });

                            _docNomPreliminar = new DTO_noNominaPreliminar(_docControl);
                            _docNomPreliminar.Empleado = (DTO_noEmpleado)this._dal_MasterSimple.DAL_MasterSimple_GetPaged(1, 1, null, lfiltros, true).FirstOrDefault();
                            if (_docNomPreliminar.Empleado != null)
                            {
                                _docNomPreliminar.EmpleadoID.Value = _docNomPreliminar.Empleado.ID.Value;
                                _docNomPreliminar.TerceroID.Value = _docNomPreliminar.Empleado.TerceroID.Value;
                                _docNomPreliminar.TerceroDesc.Value = _docNomPreliminar.Empleado.TerceroDesc.Value;
                                _docNomPreliminar.NombreEmpleado.Value = _docNomPreliminar.Empleado.Descriptivo.Value;
                                _docNomPreliminar.BancoID.Value = _docNomPreliminar.Empleado.BancoID.Value;
                                _docNomPreliminar.BancoDesc.Value = _docNomPreliminar.Empleado.BancoDesc.Value;
                                _docNomPreliminar.TipoCuenta.Value = _docNomPreliminar.Empleado.TipoCuenta.Value;
                                _docNomPreliminar.CuentaAbono.Value = _docNomPreliminar.Empleado.CuentaAbono.Value;
                                _docNomPreliminar.DocLiquidacion = doc;
                                _docNomPreliminar.Detalle = this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_GetAll(doc.NumeroDoc.Value.Value);
                                _docNomPreliminar.Valor.Value = _docNomPreliminar.Detalle.Sum(x => x.Valor.Value.Value);
                                _docNomPreliminar.FechaCorteCesantias.Value = doc.Fecha1.Value;
                                _docNomPreliminar.FechaPagoCesantias.Value = doc.Fecha2.Value;
                                _docNomPreliminar.ValorCesantias.Value = doc.Valor1.Value;
                                _docNomPreliminar.ValorInteresesCesantias.Value = doc.Valor2.Value;
                                _docNomPreliminar.Resolucion.Value = doc.DatoAdd2.Value;
                                _docNomPreliminar.Estado.Value = _docControl.Estado.Value;

                                liqCompleta.Add(_docNomPreliminar); 
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }

                return liqCompleta;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_LiquidacionPreliminarCompletaGet");
                throw exception;
            }
        }

        #endregion

        #region Novedades Contrato

        /// <summary>
        /// Lista las novedades de contrato por empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de novedades de contrato</returns>
        public List<DTO_noNovedadesContrato> Nomina_GetNovedadesContrato(string empleadoID)
        {
            try
            {
                _dal_noNovedadesContrato = (DAL_noNovedadesContrato)this.GetInstance(typeof(DAL_noNovedadesContrato), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_noNovedadesContrato.DAL_noNovedadesContrato_GetNovedades(empleadoID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_GetNovedadesContrato");
                throw exception;
            }
        }

        /// <summary>
        /// Adicona una novedad de contrato 
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la operacion es exitosa</returns>
        public DTO_TxResult Nomina_AddNovedadesContrato(List<DTO_noNovedadesContrato> novedades, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noNovedadesContrato = (DAL_noNovedadesContrato)this.GetInstance(typeof(DAL_noNovedadesContrato), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var nov in novedades)
                {
                    int count = this._dal_noNovedadesContrato.DAL_noNovedadesContrato_ExistNovedad(nov);
                    if (count == 0)
                    {
                        this._dal_noNovedadesContrato.DAL_noNovedadesContrato_AddNovedades(nov);
                    }
                    else
                    {
                        this._dal_noNovedadesContrato.DAL_noNovedadesContrato_UpdNovedad(nov);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Elimina una novedad de contrato
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la elimina</returns>
        public DTO_TxResult Nomina_noNovedadesContrato_Delete(DTO_noNovedadesContrato novedad)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                this._dal_noNovedadesContrato = (DAL_noNovedadesContrato)this.GetInstance(typeof(DAL_noNovedadesContrato), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bool p = this._dal_noNovedadesContrato.DAL_noNovedadesContrato_Delete(novedad);
                if (!p)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.No_NovedadContratoDelete;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                return result;
            }
        }

        #endregion

        #region Novedades Nomina

        /// <summary>
        /// lista las novedades de nomina por empleado
        /// </summary>
        /// <param name="empleadoID">identificador de empleado</param>
        /// <returns>listado de novedades</returns>
        public List<DTO_noNovedadesNomina> Nomina_GetNovedades(string empleadoID)
        {
            try
            {
                _dal_noNovedadesNomina = (DAL_noNovedadesNomina)this.GetInstance(typeof(DAL_noNovedadesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_noNovedadesNomina.DAL_noNovedadesNomina_GetNovedades(empleadoID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_GetEmpleados");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega las novedades de nomina 
        /// </summary>
        /// <param name="novedades">listado de novedades</param>
        /// <param name="insideAnotherTx">verifica si viene de una transaccion</param>
        /// <returns></returns>
        public DTO_TxResult Nomina_AddNovedadNomina(List<DTO_noNovedadesNomina> novedades, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noNovedadesNomina = (DAL_noNovedadesNomina)this.GetInstance(typeof(DAL_noNovedadesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var nov in novedades)
                {
                    try
                    {
                        int numNovedad = this._dal_noNovedadesNomina.DAL_noNovedadesNomina_ExistNovedad(nov);
                        if (numNovedad == 0)
                        {
                            this._dal_noNovedadesNomina.DAL_noNovedadesNomina_AddNovedades(nov);
                        }
                        else if (numNovedad > 0)
                        {
                            this._dal_noNovedadesNomina.DAL_noNovedadesNomina_UpdNovedad(nov);
                        }
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "OK"
                        };

                        result.Details.Add(detalle);
                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "NOK",
                            Key = ex.Message
                        };
                        result.Details.Add(detalle);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Elimina una novedad de Nomina
        /// </summary>
        /// <param name="novedad">novedad de nomina</param>
        public void Nomina_DelNovedadesNomina(DTO_noNovedadesNomina novedad)
        {
            try
            {
                this._dal_noNovedadesNomina = (DAL_noNovedadesNomina)this.GetInstance(typeof(DAL_noNovedadesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noNovedadesNomina.DAL_noNovedadesNomina__Delete(novedad);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Nomina_DelNovedadesNomina");
                throw exception;
            }
        }

        #endregion

        #region Planilla de Aportes

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public DTO_noPlanillaAportesDeta Nomina_GetPlanillaAportes(string empleadoID, DateTime periodo)
        {
            try
            {
                DAL_noPlanillaAportesDeta dal_noPlanillaAportesDeta = (DAL_noPlanillaAportesDeta)this.GetInstance(typeof(DAL_noPlanillaAportesDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return dal_noPlanillaAportesDeta.DAL_noPlanillaAportesDeta_Get(empleadoID, periodo);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_GetPlanillaAportesDeta");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public List<DTO_noPlanillaAportesDeta> Nomina_GetAllPlanillaAportes(DateTime periodo)
        {
            try
            {
                DAL_noPlanillaAportesDeta dal_noPlanillaAportesDeta = (DAL_noPlanillaAportesDeta)this.GetInstance(typeof(DAL_noPlanillaAportesDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return dal_noPlanillaAportesDeta.DAL_noPlanillaAportesDeta_GetByPeriodo(periodo);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_GetAllPlanillaAportes");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza listado de planillas de aportes modificadas
        /// </summary>
        /// <param name="lplanilla">listado de planillas</param>
        /// <param name="insideAnotherTx">si viene de alguna tx</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_PlanillaAportesDeta_Upd(List<DTO_noPlanillaAportesDeta> lplanilla, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noPlanillaAportesDeta = (DAL_noPlanillaAportesDeta)this.GetInstance(typeof(DAL_noPlanillaAportesDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var item in lplanilla)
                {
                    try
                    {
                        this._dal_noPlanillaAportesDeta.DAL_noPlanillaAportesDeta_Upd(item);
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "OK",
                        };
                        result.Details.Add(detalle);
                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "NOK",
                            Key = ex.Message
                        };
                        result.Details.Add(detalle);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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

        #region Planillas diarias trabajo

        /// <summary>
        /// Obtiene las planillas diarias de trabajos asociadas al empleado
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de planillas</returns>
        public List<DTO_noPlanillaDiariaTrabajo> Nomina_GetPlanillaDiaria(string empleadoID, DateTime periodo, string empresaId, Int16 contratoNo)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                _dal_noPlanillaDiariaTrabajo = (DAL_noPlanillaDiariaTrabajo)this.GetInstance(typeof(DAL_noPlanillaDiariaTrabajo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_noPlanillaDiariaTrabajo> result = new List<DTO_noPlanillaDiariaTrabajo>();

                string isQuincenal = this.GetControlValueByCompany(ModulesPrefix.no, "002");

                int diaInicial = 0;
                int diaFinal = 30;

                if (isQuincenal == "1")
                {
                    int quincena = Convert.ToInt16(this.GetControlValueByCompany(ModulesPrefix.no, "003"));
                    if (quincena == (Int16)PeriodoPago.PrimeraQuincena)
                    {
                        diaInicial = 1;
                        diaFinal = 15;
                    }
                    else
                    {
                        diaInicial = 16;
                        diaFinal = 31;
                    }
                }

                for (int i = diaInicial; i <= diaFinal; i++)
                {
                    DateTime dia = periodo;
                    dia = dia.AddDays(i);
                    var dto = _dal_noPlanillaDiariaTrabajo.DAL_noPlanillaDiariaTrabajo_GetPlanillaDiaria(empleadoID, dia);
                    if (dto != null)
                    {
                        result.Add(dto);
                    }
                    else
                    {
                        DTO_noPlanillaDiariaTrabajo newDto = new DTO_noPlanillaDiariaTrabajo();
                        newDto.FechaPlanilla.Value = dia;
                        newDto.EmpresaID.Value = empresaId;
                        newDto.ContratoNOID.Value = contratoNo;
                        newDto.EmpleadoID.Value = empleadoID;
                        newDto.TipoConceptoPlanilla.Value = (Byte)TipoConceptoPlanillaDiaria.T;
                        result.Add(newDto);

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_GetPlanillaDiaria");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona una planilla diaria de trabajo
        /// </summary>
        /// <param name="prestamo">planilla</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddPlanillaDiaria(List<DTO_noPlanillaDiariaTrabajo> planillas, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noPlanillaDiariaTrabajo = (DAL_noPlanillaDiariaTrabajo)this.GetInstance(typeof(DAL_noPlanillaDiariaTrabajo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var planilla in planillas)
                {
                    try
                    {
                        if (planilla.ConceptoNOPlanillaID.Value != null
                            && planilla.HorasEXTDiu.Value != null
                            && planilla.HorasEXTNoc.Value != null
                            && planilla.HorasNORDiu.Value != null
                            && planilla.HorasRECNoc.Value != null
                        )
                        {
                            this._dal_noPlanillaDiariaTrabajo.DAL_noPlanillaDiariaTrabajo_AddPlanillaDiaria(planilla);
                            DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                            {
                                Message = "OK"
                            };
                            result.Details.Add(detalle);
                        }
                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "NOK",
                            Key = ex.Message
                        };
                        result.Details.Add(detalle);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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

        #region Prestamos

        /// <summary>
        /// Obtiene prestamos asociados al empleados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de prestamos</returns>
        public List<DTO_noPrestamo> Nomina_GetPrestamos(string empleadoID)
        {
            try
            {
                _dal_noPrestamo = (DAL_noPrestamo)this.GetInstance(typeof(DAL_noPrestamo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_noPrestamo.DAL_noPrestamo_GetPrestamos(empleadoID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_GetPrestamos");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona un Prestamo
        /// </summary>
        /// <param name="prestamo">objeto prestamo</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddPrestamo(List<DTO_noPrestamo> prestamos, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noPrestamo = (DAL_noPrestamo)this.GetInstance(typeof(DAL_noPrestamo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var prestamo in prestamos)
                {
                    try
                    {
                        int count = this._dal_noPrestamo.DAL_noPrestamo_ExistPrestamo(prestamo);
                        if (count > 0)
                        {
                            this._dal_noPrestamo.DAL_noPrestamo_UpdPrestamo(prestamo);
                        }
                        else
                        {
                            this._dal_noPrestamo.DAL_noPrestamo_AddPrestamo(prestamo);
                        }
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "OK"
                        };
                        result.Details.Add(detalle);
                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "NOK",
                            Key = ex.Message
                        };
                        result.Details.Add(detalle);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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
        /// Actualiza información de un Prestamo
        /// </summary>
        /// <param name="prestamo">objeto prestamo</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_UpdPrestamo(DTO_noPrestamo prestamo, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noPrestamo = (DAL_noPrestamo)this.GetInstance(typeof(DAL_noPrestamo), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                try
                {
                    this._dal_noPrestamo.DAL_noPrestamo_UpdPrestamo(prestamo);
                    DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                    {
                        Message = "OK"
                    };
                    result.Details.Add(detalle);
                }
                catch (Exception ex)
                {
                    DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                    {
                        Message = "NOK",
                        Key = ex.Message
                    };
                    result.Details.Add(detalle);
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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

        #region Procesos

        #region Liquidacion Nomina

        /// <summary>
        /// Liquidación Nomina
        /// </summary>
        /// <param name="periodo">perido del documento</param>
        /// <param name="lEmpleados">listado de empleados a liquidar</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarNomina(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionNomina(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;
                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);

            }

            objLiquidacion.GenerarConsecutivos(numDocs);

            batchProgress[tupProgress] = 100;
            return lresultados;
        }

        #endregion

        #region Liquidacion Contrato

        /// <summary>
        /// Liquidación de Contrato 
        /// </summary>
        /// <param name="periodo">Periodo de Documento</param>
        /// <param name="lEmpleados">Listado de Empleados</param>
        /// <param name="fechaRetiro">Fecha de Retiro</param>
        /// <param name="causa">Causa Liquidación</param>
        /// <param name="batchProgress">barra de Progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarContrato(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaRetiro, int causa, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.LiquidacionContrato);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionContrato(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;
                ((noLiquidacionContrato)objLiquidacion).FechaRetiro = fechaRetiro;
                ((noLiquidacionContrato)objLiquidacion).CausaLiquidacion = causa;

                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);
            }

            objLiquidacion.GenerarConsecutivos(numDocs);

            batchProgress[tupProgress] = 100;
            return lresultados;
        }

        #endregion

        #region LiquidarCesantias

        /// <summary>
        /// Liquida las Cesantias
        /// </summary>
        /// <param name="periodo">periodo actual</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="fechaIniLiq">fecha inicial corte prima</param>
        /// <param name="fechaFinLiq">fecha final corte prima</param>
        /// <param name="resolucion">resolución</param>
        /// <param name="batchProgress">barrr de progresio</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarCesantias(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaIniLiq, DateTime fechaFinLiq, DateTime fechaPago, string resolucion, TipoLiqCesantias tLiqCesantias, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionCesantias(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;
                ((noLiquidacionCesantias)objLiquidacion).FechaIniLiq = fechaIniLiq;
                ((noLiquidacionCesantias)objLiquidacion).FechaFinLiq = fechaFinLiq;
                ((noLiquidacionCesantias)objLiquidacion).Resolucion = resolucion;
                ((noLiquidacionCesantias)objLiquidacion).tliqCesantias = tLiqCesantias;
                ((noLiquidacionCesantias)objLiquidacion).FechaPago = fechaPago;

                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);

            }

            objLiquidacion.GenerarConsecutivos(numDocs);

            batchProgress[tupProgress] = 100;
            return lresultados;
        }

        /// <summary>
        /// Actualiza los valores de cesantias ó intereses de cesantias
        /// </summary>
        /// <param name="numeroDoc">numero Doc</param>
        /// <param name="valorCesantias">valor cesantias</param>
        /// <param name="valorIntereses">valor intereses</param>
        /// <param name="indCesantias">ind cesantias</param>
        public void UpdateCesantias(int numeroDoc, decimal valorCesantias, decimal valorIntereses, bool indCesantias)
        {
            noLiquidacionCesantias objLiquidacion = new noLiquidacionCesantias(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            objLiquidacion.UpdateCesantias(numeroDoc, valorCesantias, valorIntereses, indCesantias);
        }

        #endregion

        #region Liquidacion Vacaciones

        /// <summary>
        /// Proceso para liquidar las vacaciones
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <param name="diasVacDinero">días vacaciones en dinero</param>
        /// <param name="indIncNomina">indica si incluye en nómina</param>
        /// <param name="indPrima">indica si se incluye la prima</param>
        /// <param name="resolucion">resolucion</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarVacaciones(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaIniLiq, DateTime fechaFinLiq,
                                                     int diasVacTiempo, int diasVacDinero, bool indIncNomina, bool indPrima, string resolucion,
                                                     DateTime fechaPagoLiq, DateTime fechaIniPagoLiq, DateTime fechaIniPendVacac, DateTime fechaFinPendVacac, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionVacaciones(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;
                ((noLiquidacionVacaciones)objLiquidacion).FechaIniLiq = fechaIniLiq;
                ((noLiquidacionVacaciones)objLiquidacion).FechaFinLiq = fechaFinLiq;
                ((noLiquidacionVacaciones)objLiquidacion).DiasVacDinero = diasVacDinero;
                ((noLiquidacionVacaciones)objLiquidacion).DiasVacTiempo = diasVacTiempo;
                ((noLiquidacionVacaciones)objLiquidacion).IndIncNomina = indIncNomina;
                ((noLiquidacionVacaciones)objLiquidacion).IndPrima = indPrima;
                ((noLiquidacionVacaciones)objLiquidacion).Resolucion = resolucion;
                ((noLiquidacionVacaciones)objLiquidacion).FechaFinPagoLiq = fechaPagoLiq;
                ((noLiquidacionVacaciones)objLiquidacion).FechaIniPagoLiq = fechaIniPagoLiq;
                ((noLiquidacionVacaciones)objLiquidacion).FechaIniPendVacac = fechaIniPendVacac;
                ((noLiquidacionVacaciones)objLiquidacion).FechaFinPendVacac = fechaFinPendVacac;

                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);

            }

            objLiquidacion.GenerarConsecutivos(numDocs);

            batchProgress[tupProgress] = 100;
            return lresultados;
        }


        /// <summary>
        /// Calculo los días reales de vacaciones del empleado
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <returns>número de días de vacaciones</returns>
        public int CalcularDiasVacaciones(string empleadoID, DateTime fechaIni, DateTime fechaFin, out decimal diasCausados)
        {
            try
            {
                DTO_noEmpleado empleado = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, empleadoID, true, false);
                DTO_noRol rol = (DTO_noRol)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noRol, empleado.RolNOID.Value, true, false);

                #region Calculo los días reales de Vacaciones

                this._dal_MasterComplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int numDias = 0;
                TimeSpan diasDif = fechaFin - fechaIni;
                DateTime currentDate = fechaIni;
                this._dal_MasterComplex.DocumentID = AppMasters.glDiasFestivos;
                var ldiasFestivos = this._dal_MasterComplex.DAL_MasterComplex_GetPaged(this._dal_MasterComplex.DAL_MasterSimple_Count(null, null, true), 1, null, true);

                if (rol.SabadoLaboralInd.Value.Value)
                {
                    //No incluye sabados, domingos y festivos como días laborales
                    for (int i = 0; i <= diasDif.Days; i++)
                    {
                        if (currentDate.DayOfWeek != DayOfWeek.Sunday && !ldiasFestivos.Any(x => ((DTO_glDiasFestivos)x).DiasFestivoID.Value.Value == currentDate))
                            numDias++;
                        currentDate = currentDate.AddDays(1);
                    }
                }
                else
                {
                    //Indica que el día sabado es laboral
                    for (int i = 0; i <= diasDif.Days; i++)
                    {
                        if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday && !ldiasFestivos.Any(x => ((DTO_glDiasFestivos)x).DiasFestivoID.Value.Value == currentDate))
                            numDias++;
                        currentDate = currentDate.AddDays(1);
                    }
                }

                #endregion

                #region Dias Causados

                decimal diasTrabajados = 0;
                decimal diasNoRemunerados = 0;

                ///Calcula los días que estuvo en novedad de tipo SLN
                var lNovedades = this.Nomina_GetNovedadesContrato(empleadoID);
                if (lNovedades != null && lNovedades.Count > 0)
                {
                    foreach (var item in lNovedades)
                    {
                        DTO_noContratoNov nov = (DTO_noContratoNov)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noContratoNov, item.ContratoNONovID.Value, true, false);
                        if (nov.TipoNovedad.Value == (byte)TipoNovedad.SLN)
                            diasNoRemunerados += ((item.FechaFinal.Value.Value - item.FechaInicial.Value.Value).Days + 1);
                    }
                }

                if (rol.DiasCalendarioInd.Value.Value)
                {
                    diasTrabajados = ((fechaFin - empleado.FechaIngreso.Value.Value).Days + 1) - diasNoRemunerados;
                }
                else
                {
                    diasTrabajados = DateTimeExtension.DateDiff(DateInterval.Month, empleado.FechaIngreso.Value.Value, fechaFin) * 30 +
                                     empleado.FechaIngreso.Value.Value.Day + fechaFin.Day;
                }

                diasCausados = (diasTrabajados * 15) / 360;

                #endregion

                return numDias;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_CalcularDiasVacaciones");
                throw exception;
            }
        }



        #endregion

        #region Liquidacion Prima

        /// <summary>
        /// Liquida la Prima 
        /// </summary>
        /// <param name="periodo">periodo actual</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="fechaIniLiq">fecha inicial corte prima</param>
        /// <param name="fechaFinLiq">fecha final corte prima</param>
        /// <param name="incNomina">determina si se incluye en la Nomina</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns> 
        public List<DTO_TxResult> LiquidarPrima(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaIniLiq, DateTime fechaFinLiq, bool incNomina, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionPrima(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;
                ((noLiquidacionPrima)objLiquidacion).FechaIniLiq = fechaIniLiq;
                ((noLiquidacionPrima)objLiquidacion).FechaFinLiq = fechaFinLiq;
                ((noLiquidacionPrima)objLiquidacion).IndNomina = incNomina;

                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);

            }

            objLiquidacion.GenerarConsecutivos(numDocs);

            batchProgress[tupProgress] = 100;
            return lresultados;
        }


        #endregion

        #region Liquidacion Planilla

        /// <summary>
        /// Liquidación Planilla de Aportes
        /// </summary>
        /// <param name="periodo">perido del documento</param>
        /// <param name="lEmpleados">listado de empleados a liquidar</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarPlanilla(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionPlanilla(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;

                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);

            }

            return lresultados;
        }

        /// <summary>
        /// Proceso trae valores planilla X tercero
        /// </summary>
        public List<DTO_NominaPlanillaContabilizacion> noPlanillaAportesDeta_GetValoreXTercero(bool isPlanilla)
        {
            try
            {
                List<DTO_NominaPlanillaContabilizacion> result = new List<DTO_NominaPlanillaContabilizacion>();
                this._dal_noPlanillaAportesDeta = (DAL_noPlanillaAportesDeta)this.GetInstance(typeof(DAL_noPlanillaAportesDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = _dal_noPlanillaAportesDeta.DAL_noPlanillaAportesDeta_GetValoreXTercero(isPlanilla);

                if (isPlanilla)
                {
                    List<DTO_NominaPlanillaContabilizacion> tmp = new List<DTO_NominaPlanillaContabilizacion>();
                    List<string> distinct = (from c in result select c.TerceroID.Value).Distinct().ToList();
                    foreach (string tercero in distinct)
                    {
                        DTO_NominaPlanillaContabilizacion ter = ObjectCopier.Clone(result.Find(x=>x.TerceroID.Value == tercero));
                        ter.EmpleadoID.Value = string.Empty;
                        ter.EmpleadoDesc.Value = string.Empty;
                        ter.Valor.Value = result.FindAll(x => x.TerceroID.Value == tercero).Sum(x=>x.Valor.Value);
                        ter.Valor2.Value = result.FindAll(x => x.TerceroID.Value == tercero).Sum(x =>x.Valor2.Value);
                        ter.Detalle.AddRange(result.FindAll(x => x.TerceroID.Value == tercero));
                        ter.Seleccionar.Value = true;
                        tmp.Add(ter);
                    }
                    result = tmp;
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "noPlanillaAportesDeta_GetValoreXTercero");
                throw exception;
            }

        }


        #endregion

        #region Liquidacion Provisiones

        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        public List<DTO_TxResult> LiquidarProvisiones(DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionProvisiones(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                objLiquidacion.Empleado = emp;
                objLiquidacion.FechaDoc = fechaDoc;

                result = objLiquidacion.Liquidar();
                if (result.Result == ResultValue.OK)
                    numDocs.Add(objLiquidacion.DocCtrl.NumeroDoc.Value.Value);
                else
                    lresultados.Add(result);

            }

            objLiquidacion.GenerarConsecutivos(numDocs);

            batchProgress[tupProgress] = 100;
            return lresultados;
        }

        #endregion

        #region Migracion Nomina

        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lEmpleados">liquidaciones Empleados</param>
        /// <param name="batchProgress">barra de Progresp</param>
        /// <returns></returns>
        public List<DTO_TxResult> MigracionNomina(List<DTO_noMigracionNomina> lEmpleados, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            //Recorre el listado de empleados y liquida el sueldo para cada uno de ellos
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            int documentoId = 81;

            try
            {
                foreach (var reg in lEmpleados)
                {
                    if (reg.TipoLiquidacion.Value == 1)
                        documentoId = AppDocuments.Nomina;
                    if (reg.TipoLiquidacion.Value == 2)
                        documentoId = AppDocuments.Vacaciones;
                    if (reg.TipoLiquidacion.Value == 3)
                        documentoId = AppDocuments.Prima;
                    if (reg.TipoLiquidacion.Value == 4)
                        documentoId = AppDocuments.LiquidacionContrato;

                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_glControl = (DAL_glControl)this.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    DTO_noEmpleado empleado = (DTO_noEmpleado)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, reg.EmpleadoID.Value, true, false);

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    //Instacias Globales
                    int numeroDoc = 0;
                    int errorInd = 0;
                    string errorMsg;
                    decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

                    try
                    {
                        #region asignación glDocumentoControl

                        DTO_glDocumentoControl _docControl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(documentoId, reg.PeriodoID.Value.Value, empleado.ContratoNOID.Value.Value);
                        if (_docControl == null)
                        {

                            _docControl = new DTO_glDocumentoControl();
                            _docControl.EmpresaID.Value = this.Empresa.ID.Value;
                            _docControl.DocumentoID.Value = documentoId;
                            _docControl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                            _docControl.Fecha.Value = DateTime.Now;
                            _docControl.PeriodoDoc.Value = reg.PeriodoID.Value.Value;
                            _docControl.PeriodoUltMov.Value = reg.PeriodoID.Value.Value;
                            _docControl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                            _docControl.PrefijoID.Value = this.GetPrefijoByDocumento(documentoId);
                            _docControl.MonedaID.Value = empleado.MonedaExtInd.Value.Value ? mdaExt : mdaLoc;
                            _docControl.TasaCambioCONT.Value = empleado.MonedaExtInd.Value.Value ? tc : 0;
                            _docControl.TasaCambioDOCU.Value = empleado.MonedaExtInd.Value.Value ? tc : 0;
                            _docControl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                            _docControl.TerceroID.Value = empleado.TerceroID.Value;
                            _docControl.ProyectoID.Value = empleado.ProyectoID.Value;
                            _docControl.CentroCostoID.Value = empleado.CentroCostoID.Value;
                            _docControl.LugarGeograficoID.Value = empleado.LugarGeograficoID.Value;
                            _docControl.seUsuarioID.Value = this.UserId;

                            if (empleado.MonedaExtInd.Value.Value)
                            {
                                if (tc == 0)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_No_ExitTasaCambioDia + "&&" + empleado.ID.Value + "&&" + today.ToShortDateString();
                                    lresultados.Add(result);
                                }
                            }

                            DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                            if (result.Result == ResultValue.OK)
                            {
                                resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoId, _docControl, true);
                                if (resultGLDC.Message != ResultValue.OK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + empleado.ID.Value + "&&" + resultGLDC.DetailsFields[0].Field;
                                    lresultados.Add(result);
                                }
                                else
                                {
                                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                                    numDocs.Add(numeroDoc);
                                }
                            }
                        }
                        else
                        {
                            if (empleado.MonedaExtInd.Value.Value)
                            {
                                if (tc == 0)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_No_ExitTasaCambioDia + "&&" + empleado.ID.Value + "&&" + today.ToShortDateString();
                                    lresultados.Add(result);
                                }
                            }
                            numeroDoc = _docControl.NumeroDoc.Value.Value;
                            numDocs.Add(numeroDoc);
                        }

                        #endregion

                        if (result.Result == ResultValue.OK)
                        {

                            #region ejecuta Procedimiento de migración de Nomina

                            this._dal_noLiquidacionesDetalle.DAL_noLiquidacionPreliminar_MigracionNomina(reg, numeroDoc, tc, out errorInd, out errorMsg);

                            #endregion

                            #region Manejo de Errores

                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                result.Result = ResultValue.NOK;

                                switch (errorInd)
                                {
                                    case 1:
                                        {
                                            result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + empleado.ID.Value + "&&" + errorMsg;
                                            break;
                                        }
                                }
                            }

                            #endregion
                        }

                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = ex.Message;
                        lresultados.Add(result);
                    }
                    finally
                    {
                        if (result.Result == ResultValue.OK)
                        {
                            base._mySqlConnectionTx.Commit();
                            base._mySqlConnectionTx = null;
                            this._moduloGlobal._mySqlConnectionTx = null;

                            DTO_glDocumentoControl docControl = null;
                            foreach (var numDoc in numDocs)
                            {
                                docControl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                                docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(AppDocuments.Nomina, docControl.PrefijoID.Value));
                                this._moduloGlobal.ActualizaConsecutivos(docControl, true, false, false);
                            }
                        }
                        else if (base._mySqlConnectionTx != null)
                            base._mySqlConnectionTx.Rollback();
                    }
                }

                batchProgress[tupProgress] = 100;
                return lresultados;
            }
            catch (Exception ex)
            {
                lresultados.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "MigracionNomina");
                lresultados.Add(result);

                return lresultados;
            }
        }

        #endregion

        #region Migracion Provisiones

        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lProvisiones">Provisiones</param>
        /// <param name="batchProgress">barra de Progresp</param>
        /// <returns></returns>
        public List<DTO_TxResult> MigracionProvisiones(List<DTO_noProvisionDeta> lProvisiones, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            //Recorre el listado de empleados y liquida el sueldo para cada uno de ellos
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lProvisiones.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, AppDocuments.Nomina);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            int documentoId = AppDocuments.Provisiones;

            try
            {
                foreach (var reg in lProvisiones)
                {

                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_noProvisionDeta = (DAL_noProvisionDeta)this.GetInstance(typeof(DAL_noProvisionDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_glControl = (DAL_glControl)this.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    DTO_noEmpleado empleado = (DTO_noEmpleado)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, reg.EmpleadoID.Value, true, false);

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    //Instacias Globales
                    int numeroDoc = 0;
                    int errorInd = 0;
                    string errorMsg;
                    decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

                    try
                    {
                        #region asignación glDocumentoControl

                        DTO_glDocumentoControl _docControl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(documentoId, reg.Periodo.Value.Value, empleado.ContratoNOID.Value.Value);
                        if (_docControl == null)
                        {

                            _docControl = new DTO_glDocumentoControl();
                            _docControl.EmpresaID.Value = this.Empresa.ID.Value;
                            _docControl.DocumentoID.Value = documentoId;
                            _docControl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                            _docControl.Fecha.Value = DateTime.Now;
                            _docControl.PeriodoDoc.Value = reg.Periodo.Value.Value;
                            _docControl.PeriodoUltMov.Value = reg.Periodo.Value.Value;
                            _docControl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                            _docControl.PrefijoID.Value = this.GetPrefijoByDocumento(documentoId);
                            _docControl.MonedaID.Value = empleado.MonedaExtInd.Value.Value ? mdaExt : mdaLoc;
                            _docControl.TasaCambioCONT.Value = empleado.MonedaExtInd.Value.Value ? tc : 0;
                            _docControl.TasaCambioDOCU.Value = empleado.MonedaExtInd.Value.Value ? tc : 0;
                            _docControl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                            _docControl.TerceroID.Value = empleado.TerceroID.Value;
                            _docControl.ProyectoID.Value = empleado.ProyectoID.Value;
                            _docControl.CentroCostoID.Value = empleado.CentroCostoID.Value;
                            _docControl.LugarGeograficoID.Value = empleado.LugarGeograficoID.Value;
                            _docControl.seUsuarioID.Value = this.UserId;

                            if (empleado.MonedaExtInd.Value.Value)
                            {
                                if (tc == 0)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_No_ExitTasaCambioDia + "&&" + empleado.ID.Value + "&&" + today.ToShortDateString();
                                    lresultados.Add(result);
                                }
                            }

                            DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                            if (result.Result == ResultValue.OK)
                            {
                                resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoId, _docControl, true);
                                if (resultGLDC.Message != ResultValue.OK.ToString())
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + empleado.ID.Value + "&&" + resultGLDC.DetailsFields[0].Field;
                                    lresultados.Add(result);
                                }
                                else
                                {
                                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                                    numDocs.Add(numeroDoc);
                                }
                            }
                        }
                        else
                        {
                            if (empleado.MonedaExtInd.Value.Value)
                            {
                                if (tc == 0)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_No_ExitTasaCambioDia + "&&" + empleado.ID.Value + "&&" + today.ToShortDateString();
                                    lresultados.Add(result);
                                }
                            }
                            numeroDoc = _docControl.NumeroDoc.Value.Value;
                            numDocs.Add(numeroDoc);
                        }

                        #endregion

                        if (result.Result == ResultValue.OK)
                        {

                            #region ejecuta Procedimiento de migración de Provisiones

                            this._dal_noProvisionDeta.DAL_noProvisionDeta_MigracionProvisiones(reg, numeroDoc, tc, out errorInd, out errorMsg);

                            #endregion

                            #region Manejo de Errores

                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                result.Result = ResultValue.NOK;

                                switch (errorInd)
                                {
                                    case 1:
                                        {
                                            result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + empleado.ID.Value + "&&" + errorMsg;
                                            break;
                                        }
                                }
                            }

                            #endregion
                        }

                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = ex.Message;
                        lresultados.Add(result);
                    }
                    finally
                    {
                        if (result.Result == ResultValue.OK)
                        {
                            base._mySqlConnectionTx.Commit();
                            base._mySqlConnectionTx = null;
                            this._moduloGlobal._mySqlConnectionTx = null;

                            DTO_glDocumentoControl docControl = null;
                            foreach (var numDoc in numDocs)
                            {
                                docControl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                                docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(AppDocuments.Nomina, docControl.PrefijoID.Value));
                                this._moduloGlobal.ActualizaConsecutivos(docControl, true, false, false);
                            }
                        }
                        else if (base._mySqlConnectionTx != null)
                            base._mySqlConnectionTx.Rollback();
                    }
                }

                batchProgress[tupProgress] = 100;
                return lresultados;
            }
            catch (Exception ex)
            {
                lresultados.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "MigracionProvisiones");
                lresultados.Add(result);

                return lresultados;
            }
        }

        #endregion

        #region Prestamos

        /// <summary>
        /// Liquida Prestamos asociados a empleados
        /// </summary>
        /// <param name="lEmpleados">listado de empleados</param>
        /// <returns>listado de resultados</returns>
        public DTO_TxResult LiquidarPrestamos(int numDoc, DTO_noEmpleado empleado, DateTime fechaIniLiquidacion, DateTime fechaFinLiquidacion, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            int numPeriodos = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {

                List<DTO_noPrestamo> lPrestamos = this.Nomina_GetPrestamos(empleado.ID.Value);
                int ultimaQuincenaProcesada = Convert.ToInt32(_moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_UltimaNominaLiquidada));

                foreach (var prestamo in lPrestamos)
                {
                    #region Prestamo

                    string Message = string.Empty;
                    decimal VlAbono = 0;
                    //Liquidacion por Valor
                    if (prestamo.VlrCuota.Value < prestamo.VlrPrestamo.Value - prestamo.VlrAbono.Value)
                    {
                        //Liquidacion Quincenal
                        if (((DTO_noEmpleado)empleado).QuincenalInd.Value.Value)
                        {
                            if (numPeriodos == 1)
                            {
                                //Liquida par un Perido
                                if (ultimaQuincenaProcesada == (int)PeriodoPago.PrimeraQuincena)
                                {
                                    if (prestamo.QuincenaPagos.Value == (int)PeriodoPago.SegundaQuincena || prestamo.QuincenaPagos.Value == (int)PeriodoPago.SegundaQuincena)
                                    {
                                        VlAbono = prestamo.VlrCuota.Value.Value;
                                    }
                                }
                                if (ultimaQuincenaProcesada == (int)PeriodoPago.SegundaQuincena)
                                {
                                    if (prestamo.QuincenaPagos.Value == (int)PeriodoPago.PrimeraQuincena || prestamo.QuincenaPagos.Value == (int)PeriodoPago.SegundaQuincena)
                                    {
                                        VlAbono = prestamo.VlrCuota.Value.Value;
                                    }
                                }

                                if (VlAbono > 0)
                                {

                                    DTO_noConceptoNOM conceptoNOM = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, prestamo.ConceptoNOID.Value, true, false);

                                    //Se incluye en registro de detalle en la prenomina
                                    DTO_noLiquidacionPreliminar detalleLiquidacion = new DTO_noLiquidacionPreliminar();
                                    detalleLiquidacion.EmpresaID.Value = this.Empresa.ID.Value;
                                    detalleLiquidacion.ConceptoNOID.Value = conceptoNOM.ID.Value;
                                    detalleLiquidacion.OrigenConcepto.Value = (int)OrigenConcepto.Prestamo;
                                    detalleLiquidacion.Dias.Value = 0;
                                    detalleLiquidacion.Valor.Value = conceptoNOM.Tipo.Value == (byte)TipoConceptoNOM.Devengo ? Convert.ToDecimal(VlAbono) : Convert.ToDecimal(VlAbono) * -1;
                                    detalleLiquidacion.Base.Value = 0; //TODO cual es la base para el prestamo?         
                                    detalleLiquidacion.NumeroDoc.Value = numDoc;
                                    detalleLiquidacion.Numero.Value = prestamo.Numero.Value;

                                    this.Nomina_LiquidacionPreliminarAdd(detalleLiquidacion, true);

                                    //Actuliza tabla prestamo
                                    prestamo.VlrAbono.Value = prestamo.VlrAbono.Value + VlAbono;
                                    this.Nomina_UpdPrestamo(prestamo, true);
                                }
                            }
                            else
                            {
                                //Liquidar con varios Periodos
                            }
                        }
                        else
                        {
                            //Liquidacion Mensual
                            if (numPeriodos == 1)
                            {
                                VlAbono = prestamo.VlrCuota.Value.Value;

                                if (VlAbono > 0)
                                {

                                    DTO_noConceptoNOM conceptoNOM = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, prestamo.ConceptoNOID.Value, true, false);

                                    //Se incluye en registro de detalle en la prenomina
                                    DTO_noLiquidacionPreliminar detalleLiquidacion = new DTO_noLiquidacionPreliminar();
                                    detalleLiquidacion.EmpresaID.Value = this.Empresa.ID.Value;
                                    detalleLiquidacion.ConceptoNOID.Value = conceptoNOM.ID.Value;
                                    detalleLiquidacion.OrigenConcepto.Value = (int)OrigenConcepto.Prestamo;
                                    detalleLiquidacion.Dias.Value = 0;
                                    detalleLiquidacion.Valor.Value = conceptoNOM.Tipo.Value == (byte)TipoConceptoNOM.Devengo ? Convert.ToDecimal(VlAbono) : Convert.ToDecimal(VlAbono) * -1;
                                    detalleLiquidacion.Base.Value = 0; //TODO cual es la base para el prestamo?         
                                    detalleLiquidacion.NumeroDoc.Value = numDoc;
                                    detalleLiquidacion.Numero.Value = prestamo.Numero.Value;

                                    this.Nomina_LiquidacionPreliminarAdd(detalleLiquidacion, true);

                                    //Actuliza tabla prestamo
                                    prestamo.VlrAbono.Value = prestamo.VlrAbono.Value + VlAbono;
                                    this.Nomina_UpdPrestamo(prestamo, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        Message = "El Valor de la cuota no debe ser mayor al valor del saldo";
                    }

                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = string.Format(DictionaryMessages.Err_No_liquidarPrestamos, ex.Message);
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

        #region Cierre Nomina

        /// <summary>
        /// Proceso Encargado de Cerrar la Nomina
        /// </summary>
        /// <param name="periodo">Periodo actual de la nomina</param>
        /// <param name="insideAnotherTx">indica si viene de una transaccion</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Proceso_CierreNomina()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            int errorInd = 0;
            string errorMsg;

            try
            {
                //Ejecuta procedimiento almacenado para liquidar la Prenomina
                this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_CierreNomina(out errorInd, out errorMsg);

                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;

                    #region Errores Generales

                    switch (errorInd)
                    {
                        case 1:
                            {
                                result.ResultMessage = errorMsg;
                                break;
                            }
                        case 2:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_NotExistLiqNomPeriodo;
                                break;
                            }
                    }
                }

                    #endregion

                #endregion
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                return result;
            }

        }

        #endregion

        #region Contabilizacion Nomina

        public List<DTO_TxResult> Contabilizar_Nomina(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones, TipoLiquidacion tipoLiquidacion)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();
            if (liquidaciones != null && liquidaciones.Count > 0)
            {
                noContabilizacionBase objContable = new noContabilizarNomina(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                objContable.Periodo = periodo;
                objContable.FechaDoc = fechaDoc;
                objContable.DocumentoId = documentoID;
                ((noContabilizarNomina)objContable).TipoLiquidacion = tipoLiquidacion;
                objContable.Liquidaciones = liquidaciones;
                lresultados = objContable.Contabilizar();
                objContable.GenerarConsecutivos(((noContabilizarNomina)objContable).glCtrlNomina.NumeroDoc.Value.Value, documentoID, objContable.Comp);
            }
            return lresultados;
        }

        public List<DTO_TxResult> Contabilizar_Provisiones(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();
            if (liquidaciones != null && liquidaciones.Count > 0)
            {
                noContabilizacionBase objContable = new noContabilizarProvisiones(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                objContable.Periodo = periodo;
                objContable.FechaDoc = fechaDoc;
                objContable.DocumentoId = documentoID;
                objContable.Liquidaciones = liquidaciones;
                lresultados = objContable.Contabilizar();
                objContable.GenerarConsecutivos(((noContabilizarProvisiones)objContable).glCtrlProvisiones.NumeroDoc.Value.Value, documentoID, objContable.Comp);
            }
            return lresultados;
        }

        public List<DTO_TxResult> Contabilizar_Planilla(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();
            if (liquidaciones != null && liquidaciones.Count > 0)
            {
                noContabilizacionBase objContable = new noContabilizarPlanilla(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                objContable.Periodo = periodo;
                objContable.FechaDoc = fechaDoc;
                objContable.DocumentoId = documentoID;
                objContable.Liquidaciones = liquidaciones;
                lresultados = objContable.Contabilizar();
                objContable.GenerarConsecutivos(((noContabilizarPlanilla)objContable).glCtrlAportes.NumeroDoc.Value.Value, documentoID, objContable.Comp);
            }
            return lresultados;
        }
        
        /// <summary>
        /// Contabiliza la Nomina para el Periodo Actual
        /// </summary>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="batchProgress">Barra de Progreso</param>
        /// <returns>Resultado</returns>
        public List<DTO_TxResult> Proceso_ContabilizarNomina(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones, byte procesarSel, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();

            switch (procesarSel)
            {
                case 1://Procesa Nomina
                {
                    #region Nomina
                    var resultNomina = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.N).ToList(), TipoLiquidacion.N);
                    lresultados.AddRange(resultNomina);
                    #endregion
                    if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                    {
                        #region Vacaciones

                        var resultVacaciones = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.V).ToList(), TipoLiquidacion.V);
                        lresultados.AddRange(resultVacaciones);

                        #endregion
                        if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                        {
                            #region Prima

                            var resultPrima = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.P).ToList(), TipoLiquidacion.P);
                            lresultados.AddRange(resultPrima);

                            #endregion
                            if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                            {
                                #region Cesantias

                                var resultCesantias = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.C).ToList(), TipoLiquidacion.C);
                                lresultados.AddRange(resultCesantias);

                                #endregion
                                if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                                {
                                    #region Liquidación Contrato

                                    var resultLiqContrato = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.L).ToList(), TipoLiquidacion.L);
                                    lresultados.AddRange(resultLiqContrato);

                                    #endregion
                                }
                            }
                        }
                    }
                    break;
                }
                case 2://Procesa Provisiones
                {
                    #region Provisiones
                    var resultProvisiones = this.Contabilizar_Provisiones(AppDocuments.ProvisionesContabilidad, periodo, fechaDoc, liquidaciones);
                    lresultados.AddRange(resultProvisiones);
                    #endregion
                    break;
                }
                case 3://Procesa Planilla Aportes
                {
                    #region Planilla Aportes
                    var resultPlanilla = this.Contabilizar_Planilla(AppDocuments.PlanillaAportesContabilidad, periodo, fechaDoc, liquidaciones);
                    lresultados.AddRange(resultPlanilla);
                    #endregion
                    break;
                }
                default : //Procesa Todos
                {
                        #region Nomina

                        var resultNomina = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.N).ToList(), TipoLiquidacion.N);
                        lresultados.AddRange(resultNomina);

                        #endregion
                        if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                        {
                            #region Vacaciones

                            var resultVacaciones = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.V).ToList(), TipoLiquidacion.V);
                            lresultados.AddRange(resultVacaciones);

                            #endregion
                            if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                            {
                                #region Prima

                                var resultPrima = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.P).ToList(), TipoLiquidacion.P);
                                lresultados.AddRange(resultPrima);

                                #endregion
                                if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                                {
                                    #region Cesantias

                                    var resultCesantias = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.C).ToList(), TipoLiquidacion.C);
                                    lresultados.AddRange(resultCesantias);

                                    #endregion
                                    if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                                    {
                                        #region Liquidación Contrato

                                        var resultLiqContrato = this.Contabilizar_Nomina(AppDocuments.NominaContabilidad, periodo, fechaDoc, liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.L).ToList(), TipoLiquidacion.L);
                                        lresultados.AddRange(resultLiqContrato);

                                        #endregion
                                        if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                                        {
                                            #region Provisiones

                                            var resultProvisiones = this.Contabilizar_Provisiones(AppDocuments.ProvisionesContabilidad, periodo, fechaDoc, liquidaciones);
                                            lresultados.AddRange(resultProvisiones);

                                            #endregion
                                            if (!lresultados.Exists(x => x.Result == ResultValue.NOK))
                                            {
                                                #region Planilla Aportes

                                                var resultPlanilla = this.Contabilizar_Planilla(AppDocuments.PlanillaAportesContabilidad, periodo, fechaDoc, liquidaciones);
                                                lresultados.AddRange(resultPlanilla);

                                                #endregion
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }                
            }
            return lresultados;
        }

        /// <summary>
        /// Proceso realiza el pago de la Nomina
        /// </summary>
        /// <param name="documentoID">identificador documento</param>
        /// <param name="periodo">Periodo de Pago</param>
        /// <param name="liquidaciones">listado de liquidaciones</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <param name="insideAnotherTx">indica si viene de una transacción</param>
        /// <returns></returns>
        public List<DTO_TxResult> Proceso_PagoNomina(int documentoID, DateTime periodo, List<DTO_noPagoLiquidaciones> liquidaciones, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_Comprobante comprobante = null;
            DTO_coComprobante comp = null;
            DTO_glDocumentoControl glCtrlCxP = null;

            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCXP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            
            string mdaLoc = string.Empty;
            string mdaExt = string.Empty;
            string docContable = string.Empty;
            string lineaPres = string.Empty;
            string cCosto = string.Empty;
            string proyectoXDef = string.Empty;
            string lugarGeografico = string.Empty;
            string conceptoCxPcontrol = string.Empty;
            string terceroPorDefecto = string.Empty;
            string prefijDef = string.Empty;
            string concCargoDef = string.Empty;
            string indOrdenPagoDetallado = string.Empty;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / liquidaciones.Count;

            try
            {
                //Valida que existan liquidaciónes para ser procesadas
                if (liquidaciones.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "No existen liquidaciónes";
                    results.Add(result);
                    return results;
                }

                mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                docContable = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_DocumContableLiquidaciones);

                lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                conceptoCxPcontrol = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPagosNomina);
                terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                indOrdenPagoDetallado = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_OrdenPagoTesoreria);

                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, DateTime.Now);

                #region Crea C x P Asociada al Doc

                glCtrlCxP.DocumentoID.Value = documentoID;
                glCtrlCxP.Fecha.Value = periodo;
                glCtrlCxP.PeriodoDoc.Value = periodo;
                glCtrlCxP.PrefijoID.Value = this.GetPrefijoByDocumento(documentoID);
                glCtrlCxP.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrlCxP.TasaCambioCONT.Value = tc;
                glCtrlCxP.TasaCambioDOCU.Value = tc;
                glCtrlCxP.TerceroID.Value = terceroPorDefecto;
                glCtrlCxP.DocumentoTercero.Value = string.Empty;
                glCtrlCxP.CuentaID.Value = string.Empty;
                glCtrlCxP.MonedaID.Value = mdaLoc;
                glCtrlCxP.ProyectoID.Value = proyectoXDef;
                glCtrlCxP.CentroCostoID.Value = cCosto;
                glCtrlCxP.LineaPresupuestoID.Value = lineaPres;
                glCtrlCxP.Observacion.Value = string.Empty;
                glCtrlCxP.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrlCxP.DocumentoNro.Value = 0;
                glCtrlCxP.PeriodoUltMov.Value = periodo;
                glCtrlCxP.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrlCxP.seUsuarioID.Value = this.UserId;

                decimal valorTotal = liquidaciones.Sum(x => x.DocLiquidacion.Valor.Value.Value);
                object obj = this._moduloCXP.CuentasXPagar_Generar(glCtrlCxP, conceptoCxPcontrol, valorTotal, null, ModulesPrefix.no, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    results.Add(result);
                    return results;
                }

                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, glCtrlCxP.ComprobanteID.Value, true, false);
                #endregion
                #region Agregar Footer Comprobante Nomina
                List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
                DTO_ComprobanteFooter footer = null;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;


                #region Crea el detalle comprobante

                footer = new DTO_ComprobanteFooter();

                DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, docContable, true, false);
                DTO_coPlanCuenta coPlanCta = coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableNomina.CuentaLOC.Value, true, false);
                DTO_glConceptoSaldo glConceptoSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, coPlanCta.ConceptoSaldoID.Value, true, false);

                footer = this.CrearComprobanteFooter(glCtrlCxP, coPlanCta, glConceptoSaldo, tc, concCargoDef, lugarGeografico, lineaPres, valorTotal, valorTotal + tc, false);
                footer.Descriptivo.Value = "Pago Nomina";
                lFooter.Add(footer);

                #endregion
                #region Crea la contrapartida

                DTO_cpConceptoCXP conceptoCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, conceptoCxPcontrol, true, false);
                DTO_coDocumento docContableCxP = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, conceptoCxP.coDocumentoID.Value, true, false);
                DTO_ComprobanteFooter footerConPtda = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerRetConPtda = new DTO_ComprobanteFooter();
                DTO_coPlanCuenta coPlanCptda = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableCxP.CuentaLOC.Value, true, false);
                DTO_glConceptoSaldo glConceptoSaldoConPdtda = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, coPlanCptda.ConceptoSaldoID.Value, true, false);

                footerConPtda = this.CrearComprobanteFooter(glCtrlCxP, coPlanCptda, glConceptoSaldoConPdtda, tc, concCargoDef, lugarGeografico, lineaPres, valorTotal, valorTotal * tc, true);
                footerConPtda.Descriptivo.Value = "Pago Nomina - Contrapartida";
                comprobante.Footer.Add(footerConPtda);

                #endregion

                #endregion

                batchProgress[tupProgress] = 100;
                return results;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_PagoNomina");
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
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        glCtrlCxP.ComprobanteID.Value = comp.ID.Value;
                        glCtrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, glCtrlCxP.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, glCtrlCxP.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(glCtrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrlCxP.NumeroDoc.Value.Value, glCtrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }


        /// <summary>
        /// Pago Nomina Detallada por Empleado
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="periodo">periodo</param>
        /// <param name="fechaDoc">fechaDoc</param>
        /// <param name="liquidaciones">liquidaciones</param>
        /// <param name="batchProgress"></param>
        /// <returns></returns>
        public List<DTO_TxResult> Proceso_PagoNominaDetallada(int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / liquidaciones.Count;

            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> lresultados = new List<DTO_TxResult>();
            List<DTO_glDocumentoControl> docsCxP = new List<DTO_glDocumentoControl>();
            List<DTO_coComprobante> comps = new List<DTO_coComprobante>();
            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCXP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            try
            {
                if (liquidaciones != null && liquidaciones.Count > 0)
                {
                    foreach (DTO_noPagoLiquidaciones pago in liquidaciones)
                    {
                        List<DTO_TxResult> results = new List<DTO_TxResult>();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        DTO_Comprobante comprobante = new DTO_Comprobante();
                        DTO_glDocumentoControl glCtrl = null;
                        DTO_coComprobante Comp = null;
                        DTO_glDocumentoControl ctrlCxP = null;

                        string mdaLoc = string.Empty;
                        string mdaExt = string.Empty;
                        string docContable = string.Empty;
                        string lineaPres = string.Empty;
                        string cCosto = string.Empty;
                        string proyectoXDef = string.Empty;
                        string lugarGeografico = string.Empty;
                        string conceptoCxPcontrol = string.Empty;
                        string terceroPorDefecto = string.Empty;
                        string prefijDef = string.Empty;
                        string concCargoDef = string.Empty;

                        //Actualiza indicador de Pago en Documento Origen
                        this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdatePagadoInd(pago.NumeroDoc.Value.Value, true);

                        mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                        mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                        docContable = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_DocumContableLiquidaciones);
                        lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                        cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                        proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                        lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                        conceptoCxPcontrol = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPagosNomina);
                        terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                        prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                        concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);

                        decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, DateTime.Now);
                        #region Descripcion documentos segun el tipo de liquidacion

                        string description = string.Empty;
                        switch (pago.DocLiquidacion.TipoLiquidacion.Value.Value)
                        {
                            case (int)TipoLiquidacion.N:
                                {
                                    description = "Nomina";
                                    break;
                                }
                            case (int)TipoLiquidacion.V:
                                {
                                    description = "Vacaciones";
                                    break;
                                }
                            case (int)TipoLiquidacion.P:
                                {
                                    description = "Prima";
                                    break;
                                }
                            case (int)TipoLiquidacion.C:
                                {
                                    description = "Cesantias";
                                    break;
                                }
                            case (int)TipoLiquidacion.L:
                                {
                                    description = "Liquidación Contrato";
                                    break;
                                }
                        }

                        #endregion
                        #region Crea C x P Asociada al Doc

                        int numeroDoc = 0;
                        glCtrl = new DTO_glDocumentoControl();
                        glCtrl.Fecha.Value = DateTime.Now;
                        glCtrl.DocumentoID.Value = documentoID;
                        glCtrl.PeriodoDoc.Value = periodo;
                        glCtrl.FechaDoc.Value = fechaDoc;
                        glCtrl.PrefijoID.Value = prefijDef;
                        glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                        glCtrl.DocumentoNro.Value = 0;
                        glCtrl.TasaCambioCONT.Value = tc;
                        glCtrl.TasaCambioDOCU.Value = tc;
                        glCtrl.TerceroID.Value = pago.EmpleadoID.Value;
                        glCtrl.DocumentoTercero.Value = string.Empty;
                        glCtrl.CuentaID.Value = string.Empty;
                        glCtrl.MonedaID.Value = mdaLoc;
                        glCtrl.ProyectoID.Value = proyectoXDef;
                        glCtrl.CentroCostoID.Value = cCosto;
                        glCtrl.LineaPresupuestoID.Value = lineaPres;
                        glCtrl.Observacion.Value = "PAGO NOMINA DETALLADA";
                        glCtrl.Descripcion.Value = "PAGO NOMINA DETALLADA ";
                        glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                        glCtrl.PeriodoUltMov.Value = periodo;
                        glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        glCtrl.seUsuarioID.Value = this.UserId;
                        DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
                        if (result.Result == ResultValue.OK)
                        {
                            resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                            if (resultGLDC.Message != ResultValue.OK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_AddDocument;
                                results.Add(result);
                            }
                            else
                            {
                                numeroDoc = Convert.ToInt32(resultGLDC.Key);
                                glCtrl.NumeroDoc.Value = numeroDoc;
                            }
                        }

                        #endregion
                        #region Agregar Footer Comprobante Nomina
                        List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
                        DTO_ComprobanteFooter footer = null;

                        #region Crea el detalle comprobante

                        footer = new DTO_ComprobanteFooter();

                        DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);
                        DTO_coPlanCuenta coPlanCta = coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableNomina.CuentaLOC.Value, true, false);
                        DTO_glConceptoSaldo glConceptoSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, coPlanCta.ConceptoSaldoID.Value, true, false);

                        footer = this.CrearComprobanteFooter(glCtrl, coPlanCta, glConceptoSaldo, tc, concCargoDef, lugarGeografico, lineaPres, pago.Valor.Value.Value, pago.Valor.Value.Value * tc, false);
                        footer.Descriptivo.Value = String.Format("{0}", "Pago " + description); ;
                        lFooter.Add(footer);

                        #endregion
                        #region Crea C x P Asociada al Doc

                        object obj = this._moduloCXP.CuentasXPagar_Generar(glCtrl, conceptoCxPcontrol, pago.Valor.Value.Value, lFooter, ModulesPrefix.no, false);
                        if (obj.GetType() == typeof(DTO_TxResult))
                        {
                            result = (DTO_TxResult)obj;
                            results.Add(result);
                            return results;
                        }
                        ctrlCxP = (DTO_glDocumentoControl)obj;
                        Comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
                        docsCxP.Add(ctrlCxP);
                        comps.Add(Comp);
                        #endregion
                        #endregion
                        lresultados.AddRange(results);
                    }

                }
                return lresultados;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_PagoNominaDetallada");
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
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        foreach (DTO_glDocumentoControl ctrl in docsCxP)
                        {
                            //glCtrlCxP.ComprobanteID.Value = comp.ID.Value;
                            DTO_coComprobante comp = comps.Find(x=>x.ID.Value == ctrl.ComprobanteID.Value);
                            ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(ctrl, false, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);

                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        #endregion

        #region Contabilizacion Planilla

        /// <summary>
        /// Proceso de contabilización de la Planilla de Aportes
        /// </summary>
        /// <param name="documentoID">identificador del documento de Pagos</param>
        /// <param name="periodo">Periodo actual de la Nomina</param>
        /// <param name="liquidaciones">liquidaciones de la Planilla</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <param name="insideAnotherTx">determina si viene de un transaccion</param>
        /// <returns>listado de resultados</returns>
        public List<DTO_TxResult> Proceso_ContabilizarPlanilla(int documentoID, DateTime periodo, List<DTO_noPlanillaAportesDeta> liquidaciones, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_Comprobante comprobante = null;
            DTO_coComprobante comp = null;
            DTO_glDocumentoControl glCtrlCxP = null;
            DTO_glDocumentoControl ctrlPlanilla = null;
            DTO_coPlanCuenta coPlanCta = null;
            DTO_noComponenteNomina componente = null;
            DTO_noConceptoNOM conceptoNom = null;

            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noPagoPlanillaAportesDocu = (DAL_noPagoPlanillaAportesDocu)this.GetInstance(typeof(DAL_noPagoPlanillaAportesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCXP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            string mdaLoc = string.Empty;
            string mdaExt = string.Empty;
            string lineaPres = string.Empty;
            string cCosto = string.Empty;
            string proyectoXDef = string.Empty;
            string lugarGeografico = string.Empty;
            string conceptoCxP = string.Empty;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / liquidaciones.Count;

            try
            {

                //Parametros de la Empresa
                mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                conceptoCxP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPPagoAportes);

                //ParaFiscales
                string cajaCompensacionByS = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySCajadeCompensacion);
                string icbfByS = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySICBF);
                string senaByS = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySSena);
                string arpByS = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ClaseBySArp);
                string terceroICBF = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TercerICBF);
                string terceroSENA = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TerceroSena);
                string terceroARP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TerceroARP);
                string ctaParaFiscales = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CxPParaFiscales);

                #region Traer glDocumentoControl 87 - NominaPlanilla

                ctrlPlanilla = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(AppDocuments.PlanillaAutoLiquidAportes, periodo).FirstOrDefault();
                var count = this._dal_MasterSimple.DAL_MasterSimple_Count(null, null, true);
                var lcomponentes = this._dal_MasterSimple.DAL_MasterSimple_GetPaged(count, 1, null, null, true);
                this._dal_MasterSimple.DocumentID = AppMasters.noComponenteNomina;

                #endregion

                #region Agregar Footer Comprobante Nomina
                List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
                DTO_ComprobanteFooter footer = null;
                foreach (var liq in liquidaciones)
                {
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #region Crea el detalle comprobante

                    #region Aportes Salud

                    string noConceptoSalud = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSalud);
                    componente = (DTO_noComponenteNomina)lcomponentes.Where(x => ((DTO_noComponenteNomina)x).ConceptoNOID.Value == noConceptoSalud).FirstOrDefault();
                    conceptoNom = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, componente.ConceptoNOID.Value, true, false);
                    DTO_glBienServicioClase claseBySSalud = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, conceptoNom.ClaseBSID.Value, true, false);
                    DTO_noFondo fondoSalud = (DTO_noFondo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noFondo, liq.FondoSalud.Value, true, false);
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, componente.CuentaID.Value, true, false);

                    if (!lFooter.Any(x => x.TerceroID.Value == fondoSalud.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSalud.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSalud.LineaPresupuestoID.Value))
                    {
                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySSalud.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySSalud.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = fondoSalud.TerceroID.Value;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = ctrlPlanilla.MonedaID.Value == mdaLoc ? liq.VlrEmpresaSLD.Value / ctrlPlanilla.TasaCambioDOCU.Value.Value : liq.VlrEmpresaSLD.Value;
                        footer.vlrMdaLoc.Value = ctrlPlanilla.MonedaID.Value == mdaLoc ? liq.VlrEmpresaSLD.Value : liq.VlrEmpresaSLD.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaOtr.Value = liq.VlrEmpresaSLD.Value;

                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == fondoSalud.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSalud.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSalud.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrEmpresaSLD.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == fondoSalud.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSalud.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSalud.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrEmpresaSLD.Value;
                        lFooter.Where(x => x.TerceroID.Value == fondoSalud.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSalud.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSalud.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrEmpresaSLD.Value;
                    }
                    #endregion
                    #region Aportes Pension

                    string noConceptoPension = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoPension);
                    componente = (DTO_noComponenteNomina)lcomponentes.Where(x => ((DTO_noComponenteNomina)x).ConceptoNOID.Value == noConceptoPension).FirstOrDefault();
                    conceptoNom = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, componente.ConceptoNOID.Value, true, false);
                    DTO_glBienServicioClase claseBySPension = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, conceptoNom.ClaseBSID.Value, true, false);
                    DTO_noFondo fondoPension = (DTO_noFondo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noFondo, noConceptoPension, true, false);
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, componente.CuentaID.Value, true, false);

                    if (!lFooter.Any(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSalud.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSalud.LineaPresupuestoID.Value))
                    {
                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySPension.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySPension.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = fondoPension.TerceroID.Value;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = liq.VlrEmpresaPEN.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaLoc.Value = liq.VlrEmpresaPEN.Value;
                        footer.vlrMdaOtr.Value = liq.VlrEmpresaPEN.Value;

                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySPension.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySPension.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrEmpresaPEN.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySPension.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySPension.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrEmpresaPEN.Value;
                        lFooter.Where(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySPension.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySPension.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrEmpresaPEN.Value;
                    }
                    #endregion
                    #region Aportes Solidaridad

                    string noConceptoSolidaridad = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSolidaridad);
                    componente = (DTO_noComponenteNomina)lcomponentes.Where(x => ((DTO_noComponenteNomina)x).ConceptoNOID.Value == noConceptoSolidaridad).FirstOrDefault();
                    conceptoNom = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, noConceptoSolidaridad, true, false);
                    DTO_glBienServicioClase claseBySSoli = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, conceptoNom.ClaseBSID.Value, true, false);
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, componente.CuentaID.Value, true, false);

                    if (!lFooter.Any(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSalud.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSalud.LineaPresupuestoID.Value))
                    {
                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySSoli.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySSoli.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = fondoPension.TerceroID.Value;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = liq.VlrSolidaridad.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaLoc.Value = liq.VlrSolidaridad.Value;
                        footer.vlrMdaOtr.Value = liq.VlrSolidaridad.Value;
                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSoli.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSoli.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrSolidaridad.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSoli.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSoli.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrSolidaridad.Value;
                        lFooter.Where(x => x.TerceroID.Value == fondoPension.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSoli.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSoli.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrSolidaridad.Value;
                    }
                    #endregion
                    #region Caja de Compensacion y SENA

                    //Cuenta Debito para Caja de Compensacion                    
                    DTO_glBienServicioClase claseBySCCF = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, cajaCompensacionByS, true, false);

                    footer = new DTO_ComprobanteFooter();
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaParaFiscales, true, false);
                    DTO_noCaja caja = (DTO_noCaja)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noCaja, liq.CajaNOID.Value, true, false);

                    if (!lFooter.Any(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySCCF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySCCF.LineaPresupuestoID.Value))
                    {
                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySCCF.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySCCF.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = caja.TerceroID.Value;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = liq.VlrCCF.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaLoc.Value = liq.VlrCCF.Value;
                        footer.vlrMdaOtr.Value = liq.VlrCCF.Value;

                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySCCF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySCCF.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrCCF.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySCCF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySCCF.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrCCF.Value;
                        lFooter.Where(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySCCF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySCCF.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrCCF.Value;
                    }

                    //Cuenta Debito para Caja el SENA               
                    DTO_glBienServicioClase claseBySSENA = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, senaByS, true, false);
                    if (!lFooter.Any(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSENA.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSENA.LineaPresupuestoID.Value))
                    {

                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySSENA.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySSENA.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = caja.TerceroID.Value;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = liq.VlrSEN.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaLoc.Value = liq.VlrSEN.Value;
                        footer.vlrMdaOtr.Value = liq.VlrSEN.Value;

                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSENA.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSENA.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrSEN.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSENA.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSENA.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrSEN.Value;
                        lFooter.Where(x => x.TerceroID.Value == caja.TerceroID.Value && x.ConceptoCargoID.Value == claseBySSENA.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySSENA.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrSEN.Value;
                    }
                    #endregion
                    #region ICBF

                    //Cuenta Debito para el ICBF                  
                    DTO_glBienServicioClase claseBySICBF = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, icbfByS, true, false);
                    if (!lFooter.Any(x => x.TerceroID.Value == terceroICBF && x.ConceptoCargoID.Value == claseBySICBF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySICBF.LineaPresupuestoID.Value))
                    {
                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySICBF.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySICBF.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = terceroICBF;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = liq.VlrICBF.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaLoc.Value = liq.VlrICBF.Value;
                        footer.vlrMdaOtr.Value = liq.VlrICBF.Value;

                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == terceroICBF && x.ConceptoCargoID.Value == claseBySICBF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySICBF.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrICBF.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == terceroICBF && x.ConceptoCargoID.Value == claseBySICBF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySICBF.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrICBF.Value;
                        lFooter.Where(x => x.TerceroID.Value == terceroICBF && x.ConceptoCargoID.Value == claseBySICBF.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySICBF.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrICBF.Value;
                    }
                    #endregion
                    #region ARP

                    //Cuenta Debito para la ARP                  
                    DTO_glBienServicioClase claseBySARP = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, arpByS, true, false);

                    if (!lFooter.Any(x => x.TerceroID.Value == terceroARP && x.ConceptoCargoID.Value == claseBySARP.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySARP.LineaPresupuestoID.Value))
                    {
                        footer = new DTO_ComprobanteFooter();
                        footer.CentroCostoID.Value = ctrlPlanilla.CentroCostoID.Value;
                        footer.CuentaID.Value = coPlanCta.ID.Value;
                        footer.ConceptoCargoID.Value = claseBySARP.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = ctrlPlanilla.DocumentoNro.Value.ToString();
                        footer.Descriptivo.Value = ctrlPlanilla.Observacion.Value;
                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                        footer.IdentificadorTR.Value = ctrlPlanilla.NumeroDoc.Value.Value;
                        footer.LineaPresupuestoID.Value = claseBySARP.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = ctrlPlanilla.LugarGeograficoID.Value;
                        footer.PrefijoCOM.Value = ctrlPlanilla.PrefijoID.Value;
                        footer.ProyectoID.Value = ctrlPlanilla.ProyectoID.Value;
                        footer.TasaCambio.Value = ctrlPlanilla.TasaCambioCONT.Value;
                        footer.TerceroID.Value = terceroARP;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = liq.VlrARP.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        footer.vlrMdaLoc.Value = liq.VlrARP.Value;
                        footer.vlrMdaOtr.Value = liq.VlrARP.Value;

                        lFooter.Add(footer);
                    }
                    else
                    {
                        lFooter.Where(x => x.TerceroID.Value == terceroARP && x.ConceptoCargoID.Value == claseBySARP.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySARP.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaExt.Value += liq.VlrARP.Value * ctrlPlanilla.TasaCambioDOCU.Value.Value;
                        lFooter.Where(x => x.TerceroID.Value == terceroARP && x.ConceptoCargoID.Value == claseBySARP.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySARP.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaLoc.Value += liq.VlrARP.Value;
                        lFooter.Where(x => x.TerceroID.Value == terceroARP && x.ConceptoCargoID.Value == claseBySARP.ConceptoCargoID.Value && x.LineaPresupuestoID.Value == claseBySARP.LineaPresupuestoID.Value).FirstOrDefault().vlrMdaOtr.Value += liq.VlrARP.Value;

                    }
                    #endregion

                    #endregion
                }


                #endregion

                #region Crea C x P Asociada al Doc  - 87 Planilla de Aportes

                //Calcula el total de acuerda a la configuracion de la moneda del documento
                decimal valorTotal = 0;
                if (mdaLoc == ctrlPlanilla.MonedaID.Value)
                    valorTotal = lFooter.Sum(x => x.vlrMdaLoc.Value.Value);
                else
                    valorTotal = lFooter.Sum(x => x.vlrMdaExt.Value.Value);

                object obj = this._moduloCXP.CuentasXPagar_Generar(ctrlPlanilla, conceptoCxP, valorTotal, lFooter, ModulesPrefix.no, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    results.Add(result);
                    return results;
                }

                //Trae la CxP para actualizar los consecutivos
                glCtrlCxP = (DTO_glDocumentoControl)obj;
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, glCtrlCxP.ComprobanteID.Value, true, false);

                #endregion

                #region Registra el Pago

                DTO_noPagoPlanillaAportesDocu pagoPlanilla = new DTO_noPagoPlanillaAportesDocu();
                pagoPlanilla.EmpresaID.Value = this.Empresa.ID.Value;
                pagoPlanilla.NumeroDoc.Value = ctrlPlanilla.NumeroDoc.Value;
                pagoPlanilla.NumeroDocCXP.Value = glCtrlCxP.NumeroDoc.Value;
                pagoPlanilla.Valor.Value = valorTotal;
                pagoPlanilla.Iva.Value = 0;

                this._dal_noPagoPlanillaAportesDocu.DAL_noPagoPlanillaAportesDocu_Add(pagoPlanilla);

                #endregion

                batchProgress[tupProgress] = 100;
                return results;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_ContabilizarPlanilla");
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
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        glCtrlCxP.ComprobanteID.Value = comp.ID.Value;
                        glCtrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, glCtrlCxP.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, glCtrlCxP.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(glCtrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrlCxP.NumeroDoc.Value.Value, glCtrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }


        }

        #endregion

        #region Contabilizacion Provisiones

        /// <summary>
        /// Contabiliza las Provisiones para el Periodo Actual
        /// </summary>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="batchProgress">Barra de Progreso</param>
        /// <returns>Resultado</returns>
        public List<DTO_TxResult> Proceso_ContabilizarProvisiones(int documentoID, DateTime periodo, List<DTO_noProvisionDeta> liquidaciones, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {

            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_Comprobante comprobante = null;
            DTO_coComprobante comp = null;
            DTO_glDocumentoControl glCtrlCxP = null;
            DTO_glDocumentoControl ctrlProvisiones = null;
            DTO_coPlanCuenta coPlanCta = null;
            DTO_noComponenteNomina componente = null;
            DTO_noConceptoNOM conceptoNom = null;

            this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noPagoPlanillaAportesDocu = (DAL_noPagoPlanillaAportesDocu)this.GetInstance(typeof(DAL_noPagoPlanillaAportesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloCXP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            string mdaLoc = string.Empty;
            string mdaExt = string.Empty;
            string lineaPres = string.Empty;
            string cCosto = string.Empty;
            string proyectoXDef = string.Empty;
            string lugarGeografico = string.Empty;
            string conceptoCxP = string.Empty;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / liquidaciones.Count;

            try
            {
                #region Traer glDocumentoControl 87 - NominaPlanilla

                ctrlProvisiones = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(AppDocuments.Provisiones, periodo).FirstOrDefault();
                var count = this._dal_MasterSimple.DAL_MasterSimple_Count(null, null, true);
                var lcomponentes = this._dal_MasterSimple.DAL_MasterSimple_GetPaged(count, 1, null, null, true);
                this._dal_MasterSimple.DocumentID = AppMasters.noComponenteNomina;

                string conceptoVacaciones = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoVacacionesTiempo);
                string conceptoPrima = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoPrimaServicios);
                string conceptoCesantias = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCesantias);
                string conceptoIntCesantias = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoInteresCesantias);

                #region Crear Footer

                List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
                DTO_ComprobanteFooter footer = null;
                DTO_noConceptoNOM _conceptoVacaciones = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, conceptoVacaciones, true, false);
                DTO_noConceptoNOM _conceptoPrima = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, conceptoPrima, true, false);
                DTO_noConceptoNOM _conceptoCesantias = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, conceptoCesantias, true, false);
                DTO_noConceptoNOM _conceptoIntCesantias = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, conceptoIntCesantias, true, false);

                //Clase de bien y servicio para los conceptos de provisiones
                DTO_glBienServicioClase claseBySVacaciones = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, _conceptoVacaciones.ClaseBSID.Value, true, false);
                DTO_glBienServicioClase claseBySPrima = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, _conceptoPrima.ClaseBSID.Value, true, false);
                DTO_glBienServicioClase claseBySCesantias = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, _conceptoCesantias.ClaseBSID.Value, true, false);
                DTO_glBienServicioClase claseBySIntCesantias = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, _conceptoIntCesantias.ClaseBSID.Value, true, false);


                foreach (var liq in liquidaciones)
                {
                    DTO_noEmpleado empleado = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, liq.EmpleadoID.Value, true, false);

                    #region Provision Vacaciones

                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _conceptoVacaciones.CuentaID.Value, true, false);

                    footer = new DTO_ComprobanteFooter();
                    footer.CentroCostoID.Value = ctrlProvisiones.CentroCostoID.Value;
                    footer.CuentaID.Value = coPlanCta.ID.Value;
                    footer.ConceptoCargoID.Value = claseBySVacaciones.ConceptoCargoID.Value;
                    footer.DocumentoCOM.Value = ctrlProvisiones.DocumentoNro.Value.ToString();
                    footer.Descriptivo.Value = ctrlProvisiones.Observacion.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = ctrlProvisiones.NumeroDoc.Value.Value;
                    footer.LineaPresupuestoID.Value = claseBySVacaciones.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = ctrlProvisiones.LugarGeograficoID.Value;
                    footer.PrefijoCOM.Value = ctrlProvisiones.PrefijoID.Value;
                    footer.ProyectoID.Value = ctrlProvisiones.ProyectoID.Value;
                    footer.TasaCambio.Value = ctrlProvisiones.TasaCambioCONT.Value;
                    footer.TerceroID.Value = empleado.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = liq.VlrPagosMES.Value * ctrlProvisiones.TasaCambioDOCU.Value.Value;
                    footer.vlrMdaLoc.Value = liq.VlrPagosMES.Value;
                    footer.vlrMdaOtr.Value = liq.VlrPagosMES.Value;

                    lFooter.Add(footer);

                    #endregion

                    #region Provision Prima

                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _conceptoPrima.CuentaID.Value, true, false);

                    footer = new DTO_ComprobanteFooter();
                    footer.CentroCostoID.Value = ctrlProvisiones.CentroCostoID.Value;
                    footer.CuentaID.Value = coPlanCta.ID.Value;
                    footer.ConceptoCargoID.Value = claseBySPrima.ConceptoCargoID.Value;
                    footer.DocumentoCOM.Value = ctrlProvisiones.DocumentoNro.Value.ToString();
                    footer.Descriptivo.Value = ctrlProvisiones.Observacion.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = ctrlProvisiones.NumeroDoc.Value.Value;
                    footer.LineaPresupuestoID.Value = claseBySPrima.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = ctrlProvisiones.LugarGeograficoID.Value;
                    footer.PrefijoCOM.Value = ctrlProvisiones.PrefijoID.Value;
                    footer.ProyectoID.Value = ctrlProvisiones.ProyectoID.Value;
                    footer.TasaCambio.Value = ctrlProvisiones.TasaCambioCONT.Value;
                    footer.TerceroID.Value = empleado.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = liq.VlrPagosMES.Value * ctrlProvisiones.TasaCambioDOCU.Value.Value;
                    footer.vlrMdaLoc.Value = liq.VlrPagosMES.Value;
                    footer.vlrMdaOtr.Value = liq.VlrPagosMES.Value;

                    lFooter.Add(footer);

                    #endregion

                    #region Prvision Cesantias

                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _conceptoCesantias.CuentaID.Value, true, false);

                    footer = new DTO_ComprobanteFooter();
                    footer.CentroCostoID.Value = ctrlProvisiones.CentroCostoID.Value;
                    footer.CuentaID.Value = coPlanCta.ID.Value;
                    footer.ConceptoCargoID.Value = claseBySCesantias.ConceptoCargoID.Value;
                    footer.DocumentoCOM.Value = ctrlProvisiones.DocumentoNro.Value.ToString();
                    footer.Descriptivo.Value = ctrlProvisiones.Observacion.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = ctrlProvisiones.NumeroDoc.Value.Value;
                    footer.LineaPresupuestoID.Value = claseBySCesantias.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = ctrlProvisiones.LugarGeograficoID.Value;
                    footer.PrefijoCOM.Value = ctrlProvisiones.PrefijoID.Value;
                    footer.ProyectoID.Value = ctrlProvisiones.ProyectoID.Value;
                    footer.TasaCambio.Value = ctrlProvisiones.TasaCambioCONT.Value;
                    footer.TerceroID.Value = empleado.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = liq.VlrPagosMES.Value * ctrlProvisiones.TasaCambioDOCU.Value.Value;
                    footer.vlrMdaLoc.Value = liq.VlrPagosMES.Value;
                    footer.vlrMdaOtr.Value = liq.VlrPagosMES.Value;

                    lFooter.Add(footer);

                    #endregion

                    #region Provision Int Cesantias

                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _conceptoIntCesantias.CuentaID.Value, true, false);

                    footer = new DTO_ComprobanteFooter();
                    footer.CentroCostoID.Value = ctrlProvisiones.CentroCostoID.Value;
                    footer.CuentaID.Value = coPlanCta.ID.Value;
                    footer.ConceptoCargoID.Value = claseBySIntCesantias.ConceptoCargoID.Value;
                    footer.DocumentoCOM.Value = ctrlProvisiones.DocumentoNro.Value.ToString();
                    footer.Descriptivo.Value = ctrlProvisiones.Observacion.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = ctrlProvisiones.NumeroDoc.Value.Value;
                    footer.LineaPresupuestoID.Value = claseBySIntCesantias.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = ctrlProvisiones.LugarGeograficoID.Value;
                    footer.PrefijoCOM.Value = ctrlProvisiones.PrefijoID.Value;
                    footer.ProyectoID.Value = ctrlProvisiones.ProyectoID.Value;
                    footer.TasaCambio.Value = ctrlProvisiones.TasaCambioCONT.Value;
                    footer.TerceroID.Value = empleado.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = liq.VlrPagosMES.Value * ctrlProvisiones.TasaCambioDOCU.Value.Value;
                    footer.vlrMdaLoc.Value = liq.VlrPagosMES.Value;
                    footer.vlrMdaOtr.Value = liq.VlrPagosMES.Value;

                    lFooter.Add(footer);

                    #endregion
                }

                #region Crea C x P Asociada al Doc  - 86 Provisiones

                //Calcula el total de acuerda a la configuracion de la moneda del documento
                decimal valorTotal = 0;
                if (mdaLoc == ctrlProvisiones.MonedaID.Value)
                    valorTotal = lFooter.Sum(x => x.vlrMdaLoc.Value.Value);
                else
                    valorTotal = lFooter.Sum(x => x.vlrMdaExt.Value.Value);

                object obj = this._moduloCXP.CuentasXPagar_Generar(ctrlProvisiones, conceptoCxP, valorTotal, lFooter, ModulesPrefix.no, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    results.Add(result);
                    return results;
                }

                //Trae la CxP para actualizar los consecutivos
                glCtrlCxP = (DTO_glDocumentoControl)obj;
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, glCtrlCxP.ComprobanteID.Value, true, false);

                #endregion

                #endregion
                #endregion

                batchProgress[tupProgress] = 100;
                return results;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_ContabilizarProvisiones");
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
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        glCtrlCxP.ComprobanteID.Value = comp.ID.Value;
                        glCtrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, glCtrlCxP.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, glCtrlCxP.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(glCtrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrlCxP.NumeroDoc.Value.Value, glCtrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Envio Boletas
        /// <summary>
        /// Obtiene los documentos de liquidación aprobados por periodo
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="tipoLiquida">Tipo de liquidacion</param>
        /// <returns>listado de liquidaciones aprobadas para generar boletas pago</returns>
        public List<DTO_NominaEnvioBoleta> Proceso_noLiquidacionesDocu_GetNominaLiquida(int documentoID,DateTime periodo, byte tipoLiquida)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                bool quincenal = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_LiquidaNominaQuincenal).Equals("1") ? true : false;

                List<DTO_NominaEnvioBoleta> documentos = this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_GetNominaLiquida(periodo, tipoLiquida, quincenal);

                return documentos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "noLiquidacionesDocu_GetNominaLiquida");
                throw exception;
            }
        } 
        #endregion
        
        #endregion

        #region Provisiones


        /// <summary>
        /// Obtiene un listado de la liquidación de Provisiones del empleado en el periodo
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="contratoNOID">número contrato empleado</param>
        /// <returns>lista de provisiones</returns>
        public List<DTO_noProvisionDeta> Nomina_ProvisionDeta_Get(DateTime periodo, int contratoNOID)
        {
            this._dal_noProvisionDeta = (DAL_noProvisionDeta)this.GetInstance(typeof(DAL_noProvisionDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return _dal_noProvisionDeta.DAL_noProvisionDeta_Get(periodo, contratoNOID);
        }

        #endregion

        #region Traslados

        /// <summary>
        /// Obtiene los traslados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de traslados</returns>
        public List<DTO_noTraslado> Nomina_GetTraslados(string empleadoID)
        {
            try
            {
                _dal_noTraslado = (DAL_noTraslado)this.GetInstance(typeof(DAL_noTraslado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return _dal_noTraslado.DAL_noTraslado_GetTraslados(empleadoID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloNomina_Nomina_GetPrestamos");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona un traslado
        /// </summary>
        /// <param name="prestamo">objeto traslado</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddTraslado(List<DTO_noTraslado> traslados, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_noTraslado = (DAL_noTraslado)this.GetInstance(typeof(DAL_noTraslado), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var traslado in traslados)
                {
                    try
                    {
                        int count = this._dal_noTraslado.DAL_noTraslado_ExistNovedad(traslado);
                        if (count == 0)
                        {
                            this._dal_noTraslado.DAL_noTraslado_AddTraslado(traslado);
                        }
                        else
                        {
                            this._dal_noTraslado.DAL_noTraslado_UpdTraslado(traslado);

                        }
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "OK"
                        };
                        result.Details.Add(detalle);
                    }
                    catch (Exception ex)
                    {
                        DTO_TxResultDetail detalle = new DTO_TxResultDetail()
                        {
                            Message = "NOK",
                            Key = ex.Message
                        };
                        result.Details.Add(detalle);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
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

        #region Reportes

        /// <summary>
        /// Funcion que genera le reporte de Prenomina  desde el cliente.
        /// </summary>
        /// <param name="numeroDoc">NUmero documento</param>
        /// <param name="empleados">Lista de empleados</param>
        /// <param name="isApro">Esta aprobado?</param>
        public void PrintPrenomina(int numeroDoc, List<DTO_MasterBasic> empleados, DateTime periodo, List<DTO_noLiquidacionPreliminar> _lDetalle, bool isApro)
        {
            int documentID = AppDocuments.Nomina;
            #region Generar el nuevo archivo
            foreach (DTO_noEmpleado dtoNoEmpleado in empleados)
            {
                this.GenerarArchivo(documentID, numeroDoc, Dto_PreNomina_Report(numeroDoc, periodo, dtoNoEmpleado, _lDetalle, true));
            }
            #endregion
        }

        /// <summary>
        /// Funcion que genera le reporte de Vacaciones  desde el cliente.
        /// </summary>
        /// <param name="numeroDoc">NUmero documento</param>
        /// <param name="empleado">EmpleadoDesc</param>
        /// <param name="liquidacion">Informacion General de la Liquidacion</param>
        /// <param name="_lDetalles">Lista de detalles</param>
        /// <param name="isApro">Esta aprobado?</param>
        public void PrintVacaciones(int numeroDoc, DTO_noEmpleado empleado, DTO_noLiquidacionesDocu liquidacion, List<DTO_noLiquidacionPreliminar> _lDetalles, bool isApro)
        {
            int documentID = AppDocuments.Vacaciones;
            #region Generar el nuevo archivo

            this.GenerarArchivo(documentID, liquidacion.NumeroDoc.Value.Value, Dto_Vacaciones_Report(numeroDoc, liquidacion, empleado, _lDetalles, isApro));
            #endregion
        }

        // <summary>
        //Crea un dto de reporte Pronomina
        //</summary>
        //<param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        //<returns></returns>(int numeroDoc, List<DTO_MasterBasic> lEmpleados, List<DTO_noLiquidacionPreliminar> _ldetalle)
        public DTO_SerializedObject Dto_PreNomina_Report(int numeroDoc, DateTime periodo, DTO_noEmpleado empleados, List<DTO_noLiquidacionPreliminar> _lDetalle, bool isApro)
        {
            try
            {
                #region Variables
                decimal topeDeducc = Convert.ToDecimal(this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TopeMaxDeducRteFte));
                DTO_ReportNoPreNomina dto_preNomina = new DTO_ReportNoPreNomina();
                DTO_ReportNoPreNominaDetail reportdetail = new DTO_ReportNoPreNominaDetail();
                decimal netoPago = 0;
                decimal totalDedu = 0;
                int documentID = 0;
                #endregion
                #region Asigan los valores al dto

                var liquidaciones = this.Nomina_LiquidacionPreliminarGetAll(AppDocuments.Nomina, periodo, (DTO_noEmpleado)empleados);
                var documento = this.noLiquidacionesDocu_Get(periodo, (DTO_noEmpleado)empleados);

                dto_preNomina.Header.CedulaEmpleado = empleados.ID.Value;
                dto_preNomina.Header.Empleado = empleados.IdName;
                dto_preNomina.Header.ProcReteFte = documento.ProcedimientoRteFte.Value.Value;
                dto_preNomina.Header.Porcentaje = documento.PorcentajeRteFte.Value.Value;
                dto_preNomina.Header.DDRete = topeDeducc;
                dto_preNomina.Header.CentroCosto = documento.CentroCostoID.Value;
                dto_preNomina.Header.LocFisica = documento.AreaFuncionalID.Value;
                dto_preNomina.Header.IDEmpleado = empleados.ID.Value;
                dto_preNomina.Header.Cargo = documento.CargoEmpID.Value;
                dto_preNomina.Header.Brigada = documento.BrigadaNOID.Value;
                dto_preNomina.isApro = false;

                List<DTO_noLiquidacionPreliminar> positivos = _lDetalle.Where(x => x.Valor.Value > 0).ToList();
                List<DTO_noLiquidacionPreliminar> negativos = _lDetalle.Where(x => x.Valor.Value < 0).ToList();
                if (positivos.Count >= negativos.Count)
                {
                    for (int i = 0; i < positivos.Count; i++)
                    {
                        reportdetail = new DTO_ReportNoPreNominaDetail();
                        reportdetail.BaseDevengos = positivos[i].Base.Value.Value;
                        reportdetail.ValorDevengos = positivos[i].Valor.Value.Value;
                        reportdetail.CodigoDevengos = positivos[i].ConceptoNOID.Value;
                        reportdetail.DescripcionDevengos = positivos[i].ConceptoNODesc.Value;
                        netoPago += positivos[i].Valor.Value.Value;


                        if (negativos[i] != null)
                        {
                            reportdetail.BaseDeducciones = negativos[i].Base.Value.Value;
                            reportdetail.ValorDeducciones = negativos[i].Valor.Value.Value;
                            reportdetail.CodigoDeducciones = negativos[i].ConceptoNOID.Value;
                            reportdetail.DescripcionDeducciones = negativos[i].ConceptoNODesc.Value;
                            totalDedu += negativos[i].Valor.Value.Value;
                        }
                        dto_preNomina.Detail.Add(reportdetail);
                    }
                }
                else
                {
                    for (int i = 0; i < negativos.Count; i++)
                    {
                        reportdetail.BaseDeducciones = negativos[i].Base.Value.Value;
                        reportdetail.ValorDeducciones = negativos[i].Valor.Value.Value;
                        reportdetail.CodigoDeducciones = negativos[i].ConceptoNOID.Value;
                        reportdetail.DescripcionDeducciones = negativos[i].ConceptoNODesc.Value;
                        totalDedu += negativos[i].Valor.Value.Value;

                        if (positivos[i] != null)
                        {
                            reportdetail = new DTO_ReportNoPreNominaDetail();
                            reportdetail.BaseDevengos = positivos[i].Base.Value.Value;
                            reportdetail.ValorDevengos = positivos[i].Valor.Value.Value;
                            reportdetail.CodigoDevengos = positivos[i].ConceptoNOID.Value;
                            reportdetail.DescripcionDevengos = positivos[i].ConceptoNODesc.Value;
                            netoPago += positivos[i].Valor.Value.Value;
                        }
                        dto_preNomina.Detail.Add(reportdetail);
                    }
                }
                dto_preNomina.Footer.NetoPagar = netoPago;
                dto_preNomina.Footer.TotalDeducido = totalDedu;

                #endregion

                return dto_preNomina;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportPreNomina");
                return null;
            }
        }

        // <summary>
        //Crea un dto de reporte Vacaciones
        //</summary>
        //<param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        //<returns></returns>(int numeroDoc, List<DTO_MasterBasic> lEmpleados, List<DTO_noLiquidacionPreliminar> _ldetalle)
        public DTO_SerializedObject Dto_Vacaciones_Report(int numeroDoc, DTO_noLiquidacionesDocu liquidacion, DTO_noEmpleado empleado, List<DTO_noLiquidacionPreliminar> _lDetalle, bool isApro)
        {
            try
            {
                #region Variables
                decimal topeDeducc = Convert.ToDecimal(this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TopeMaxDeducRteFte));
                DTO_ReportNoVacaciones dto_Vacaciones = new DTO_ReportNoVacaciones();
                DTO_ReportNoVacacionesDetail reportdetail = new DTO_ReportNoVacacionesDetail();
                decimal netoPago = 0;
                decimal totalDedu = 0;
                int documentID = 0;
                #endregion
                #region Asigan los valores al dto

                dto_Vacaciones.Header.CedulaEmpleado = empleado.ID.Value;
                dto_Vacaciones.Header.Empleado = empleado.IdName;
                dto_Vacaciones.Header.DiasPagados = liquidacion.Dias2.Value.HasValue? liquidacion.Dias2.Value.Value : 0;
                dto_Vacaciones.Header.DiasTomados = liquidacion.Dias1.Value.HasValue? liquidacion.Dias1.Value.Value : 0;
                dto_Vacaciones.Header.FechaIngreso = empleado.FechaIngreso.Value.HasValue? empleado.FechaIngreso.Value.Value.Date.ToShortDateString() :string.Empty;
                dto_Vacaciones.Header.FechaReIntegro = liquidacion.Fecha4.Value.HasValue? liquidacion.Fecha4.Value.Value.Date.ToShortDateString() : string.Empty;
                dto_Vacaciones.Header.Periodo = liquidacion.FechaIni1.Value.Value.Date.ToShortDateString() + "-" + liquidacion.FechaFin1.Value.Value.Date.ToShortDateString();
                dto_Vacaciones.Header.PeriodoDescanso = liquidacion.FechaIni2.Value.HasValue? liquidacion.FechaIni2.Value.Value.Date.ToShortDateString() + "-" + liquidacion.FechaFin2.Value.Value.Date.ToShortDateString() : string.Empty;
                dto_Vacaciones.Header.PeriodoPago = liquidacion.FechaIni3.Value.HasValue? liquidacion.FechaIni3.Value.Value.Date.ToShortDateString() + "-" + liquidacion.FechaFin3.Value.Value.Date.ToShortDateString() : string.Empty;
                dto_Vacaciones.Header.Resolucion = liquidacion.DatoAdd2.Value;
                if ((!empleado.MonedaExtInd.Value.Value))
                    dto_Vacaciones.Header.Salario = liquidacion.SueldoML.Value.HasValue? liquidacion.SueldoML.Value.Value : 0;
                else
                    dto_Vacaciones.Header.Salario = liquidacion.SueldoME.Value.HasValue? liquidacion.SueldoME.Value.Value : 0;

                dto_Vacaciones.isApro = false;

                List<DTO_noLiquidacionPreliminar> positivos = _lDetalle.Where(x => x.Valor.Value > 0).ToList();
                List<DTO_noLiquidacionPreliminar> negativos = _lDetalle.Where(x => x.Valor.Value < 0).ToList();
                if (positivos.Count >= negativos.Count)
                {
                    for (int i = 0; i < positivos.Count; i++)
                    {
                        reportdetail = new DTO_ReportNoVacacionesDetail();
                        reportdetail.BaseDevengos = positivos[i].Base.Value.Value;
                        reportdetail.ValorDevengos = positivos[i].Valor.Value.Value;
                        reportdetail.CodigoDevengos = positivos[i].ConceptoNOID.Value;
                        reportdetail.DescripcionDevengos = positivos[i].ConceptoNODesc.Value;
                        netoPago += positivos[i].Valor.Value.Value;
                        if (i < negativos.Count)
                        {
                            if (negativos[i] != null)
                            {
                                reportdetail.BaseDeducciones = negativos[i].Base.Value.Value;
                                reportdetail.ValorDeducciones = negativos[i].Valor.Value.Value;
                                reportdetail.CodigoDeducciones = negativos[i].ConceptoNOID.Value;
                                reportdetail.DescripcionDeducciones = negativos[i].ConceptoNODesc.Value;
                                totalDedu += negativos[i].Valor.Value.Value;
                            }
                        }
                        dto_Vacaciones.Detail.Add(reportdetail);
                    }
                }
                else
                {
                    for (int i = 0; i < negativos.Count; i++)
                    {
                        reportdetail.BaseDeducciones = negativos[i].Base.Value.Value;
                        reportdetail.ValorDeducciones = negativos[i].Valor.Value.Value;
                        reportdetail.CodigoDeducciones = negativos[i].ConceptoNOID.Value;
                        reportdetail.DescripcionDeducciones = negativos[i].ConceptoNODesc.Value;
                        totalDedu += negativos[i].Valor.Value.Value;
                        if (i < positivos.Count)
                        {
                            if (positivos[i] != null)
                            {
                                reportdetail = new DTO_ReportNoVacacionesDetail();
                                reportdetail.BaseDevengos = positivos[i].Base.Value.Value;
                                reportdetail.ValorDevengos = positivos[i].Valor.Value.Value;
                                reportdetail.CodigoDevengos = positivos[i].ConceptoNOID.Value;
                                reportdetail.DescripcionDevengos = positivos[i].ConceptoNODesc.Value;
                                netoPago += positivos[i].Valor.Value.Value;
                            }
                        }
                        dto_Vacaciones.Detail.Add(reportdetail);
                    }
                }
                dto_Vacaciones.Footer.NetoPagar = netoPago;
                dto_Vacaciones.Footer.TotalDeducido = totalDedu;

                #endregion

                return dto_Vacaciones;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportPreNomina");
                return null;
            }
        }

        /// <summary>
        /// Funcion para generar el reporte de nomina Detallado
        /// </summary>
        /// <param name="documentoID">Documento por el cual se genera la consulta</param>
        /// <param name="periodo">Periodo del modulo</param>
        /// <param name="isApro"></param>
        /// <param name="isApre"></param>
        /// <returns></returns>
        public DTO_noReportResumidoXEmpleadoTotal Report_No_Detalle(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                DTO_noReportResumidoXEmpleadoTotal dtosGeneral = new DTO_noReportResumidoXEmpleadoTotal();
                this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dtosGeneral.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_Detalle(documentoID, periodo, fechaini, fechaFin, isAll, isOrderByName, isPre, terceroid, operacionnoid, areafuncionalid, conceptonoid);
                if (dtosGeneral.Detalles.Count > 0)
                {
                    dtosGeneral.Detalles.FirstOrDefault().FechaIni = fechaini;
                    dtosGeneral.Detalles.FirstOrDefault().FechaFin = fechaFin;
                }
                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNominaDetail");
                return null;
            }
        }

        /// <summary>
        /// Obtiene el detalle de las liquidaciones
        /// </summary>
        /// <param name="documentoID">Documento de Nomina</param>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="orden">orden</param>
        /// <param name="fechaini">fecha inicial</param>
        /// <param name="fechaFin">fecha final</param>
        /// <param name="isAll">si son todos los documentos</param>
        /// <param name="isOrderByName">si se ordena por nombre</param>
        /// <param name="isPre">si son liquidaciones preliminares</param>
        /// <returns>Detalle Liquidacion</returns>
        public List<DTO_ReportNominaInfoEmpleado> Report_No_DetailLiquidaciones(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            #region Variables

            int j = 0;
            List<DTO_ReportNominaInfoEmpleado> headers = new List<DTO_ReportNominaInfoEmpleado>();
            List<DTO_ReportNominaInfoEmpleado> headersTotal = new List<DTO_ReportNominaInfoEmpleado>();
            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            #endregion

            headers = this._dal_ReportesNomina.DAL_ReportesNomina_DetalleNominaHeader(documentoID, periodo, fechaini, fechaFin, isAll, isPre, terceroid, operacionnoid, areafuncionalid, conceptonoid);
            headers = isOrderByName ? headers.OrderBy(x => x.EmpleadoDesc).ToList() : headers.OrderBy(x => x.EmpleadoDesc).ToList();

            foreach (DTO_ReportNominaInfoEmpleado item in headers)
            {
                DTO_coCentroCosto ctroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, item.CentroCosto, true, false);
                DTO_coProyecto proyectos = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, item.Proyecto, true, false);
                if (ctroCosto != null)
                    item.CentroCostDesc = ctroCosto.ID.Value;
                if (proyectos != null)
                    item.ProyectoDesc = proyectos.Descriptivo.Value;

                if (item.DiasDescanso != null)
                    item.DiasDescanso = 0;

                DTO_ReportNominaInfoEmpleado header = new DTO_ReportNominaInfoEmpleado();
                item.Detalles = new List<DTO_noReportDetalleEmpleadoConcepto>();
                item.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_DetailLiquidaciones(item.EmpleadoID, item.NumeroDoc, documentoID, periodo, fechaini, fechaFin, isAll, isPre);
                foreach (var iter in item.Detalles)
                {
                    if (iter.Tipo.Value == 1)
                    {
                        iter.ValorDevengos.Value = iter.Valor.Value.Value;
                        iter.ValorDeducciones.Value = 0;
                    }
                    else
                    {
                        iter.ValorDeducciones.Value = iter.Valor.Value.Value;
                        iter.ValorDevengos.Value = 0;
                    }

                    iter.ValorTotal.Value = iter.ValorDevengos.Value - iter.ValorDeducciones.Value;
                }
                decimal totalDeducciones = item.Detalles.Sum(x => x.ValorDeducciones.Value.Value);
                decimal totalDevengos = item.Detalles.Sum(x => x.ValorDevengos.Value.Value);
                item.TotalPagar = totalDevengos - totalDeducciones;
                item.FechaInicial = fechaini;
                item.FechaFinal = fechaFin;

                header = item;
                headersTotal.Add(header);
            }
            return headersTotal;
        }

        /// <summary>
        /// Funcion que arma el Dto_NominaDetail por Concepto
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        public List<DTO_ReportNominaInfoEmpleado_Totales> Report_No_XConcepto(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                List<DTO_ReportNominaInfoEmpleado_Totales> result = new List<DTO_ReportNominaInfoEmpleado_Totales>();
                decimal vlrTotal = 0;

                // Trae los datos
                this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_noReportDetalleNominaXConcepto> detalles = this._dal_ReportesNomina.DAL_ReportesNomina_XConcepto(documentoID, periodo, fechaini, fechaFin, isAll, isOrderByName, isPre, terceroid, operacionnoid, areafuncionalid, conceptonoid);
                vlrTotal = detalles.Sum(x => x.Valor.Value.Value);

                // Diferencia los conceptos
                List<string> distinct = (from c in detalles select c.ConceptoNODesc.Value).Distinct().ToList();
                foreach (string concepto in distinct)
                {
                    DTO_ReportNominaInfoEmpleado_Totales empleado = new DTO_ReportNominaInfoEmpleado_Totales();

                    empleado.FechaInicial = fechaini;
                    empleado.FechaFinal = fechaFin;
                    empleado.VlrTotal.Value = vlrTotal;
                    empleado.Detalles.AddRange(detalles.Where(x => x.ConceptoNODesc.Value == concepto));

                    result.Add(empleado);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNominaDetailXConcepto");
                return null;
            }
        }

        /// <summary>
        /// Funcion que arma el Dto_NominaDetail por Concepto
        /// </summary>
        /// <param name="documentoID">Documento por el cual se ConsultaEj: Vacaciones, Nomina, Prenómina</param>
        /// <param name="periodo">Periodod de Modulo</param>
        /// <param name="orden">Orden de como se van a mostrar las cosas</param>
        /// <param name="fechaini">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Fnal Del reporte</param>
        /// <param name="isApro">es para aprobar?</param>
        /// <returns>Lista de detales </returns>
        public DTO_FormulariosResumidoDevengosDedTotal Report_No_TotalXConcepto(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool orerByName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            try
            {
                #region Variables
                this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_FormulariosResumidoDevengosDedTotal totalReturn = new DTO_FormulariosResumidoDevengosDedTotal();
                totalReturn.Detalles = new List<DTO_noReportNominaResumidoXConcepto>();

                #endregion
                List<DTO_noReportNominaResumidoXConcepto> detalles = this._dal_ReportesNomina.DAL_ReportesNomina_TotalXConcepto(documentoID, periodo, fechaini, fechaFin, isAll, orerByName, isPre, terceroid, operacionnoid, areafuncionalid, conceptonoid);

                foreach (DTO_noReportNominaResumidoXConcepto item in detalles)
                {
                    if (item.Tipo.Value.Value == 2)
                    {
                        item.Deducciones.Value = item.Valor.Value.Value;
                        item.Devengos.Value = 0;
                    }
                    else
                    {
                        item.Devengos.Value = item.Valor.Value.Value;
                        item.Deducciones.Value = 0;
                    }

                }
                decimal totalDeducciones = detalles.Sum(x => x.Deducciones.Value.Value);
                decimal totalDevengos = detalles.Sum(x => x.Devengos.Value.Value);

                for (int i = 0; i < detalles.Count; i++)
                    detalles[i].VlrTotal.Value = totalDevengos - totalDeducciones;

                detalles.FirstOrDefault().FechaInicial = fechaini;
                detalles.FirstOrDefault().FechaFinal = fechaFin;
                totalReturn.Detalles.AddRange(detalles);
                return totalReturn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportTotalesXConcepto");
                return null;
            }
        }

        /// <summary>
        /// Funcion para cargar una lista de dtos correspondientes al detalle del reporte no_ReportVacacionesPagadas
        /// </summary>
        /// <param name="fechaIni">fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="empleadoFil">Filtro</param>
        /// <param name="orderBy">Es ordenado o no?</param>
        /// <returns></returns>
        public List<DTO_noReportVacionesPagadasTotal> Report_No_VacacionesPagadas(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String empleadoid)
        {
            #region Variables
            DTO_noReportVacionesPagadasTotal totales = new DTO_noReportVacionesPagadasTotal();
            List<DTO_noReportVacionesPagadasTotal> totalesReturn = new List<DTO_noReportVacionesPagadasTotal>();

            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            //Trae los datos
            totales.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_VacacionesPagadas(fechaIni, fechaFin, empleadoFil, orderBy, empleadoid);
            List<string> distinct = (from c in totales.Detalles select c.Descriptivo.Value).Distinct().ToList();

            foreach (string item in distinct)
            {
                DTO_noReportVacionesPagadasTotal totalesEmpleado = new DTO_noReportVacionesPagadasTotal();
                totalesEmpleado.Detalles = new List<DTO_noReportVacionesPagadas>();

                totalesEmpleado.Detalles = totales.Detalles.Where(x => x.Descriptivo.Value == item).ToList();
                for (int i = 0; i < totalesEmpleado.Detalles.Count; i++)
                    totalesEmpleado.Detalles[i].DiasTotal += (totalesEmpleado.Detalles[i].Dias1 + totalesEmpleado.Detalles[i].Dias2);

                totalesReturn.Add(totalesEmpleado);
            }

            return totalesReturn;
        }

        /// <summary>
        /// Funcion que carga el dtoPara el reporte de noReportPrestamo
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha inicial del reporte</param>
        /// <param name="orderByName">Se ordena por nombre?</param>
        /// <returns>Lista de Dto_ReportPrestamo</returns>
        public List<DTO_noReportPrestamo> Report_No_Prestamo(DateTime fechaIni, DateTime fechaFin, bool orderByName, String empleadoid)
        {
            #region Variables

            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            List<DTO_noReportPrestamo> detalles = this._dal_ReportesNomina.DAL_ReportesNomina_Prestamo(fechaIni, fechaFin, orderByName, empleadoid);
            return detalles;
        }

        /// <summary>
        /// Funcion que carga una lista de Dto_noAportesPensiontotales
        /// </summary>
        /// <param name="fechaIni">FechaInicial del Reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="filtro">Parametro q carga el filtro</param>
        /// <param name="orderByName">Ordenado por NombredeFondo?</param>
        /// <returns>Lista de Dto_noAportesPensiontotales</returns>
        public List<DTO_noAportesPensionTotales> Report_No_AportesPension(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, String terceroid, String nofondosaludid, String nocajaid)
        {
            #region Variables
            List<DTO_noAportesPensionTotales> totalesReturn = new List<DTO_noAportesPensionTotales>();
            DTO_noAportesPensionTotales total = new DTO_noAportesPensionTotales();
            total.Detalles = new List<DTO_noAportesPension>();
            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            //Trae los datos
            total.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_AportesPension(fechaIni, fechaFin, filtro, orderByName, terceroid, nofondosaludid, nocajaid);
            List<string> distinct = (from c in total.Detalles select c.FondoNOID.Value).Distinct().ToList();

            foreach (string item in distinct)
            {
                DTO_noAportesPensionTotales totalesFondoID = new DTO_noAportesPensionTotales();
                totalesFondoID.Detalles = new List<DTO_noAportesPension>();

                totalesFondoID.Detalles = total.Detalles.Where(x => x.FondoNOID.Value == item).ToList();
                totalesReturn.Add(totalesFondoID);
            }
            if (total.Detalles.Count != 0)
            {
                totalesReturn.FirstOrDefault().FechaIni = fechaIni;
                totalesReturn.FirstOrDefault().FechaFin = fechaFin;
            }

            return totalesReturn;
        }

        /// <summary>
        /// Funcion que carga una lista de Dto_noAportesPensiontotales
        /// </summary>
        /// <param name="fechaIni">FechaInicial del Reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="filtro">Parametro q carga el filtro</param>
        /// <param name="orderByName">Ordenado por NombredeFondo?</param>
        /// <returns>Lista de Dto_noAportesPensiontotales</returns>
        public List<DTO_noAportesSaludTotales> Report_No_AportesSalud(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, String terceroid, String nofondosaludid, String nocajaid)
        {
            #region Variables
            List<DTO_noAportesSaludTotales> totalesReturn = new List<DTO_noAportesSaludTotales>();
            DTO_noAportesSaludTotales total = new DTO_noAportesSaludTotales();
            total.Detalles = new List<DTO_noAportesSalud>();

            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            //Trae los datos
            total.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_AportesSalud(fechaIni, fechaFin, filtro, orderByName, terceroid, nofondosaludid, nocajaid);

            List<string> distinct = (from c in total.Detalles select c.FondoNOID.Value).Distinct().ToList();
            foreach (string item in distinct)
            {
                DTO_noAportesSaludTotales totalesFondoID = new DTO_noAportesSaludTotales();
                totalesFondoID.Detalles = new List<DTO_noAportesSalud>();

                totalesFondoID.Detalles = total.Detalles.Where(x => x.FondoNOID.Value == item).ToList();
                totalesReturn.Add(totalesFondoID);
            }
            if (total.Detalles.Count != 0)
            {
                totalesReturn.FirstOrDefault().FechaIni = fechaIni;
                totalesReturn.FirstOrDefault().FechaFin = fechaFin;
            }

            return totalesReturn;
        }

        /// <summary>
        /// Funcion que carga una lista de DTO_noAportesVoluntariosPensionTotales
        /// </summary>
        /// <param name="fechaIni">FechaInicial del Reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="filtro">Parametro q carga el filtro</param>
        /// <param name="orderByName">Ordenado por NombredeFondo?</param>
        /// <returns>Lista de Dto_noAportesPensiontotales</returns>
        public List<DTO_noAportesVoluntariosPensionTotales> Report_No_AporteVoluntarioPension(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, String terceroid, String nofondosaludid, String nocajaid)
        {
            #region Variables
            List<DTO_noAportesVoluntariosPensionTotales> empleado = new List<DTO_noAportesVoluntariosPensionTotales>();
            DTO_noAportesVoluntariosPensionTotales total = new DTO_noAportesVoluntariosPensionTotales();
            total.Detalles = new List<DTO_noAportesVoluntariosPension>();

            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            //Trae los datos
            total.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_AporteVoluntarioPension(fechaIni, fechaFin, filtro, orderByName, terceroid, nofondosaludid, nocajaid);

            List<string> distinct = (from c in total.Detalles select c.FondoNOID.Value).Distinct().ToList();
            foreach (string item in distinct)
            {
                DTO_noAportesVoluntariosPensionTotales totalesFondoID = new DTO_noAportesVoluntariosPensionTotales();
                totalesFondoID.Detalles = new List<DTO_noAportesVoluntariosPension>();

                totalesFondoID.Detalles = total.Detalles.Where(x => x.FondoNOID.Value == item).ToList();
                empleado.Add(totalesFondoID);
            }
            if (total.Detalles.Count != 0)
            {
                empleado.FirstOrDefault().FechaIni = fechaIni;
                empleado.FirstOrDefault().FechaFin = fechaFin;
            }

            return empleado;
        }

        /// <summary>
        /// Funcion que carga una lista de DTO_noAportesArp
        /// </summary>
        /// <param name="fechaIni">FechaInicial del Reporte</param>
        /// <param name="fechaFin">Fecha Final del Reporte</param>
        /// <param name="filtro">Parametro q carga el filtro</param>
        /// <param name="orderByName">Ordenado por NombredeFondo?</param>
        /// <returns>Lista de Dto_noAportesPensiontotales</returns>
        public List<DTO_noAportesARPTotales> Report_No_AportesARP(DateTime fechaIni, DateTime fechaFin, string filtro, bool orderByName, String terceroid, String nofondosaludid, String nocajaid)
        {
            try
            {
                #region Variables
                List<DTO_noAportesARPTotales> totalReturn = new List<DTO_noAportesARPTotales>();
                DTO_noAportesARPTotales total = new DTO_noAportesARPTotales();
                total.Detalles = new List<DTO_noAportesArp>();
                this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion
                total.Detalles = this._dal_ReportesNomina.DAL_ReportesNomina_AportesARP(fechaIni, fechaFin, filtro, orderByName, terceroid, nofondosaludid, nocajaid);

                totalReturn.Add(total);


                return totalReturn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNominaDetail");
                return null;
            }
        }
        
        #region Nuevos Reportes

        /// <summary>
        /// Reporte Boleta de Pago Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Liquidación</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="getAll">Todos los Empleados</param>
        /// <returns>DataSet con Resultados</returns>
        public DataSet Report_No_BoletaPago(DateTime periodo, string empleadoID, bool getAll)
        {
            this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_ReportesNomina.DAL_ReportesNomina_BoletaPago(periodo, empleadoID, getAll);
        }

        /// <summary>
        /// Consulta para traer las fechas del combo en el formulario
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <param name="_vacaciones"></param>
        /// <returns>Consulta para traer las fechas del combo en el formulario</returns>
        public List<DTO_ReportVacacionesDocumento> Report_No_GetVacacionesByEmpleado(string _empleadoID)
        {
            DTO_ReportVacacionesDocumento Doc = new DTO_ReportVacacionesDocumento();
            List<DTO_ReportVacacionesDocumento> listDoc = new List<DTO_ReportVacacionesDocumento>();

            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            listDoc = this._dal_noLiquidacionesDetalle.DAL_Report_No_GetVacacionesByEmpleado(_empleadoID);
            return listDoc;
        }

        /// <summary>
        /// Consulta de por empleado para traer las fechas y utilizarlas como filtro.
        /// </summary>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> Report_No_GetLiquidaContratoByEmpleado(string _empleadoID)
        {
            DTO_ReportVacacionesDocumento Doc = new DTO_ReportVacacionesDocumento();
            List<DTO_ReportVacacionesDocumento> listDoc = new List<DTO_ReportVacacionesDocumento>();

            this._dal_noLiquidacionesDetalle = (DAL_noLiquidacionesDetalle)this.GetInstance(typeof(DAL_noLiquidacionesDetalle), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            listDoc = this._dal_noLiquidacionesDetalle.DAL_Report_No_GetLiquidaContratoByEmpleado(_empleadoID);
            return listDoc;
        }

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="operacionNoID">Operacion Nomina</param>
        /// <param name="conceptoNoID">Concepto Nomina</param>
        /// <param name="areaFuncID">Area Funcional</param>
        /// <param name="fondoID">Fondo Nom</param>
        /// <param name="cajaID">Caja Nomina</param>
        /// <param name="otroFilter">Filtro adicional</param>
        /// <param name="agrup">Agrupar u ordenar</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_No_NominaToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                         string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                DataTable result;
                if (documentoID == AppReports.noDetalleNomina && tipoReporte == 1)
                {
                    DataTableOperations tableOp = new DataTableOperations();
                    List<DTO_ReportNominaInfoEmpleado> list = this.Report_No_DetailLiquidaciones((int)romp, fechaIni.Value, tipoReporte.ToString(), fechaIni.Value, fechaFin.Value, Convert.ToBoolean(cajaID), Convert.ToBoolean(fondoID), Convert.ToBoolean(otroFilter), terceroID, operacionNoID, areaFuncID, conceptoNoID);
                    result = tableOp.Convert_GenericListToDataTable(typeof(DTO_ReportNominaInfoEmpleado), list);
                    result.Columns.Remove("NumeroDoc");
                    result.Columns.Remove("Detalles");
                    result.Columns.Remove("Brigada");
                    result.Columns.Remove("Localidad");
                    result.Columns.Remove("LocaclidadDesc");
                    result.Columns.Remove("CentroCosto");
                    result.Columns.Remove("Proyecto");
                }
                else
                {
                    this._dal_ReportesNomina = (DAL_ReportesNomina)this.GetInstance(typeof(DAL_ReportesNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    result = this._dal_ReportesNomina.DAL_Reportes_No_NominaToExcel(documentoID, tipoReporte, fechaIni, fechaFin, empleadoID, operacionNoID, conceptoNoID, areaFuncID, fondoID,
                                                                                     cajaID, terceroID, otroFilter, agrup, romp);
                }


                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_No_NominaToExcel");
                throw ex;
            }
        }

        /// <summary>
        /// Genera un archivo de las planillas de los empleados
        /// </summary>
        /// <param name="rutaArchivo">Ruta de archivo a guardar</param>
        /// <param name="planillas">Planillas a migrar</param>
        /// <returns>respuesta del archivo</returns>
        public string Reportes_No_GenerarArchivoPlanilla(string rutaArchivo, List<DTO_noPlanillaAportesDeta> planillas)
        {
            string result = "OK";
            try
            {               
                //string str = this.GetCvsName(rutaArchivo, out fileName);
                string registros = string.Empty;

                if (planillas.Count > 0)
                {
                    //1
                    int count = 1;
                    string terceroEmp = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    string terceroArp = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_TerceroARP);
                    string codigoARP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CodigoARPEmpresa);
                    string periodoNomStr = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
                    DateTime periodoNom = !string.IsNullOrEmpty(periodoNomStr) ? Convert.ToDateTime(periodoNomStr) : DateTime.Today;
                    DTO_coTercero terARP = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroArp, true, false);

                    #region Concatena campos
                    registros += "01" + StringExtensions.CustomizeString(typeof(int), count, 5) +         //NroReg                     
                        StringExtensions.CustomizeString(typeof(string), this.Empresa.Descriptivo.Value, 200) +// Nombre aportante
                        StringExtensions.CustomizeString(typeof(string), "NI", 2) +                       //Nit aportante
                        StringExtensions.CustomizeString(typeof(string), terceroEmp, 16) +                //Ind de Correccion
                        StringExtensions.CustomizeString(typeof(bool), false, 1) +                        //cVerifica
                        StringExtensions.CustomizeString(typeof(bool), false, 1) +                        //Ind de Correccion	
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                            //Planilla corregida
                        StringExtensions.CustomizeString(typeof(string), "", 10) +                        //Ind de Correccion
                        StringExtensions.CustomizeString(typeof(string), "U", 1) +                         //Inf  C(Consolidado), U(Unico),  S(Sucursal)		
                        StringExtensions.CustomizeString(typeof(string), "", 10) +                        //Cod. Sucursal
                        StringExtensions.CustomizeString(typeof(string), "", 40) +                        //Nombre sucursal
                        StringExtensions.CustomizeString(typeof(string), codigoARP, 6) +                 //Cod de arp 
                        StringExtensions.CustomizeString(typeof(string), periodoNom.ToString("yyyy-MM"), 7) +//Periodo
                        StringExtensions.CustomizeString(typeof(string), periodoNom.ToString("yyyy-MM"), 7) +//Periodo salud	
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //Numero radicacion	
                        StringExtensions.CustomizeString(typeof(string), "", 10) +                         //Periodo salud
                        StringExtensions.CustomizeString(typeof(string), "", 0) +                          //Periodo asignado por   
                        StringExtensions.CustomizeString(typeof(int), planillas.Count(), 5) +              //Numero empleados  	           
                        StringExtensions.CustomizeString(typeof(int), planillas.Sum(x => x.Sueldo.Value), 12) + //Valor total de la nomina	  
                        StringExtensions.CustomizeString(typeof(int), "1", 1) +                            //tipo de Aportante	  
                        StringExtensions.CustomizeString(typeof(int), "00", 2) +                           //Codigo Operador	  
                        "\r\n";
                    count++;
                    #endregion 

                    //2
                    count = 1;
                    planillas.AddRange(planillas);
                    foreach (var plan in planillas)
                    {
                        DTO_noEmpleado emp = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.noEmpleado,plan.EmpleadoID.Value,true,false);
                        DTO_coTercero ter = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.coTercero,emp.TerceroID.Value,true,false);

                        #region Asigna Campos Especiales
                        string tipoDoc = string.Empty;
                        string cotizante = string.Empty;
                        if (plan.TipoDocNomina.Value == 1)
                            tipoDoc = "CC";
                        else if (plan.TipoDocNomina.Value == 2)
                            tipoDoc = "CE";
                        else if (plan.TipoDocNomina.Value == 3)
                            tipoDoc = "TI";
                        else if (plan.TipoDocNomina.Value == 4)
                            tipoDoc = "PA";

                        if (plan.TipoCotizante.Value == 1)
                            cotizante = "01";
                        else if (plan.TipoCotizante.Value == 2)
                            cotizante = "12";
                        else if (plan.TipoCotizante.Value == 3)
                            cotizante = "19";
                        if (plan.SubTipo.Value == 1)
                            cotizante += "00";
                        if (plan.SubTipo.Value == 2)
                            cotizante += "04"; 
                        #endregion

                        #region Concatena campos
                        registros += "02" + StringExtensions.CustomizeString(typeof(int), count, 5) +          // NroReg  -5
                         StringExtensions.CustomizeString(typeof(string), tipoDoc, 2) +                        //TipoDoc -2
                         StringExtensions.CustomizeString(typeof(string), plan.EmpleadoID.Value, 16) +         //CEdula - 16
                         StringExtensions.CustomizeString(typeof(string), cotizante, 4) +                      //TipoCotiza - SubCotiza - 4
                         StringExtensions.CustomizeString(typeof(bool), plan.ExtranjeroInd.Value, 1) +         //Trab. Extranjero - 1
                         StringExtensions.CustomizeString(typeof(bool), plan.ExteriorInd.Value, 1) +           //exterior - 1
                         StringExtensions.CustomizeString(typeof(string), plan.LugarGeograficoID.Value, 5) +   //Departamento y Ciudad - 5
                         StringExtensions.CustomizeString(typeof(string), ter.ApellidoPri.Value, 20) +         //Apellido1 - 20
                         StringExtensions.CustomizeString(typeof(string), ter.ApellidoSdo.Value, 30) +         //Apellido2 - 30
                         StringExtensions.CustomizeString(typeof(string), ter.NombrePri.Value, 20) +           //Nombre1 - 20
                         StringExtensions.CustomizeString(typeof(string), ter.NombreSdo.Value, 30) +           //Nombre2 - 30
                         StringExtensions.CustomizeString(typeof(bool), plan.INGInd.Value, 1) +                //ING- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.RETInd.Value, 1) +                //RET- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.TDEInd.Value, 1) +                //TDE- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.TAEInd.Value, 1) +                //TAE- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.TDPInd.Value, 1) +                //TDP- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.TAPInd.Value, 1) +                //TAP- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.VSPInd.Value, 1) +                //VSP- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.VTEInd.Value, 1) +                //VTE- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.VSTInd.Value, 1) +                //ING- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.SLNInd.Value, 1) +                //ING- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.IGEInd.Value, 1) +                //ING- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.LMAInd.Value, 1) +                //VST- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.VACInd.Value, 1) +                //VAC- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.AVPInd.Value, 1) +                //AVP- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.VCTInd.Value, 1) +                //VCT- 1
                         StringExtensions.CustomizeString(typeof(bool), plan.IRPInd.Value, 1) +                //IRP- 1
                         StringExtensions.CustomizeString(typeof(string), plan.FondoPension.Value, 6) +        //Cod de pension Actual   - 6
                         StringExtensions.CustomizeString(typeof(string), plan.FondoPensionTR.Value, 6) +    //Cod de pension traslado  - 6 
                         StringExtensions.CustomizeString(typeof(string), plan.FondoSalud.Value, 6) +        //Cod de salud Actual  - 6
                         StringExtensions.CustomizeString(typeof(string), plan.FondoSaludTR.Value, 6) +      //Cod de salud traslado   - 6
                         StringExtensions.CustomizeString(typeof(string), plan.CajaNOID.Value, 6) +          //Cod de CCF Actual -  6
                         StringExtensions.CustomizeString(typeof(int), plan.DiasCotizadosPEN.Value, 2) +     //Dias cotizados pension	-  2
                         StringExtensions.CustomizeString(typeof(int), plan.DiasCotizadosSLD.Value, 2) +     //Dias cotizados salud -  2
                         StringExtensions.CustomizeString(typeof(int), plan.DiasCotizadosARP.Value, 2) +     //Dias cotizados arp	-  2
                         StringExtensions.CustomizeString(typeof(int), plan.DiasCotizadosCCF.Value, 2) +     //Dias cotizados ccf	-  2
                         StringExtensions.CustomizeString(typeof(int), plan.Sueldo.Value, 9) +               //Salario Basico	-  9
                         StringExtensions.CustomizeString(typeof(bool), plan.SalIntegralInd.Value, 1) +      //Salario Integral-  1
                         StringExtensions.CustomizeString(typeof(int), plan.IngresoBasePEN.Value, 9) +       //Base Pension	-  9
                         StringExtensions.CustomizeString(typeof(int), plan.IngresoBaseSLD.Value, 9) +       //ase Salud 	-  9
                         StringExtensions.CustomizeString(typeof(int), plan.IngresoBaseARP.Value, 9) +       //Base Arp -  9
                         StringExtensions.CustomizeString(typeof(int), plan.IngresoBaseCCF.Value, 9) +       //Base CCF -  9

                         StringExtensions.CustomizeString(typeof(decimal), (plan.TarifaPEN.Value / 100), 7) +        //  Tarifa pension - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), (plan.VlrTrabajadorPEN.Value+plan.VlrEmpresaPEN.Value), 9) +     //  Aporte pension - 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrTrabajadorVOL.Value, 9) +     //  Aportes voluntarios Empleado - 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrEmpresaVOL.Value, 9) +        //  Aportes voluntarios Empresa- 9
                         StringExtensions.CustomizeString(typeof(int), (plan.VlrTrabajadorPEN.Value + plan.VlrEmpresaPEN.Value + plan.VlrTrabajadorVOL.Value + plan.VlrEmpresaVOL.Value), 9) +// Total Aportes- 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrSolidaridad.Value, 9) +       //  Ap. Solidaridad- 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrSubsistencia.Value, 9) +      //  Ap. Subsistencia- 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrNoRetenido.Value, 9) +        //  Ap. No Retefuente - 9

                         StringExtensions.CustomizeString(typeof(decimal), (plan.TarifaSLD.Value / 100), 7) +  //  Tarifa salud - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), (plan.VlrTrabajadorSLD.Value+plan.VlrEmpresaSLD.Value), 9) +     //  Aporte salud - 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrUPC.Value, 9) +               //  Valor UPC Adicional	 - 9
                         StringExtensions.CustomizeString(typeof(string), plan.AutorizacionEnf.Value, 15) +    //  Autorizacion enfermedad	- 15
                         StringExtensions.CustomizeString(typeof(int), plan.VlrIncapacidad.Value, 9) +       //  Valor incapacidad	 - 9
                         StringExtensions.CustomizeString(typeof(string), plan.AutorizacioMat.Value, 15) +   //  Autorizacion Maternidad	 - 15
                         StringExtensions.CustomizeString(typeof(int), plan.VlrMaternidad.Value, 9) +        //  Valor maternidad	 - 9

                         StringExtensions.CustomizeString(typeof(decimal), (plan.TarifaARP.Value / 100), 9) +  //  Tarifa salud - 9(7 decimales)
                         StringExtensions.CustomizeString(typeof(int), 0, 9) +                                 //  Centro Trabajo	 - 9
                         StringExtensions.CustomizeString(typeof(int), plan.VlrARP.Value, 9) +               //  Aporte ARP - 9

                         StringExtensions.CustomizeString(typeof(decimal), (plan.TarifaCCF.Value / 100), 7) +  //  Tarifa CCF - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), plan.VlrCCF.Value, 9) +               //  Aporte CCF - 9
                         StringExtensions.CustomizeString(typeof(decimal), (plan.TarifaSEN.Value / 100), 7) +  //  Tarifa SENA - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), plan.VlrSEN.Value, 9) +               //  Aporte SENA - 9
                         StringExtensions.CustomizeString(typeof(decimal), (plan.TarifaIBF.Value / 100), 7) +  //  Tarifa ICBF - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), plan.VlrICBF.Value, 9) +              //  Aporte ICBF - 9
                         StringExtensions.CustomizeString(typeof(decimal), 0 / 100, 7) +                         //  Tarifa ESAP - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), 0, 9) +                                 //  Aporte ESAP - 9
                         StringExtensions.CustomizeString(typeof(decimal), 0 / 100, 7) +                         //  Tarifa Min Edu - 7(5 decimales)
                         StringExtensions.CustomizeString(typeof(int), 0, 9) +                                 //  Aporte Min Edu - 9
                         "\r\n";
                        count++; 
                        #endregion
                   }

                    //3
                    count = 1;
                    List<string> distinctFondo = (from c in planillas select c.FondoSalud.Value).Distinct().ToList();
                    distinctFondo = distinctFondo.OrderBy(x=>x).ToList();
                    foreach (var fon in distinctFondo)
                    {
                        DTO_noFondo fondoSal = (DTO_noFondo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noFondo, fon, true, false);
                        DTO_coTercero ter = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, fondoSal.TerceroID.Value, true, false);

                        #region Totales
                        decimal? vlrSaludTot = planillas.FindAll(x => x.FondoSalud.Value == fon).Sum(y => (y.VlrTrabajadorSLD.Value+y.VlrEmpresaSLD.Value));
                        decimal? vlrUPCTot = planillas.FindAll(x => x.FondoSalud.Value == fon).Sum(y => y.VlrUPC.Value);
                        decimal? vlrIncapTot = planillas.FindAll(x => x.FondoSalud.Value == fon).Sum(y => y.VlrIncapacidad.Value);
                        decimal? vlrMaternTot = planillas.FindAll(x => x.FondoSalud.Value == fon).Sum(y => y.VlrMaternidad.Value);
                        decimal? totalafil = planillas.FindAll(x => x.FondoSalud.Value == fon).Count;
                        
                        #endregion

                        #region Concatena campos
                        registros += "04" + StringExtensions.CustomizeString(typeof(int), count, 5) +       // NroReg                     
                         StringExtensions.CustomizeString(typeof(string), fon, 6) +                         //Cod de salud Actual 
                         StringExtensions.CustomizeString(typeof(int), fondoSal.TerceroID.Value, 16) +      //Nit fondo salud
                         StringExtensions.CustomizeString(typeof(int), ter.DigitoVerif.Value, 1) +          //Dig Verificacion
                         StringExtensions.CustomizeString(typeof(int), vlrSaludTot, 10) +                   //Total cotizaciones
                         StringExtensions.CustomizeString(typeof(int), vlrUPCTot, 10) +                     //Total UPC adicional
                         StringExtensions.CustomizeString(typeof(int), planillas.FindAll(x => x.FondoSalud.Value == fon && !string.IsNullOrEmpty(x.AutorizacionEnf.Value)).Count, 15) +       //# aut. Pago Incapac.
                         StringExtensions.CustomizeString(typeof(int), vlrIncapTot, 10) +                   //# desc. maternidad		
                         StringExtensions.CustomizeString(typeof(int), planillas.FindAll(x => x.FondoSalud.Value == fon && !string.IsNullOrEmpty(x.AutorizacioMat.Value)).Count, 15) +       //# aut. Pago Incapac.
                         StringExtensions.CustomizeString(typeof(int), vlrMaternTot, 10) +                  //# desc. maternidad	
                         StringExtensions.CustomizeString(typeof(int), (vlrSaludTot-vlrIncapTot-vlrMaternTot), 10) + //# desc. maternidad
                         StringExtensions.CustomizeString(typeof(int), 0, 4) +                              //  Dias Mora
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             // MoraCot
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //  MoraUpc 
                         StringExtensions.CustomizeString(typeof(int), (vlrSaludTot-vlrIncapTot-vlrMaternTot+0), 10) + // SubTotCot
                         StringExtensions.CustomizeString(typeof(int), (vlrUPCTot+0), 10) +                 // SubTotUpc
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             // NumForm
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             // SdoAntCot
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             // SdoAntUpc
                         StringExtensions.CustomizeString(typeof(int), (vlrSaludTot-vlrIncapTot-vlrMaternTot+0-0), 10) +      // TotalCot
                         StringExtensions.CustomizeString(typeof(int), (vlrUPCTot+0-0), 10) +               // TotalUpc
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             // Total
                         StringExtensions.CustomizeString(typeof(int), 0, 10) +                             // VlrFondo
                         StringExtensions.CustomizeString(typeof(int), totalafil, 6) +                      // VlrFondo                         
                         "\r\n";
                        count++;
                        #endregion
                    }

                    //4
                    count = 1;

                    #region Totales
                    decimal? vlrARPTot = planillas.Sum(y => (y.VlrARP.Value));
                    decimal? totalafilArp = planillas.FindAll(x => x.VlrARP.Value != 0).Count;
                    #endregion

                    #region Concatena campos
                    registros += "05" + StringExtensions.CustomizeString(typeof(int), count, 5) +          //NroReg                     
                        StringExtensions.CustomizeString(typeof(string), codigoARP, 6) +                  //Cod de arp 
                        StringExtensions.CustomizeString(typeof(int), terceroArp, 16) +                    //Nit arp
                        StringExtensions.CustomizeString(typeof(int), terARP.DigitoVerif.Value, 1) +       //Dig Verificacion
                        StringExtensions.CustomizeString(typeof(int), vlrARPTot, 10) +                     //Total cotizaciones
                        StringExtensions.CustomizeString(typeof(string), "", 15) +                         //# aut. Pago Incapac.
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //VlrIncaPag, 		
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //VlrPagSub,
                        StringExtensions.CustomizeString(typeof(int), vlrARPTot, 10) +                     //VlrNetoAp, 	
                        StringExtensions.CustomizeString(typeof(int), 0, 4) +                              //Dias Mora
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //MoraCot
                        StringExtensions.CustomizeString(typeof(int), vlrARPTot, 10) +                     //SubTotCot
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //NumForm
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //VlrSdoAnt
                        StringExtensions.CustomizeString(typeof(int), vlrARPTot, 10) +                     //Total
                        StringExtensions.CustomizeString(typeof(int), 0, 10) +                             //VlrFondo
                        StringExtensions.CustomizeString(typeof(int), totalafilArp, 6) +                   //VlrFondo                         
                        "\r\n";
                    count++;
                    #endregion                    

                    System.IO.File.WriteAllText(rutaArchivo, registros, Encoding.UTF8);
                }
                else
                    result = string.Empty;

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_No_GenerarArchivoPlanilla");
                result = "NOK";
                throw ex;
            }
        }


        #endregion

        #endregion

        #region Reversiones

        /// <summary>
        /// Proceso para revertir la liquidación de la Nomina
        /// </summary>
        /// <param name="documentoID">Documento de Nomina</param>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <param name="lEmpleados">lista de Empleados</param>
        /// <param name="batchProgress">Barra de progreso</param>
        [Transaction]
        public void RevertirLiqNomina(int documentoID, DateTime periodo, List<DTO_noEmpleado> lEmpleados, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            decimal porcTotal = 0;
            decimal porcParte = (decimal)100 / lEmpleados.Count;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            List<int> numDocs = new List<int>();
            noLiquidacionBase objLiquidacion = new noLiquidacionNomina(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            foreach (var emp in lEmpleados)
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                objLiquidacion.Empleado = emp;
                objLiquidacion.Periodo = periodo;
                objLiquidacion.DocumentoID = documentoID;
                objLiquidacion.RevertirLiquidacion();
            }

            batchProgress[tupProgress] = 100;
        }

        #endregion

    }
}



