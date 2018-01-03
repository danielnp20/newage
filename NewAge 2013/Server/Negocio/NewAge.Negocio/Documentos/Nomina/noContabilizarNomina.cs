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
    public class noContabilizarNomina: noContabilizacionBase
    {
        public DTO_glDocumentoControl glCtrlNomina = null;
        private TipoLiquidacion tipoLiquidacion;

        public TipoLiquidacion TipoLiquidacion
        {
            get { return tipoLiquidacion; }
            set { tipoLiquidacion = value; }
        }

        DTO_Comprobante comprobante = new DTO_Comprobante();

        public noContabilizarNomina(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        [Transaction] 
        public override List<DTO_TxResult> Contabilizar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            //Valida que existan liquidaciónes para ser procesadas
            if (this.Liquidaciones.Count == 0)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = "No existen Liquidaciones para procesar, Verifique de nuevo";
                this.Results.Add(result);
                return this.Results;
            }

            DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);
            this.Comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docContableNomina.ComprobanteID.Value, true, false);


            #region Crea GlDocumentoControl 881 - NominaContabilidad

            glCtrlNomina = new DTO_glDocumentoControl();
            glCtrlNomina.DocumentoID.Value = AppDocuments.NominaContabilidad;
            glCtrlNomina.Fecha.Value = DateTime.Now;
            glCtrlNomina.FechaDoc.Value = this.FechaDoc;
            glCtrlNomina.PeriodoDoc.Value = this.Periodo;
            glCtrlNomina.PrefijoID.Value = this.GetPrefijoByDocumento(this.DocumentoId);
            glCtrlNomina.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
            glCtrlNomina.TasaCambioCONT.Value = tc;
            glCtrlNomina.TasaCambioDOCU.Value = tc;
            glCtrlNomina.TerceroID.Value = terceroPorDefecto;
            glCtrlNomina.DocumentoTercero.Value = string.Empty;
            glCtrlNomina.CuentaID.Value = string.Empty;
            glCtrlNomina.MonedaID.Value = mdaLoc;
            glCtrlNomina.ProyectoID.Value = proyectoXDef;
            glCtrlNomina.CentroCostoID.Value = cCosto;
            glCtrlNomina.LugarGeograficoID.Value = lugarGeografico;
            glCtrlNomina.LineaPresupuestoID.Value = lineaPres;
            glCtrlNomina.Observacion.Value = "CONTABILIZAR NOMINA";
            glCtrlNomina.Descripcion.Value = "CONTABILIZAR NOMINA";
            glCtrlNomina.Estado.Value = (Byte)EstadoDocControl.Aprobado;
            glCtrlNomina.DocumentoNro.Value = 0;
            glCtrlNomina.PeriodoUltMov.Value = Periodo;
            glCtrlNomina.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
            glCtrlNomina.seUsuarioID.Value = this.UserId;


            DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(this.DocumentoId, glCtrlNomina, true);
            if (resultGLDC.Message != ResultValue.OK.ToString())
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = "NOK";
                result.Details.Add(resultGLDC);
                this.Results.Add(result);
                return this.Results;
            }

            int numDocNomina = Convert.ToInt32(resultGLDC.Key);
            glCtrlNomina.NumeroDoc.Value = numDocNomina;

            #endregion
            #region Agregar Header Comprobante

            TipoMoneda_LocExt tipoM = glCtrlNomina.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
            DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
            header.ComprobanteID.Value = this.Comp.ID.Value;
            header.ComprobanteNro.Value = 0;
            header.EmpresaID.Value = glCtrlNomina.EmpresaID.Value;
            header.Fecha.Value = glCtrlNomina.FechaDoc.Value;
            header.MdaOrigen.Value = (Byte)tipoM;
            header.MdaTransacc.Value = glCtrlNomina.MonedaID.Value;
            header.NumeroDoc.Value = Convert.ToInt32(numDocNomina);
            header.PeriodoID.Value = glCtrlNomina.PeriodoDoc.Value;
            header.TasaCambioBase.Value = glCtrlNomina.TasaCambioCONT.Value;
            header.TasaCambioOtr.Value = glCtrlNomina.TasaCambioDOCU.Value;
            comprobante.Header = header;

            #endregion
            #region Agregar Footer Comprobante Nomina

            List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
            DTO_ComprobanteFooter footer = null;

            string valores = string.Empty;

            #region Descripcion documentos segun el tipo de liquidacion

            string description = string.Empty;
            int docId = 0;
            switch (this.tipoLiquidacion)
            {
                case TipoLiquidacion.N:
                    {
                    this.Liquidaciones = this.Liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.N).ToList();
                    description = "Nomina";
                    docId = (int)AppDocuments.Nomina;
                    break;
                }
                case TipoLiquidacion.V:
                    {
                        this.Liquidaciones = this.Liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.V).ToList();
                        description = "Vacaciones";
                        docId = (int)AppDocuments.Vacaciones;
                        break;
                    }
                case TipoLiquidacion.P:
                    {
                        this.Liquidaciones = this.Liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.P).ToList();
                        description = "Prima";
                        docId = (int)AppDocuments.Prima;
                        break;
                    }
                case TipoLiquidacion.C:
                    {
                        this.Liquidaciones = this.Liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.C).ToList();
                        description = "Cesantias";
                        docId = (int)AppDocuments.Cesantias;
                        break;
                    }
                case TipoLiquidacion.L:
                    {
                        this.Liquidaciones = this.Liquidaciones.Where(x => x.Liquidacion.Value == (byte)TipoLiquidacion.L).ToList();
                        description = "Liquidación Contrato";
                        docId = (int)AppDocuments.LiquidacionContrato;
                        break;
                    }
            }

            #endregion

            foreach (var liq in this.Liquidaciones)
            {
                this.Empleado = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, liq.EmpleadoID.Value, true, false);
                var detalleLiquidaciones = this.GetLiquidacionesDetalle(docId);

                foreach (var detalleliq in detalleLiquidaciones.Detalle)
                {
                    #region Traer Cuenta Componente

                    //Obtiene la cuenta a partir del Concepto Bien y Servicio
                    string operacion = string.Empty;
                    string linePresupuestal = string.Empty;
                    string conceptoCargoID = string.Empty;
                    DTO_noConceptoNOM conceptoNom = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, detalleliq.ConceptoNOID.Value, true, false);
                    DTO_glBienServicioClase claseByS = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, conceptoNom.ClaseBSID.Value, true, false);
                    if (claseByS == null)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "Clase Servicio" + "&&" + conceptoNom.ClaseBSID.Value;
                        this.Results.Add(result);
                        return this.Results;
                    }                    
                    DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, conceptoNom.CuentaID.Value, true, false);

                    if (coPlanCta == null)
                    {
                        DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, this.Empleado.ProyectoID.Value, true, false);
                        operacion = proyecto.OperacionID.Value;
                        if (string.IsNullOrEmpty(operacion))
                        {
                            DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, this.Empleado.CentroCostoID.Value, true, false);
                            operacion = centroCosto.OperacionID.Value;
                            if (string.IsNullOrEmpty(operacion))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(claseByS.LineaPresupuestoID.Value))
                            linePresupuestal = lineaPres;
                        else
                            linePresupuestal = claseByS.LineaPresupuestoID.Value;

                        if (string.IsNullOrEmpty(claseByS.ConceptoCargoID.Value))
                            conceptoCargoID = concCargoDef;
                        else
                            conceptoCargoID = claseByS.ConceptoCargoID.Value;

                        DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(claseByS.ConceptoCargoID.Value, operacion, claseByS.LineaPresupuestoID.Value, 0);
                        if (cuenta == null || cuenta.GetType() == typeof(List<DTO_TxResult>))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                            this.Results.Add(result);
                            return this.Results;
                        }
                        coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cuenta.CuentaID.Value, true, false);
                    }
                    else
                    {
                        linePresupuestal = lineaPres;
                        conceptoCargoID = concCargoDef;
                    }


                    #endregion

                    #region Validar Tercero

                    string TerceroIDLiq = detalleLiquidaciones.DocControl.TerceroID.Value;
                    if (detalleliq.OrigenConcepto.Value == (byte)OrigenConcepto.Fondo)
                    {
                        DTO_noFondo fondoIDLiq = (DTO_noFondo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noFondo, detalleliq.FondoNOID.Value, true, false);
                        TerceroIDLiq = fondoIDLiq.TerceroID.Value;
                    }

                    #endregion

                    #region Crea el detalle comprobante
                    footer = new DTO_ComprobanteFooter();
                    footer.CentroCostoID.Value = this.Empleado.CentroCostoID.Value;
                    footer.CuentaID.Value = coPlanCta.ID.Value;
                    footer.ConceptoCargoID.Value = conceptoCargoID;
                    footer.DocumentoCOM.Value = detalleLiquidaciones.DocControl.DocumentoNro.Value.ToString();
                    footer.Descriptivo.Value = detalleLiquidaciones.DocControl.Descripcion.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = detalleLiquidaciones.DocLiquidacion.NumeroDoc.Value.Value;
                    footer.LineaPresupuestoID.Value = linePresupuestal;
                    footer.LugarGeograficoID.Value = detalleLiquidaciones.DocControl.LugarGeograficoID.Value;
                    footer.PrefijoCOM.Value = detalleLiquidaciones.DocControl.PrefijoID.Value;
                    footer.ProyectoID.Value = this.Empleado.ProyectoID.Value;
                    footer.TasaCambio.Value = detalleLiquidaciones.DocControl.TasaCambioCONT.Value;
                    footer.TerceroID.Value = TerceroIDLiq;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = detalleliq.Valor.Value.Value * tc;
                    footer.vlrMdaLoc.Value = detalleliq.Valor.Value.Value;
                    footer.vlrMdaOtr.Value = 0;
                    footer.DatoAdd10.Value = detalleLiquidaciones.DocLiquidacion.NumeroDoc.Value.ToString(); // Asigna el numero Doc del documento de detalle                 
                    footer.Descriptivo.Value =  string.Format("Contabilización  {0}", description);

                    lFooter.Add(footer);

                    valores = valores + "\n" + detalleliq.Valor.Value.Value.ToString();
                    #endregion
                }

                //Actualiza Procesado Ind
                this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdateProcesadoInd(detalleLiquidaciones.NumeroDoc.Value.Value, true);
            }


            #endregion
            #region Contrapartida Nomina

            footer = new DTO_ComprobanteFooter();

            DTO_coPlanCuenta coPlanCtaCptda = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableNomina.CuentaLOC.Value, true, false);
            glCtrlNomina.CuentaID.Value = coPlanCtaCptda.ID.Value;

            decimal valorTotal = this.Liquidaciones.Sum(x => x.ValorDetalle.Value.Value);
            footer = this._moduloContabilidad.CrearComprobanteFooter(glCtrlNomina, tc, concCargoDef, lugarGeografico, lineaPres, valorTotal * -1, (valorTotal * -1) * tc, true);
            footer.Descriptivo.Value = string.Format("Contabilización  {0} Contrapartida", description);
            lFooter.Add(footer);
            comprobante.Footer = lFooter;
            #endregion           
            #region Contabilizacion
            result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.NominaContabilidad, comprobante, glCtrlNomina.PeriodoDoc.Value.Value, ModulesPrefix.no, 0, false);
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
