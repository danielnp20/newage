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
    public class noContabilizarProvisiones: noContabilizacionBase
    {
        public noContabilizarProvisiones(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        public DTO_glDocumentoControl glCtrlProvisiones = null;
        DTO_Comprobante comprobante = new DTO_Comprobante();
        
        [Transaction]
        public override List<DTO_TxResult> Contabilizar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noProvisionDeta = (DAL_noProvisionDeta)this.GetInstance(typeof(DAL_noProvisionDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        
            DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);
            this.Comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docContableNomina.ComprobanteID.Value, true, false);

            #region Crea GlDocumentoControl 886 - NominaProvisiones

            this.glCtrlProvisiones = new DTO_glDocumentoControl();
            this.glCtrlProvisiones.DocumentoID.Value = AppDocuments.ProvisionesContabilidad;
            this.glCtrlProvisiones.Fecha.Value = DateTime.Now;
            this.glCtrlProvisiones.FechaDoc.Value = this.FechaDoc;
            this.glCtrlProvisiones.PeriodoDoc.Value = this.Periodo;
            this.glCtrlProvisiones.PrefijoID.Value = this.GetPrefijoByDocumento(this.DocumentoId);
            this.glCtrlProvisiones.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
            this.glCtrlProvisiones.TasaCambioCONT.Value = tc;
            this.glCtrlProvisiones.TasaCambioDOCU.Value = tc;
            this.glCtrlProvisiones.TerceroID.Value = terceroPorDefecto;
            this.glCtrlProvisiones.DocumentoTercero.Value = string.Empty;
            this.glCtrlProvisiones.CuentaID.Value = string.Empty;
            this.glCtrlProvisiones.MonedaID.Value = mdaLoc;
            this.glCtrlProvisiones.ProyectoID.Value = proyectoXDef;
            this.glCtrlProvisiones.CentroCostoID.Value = cCosto;
            this.glCtrlProvisiones.LugarGeograficoID.Value = lugarGeografico;
            this.glCtrlProvisiones.LineaPresupuestoID.Value = lineaPres;
            glCtrlProvisiones.Observacion.Value = "CONTABILIZAR PROVISIONES";
            glCtrlProvisiones.Descripcion.Value = "CONTABILIZAR PROVISIONES";
            this.glCtrlProvisiones.Estado.Value = (Byte)EstadoDocControl.Aprobado;
            this.glCtrlProvisiones.DocumentoNro.Value = 0;
            this.glCtrlProvisiones.PeriodoUltMov.Value = this.Periodo;
            this.glCtrlProvisiones.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
            this.glCtrlProvisiones.seUsuarioID.Value = this.UserId;


            DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(this.DocumentoId, glCtrlProvisiones, true);
            if (resultGLDC.Message != ResultValue.OK.ToString())
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = "NOK";
                result.Details.Add(resultGLDC);
                this.Results.Add(result);
                return this.Results;
            }

            int numDocNominaProv = Convert.ToInt32(resultGLDC.Key);
            glCtrlProvisiones.NumeroDoc.Value = numDocNominaProv;

            #endregion
            #region Agregar Header Comprobante

            TipoMoneda_LocExt tipoM = glCtrlProvisiones.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
            DTO_ComprobanteHeader headerProv = new DTO_ComprobanteHeader();
            headerProv.ComprobanteID.Value = this.Comp.ID.Value;
            headerProv.ComprobanteNro.Value = 0;
            headerProv.EmpresaID.Value = glCtrlProvisiones.EmpresaID.Value;
            headerProv.Fecha.Value = glCtrlProvisiones.FechaDoc.Value;
            headerProv.MdaOrigen.Value = (Byte)tipoM;
            headerProv.MdaTransacc.Value = glCtrlProvisiones.MonedaID.Value;
            headerProv.NumeroDoc.Value = Convert.ToInt32(numDocNominaProv);
            headerProv.PeriodoID.Value = glCtrlProvisiones.PeriodoDoc.Value;
            headerProv.TasaCambioBase.Value = glCtrlProvisiones.TasaCambioCONT.Value;
            headerProv.TasaCambioOtr.Value = glCtrlProvisiones.TasaCambioDOCU.Value;
            comprobante.Header = headerProv;

            #endregion
            #region Agregar Footer Comprobante Provisiones

            List<DTO_noProvisionDeta> lProvisiones = this._dal_noProvisionDeta.DAL_noProvisionDeta_Get(this.Periodo);
            List<DTO_ComprobanteFooter> lFooterProv = new List<DTO_ComprobanteFooter>();
            DTO_ComprobanteFooter footerProv = null;

            foreach (var item in lProvisiones)
            {
                #region Traer Cuenta Componente

                //Obtiene la cuenta a partir del Concepto Bien y Servicio
                string operacion = string.Empty;
                string linePresupuestal = string.Empty;
                string conceptoCargoID = string.Empty;

                DTO_noConceptoNOM conceptoNom = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, item.ConceptoNOID.Value, true, false);
                DTO_glBienServicioClase claseByS = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, conceptoNom.ClaseBSID.Value, true, false);
                DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, conceptoNom.CuentaID.Value, true, false);

                if (claseByS == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "Clase Servicio" + "&&" + conceptoNom.ClaseBSID.Value;
                    this.Results.Add(result);
                    return this.Results;
                }

                DTO_noEmpleado emplProv = (DTO_noEmpleado)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, item.EmpleadoID.Value, true, false);

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


                #endregion
                #region Crea el detalle comprobante

                footerProv = new DTO_ComprobanteFooter();
                footerProv.CentroCostoID.Value = emplProv.CentroCostoID.Value;
                footerProv.CuentaID.Value = coPlanCta.ID.Value;
                footerProv.ConceptoCargoID.Value = conceptoCargoID;
                footerProv.DocumentoCOM.Value = "0";
                footerProv.Descriptivo.Value = glCtrlProvisiones.Descripcion.Value;
                footerProv.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                footerProv.IdentificadorTR.Value = glCtrlProvisiones.NumeroDoc.Value;
                footerProv.LineaPresupuestoID.Value = lineaPres;
                footerProv.LugarGeograficoID.Value = glCtrlProvisiones.LugarGeograficoID.Value;
                footerProv.PrefijoCOM.Value = glCtrlProvisiones.PrefijoID.Value;
                footerProv.ProyectoID.Value = emplProv.ProyectoID.Value;
                footerProv.TasaCambio.Value = glCtrlProvisiones.TasaCambioCONT.Value;
                footerProv.TerceroID.Value = item.EmpleadoID.Value;
                footerProv.vlrBaseME.Value = 0;
                footerProv.vlrBaseML.Value = 0;
                footerProv.vlrMdaExt.Value = item.VlrProvisionMES.Value * tc;
                footerProv.vlrMdaLoc.Value = item.VlrProvisionMES.Value;
                footerProv.vlrMdaOtr.Value = 0;
                footerProv.Descriptivo.Value = "Contabilizaciób Provisiones";

                lFooterProv.Add(footerProv);
                #endregion
                //Actualizar Procesado Ind Provisiones
                this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdateProcesadoInd(item.NumeroDoc.Value.Value, true);
            }

            #endregion
            #region Contrapartida Provisiones

            foreach (var item in lProvisiones)
            {
                #region Traer Cuenta Componente

                //Obtiene la cuenta a partir del Concepto Bien y Servicio
                string operacion = string.Empty;
                string linePresupuestal = string.Empty;
                string conceptoCargoID = string.Empty;

                DTO_noConceptoNOM conceptoNom = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, item.ConceptoNOID.Value, true, false);
                DTO_coPlanCuenta coPlanCtaProCtp = new DTO_coPlanCuenta();
                coPlanCtaProCtp = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, conceptoNom.CuentaID.Value, true, false);

                if (coPlanCtaProCtp != null)
                {
                    #region Crea el detalle comprobante

                    footerProv = new DTO_ComprobanteFooter();
                    footerProv.CentroCostoID.Value = glCtrlProvisiones.CentroCostoID.Value;
                    footerProv.CuentaID.Value = coPlanCtaProCtp.ID.Value;
                    footerProv.ConceptoCargoID.Value = concCargoDef;
                    footerProv.DocumentoCOM.Value = "0";
                    footerProv.Descriptivo.Value = glCtrlProvisiones.Descripcion.Value;
                    footerProv.ConceptoSaldoID.Value = coPlanCtaProCtp.ConceptoSaldoID.Value;
                    footerProv.IdentificadorTR.Value = glCtrlProvisiones.NumeroDoc.Value;
                    footerProv.LineaPresupuestoID.Value = lineaPres;
                    footerProv.LugarGeograficoID.Value = glCtrlProvisiones.LugarGeograficoID.Value;
                    footerProv.PrefijoCOM.Value = glCtrlProvisiones.PrefijoID.Value;
                    footerProv.ProyectoID.Value = glCtrlProvisiones.ProyectoID.Value;
                    footerProv.TasaCambio.Value = glCtrlProvisiones.TasaCambioCONT.Value;
                    footerProv.TerceroID.Value = item.EmpleadoID.Value;
                    footerProv.vlrBaseME.Value = 0;
                    footerProv.vlrBaseML.Value = 0;
                    footerProv.vlrMdaExt.Value = (item.VlrProvisionMES.Value * tc) * -1;
                    footerProv.vlrMdaLoc.Value = (item.VlrProvisionMES.Value) * -1;
                    footerProv.vlrMdaOtr.Value = (item.VlrProvisionMES.Value) * -1;
                    footerProv.Descriptivo.Value = "Contabilización Provisiones - Contrapartida";
                    footerProv.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    lFooterProv.Add(footerProv);
                    #endregion
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                    this.Results.Add(result);
                    return this.Results;
                }

                #endregion

            }

            comprobante.Footer = lFooterProv;
            #endregion
            #region Contabiliza el comprobante Provisiones

            //Contabiliza Comprobante Fiscal
            result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.ProvisionesContabilidad, comprobante, glCtrlProvisiones.PeriodoDoc.Value.Value, ModulesPrefix.no, 0, false);
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
