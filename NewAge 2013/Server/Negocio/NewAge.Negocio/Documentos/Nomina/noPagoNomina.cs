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
    public class noPagoNomina : ModuloBase
    {
        #region Private Variables

        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_noLiquidacionesDetalle _dal_noLiquidacionesDetalle = null;
        private DAL_noLiquidacionesDocu _dal_noLiquidacionesDocu = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloCuentasXPagar _moduloCXP = null;
        private ModuloContabilidad _moduloContabilidad = null;

        #endregion

        public DateTime Periodo { get; set; }
        public DateTime FechaDoc { get; set; }
        public DTO_noPagoLiquidaciones Pago { get; set; }
        public int DocumentoID { get; set; }
        public DTO_glDocumentoControl glCtrl = null;
        public DTO_coComprobante Comp { get; set; }
        public DTO_glDocumentoControl ctrlCxP = null;

        public noPagoNomina(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        [Transaction]
        public List<DTO_TxResult> Pagar()
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            
            DTO_Comprobante comprobante = new DTO_Comprobante();

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

           
            //Actualiza indicador de Pago en Documento Origen
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdatePagadoInd(this.Pago.NumeroDoc.Value.Value, true);
            

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
            switch (this.Pago.DocLiquidacion.TipoLiquidacion.Value.Value)
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
            glCtrl.DocumentoID.Value = this.DocumentoID;
            glCtrl.PeriodoDoc.Value = this.Periodo;
            glCtrl.FechaDoc.Value = this.FechaDoc;
            glCtrl.PrefijoID.Value = prefijDef;
            glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
            glCtrl.DocumentoNro.Value = 0;
            glCtrl.TasaCambioCONT.Value = tc;
            glCtrl.TasaCambioDOCU.Value = tc;
            glCtrl.TerceroID.Value = this.Pago.EmpleadoID.Value;
            glCtrl.DocumentoTercero.Value = string.Empty;
            glCtrl.CuentaID.Value = string.Empty;
            glCtrl.MonedaID.Value = mdaLoc;
            glCtrl.ProyectoID.Value = proyectoXDef;
            glCtrl.CentroCostoID.Value = cCosto;
            glCtrl.LineaPresupuestoID.Value = lineaPres;
            glCtrl.Observacion.Value = "PAGO NOMINA";
            glCtrl.Descripcion.Value = "PAGO NOMINA";
            glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
            glCtrl.PeriodoUltMov.Value = this.Periodo;
            glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
            glCtrl.seUsuarioID.Value = this.UserId;           
            DTO_TxResultDetail resultGLDC = new DTO_TxResultDetail();
            if (result.Result == ResultValue.OK)
            {
                resultGLDC = this._moduloGlobal.glDocumentoControl_Add(this.DocumentoID, glCtrl, true);
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
            #region Agregar Footer Comprobante Nomina
            List<DTO_ComprobanteFooter> lFooter = new List<DTO_ComprobanteFooter>();
            DTO_ComprobanteFooter footer = null;

            #region Crea el detalle comprobante

            footer = new DTO_ComprobanteFooter();

            DTO_coDocumento docContableNomina = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);
            DTO_coPlanCuenta coPlanCta = coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableNomina.CuentaLOC.Value, true, false);
            DTO_glConceptoSaldo glConceptoSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, coPlanCta.ConceptoSaldoID.Value, true, false);

            footer = this.CrearComprobanteFooter(glCtrl, coPlanCta, glConceptoSaldo, tc, concCargoDef, lugarGeografico, lineaPres, Pago.Valor.Value.Value, Pago.Valor.Value.Value * tc, false);
            footer.Descriptivo.Value = String.Format("{0}", "Pago " + description); ;
            lFooter.Add(footer);

            #endregion
            #region Crea C x P Asociada al Doc

            object obj = this._moduloCXP.CuentasXPagar_Generar(glCtrl, conceptoCxPcontrol, this.Pago.Valor.Value.Value, lFooter, ModulesPrefix.no, false);
            if (obj.GetType() == typeof(DTO_TxResult))
            {
                result = (DTO_TxResult)obj;
                results.Add(result);
                return results;
            }
            ctrlCxP = (DTO_glDocumentoControl)obj;
            this.Comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
            #endregion

            #endregion
            return results;
            
        }

        /// <summary>
        /// Proceso para Generar los consecutivos de los documentos
        /// </summary>
        /// <returns></returns>
        public void GenerarConsecutivos(int numDoc, int documentoId, DTO_coComprobante coComp)
        {
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glDocumentoControl docControl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
            docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(documentoId, docControl.PrefijoID.Value));
            int comprobanteNro = this.GenerarComprobanteNro(coComp, docControl.PrefijoID.Value, docControl.PeriodoDoc.Value.Value, docControl.DocumentoNro.Value.Value);
            this._moduloGlobal.ActualizaConsecutivos(docControl, true, true, false);
            this._moduloContabilidad.ActualizaComprobanteNro(docControl.NumeroDoc.Value.Value, comprobanteNro, false, coComp.ID.Value);
        }
    }
}
