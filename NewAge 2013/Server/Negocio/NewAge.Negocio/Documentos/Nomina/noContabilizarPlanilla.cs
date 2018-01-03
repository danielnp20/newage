using NewAge.ADO;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Negocio.PostSharpAspects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NewAge.Negocio
{
    public class noContabilizarPlanilla :  noContabilizacionBase
    {
        public DTO_glDocumentoControl glCtrlAportes = null;
        DTO_Comprobante comprobante = new DTO_Comprobante();

        public noContabilizarPlanilla(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        [Transaction]
        public override List<DTO_TxResult> Contabilizar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noPlanillaAportesDeta = (DAL_noPlanillaAportesDeta)this.GetInstance(typeof(DAL_noPlanillaAportesDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            string conceptoSena = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSalud);
            string conceptoICBF = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSalud);
            string conceptoCajaComp = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSalud);
            string conceptoVacaciones = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoVacacionesTiempo);
            string conceptoFondoSoli = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoFondoSolidaridad);
            string conceptoPrima = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoPrimaServicios);
            string conceptoCesantias = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCesantias);
            string conceptoIntCesantias = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoInteresCesantias);
            string cuentaParaFiscales = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CxPParaFiscales);
            string cuentaARP = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CuentaARP);


            DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);
            this.Comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docContableNomina.ComprobanteID.Value, true, false);
            DTO_coPlanCuenta ctaParaFiscales = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuentaParaFiscales, true, false);


            #region Crea GlDocumentoControl 887 - NominaPlanillaAportes

            glCtrlAportes = new DTO_glDocumentoControl();
            glCtrlAportes.DocumentoID.Value = AppDocuments.PlanillaAportesContabilidad;
            glCtrlAportes.Fecha.Value = DateTime.Now;
            glCtrlAportes.FechaDoc.Value = this.FechaDoc;
            glCtrlAportes.PeriodoDoc.Value = this.Periodo;
            glCtrlAportes.PrefijoID.Value = this.GetPrefijoByDocumento(this.DocumentoId);
            glCtrlAportes.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
            glCtrlAportes.TasaCambioCONT.Value = tc;
            glCtrlAportes.TasaCambioDOCU.Value = tc;
            glCtrlAportes.TerceroID.Value = terceroPorDefecto;
            glCtrlAportes.DocumentoTercero.Value = string.Empty;
            glCtrlAportes.CuentaID.Value = string.Empty;
            glCtrlAportes.MonedaID.Value = mdaLoc;
            glCtrlAportes.ProyectoID.Value = proyectoXDef;
            glCtrlAportes.CentroCostoID.Value = cCosto;
            glCtrlAportes.LugarGeograficoID.Value = lugarGeografico;
            glCtrlAportes.LineaPresupuestoID.Value = lineaPres;
            glCtrlAportes.Observacion.Value = "CONTABILIZAR PLANILLA";
            glCtrlAportes.Descripcion.Value = "CONTABILIZAR PLANILLA";
            glCtrlAportes.Estado.Value = (Byte)EstadoDocControl.Aprobado;
            glCtrlAportes.DocumentoNro.Value = 0;
            glCtrlAportes.PeriodoUltMov.Value = this.Periodo;
            glCtrlAportes.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
            glCtrlAportes.seUsuarioID.Value = this.UserId;


            DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(this.DocumentoId, glCtrlAportes, true);
            if (resultGLDC.Message != ResultValue.OK.ToString())
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = "NOK";
                result.Details.Add(resultGLDC);
                this.Results.Add(result);
                return this.Results;
            }

            int numDocNominaAportes = Convert.ToInt32(resultGLDC.Key);
            glCtrlAportes.NumeroDoc.Value = numDocNominaAportes;

            #endregion
            #region Agregar Header Comprobante

            TipoMoneda_LocExt tipoM = glCtrlAportes.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
            DTO_ComprobanteHeader headerAportes = new DTO_ComprobanteHeader();
            headerAportes.ComprobanteID.Value = this.Comp.ID.Value;
            headerAportes.ComprobanteNro.Value = 0;
            headerAportes.EmpresaID.Value = glCtrlAportes.EmpresaID.Value;
            headerAportes.Fecha.Value = glCtrlAportes.FechaDoc.Value;
            headerAportes.MdaOrigen.Value = (Byte)tipoM;
            headerAportes.MdaTransacc.Value = glCtrlAportes.MonedaID.Value;
            headerAportes.NumeroDoc.Value = Convert.ToInt32(numDocNominaAportes);
            headerAportes.PeriodoID.Value = glCtrlAportes.PeriodoDoc.Value;
            headerAportes.TasaCambioBase.Value = glCtrlAportes.TasaCambioCONT.Value;
            headerAportes.TasaCambioOtr.Value = glCtrlAportes.TasaCambioDOCU.Value;
            comprobante.Header = headerAportes;

            #endregion
            #region Agregar Footer Comprobante Planilla Aportes
            List<DTO_ComprobanteFooter> lFooterAportes = new List<DTO_ComprobanteFooter>();
            DTO_ComprobanteFooter footerAportes = null;

            List<DTO_NominaPlanillaContabilizacion> liqPlanilla = this._dal_noPlanillaAportesDeta.DAL_noPlanillaAportesDeta_GetValoreXTercero(false).Where(x => x.Valor.Value.Value != 0).ToList();
            // liqPlanilla = liqPlanilla.FindAll(x => x.TerceroID.Value == "860039988");
            foreach (DTO_NominaPlanillaContabilizacion item in liqPlanilla)
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

                DTO_noEmpleado emplProv = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, item.EmpleadoID.Value, true, false);

                switch (item.Liquidacion.Value.Value)
                {

                    case (byte)TerceroPlanilla.Caja:
                        {
                            DTO_glBienServicioClase claseBySCAJA = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, claseBYSCAJA, true, false);
                            DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, emplProv.ProyectoID.Value, true, false);
                            if (claseBySCAJA == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "Clase Servicio" + "&&" + claseBYSCAJA;
                                this.Results.Add(result);
                                return this.Results;
                            }
                            operacion = proyecto.OperacionID.Value;
                            if (string.IsNullOrEmpty(operacion))
                            {
                                DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, emplProv.CentroCostoID.Value, true, false);
                                operacion = centroCosto.OperacionID.Value;
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(claseBySCAJA.LineaPresupuestoID.Value))
                                linePresupuestal = lineaPres;
                            else
                                linePresupuestal = claseBySCAJA.LineaPresupuestoID.Value;

                            if (string.IsNullOrEmpty(claseBySCAJA.ConceptoCargoID.Value))
                                conceptoCargoID = concCargoDef;
                            else
                                conceptoCargoID = claseBySCAJA.ConceptoCargoID.Value;

                            DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(claseBySCAJA.ConceptoCargoID.Value, operacion, claseBySCAJA.LineaPresupuestoID.Value, 0);
                            if (cuenta.GetType() == typeof(List<DTO_TxResult>))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                                break;
                            }
                            coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuenta.CuentaID.Value, true, false);
                            item.CuentaID.Value = coPlanCta.ID.Value;
                            item.CuentaCtpID.Value = ctaParaFiscales.ID.Value;
                            break;
                        }
                    case (byte)TerceroPlanilla.SENA:
                        {
                            DTO_glBienServicioClase claseBySSENA = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, claseBYSSENA, true, false);
                            DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, emplProv.ProyectoID.Value, true, false);
                            if (claseBySSENA == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "Clase Servicio" + "&&" + claseBYSSENA;
                                this.Results.Add(result);
                                return this.Results;
                            }
                            operacion = proyecto.OperacionID.Value;
                            if (string.IsNullOrEmpty(operacion))
                            {
                                DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, emplProv.CentroCostoID.Value, true, false);
                                operacion = centroCosto.OperacionID.Value;
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(claseBySSENA.LineaPresupuestoID.Value))
                                linePresupuestal = lineaPres;
                            else
                                linePresupuestal = claseBySSENA.LineaPresupuestoID.Value;

                            if (string.IsNullOrEmpty(claseBySSENA.ConceptoCargoID.Value))
                                conceptoCargoID = concCargoDef;
                            else
                                conceptoCargoID = claseBySSENA.ConceptoCargoID.Value;

                            DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(claseBySSENA.ConceptoCargoID.Value, operacion, claseBySSENA.LineaPresupuestoID.Value, 0);
                            if (cuenta.GetType() == typeof(List<DTO_TxResult>))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                                break;
                            }
                            coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuenta.CuentaID.Value, true, false);
                            item.CuentaID.Value = coPlanCta.ID.Value;
                            item.CuentaCtpID.Value = ctaParaFiscales.ID.Value;
                            break;
                        }
                    case (byte)TerceroPlanilla.ICBF:
                        {
                            DTO_glBienServicioClase claseBySICBF = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, claseBYSICBF, true, false);
                            DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, emplProv.ProyectoID.Value, true, false);
                            if (claseBySICBF == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "Clase Servicio" + "&&" + claseBYSICBF;
                                this.Results.Add(result);
                                return this.Results;
                            }
                            operacion = proyecto.OperacionID.Value;
                            if (string.IsNullOrEmpty(operacion))
                            {
                                DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, emplProv.CentroCostoID.Value, true, false);
                                operacion = centroCosto.OperacionID.Value;
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(claseBySICBF.LineaPresupuestoID.Value))
                                linePresupuestal = lineaPres;
                            else
                                linePresupuestal = claseBySICBF.LineaPresupuestoID.Value;

                            if (string.IsNullOrEmpty(claseBySICBF.ConceptoCargoID.Value))
                                conceptoCargoID = concCargoDef;
                            else
                                conceptoCargoID = claseBySICBF.ConceptoCargoID.Value;

                            DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(claseBySICBF.ConceptoCargoID.Value, operacion, claseBySICBF.LineaPresupuestoID.Value, 0);
                            if (cuenta.GetType() == typeof(List<DTO_TxResult>))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                                break;
                            }
                            coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuenta.CuentaID.Value, true, false);
                            item.CuentaID.Value = coPlanCta.ID.Value;
                            item.CuentaCtpID.Value = ctaParaFiscales.ID.Value;
                            break;
                        }
                    case (byte)TerceroPlanilla.ARP:
                        {
                            DTO_glBienServicioClase claseBySARP = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, claseBYSARP, true, false);
                            DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, emplProv.ProyectoID.Value, true, false);
                            if (claseBySARP == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "Clase Servicio" + "&&" + claseBYSARP;
                                this.Results.Add(result);
                                return this.Results;
                            }
                            operacion = proyecto.OperacionID.Value;
                            if (string.IsNullOrEmpty(operacion))
                            {
                                DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, emplProv.CentroCostoID.Value, true, false);
                                operacion = centroCosto.OperacionID.Value;
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(claseBySARP.LineaPresupuestoID.Value))
                                linePresupuestal = lineaPres;
                            else
                                linePresupuestal = claseBySARP.LineaPresupuestoID.Value;

                            if (string.IsNullOrEmpty(claseBySARP.ConceptoCargoID.Value))
                                conceptoCargoID = concCargoDef;
                            else
                                conceptoCargoID = claseBySARP.ConceptoCargoID.Value;

                            DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(claseBySARP.ConceptoCargoID.Value, operacion, claseBySARP.LineaPresupuestoID.Value, 0);
                            if (cuenta.GetType() == typeof(List<DTO_TxResult>))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                                break;
                            }
                            coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuenta.CuentaID.Value, true, false);
                            item.CuentaID.Value = coPlanCta.ID.Value;
                            item.CuentaCtpID.Value = cuentaARP;
                            break;

                        }
                    default:
                        {
                            if (item.ConceptoNOID.Value != conceptoFondoSoli)
                            {
                                DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, emplProv.ProyectoID.Value, true, false);
                                operacion = proyecto.OperacionID.Value;
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, emplProv.CentroCostoID.Value, true, false);
                                    operacion = centroCosto.OperacionID.Value;
                                    if (string.IsNullOrEmpty(operacion))
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                        break;
                                    }
                                }

                                if (string.IsNullOrEmpty(item.LineaPresupuestoID.Value))
                                    linePresupuestal = lineaPres;
                                else
                                    linePresupuestal = item.LineaPresupuestoID.Value;

                                if (string.IsNullOrEmpty(item.ConceptoCargoID.Value))
                                    conceptoCargoID = concCargoDef;
                                else
                                    conceptoCargoID = item.ConceptoCargoID.Value;

                                DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(conceptoCargoID, operacion, linePresupuestal, 0);
                                if (cuenta.GetType() == typeof(List<DTO_TxResult>))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                                    break;
                                }
                                coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuenta.CuentaID.Value, true, false);
                                item.CuentaCtpID.Value = item.CuentaID.Value; // Asigna la Cuenta de la Contrapartida del fondo
                                item.CuentaID.Value = coPlanCta.ID.Value; // Reasigna la cuenta del debito
                            }
                            break;
                        }
                }

                if (item.ConceptoNOID.Value != conceptoFondoSoli)
                {
                    #region Crea el detalle comprobante
                    footerAportes = new DTO_ComprobanteFooter();
                    footerAportes.CentroCostoID.Value = emplProv.CentroCostoID.Value;
                    footerAportes.CuentaID.Value = item.CuentaID.Value;
                    footerAportes.ConceptoCargoID.Value = item.ConceptoCargoID.Value;
                    footerAportes.DocumentoCOM.Value = "0";
                    footerAportes.Descriptivo.Value = glCtrlAportes.Descripcion.Value;
                    footerAportes.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footerAportes.IdentificadorTR.Value = glCtrlAportes.NumeroDoc.Value;
                    footerAportes.LineaPresupuestoID.Value = item.LineaPresupuestoID.Value;
                    footerAportes.LugarGeograficoID.Value = glCtrlAportes.LugarGeograficoID.Value;
                    footerAportes.PrefijoCOM.Value = glCtrlAportes.PrefijoID.Value;
                    footerAportes.ProyectoID.Value = emplProv.ProyectoID.Value;
                    footerAportes.TasaCambio.Value = glCtrlAportes.TasaCambioCONT.Value;
                    footerAportes.TerceroID.Value = item.TerceroID.Value;
                    footerAportes.vlrBaseME.Value = 0;
                    footerAportes.vlrBaseML.Value = 0;
                    footerAportes.vlrMdaExt.Value = item.Valor.Value * tc;
                    footerAportes.vlrMdaLoc.Value = item.Valor.Value;
                    footerAportes.vlrMdaOtr.Value = 0;
                    footerAportes.Descriptivo.Value = "(Céd:" + emplProv.TerceroID.Value +  ") Cont. Planilla de Aportes";

                    lFooterAportes.Add(footerAportes);

                    #endregion
                    #region Crea el detalle comprobante Ctp

                    glCtrlAportes.CuentaID.Value = item.CuentaCtpID.Value;
                    glCtrlAportes.TerceroID.Value = item.TerceroID.Value;
                    footerAportes = this._moduloContabilidad.CrearComprobanteFooter(glCtrlAportes, tc, concCargoDef, lugarGeografico, lineaPres, item.Valor.Value.Value * -1, (item.Valor.Value.Value * tc * -1), true);
                    footerAportes.Descriptivo.Value = "Contabilización Planilla de Aportes - Contrapartida";
                    lFooterAportes.Add(footerAportes);
                    comprobante.Footer = lFooterAportes;

                    #endregion
                }
            }

            this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdateProcesadoPlanillaInd(true, this.Periodo);

            #endregion
            #region Contabiliza el comprobante Planilla Aportes

            //Contabiliza Comprobante Fiscal
            result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.PlanillaAportesContabilidad, comprobante, glCtrlAportes.PeriodoDoc.Value.Value, ModulesPrefix.no, 0, false);
            if (result.Result == ResultValue.NOK)
            {
                this.Results.Clear();
                this.Results.Add(result);
                return this.Results;
            }

            #endregion

            return this.Results;
        }
    }
}
