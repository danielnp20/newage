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
    public abstract class noLiquidacionBase : ModuloBase
    {
        #region Variables

        public ModuloGlobal _moduloGlobal = null;
        public ModuloNomina _moduloNomina = null;
        public DAL_noLiquidacionPreliminar _dal_noLiquidacionPreliminar = null;
        public DAL_noPlanillaAportesDeta _dal_noPlanillaAportesDeta = null;
        public DAL_noProvisionDeta _dal_noProvisionDeta = null;
        public DAL_noLiquidacionesDocu _dal_noLiquidacionesDocu = null;

        public decimal tasaCambio = 0;
        public int errorInd = 0;
        public string errorMsg;
        public bool errorGral = false;

        #endregion

        #region Propiedades

        public int DocumentoID { get; set; }
        public DTO_glDocumentoControl DocCtrl { get; set; }
        public DTO_noEmpleado Empleado { get; set; }
        public DateTime Periodo { get; set; }
        public DateTime FechaDoc { get; set; }

        #endregion

        public noLiquidacionBase(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Metodos

        #region Funciones Base

        /// <summary>
        /// Aprobar Liquidaciones de Nomina
        /// <param name="liq">liquidación</param>
        /// <param name="actividadFlujoID">actividad Flujo relacionada</param>
        /// </summary>
        [Transaction]
        public DTO_TxResult Aprobar(DTO_noNominaPreliminar liq, string actividadFlujoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
            this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_LiquidarNomina(liq.NumeroDoc.Value.Value, out errorInd, out errorMsg);
            this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_Delete(this.DocCtrl.NumeroDoc.Value.Value);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                #region Manejo de Errores
                result.Result = ResultValue.NOK;
                switch (errorInd)
                {
                    case 1:
                        {
                            result.ResultMessage = string.Format(DictionaryMessages.Err_No_AprobarNominaEmpleado, liq.EmpleadoID.Value);
                            break;
                        }
                    default: { result.ResultMessage = errorMsg; break; }
                }
                #endregion
            }
            else
            {
                #region Activa las alarmas
                this.AsignarFlujo(liq.DocumentoID.Value.Value, liq.NumeroDoc.Value.Value, actividadFlujoID, false, liq.DocControl.Observacion.Value);
                #endregion
            }

            return result;
        }

        /// <summary>
        /// Genera el Documento de la Liquidacion
        /// </summary>
        public DTO_TxResult GenerateDocument()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            this.tasaCambio = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

            this.DocCtrl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(this.DocumentoID, this.Empleado.ID.Value, this.Periodo);

            if (this.DocCtrl == null)
            {
                this.DocCtrl = new DTO_glDocumentoControl();
                this.DocCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                this.DocCtrl.DocumentoID.Value = this.DocumentoID;
                this.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                this.DocCtrl.Fecha.Value = DateTime.Now;
                this.DocCtrl.PeriodoDoc.Value = this.Periodo;
                this.DocCtrl.PeriodoUltMov.Value = this.Periodo;
                this.DocCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                this.DocCtrl.PrefijoID.Value = this.GetPrefijoByDocumento(this.DocumentoID);
                this.DocCtrl.MonedaID.Value = this.Empleado.MonedaExtInd.Value.Value ? mdaExt : mdaLoc;
                this.DocCtrl.TasaCambioCONT.Value = this.Empleado.MonedaExtInd.Value.Value ? this.tasaCambio : 0;
                this.DocCtrl.TasaCambioDOCU.Value = this.Empleado.MonedaExtInd.Value.Value ? this.tasaCambio : 0;
                this.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                this.DocCtrl.TerceroID.Value = this.Empleado.TerceroID.Value;
                this.DocCtrl.ProyectoID.Value = this.Empleado.ProyectoID.Value;
                this.DocCtrl.CentroCostoID.Value = this.Empleado.CentroCostoID.Value;
                this.DocCtrl.LugarGeograficoID.Value = this.Empleado.LugarGeograficoID.Value;
                this.DocCtrl.seUsuarioID.Value = this.UserId;
                this.DocCtrl.FechaDoc.Value = this.FechaDoc;

                //Valida que exista tasa de cambio si el empleado se liquida en moneda extrangera
                if (this.Empleado.MonedaExtInd.Value.Value)
                {
                    if (this.tasaCambio == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_No_ExitTasaCambioDia + "&&" + this.Empleado.ID.Value + "&&" + today.ToShortDateString();
                    }
                }

                // Inserta el documento a la base de datos
                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(this.DocumentoID, this.DocCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + this.Empleado.ID.Value + "&&" + resultGLDC.DetailsFields[0].Field;
                }
                else
                {
                    this.DocCtrl = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(resultGLDC.Key));
                }
            }

            return result;
        }

        /// <summary>
        /// Proceso para Generar los consecutivos de los documentos
        /// </summary>
        /// <returns></returns>
        public void GenerarConsecutivos(List<int> numDocs)
        {
            DTO_glDocumentoControl docControl = null;
            foreach (var numDoc in numDocs)
            {
                docControl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                docControl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(AppDocuments.Nomina, docControl.PrefijoID.Value));
                this._moduloGlobal.ActualizaConsecutivos(docControl, true, false, false);
            }
        }

        /// <summary>
        /// Procedimiento para Liquidar las Novedades de Nomina
        /// </summary>
        /// <returns></returns>
        public DTO_TxResult LiquidarNovedadesNomina()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            this._moduloNomina = (ModuloNomina)this.GetInstance(typeof(ModuloNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            //Lista las Novedades de Nomina
            List<DTO_noNovedadesNomina> novedades = this._moduloNomina.Nomina_GetNovedades(this.Empleado.ID.Value);
            this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_Delete(this.DocCtrl.NumeroDoc.Value.Value);

            foreach (var nov in novedades)
            {
                DTO_noConceptoNOM conceptoNOID = (DTO_noConceptoNOM)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, nov.ConceptoNOID.Value, true, false);
                double valorFormula = 0;
                decimal sueldo = this.Empleado.Sueldo.Value.Value;


                if (conceptoNOID.TipoLiquidacion.Value == (byte)TipoLiquidacionNOM.Formula)
                {
                    #region Formulacion

                    try
                    {
                        if (this.Empleado.MonedaExtInd.Value.Value)
                            sueldo = sueldo * this.tasaCambio;

                        // Base Valor = 1 (Sueldo)
                        if (conceptoNOID.BaseFormula.Value == 1)
                        {
                            valorFormula = Evaluador.Evaluate(
                                conceptoNOID.Formula.Value.Replace("S", sueldo.ToString()).Replace("H", nov.Valor.Value.ToString()).Replace('.', ',').Replace(" ", string.Empty)
                            );
                        }
                        else if (conceptoNOID.BaseFormula.Value == 2) // Base Valor = 2 (Dias Trabajados)
                        {
                            valorFormula = Evaluador.Evaluate(
                                conceptoNOID.Formula.Value.Replace("D", sueldo.ToString()).Replace('.', ',').Replace(" ", string.Empty)
                                );
                        }
                    }
                    catch
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_No_FormulaNovNomina + "&&" + this.Empleado.ID.Value + "&&" + conceptoNOID.Formula.Value + "&&" + conceptoNOID.ID.Value;
                        return result;
                    }

                    #endregion
                }
                else if (conceptoNOID.TipoLiquidacion.Value == (byte)TipoLiquidacionNOM.Valor)
                {
                    valorFormula = Convert.ToDouble(nov.Valor.Value.Value);
                }

                //Ejecuta el procedimiento para liquidar las novedades de nómina
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_LiquidarNovedadesNomina(this.Empleado.ID.Value, this.DocCtrl.NumeroDoc.Value.Value, conceptoNOID.ID.Value, valorFormula, out errorInd, out errorMsg);

                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;

                    if (errorInd == 1)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_No_liquidarNovedadesNomina + "&&" + this.Empresa.Descriptivo.Value;
                    }
                }

                #endregion

            }
            return result;
        }

        /// <summary>
        /// Proceso revierte la liquidacion de NOMINA
        /// </summary>
        /// <returns></returns>
        public void RevertirLiquidacion()
        {
            this.DocCtrl = this._moduloGlobal.glDocumentoControl_GetDocEmpleado(this.DocumentoID, this.Empleado.ID.Value, this.Periodo);
            if (this.DocCtrl != null)
            {
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.Anulado, "Anulación Documento de Liq Nomina", true);
                this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_RevertirLiqNomina(this.DocumentoID, this.Periodo, this.Empleado.ID.Value);
            }
        }


        #endregion

        #region Funciones Abstractas

        /// <summary>
        /// Proceso para Liquidar la Nomina
        /// </summary>
        /// <returns></returns>
        public abstract DTO_TxResult Liquidar();



        #endregion

        #endregion

    }
}
