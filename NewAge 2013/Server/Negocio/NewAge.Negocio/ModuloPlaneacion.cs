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
using NewAge.DTO.Reportes;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloPlaneacion : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_plCierres _dal_plCierres = null;
        private DAL_plPresupuestoDeta _dal_plPresupuestoDeta;
        private DAL_plPresupuestoTotal _dal_plPresupuestoTotal;
        private DAL_plCierreLegalizacion _dal_plCierreLegalizacion;
        private DAL_ReportesPlaneacion _dal_reportesPlaneacion;
        private DAL_plPresupuestoDocu _dal_plPresupuestoDocu;
        private DAL_MasterComplex _dal_MasterComplex;
        private DAL_MasterSimple _dal_MasterSimple;
        private DAL_MasterHierarchy _dal_MasterHierarchy;
        private DAL_plSobreEjecucion _dal_plSobreEjecucion;
        private DAL_plPresupuestoPxQ _dal_plPresupuestoPxQ;
        private DAL_plPresupuestoPxQDeta _dal_plPresupuestoPxQDeta;
        private DAL_plPresupuestoSoporte _dal_plPresupuestoSoporte;
        private DAL_plPlaneacion_Proveedores _dal_plPlaneacionProveedor;

        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloProveedores _moduloProveedores = null;

        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Planeacion
        /// </summary>
        /// <param name="conn"></param>
        public ModuloPlaneacion(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region COM

        /// <summary>
        /// Actualiza un registro de plCierreLegalizacion
        /// </summary>
        /// <param name="deta">Cierre</param>
        public void plCierreLegalizacion_Add(DTO_plCierreLegalizacion cierre)
        {
            this._dal_plCierreLegalizacion = (DAL_plCierreLegalizacion)this.GetInstance(typeof(DAL_plCierreLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_plCierreLegalizacion.DAL_plCierreLegalizacion_Add(cierre);
        }

        /// <summary>
        /// Actualiza un registro de plSobreEjecucion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void plSobreEjecucion_Add(DTO_plSobreEjecucion deta)
        {
            this._dal_plSobreEjecucion = (DAL_plSobreEjecucion)this.GetInstance(typeof(DAL_plSobreEjecucion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_plSobreEjecucion.DAL_plSobreEjecucion_Add(deta);
        }

        /// <summary>
        /// Actualiza un registro de plPresupuestoSoporte
        /// </summary>
        /// <param name="deta">Registro</param>
        /// <returns></returns>
        public void plPresupuestoSoporte_Add(DTO_plPresupuestoSoporte deta)
        {
            this._dal_plPresupuestoSoporte = (DAL_plPresupuestoSoporte)this.GetInstance(typeof(DAL_plPresupuestoSoporte), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_plPresupuestoSoporte.DAL_plPresupuestoSoporte_Add(deta);
        }

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQ
        /// </summary>
        /// <param name="deta">Registro</param>
        /// <returns></returns>
        public void plPresupuestoPxQ_Add(DTO_plPresupuestoPxQ deta)
        {
            this._dal_plPresupuestoPxQ = (DAL_plPresupuestoPxQ)this.GetInstance(typeof(DAL_plPresupuestoPxQ), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_plPresupuestoPxQ.DAL_plPresupuestoPxQ_Add(deta);
        }

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQ
        /// </summary>
        /// <param name="deta">Registro</param>
        /// <returns></returns>
        public void plPresupuestoPxQDeta_Add(DTO_plPresupuestoPxQDeta deta)
        {
            this._dal_plPresupuestoPxQDeta = (DAL_plPresupuestoPxQDeta)this.GetInstance(typeof(DAL_plPresupuestoPxQDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_Add(deta);
        }

        #endregion

        #region Cierres

        #region Funciones privadas

        /// <summary>
        /// Realiza el proceso de cierre diario
        /// </summary>
        private DTO_TxResult CerrarDia(DateTime periodo, DateTime periodo_co, DateTime fechaCierre, string keyControl)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
                this._moduloGlobal._mySqlConnectionTx = base._mySqlConnectionTx;
                this._dal_plCierres.MySqlConnectionTx = base._mySqlConnectionTx;

                result = this._dal_plCierres.DAL_plCierreDia_Procesar(periodo, periodo_co, fechaCierre);
                if (result.Result == ResultValue.OK)
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Planeacion_CerrarDia");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                    base._mySqlConnectionTx.Commit();
                else
                    base._mySqlConnectionTx.Rollback();

                this._dal_plCierres.MySqlConnectionTx = null;
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
                this._dal_plCierres = (DAL_plCierres)base.GetInstance(typeof(DAL_plCierres), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables y validaciones

                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo));
                DateTime periodo_co = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));

                //Fecha final 
                int maxDay = DateTime.Now.Day;
                DateTime maxDate = DateTime.Now;
                if (periodo.Year != maxDate.Year || periodo.Month != maxDate.Month)
                {
                    maxDay = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    maxDate = new DateTime(periodo.Year, periodo.Month, maxDay);
                }

                //Fecha inicial
                string diaIniStr = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_DiaUltimoCierre);
                int diaIni = string.IsNullOrWhiteSpace(diaIniStr) ? 1 : Convert.ToInt16(diaIniStr) + 1;

                #endregion

                if (diaIni <= maxDay)
                {
                    for (int i = diaIni; i <= maxDay; ++i)
                    {
                        #region Realiza el proceso de cierre por día

                        //Carga las variables
                        DateTime fechaCierre = new DateTime(periodo.Year, periodo.Month, i);
                        string EmpNro = this.Empresa.NumeroControl.Value;
                        string _modId = ((int)ModulesPrefix.pl).ToString();
                        if (_modId.Length == 1)
                            _modId = "0" + _modId;
                        string keyControl = EmpNro + _modId + AppControl.pl_DiaUltimoCierre;

                        result = this.CerrarDia(periodo, periodo_co, fechaCierre, keyControl);
                        if (result.Result == ResultValue.NOK)
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Planeacion_Proceso_CierreDia");

                return result;
            }
        }

        #endregion

        #endregion

        #region General

        /// <summary>
        ///  Obtiene el valor de un periodo en particular
        /// </summary>
        /// <param name="deta">dto de valores</param>
        /// <param name="PeriodoID">Periodo a consultar</param>
        ///  <param name="isOrigenML">Origen de la moneda</param>
        ///  <param name="isMdaLocal">Moneda a mostrar</param>
        /// <returns>Valor del periodo</returns>
        private decimal GetValueByPeriodo(DTO_plPresupuestoDeta deta, int PeriodoID, bool isOrigenML, bool isMdaLocal)
        {
            decimal valuePeriod = 0;
            switch (PeriodoID)
            {
                case 1:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc01.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt01.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt01.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc01.Value.Value;
                    break;
                case 2:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc02.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt02.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt02.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc02.Value.Value;
                    break;
                case 3:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc03.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt03.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt03.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc03.Value.Value;
                    break;
                case 4:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc04.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt04.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt04.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc04.Value.Value;
                    break;
                case 5:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc05.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt05.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt05.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc05.Value.Value;
                    break;
                case 6:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc06.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt06.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt06.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc06.Value.Value;
                    break;
                case 7:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc07.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt07.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt07.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc07.Value.Value;
                    break;
                case 8:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc08.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt08.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt08.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc08.Value.Value;
                    break;
                case 9:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc09.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt09.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt09.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc09.Value.Value;
                    break;
                case 10:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc10.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt10.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt10.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc10.Value.Value;
                    break;
                case 11:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc11.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt11.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt11.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc11.Value.Value;
                    break;
                case 12:
                    if (isOrigenML && isMdaLocal)
                        valuePeriod = deta.ValorLoc12.Value.Value;
                    else if (isOrigenML && !isMdaLocal)
                        valuePeriod = deta.EquivExt12.Value.Value;
                    else if (!isOrigenML && !isMdaLocal)
                        valuePeriod = deta.ValorExt12.Value.Value;
                    else if (!isOrigenML && isMdaLocal)
                        valuePeriod = deta.EquivLoc12.Value.Value;
                    break;
            }
            return valuePeriod;
        }

        /// <summary>
        /// Genera el presupuesto de una transaccion de proveedores o planeacion
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="numeroDoc">identificador de la transaccion</param>
        /// <returns>Resultado</returns>
        internal DTO_TxResult GeneraPresupuesto(int documentID, int numeroDoc)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            try
            {
                #region Variables iniciales
                this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plCierreLegalizacion = (DAL_plCierreLegalizacion)this.GetInstance(typeof(DAL_plCierreLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_plCierreLegalizacion> listCierres = new List<DTO_plCierreLegalizacion>();
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string periodoIDPlaneacion = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_Periodo);
                DateTime periodoPlaneacion = !string.IsNullOrEmpty(periodoIDPlaneacion) ? Convert.ToDateTime(periodoIDPlaneacion) : DateTime.Now;

                DTO_glDocumentoControl docControl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                bool isOrigenML = docControl.MonedaID.Value == monedaLocal ? true : false;
                bool isFactura = docControl.DocumentoID.Value == AppDocuments.CausarFacturas ? true : false;
                #endregion

                if (documentID != AppDocuments.Presupuesto && documentID != AppDocuments.AdicionPresupuesto &&
                    documentID != AppDocuments.ReclasifPresupuesto && documentID != AppDocuments.TrasladoPresupuesto)
                {
                    #region Presupuesto Proveedores
                    List<DTO_prDetalleDocu> listDetalleDocu = this._moduloProveedores.prDetalleDocu_GetByNumeroDoc(numeroDoc, isFactura);
                    if (listDetalleDocu != null)
                    {
                        foreach (DTO_prDetalleDocu det in listDetalleDocu)
                        {
                            List<DTO_prSolicitudCargos> cargosSol = null;
                            if (det.SolicitudDetaID.Value != null)
                                cargosSol = this._moduloProveedores.prSolicitudCargos_GetByConsecutivoDetaID(documentID, det.SolicitudDetaID.Value.Value);
                            else if (det.Detalle5ID.Value != null)
                                cargosSol = this._moduloProveedores.prSolicitudCargos_GetByConsecutivoDetaID(documentID, det.Detalle5ID.Value.Value);
                            else if (det.Detalle1ID.Value != null)
                                cargosSol = this._moduloProveedores.prSolicitudCargos_GetByConsecutivoDetaID(documentID, det.Detalle1ID.Value.Value);

                            foreach (DTO_prSolicitudCargos sol in cargosSol)
                            {
                                DTO_plCierreLegalizacion cierreLegal = new DTO_plCierreLegalizacion(true);
                                cierreLegal.PeriodoID.Value = periodoPlaneacion;
                                cierreLegal.ProyectoID.Value = sol.ProyectoID.Value;
                                cierreLegal.CentroCostoID.Value = sol.CentroCostoID.Value;
                                cierreLegal.LineaPresupuestoID.Value = sol.LineaPresupuestoID.Value;

                                #region Orden de Compra - Consumo Proyectos
                                if (documentID == AppDocuments.OrdenCompra || documentID == AppDocuments.ConsumoProyecto)
                                {
                                    if (isOrigenML)
                                    {
                                        cierreLegal.CompActLocML.Value = det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                        cierreLegal.CompActLocME.Value = docControl.TasaCambioDOCU.Value != 0 ?
                                                                         det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100) / docControl.TasaCambioDOCU.Value : 0;
                                    }
                                    else
                                    {
                                        cierreLegal.CompActExtME.Value = det.ValorTotME.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                        cierreLegal.CompActExtML.Value = det.ValorTotME.Value.Value * (sol.PorcentajeID.Value.Value / 100) * docControl.TasaCambioDOCU.Value;
                                    }
                                }
                                #endregion
                                #region Recibidos
                                else if (documentID == AppDocuments.Recibido)
                                {
                                    if (isOrigenML)
                                    {
                                        cierreLegal.RecibidoLocML.Value = det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                        cierreLegal.RecibidoLocME.Value = docControl.TasaCambioDOCU.Value != 0 ?
                                                                         det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100) / docControl.TasaCambioDOCU.Value : 0;

                                        if (det.Detalle5ID.Value == null) //Valida que no sea Solicitud Directa para hacer retiro
                                        {
                                            cierreLegal.CompActLocML.Value = cierreLegal.RecibidoLocML.Value * -1;
                                            cierreLegal.CompActLocME.Value = cierreLegal.RecibidoLocME.Value * -1;
                                        }
                                    }
                                    else
                                    {
                                        cierreLegal.RecibidoExtME.Value = det.ValorTotME.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                        cierreLegal.RecibidoExtML.Value = det.ValorTotME.Value.Value * (sol.PorcentajeID.Value.Value / 100) * docControl.TasaCambioDOCU.Value;

                                        if (det.Detalle5ID.Value == null) //Valida que no sea Solicitud Directa para hacer retiro
                                        {
                                            cierreLegal.CompActExtME.Value = cierreLegal.RecibidoExtML.Value * -1;
                                            cierreLegal.CompActExtML.Value = cierreLegal.RecibidoExtME.Value * -1;
                                        }

                                    }
                                }
                                #endregion
                                #region Causacion Facturas
                                else if (documentID == AppDocuments.CausarFacturas)
                                {
                                    if (isOrigenML)
                                    {
                                        cierreLegal.CtoOrigenLocML.Value = det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                        cierreLegal.CtoOrigenLocME.Value = docControl.TasaCambioDOCU.Value != 0 ?
                                                                         det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100) / docControl.TasaCambioDOCU.Value : 0;

                                        cierreLegal.RecibidoLocML.Value = cierreLegal.CtoOrigenLocML.Value * -1;
                                        cierreLegal.RecibidoLocME.Value = cierreLegal.CtoOrigenLocME.Value * -1;
                                    }
                                    else
                                    {
                                        cierreLegal.CtoOrigenExtME.Value = det.ValorTotME.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                        cierreLegal.CtoOrigenExtML.Value = det.ValorTotME.Value.Value * (sol.PorcentajeID.Value.Value / 100) * docControl.TasaCambioDOCU.Value;

                                        cierreLegal.RecibidoExtME.Value = cierreLegal.CtoOrigenExtME.Value * -1;
                                        cierreLegal.RecibidoExtML.Value = cierreLegal.CtoOrigenExtML.Value * -1;
                                    }
                                }
                                #endregion
                                this._dal_plCierreLegalizacion.DAL_plCierreLegalizacion_Add(cierreLegal);
                            }
                        }
                    }
                    else
                    {
                        #region Causacion Facturas
                        if (documentID == AppDocuments.CausarFacturas)
                        {
                            DTO_plCierreLegalizacion cierreLegal = new DTO_plCierreLegalizacion(true);
                            if (isOrigenML)
                            {
                                cierreLegal.CtoOrigenLocML.Value = docControl.Valor.Value;
                                cierreLegal.CtoOrigenLocME.Value = docControl.TasaCambioDOCU.Value != 0 ?
                                                                    docControl.Valor.Value / docControl.TasaCambioDOCU.Value : 0;

                            }
                            else
                            {
                                cierreLegal.CtoOrigenExtME.Value = docControl.Valor.Value;
                                cierreLegal.CtoOrigenExtML.Value = docControl.Valor.Value * docControl.TasaCambioDOCU.Value;
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region Presupuesto Planeacion
                    List<DTO_plPresupuestoDeta> listPresupDeta = this.plPresupuestoDeta_GetByNumeroDoc(numeroDoc);
                    if (listPresupDeta != null)
                    {
                        foreach (DTO_plPresupuestoDeta det in listPresupDeta)
                        {
                            DTO_plCierreLegalizacion cierreLegal = new DTO_plCierreLegalizacion(true);
                            cierreLegal.PeriodoID.Value = periodoPlaneacion;
                            cierreLegal.ProyectoID.Value = det.ProyectoID.Value;
                            cierreLegal.CentroCostoID.Value = det.CentroCostoID.Value;
                            cierreLegal.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                            if (isOrigenML)
                            {
                                cierreLegal.PtoMesLocML.Value = this.GetValueByPeriodo(det, docControl.PeriodoDoc.Value.Value.Month, isOrigenML, true);
                                cierreLegal.PtoMesLocME.Value = this.GetValueByPeriodo(det, docControl.PeriodoDoc.Value.Value.Month, isOrigenML, false);

                                cierreLegal.PtoTotalLocML.Value = Math.Round(det.ValorLoc01.Value.Value + det.ValorLoc02.Value.Value + det.ValorLoc03.Value.Value + det.ValorLoc04.Value.Value
                                                                 + det.ValorLoc05.Value.Value + det.ValorLoc06.Value.Value + det.ValorLoc07.Value.Value + det.ValorLoc08.Value.Value
                                                                 + det.ValorLoc09.Value.Value + det.ValorLoc10.Value.Value + det.ValorLoc11.Value.Value + det.ValorLoc12.Value.Value, 0);
                                cierreLegal.PtoTotalLocME.Value = Math.Round(det.EquivExt01.Value.Value + det.EquivExt02.Value.Value + det.EquivExt03.Value.Value + det.EquivExt04.Value.Value
                                                                 + det.EquivExt05.Value.Value + det.EquivExt06.Value.Value + det.EquivExt07.Value.Value + det.EquivExt08.Value.Value
                                                                 + det.EquivExt09.Value.Value + det.EquivExt10.Value.Value + det.EquivExt11.Value.Value + det.EquivExt12.Value.Value, 0);
                            }
                            else
                            {
                                cierreLegal.PtoMesExtME.Value = this.GetValueByPeriodo(det, docControl.PeriodoDoc.Value.Value.Month, isOrigenML, false);
                                cierreLegal.PtoMesExtML.Value = this.GetValueByPeriodo(det, docControl.PeriodoDoc.Value.Value.Month, isOrigenML, true);

                                cierreLegal.PtoTotalExtME.Value = Math.Round(det.ValorExt01.Value.Value + det.ValorExt02.Value.Value + det.ValorExt03.Value.Value + det.ValorExt04.Value.Value
                                                                + det.ValorExt05.Value.Value + det.ValorExt06.Value.Value + det.ValorExt07.Value.Value + det.ValorExt08.Value.Value
                                                                + det.ValorExt09.Value.Value + det.ValorExt10.Value.Value + det.ValorExt11.Value.Value + det.ValorExt12.Value.Value, 0);
                                cierreLegal.PtoTotalExtML.Value = Math.Round(det.EquivLoc01.Value.Value + det.EquivLoc02.Value.Value + det.EquivLoc03.Value.Value + det.EquivLoc04.Value.Value
                                                                + det.EquivLoc05.Value.Value + det.EquivLoc06.Value.Value + det.EquivLoc07.Value.Value + det.EquivLoc08.Value.Value
                                                                + det.EquivLoc09.Value.Value + det.EquivLoc10.Value.Value + det.EquivLoc11.Value.Value + det.EquivLoc12.Value.Value, 0);
                            }
                            this._dal_plCierreLegalizacion.DAL_plCierreLegalizacion_Add(cierreLegal);
                        }
                    }
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GeneraPresupuesto");
                return result;
            }
        }

        #endregion

        #region Presupuesto

        #region funciones Privadas

        /// <summary>
        /// Aprueba Un Presupuesto
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <returns></returns>
        private DTO_TxResult Presupuesto_Aprobar(int documentID, string actividadFlujoID, DTO_PresupuestoAprob presupuesto, DateTime periodoDoc, DateTime fechaDoc, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            bool validateSaldoMensual = true;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrlPrincipal = new DTO_glDocumentoControl();
            DTO_glDocumentoControl ctrlSecundario = null;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoTotal = (DAL_plPresupuestoTotal)base.GetInstance(typeof(DAL_plPresupuestoTotal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoPxQDeta = (DAL_plPresupuestoPxQDeta)base.GetInstance(typeof(DAL_plPresupuestoPxQDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoPxQ = (DAL_plPresupuestoPxQ)base.GetInstance(typeof(DAL_plPresupuestoPxQ), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables por defecto
                //string ctaRecDisp = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_CuentaRecursoDisp);
                string lugGeoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string concCargXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string linPresXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string concSaldoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);
                string mdaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExtranj = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                ctrlPrincipal = this._moduloGlobal.glDocumentoControl_GetByID(presupuesto.NumeroDoc.Value.Value);
                List<DTO_plPresupuestoDeta> presupuestoDeta = this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_Get(presupuesto.NumeroDoc.Value.Value);
                #region Carga y Guarda la info de Presupuesto Total
                foreach (DTO_plPresupuestoDeta plDeta in presupuestoDeta)
                {
                    DTO_plPresupuestoTotal total = new DTO_plPresupuestoTotal();
                    total.Ano.Value = periodoDoc.Year;
                    total.ProyectoID.Value = plDeta.ProyectoID.Value;
                    total.CentroCostoID.Value = plDeta.CentroCostoID.Value;
                    total.LineaPresupuestoID.Value = plDeta.LineaPresupuestoID.Value;
                    total.ValorLoc00.Value = plDeta.ValorLoc00.Value;
                    total.ValorLoc01.Value = plDeta.ValorLoc01.Value;
                    total.ValorLoc02.Value = plDeta.ValorLoc02.Value;
                    total.ValorLoc03.Value = plDeta.ValorLoc03.Value;
                    total.ValorLoc04.Value = plDeta.ValorLoc04.Value;
                    total.ValorLoc05.Value = plDeta.ValorLoc05.Value;
                    total.ValorLoc06.Value = plDeta.ValorLoc06.Value;
                    total.ValorLoc07.Value = plDeta.ValorLoc07.Value;
                    total.ValorLoc08.Value = plDeta.ValorLoc08.Value;
                    total.ValorLoc09.Value = plDeta.ValorLoc09.Value;
                    total.ValorLoc10.Value = plDeta.ValorLoc10.Value;
                    total.ValorLoc11.Value = plDeta.ValorLoc11.Value;
                    total.ValorLoc12.Value = plDeta.ValorLoc12.Value;
                    total.EquivExt00.Value = plDeta.EquivExt00.Value;
                    total.EquivExt01.Value = plDeta.EquivExt01.Value;
                    total.EquivExt02.Value = plDeta.EquivExt02.Value;
                    total.EquivExt03.Value = plDeta.EquivExt03.Value;
                    total.EquivExt04.Value = plDeta.EquivExt04.Value;
                    total.EquivExt05.Value = plDeta.EquivExt05.Value;
                    total.EquivExt06.Value = plDeta.EquivExt06.Value;
                    total.EquivExt07.Value = plDeta.EquivExt07.Value;
                    total.EquivExt08.Value = plDeta.EquivExt08.Value;
                    total.EquivExt09.Value = plDeta.EquivExt09.Value;
                    total.EquivExt10.Value = plDeta.EquivExt10.Value;
                    total.EquivExt11.Value = plDeta.EquivExt11.Value;
                    total.EquivExt11.Value = plDeta.EquivExt11.Value;
                    total.EquivExt12.Value = plDeta.EquivExt11.Value;
                    total.ValorExt00.Value = plDeta.ValorExt00.Value;
                    total.ValorExt01.Value = plDeta.ValorExt01.Value;
                    total.ValorExt02.Value = plDeta.ValorExt02.Value;
                    total.ValorExt03.Value = plDeta.ValorExt03.Value;
                    total.ValorExt04.Value = plDeta.ValorExt04.Value;
                    total.ValorExt05.Value = plDeta.ValorExt05.Value;
                    total.ValorExt06.Value = plDeta.ValorExt06.Value;
                    total.ValorExt07.Value = plDeta.ValorExt07.Value;
                    total.ValorExt08.Value = plDeta.ValorExt08.Value;
                    total.ValorExt09.Value = plDeta.ValorExt09.Value;
                    total.ValorExt10.Value = plDeta.ValorExt10.Value;
                    total.ValorExt11.Value = plDeta.ValorExt11.Value;
                    total.ValorExt12.Value = plDeta.ValorExt12.Value;
                    total.EquivLoc00.Value = plDeta.EquivLoc00.Value;
                    total.EquivLoc01.Value = plDeta.EquivLoc01.Value;
                    total.EquivLoc02.Value = plDeta.EquivLoc02.Value;
                    total.EquivLoc03.Value = plDeta.EquivLoc03.Value;
                    total.EquivLoc04.Value = plDeta.EquivLoc04.Value;
                    total.EquivLoc05.Value = plDeta.EquivLoc05.Value;
                    total.EquivLoc06.Value = plDeta.EquivLoc06.Value;
                    total.EquivLoc07.Value = plDeta.EquivLoc07.Value;
                    total.EquivLoc08.Value = plDeta.EquivLoc08.Value;
                    total.EquivLoc09.Value = plDeta.EquivLoc09.Value;
                    total.EquivLoc10.Value = plDeta.EquivLoc10.Value;
                    total.EquivLoc11.Value = plDeta.EquivLoc11.Value;
                    total.EquivLoc12.Value = plDeta.EquivLoc12.Value;
                    this._dal_plPresupuestoTotal.DAL_plPresupuestoTotal_Add(total, ref validateSaldoMensual);
                    if (!validateSaldoMensual)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Pl_SaldoMensualNotAvailable + "&&" + total.CentroCostoID.Value + "&&" + total.LineaPresupuestoID.Value;
                        return result;
                    }
                }
                #endregion
                #region Carga y Guarda la info de PresupuestoPxQ
                List<DTO_plPresupuestoPxQDeta> presupuestoPxQDeta = this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_Get(presupuesto.NumeroDoc.Value.Value);
                presupuestoPxQDeta = presupuestoPxQDeta.FindAll(x => x.CantidadPRELoc.Value != 0 || x.CantidadPREExt.Value != 0);
                foreach (DTO_plPresupuestoPxQDeta plDeta in presupuestoPxQDeta)
                {
                    DTO_plPresupuestoPxQ pxq = new DTO_plPresupuestoPxQ();
                    pxq.Ano.Value = periodoDoc.Year;
                    pxq.ProyectoID.Value = plDeta.ProyectoID.Value;
                    pxq.CentroCostoID.Value = plDeta.CentroCostoID.Value;
                    pxq.LineaPresupuestoID.Value = plDeta.LineaPresupuestoID.Value;
                    pxq.CodigoBSID.Value = plDeta.CodigoBSID.Value;
                    pxq.inReferenciaID.Value = plDeta.inReferenciaID.Value;
                    pxq.UnidadInvID.Value = plDeta.UnidadInvID.Value;
                    pxq.CantidadPRELoc.Value = plDeta.CantidadPRELoc.Value;
                    pxq.CantidadPREExt.Value = plDeta.CantidadPREExt.Value;
                    pxq.ValorUniLoc.Value = plDeta.ValorUniLoc.Value;
                    pxq.ValorUniExt.Value = plDeta.ValorUniExt.Value;
                    pxq.CantidadPEN.Value = plDeta.CantidadPEN.Value;
                    pxq.CantidadSOL.Value = plDeta.CantidadSOL.Value;
                    this._dal_plPresupuestoPxQ.DAL_plPresupuestoPxQ_Add(pxq);
                    if (!validateSaldoMensual)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Pl_SaldoMensualNotAvailable + "&&" + pxq.CentroCostoID.Value + "&&" + pxq.LineaPresupuestoID.Value;
                        return result;
                    }
                }
                #endregion
                #region Elimina info innecesaria de plPresupuestoPxQDeta 
                this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_DeleteByCant(presupuesto.NumeroDoc.Value.Value);
                #endregion
                #region Guarda el Presupuesto en plCierreLegalizacion
                result = this.GeneraPresupuesto(ctrlPrincipal.DocumentoID.Value.Value, presupuesto.NumeroDoc.Value.Value);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Aprueba el documento y asigna el flujo
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ctrlPrincipal.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                if (ctrlSecundario != null)
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ctrlSecundario.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                result = this.AsignarFlujo(documentID, ctrlPrincipal.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_Aprobar");
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

                        //ctrlPrincipal.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrlPrincipal.PrefijoID.Value, ctrlPrincipal.PeriodoDoc.Value.Value, ctrlPrincipal.DocumentoNro.Value.Value);
                        //this._moduloGlobal.ActualizaConsecutivos(ctrlPrincipal, false, true, false);
                        //this._moduloContabilidad.ActualizaComprobanteNro(ctrlPrincipal.NumeroDoc.Value.Value, ctrlPrincipal.ComprobanteIDNro.Value.Value, false);
                        //if (ctrlSecundario != null)
                        //{
                        //    ctrlSecundario.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrlSecundario.PrefijoID.Value, ctrlSecundario.PeriodoDoc.Value.Value, ctrlSecundario.DocumentoNro.Value.Value);
                        //    this._moduloGlobal.ActualizaConsecutivos(ctrlSecundario, false, true, false);
                        //    this._moduloContabilidad.ActualizaComprobanteNro(ctrlSecundario.NumeroDoc.Value.Value, ctrlSecundario.ComprobanteIDNro.Value.Value, false);
                        //}
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoDeta> plPresupuestoDeta_GetByNumeroDoc(int numeroDoc)
        {
            try
            {
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_plPresupuestoDeta> presupuestoDeta = this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_Get(numeroDoc);
                return presupuestoDeta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plPresupuestoDeta_GetByNumeroDoc");
                return null;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Carga la información de un nuevo presupuesto
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta el proceso</param>
        /// <param name="actividadFlujoID">Actuvidad de flujo relacionada</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="presupuesto">Presupuesto que se va a generar</param>
        /// <param name="isAnotherTx">Revisa si se esta ejecutando en otar transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject Presupuesto_Add(int documentoID, DateTime periodoPresupuesto, string proyectoID, decimal tc, DTO_Presupuesto presupuesto, bool onlySave, bool isAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            DTO_Alarma alarma = null;
            try
            {
                #region Dals y Modulos
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoDocu = (DAL_plPresupuestoDocu)base.GetInstance(typeof(DAL_plPresupuestoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion

                ctrl = presupuesto.DocCtrl;
                if (ctrl == null)
                {
                    #region Variables
                    //Variables de operacion
                    bool isMultimoneda = this.Multimoneda();
                    string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    //Variables por defecto
                    string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                    string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                    string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    string terceroDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    string ctoCostoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                    //Variables para el documento
                    //string coDocID = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_DocumentoContablePresupuesto);
                    string af = this.GetAreaFuncionalByUser();
                    DTO_coDocumento coDoc = new DTO_coDocumento();

                    //Periodo documento
                    string periodoStr = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.co_Periodo);
                    DateTime periodoDoc = Convert.ToDateTime(periodoStr);
                    decimal valorMdaLocal = presupuesto.Detalles.Sum(x => x.VlrMvtoLocal.Value.Value);
                    decimal valorMdaExtr = presupuesto.Detalles.Sum(x => x.VlrMvtoExtr.Value.Value);

                    #endregion
                    #region Validaciones

                    //Valida el coDocumento
                    //if (string.IsNullOrWhiteSpace(coDocID))
                    //{
                    //    result.Result = ResultValue.NOK;
                    //    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.pl).ToString() + AppControl.pl_DocumentoContablePresupuesto + "&&" + string.Empty;
                    //    return result;
                    //}
                    //else
                    //    coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocID, true, false);

                    ////Valida que tenga comprobante
                    //if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                    //{
                    //    result.Result = ResultValue.NOK;
                    //    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;

                    //    return result;
                    //}

                    ////Valida que el documento asociado tenga cuenta local
                    //if (string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                    //{
                    //    result.Result = ResultValue.NOK;
                    //    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDocID;

                    //    return result;
                    //}
                    ////Valida que el documento asociado tenga cuenta local
                    //if (this.Multimoneda() && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value))
                    //{
                    //    result.Result = ResultValue.NOK;
                    //    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDocID;

                    //    return result;
                    //}

                    #endregion
                    #region Carga y Guarda glDocumentoControl

                    ctrl = new DTO_glDocumentoControl();
                    ctrl.DocumentoNro.Value = 0;
                    ctrl.DocumentoID.Value = documentoID;
                    ctrl.NumeroDoc.Value = 0;
                    ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                    //ctrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                    ctrl.Fecha.Value = DateTime.Now;
                    ctrl.FechaDoc.Value = DateTime.Now;
                    ctrl.PeriodoDoc.Value = periodoPresupuesto;
                    ctrl.PeriodoUltMov.Value = periodoPresupuesto;
                    ctrl.AreaFuncionalID.Value = af;
                    ctrl.PrefijoID.Value = prefijoDef;
                    ctrl.ProyectoID.Value = proyectoID;
                    ctrl.CentroCostoID.Value = ctoCostoDef;
                    ctrl.LugarGeograficoID.Value = lugGeoDef;
                    ctrl.LineaPresupuestoID.Value = linPresDef;
                    ctrl.TerceroID.Value = terceroDef;
                    if (documentoID == AppDocuments.Presupuesto || documentoID == AppDocuments.AdicionPresupuesto)
                    {
                        // ctrl.CuentaID.Value = valorMdaLocal != 0 ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                        ctrl.MonedaID.Value = valorMdaLocal != 0 ? mdaLoc : mdaExt;
                        ctrl.Valor.Value = valorMdaLocal != 0 ? valorMdaLocal : valorMdaExtr;
                    }
                    else
                    {
                        bool isValid = false;
                        foreach (var item in presupuesto.Detalles)
                        {
                            if (item.VlrMvtoLocal.Value != 0 || item.ValorLoc00.Value != 0 || item.ValorLoc01.Value != 0 || item.ValorLoc02.Value != 0 ||
                                item.ValorLoc03.Value != 0 || item.ValorLoc04.Value != 0 || item.ValorLoc05.Value != 0 || item.ValorLoc06.Value != 0 ||
                                item.ValorLoc07.Value != 0 || item.ValorLoc08.Value != 0 || item.ValorLoc09.Value != 0 || item.ValorLoc10.Value != 0 ||
                                item.ValorLoc11.Value != 0 || item.ValorLoc12.Value != 0)
                            {
                                isValid = true;
                                break;
                            }
                        }
                        //ctrl.CuentaID.Value = isValid ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                        ctrl.MonedaID.Value = isValid ? mdaLoc : mdaExt;
                        ctrl.Valor.Value = isValid ? valorMdaLocal : valorMdaExtr;
                    }
                    ctrl.TasaCambioCONT.Value = tc;
                    ctrl.TasaCambioDOCU.Value = tc;
                    if (documentoID == AppDocuments.Presupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Nuevo";
                    if (documentoID == AppDocuments.AdicionPresupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Adicion";
                    if (documentoID == AppDocuments.ReclasifPresupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Reclasificacion";
                    if (documentoID == AppDocuments.TrasladoPresupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Traslado";
                    ctrl.Estado.Value = onlySave ? (byte)EstadoDocControl.SinAprobar : (byte)EstadoDocControl.ParaAprobacion;
                    ctrl.seUsuarioID.Value = this.UserId;
                    ctrl.Iva.Value = 0;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }
                    ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    #endregion
                    #region Guarda en plPresupuestoDocu
                    DTO_plPresupuestoDocu docu = new DTO_plPresupuestoDocu();
                    docu.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    if (documentoID == AppDocuments.Presupuesto)
                        docu.NumeroDocPresup.Value = ctrl.NumeroDoc.Value;
                    else
                        docu.NumeroDocPresup.Value = presupuesto.NumeroDocPresup.Value;
                    this._dal_plPresupuestoDocu.DAL_plPresupuestoDocu_Add(docu);
                    #endregion
                    #region Guarda detalle (plPresupuestoDeta)
                    foreach (DTO_plPresupuestoDeta det in presupuesto.Detalles)
                    {
                        #region Valida los valores
                        bool isValid = true;
                        isValid = det.VlrMvtoLocal.Value != 0 || det.VlrMvtoExtr.Value != 0 ? true : false;
                        if (!isValid)
                        {
                            isValid = det.ValorLoc00.Value != 0 || det.ValorExt00.Value != 0 ? true : false;
                            if (!isValid)
                            {
                                isValid = det.ValorLoc01.Value != 0 || det.ValorExt01.Value != 0 ? true : false;
                                if (!isValid)
                                {
                                    isValid = det.ValorLoc02.Value != 0 || det.ValorExt02.Value != 0 ? true : false;
                                    if (!isValid)
                                    {
                                        isValid = det.ValorLoc03.Value != 0 || det.ValorExt03.Value != 0 ? true : false;
                                        if (!isValid)
                                        {
                                            isValid = det.ValorLoc04.Value != 0 || det.ValorExt04.Value != 0 ? true : false;
                                            if (!isValid)
                                            {
                                                isValid = det.ValorLoc05.Value != 0 || det.ValorExt05.Value != 0 ? true : false;
                                                if (!isValid)
                                                {
                                                    isValid = det.ValorLoc06.Value != 0 || det.ValorExt06.Value != 0 ? true : false;
                                                    if (!isValid)
                                                    {
                                                        isValid = det.ValorLoc07.Value != 0 || det.ValorExt07.Value != 0 ? true : false;
                                                        if (!isValid)
                                                        {
                                                            isValid = det.ValorLoc08.Value != 0 || det.ValorExt08.Value != 0 ? true : false;
                                                            if (!isValid)
                                                            {
                                                                isValid = det.ValorLoc09.Value != 0 || det.ValorExt09.Value != 0 ? true : false;
                                                                if (!isValid)
                                                                {
                                                                    isValid = det.ValorLoc10.Value != 0 || det.ValorExt10.Value != 0 ? true : false;
                                                                    if (!isValid)
                                                                    {
                                                                        isValid = det.ValorLoc11.Value != 0 || det.ValorExt11.Value != 0 ? true : false;
                                                                        if (!isValid)
                                                                            isValid = det.ValorLoc12.Value != 0 || det.ValorExt12.Value != 0 ? true : false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        if (isValid)
                        {
                            det.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                            this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_AddItem(det);
                        }
                    }
                    #endregion
                }
                else
                {
                    int documentUpdate = documentoID;
                    if (documentoID == AppDocuments.Presupuesto)
                        documentUpdate = AppDocuments.PresupuestoPxQ;
                    else if (documentoID == AppDocuments.AdicionPresupuesto)
                        documentUpdate = AppDocuments.AdicionPresupuestoPxQ;
                    else if (documentoID == AppDocuments.ReclasifPresupuesto)
                        documentUpdate = AppDocuments.ReclasifPresupuestoPxQ;
                    else if (documentoID == AppDocuments.TrasladoPresupuesto)
                        documentUpdate = AppDocuments.TrasladoPresupuestoPxQ;

                    #region Actualiza docControl
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentoID, ctrl.NumeroDoc.Value.Value, (onlySave ? EstadoDocControl.SinAprobar : EstadoDocControl.ParaAprobacion), ctrl.Observacion.Value, true);
                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentUpdate);
                    if (act.Count > 0)
                        this.AsignarFlujo(AppDocuments.PresupuestoPxQ, ctrl.NumeroDoc.Value.Value, act[0], false, string.Empty);
                    #endregion
                    #region Actualiza la info del presupuesto
                    this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_Delete(ctrl.NumeroDoc.Value.Value);
                    foreach (DTO_plPresupuestoDeta det in presupuesto.Detalles)
                    {
                        det.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                        this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_AddItem(det);
                    }
                    #endregion
                }

                #region Asigna las alarmas
                alarma = this.GetFirstMailInfo(ctrl.NumeroDoc.Value.Value, false);
                alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                return alarma;
                #endregion

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!isAnotherTx)
                    {
                        #region Genera el consecutivo
                        base._mySqlConnectionTx.Commit();

                        if (ctrl.DocumentoNro.Value == 0)
                        {
                            base._mySqlConnectionTx = null;
                            this._moduloGlobal._mySqlConnectionTx = null;

                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentoID, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, false);
                            alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !isAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        /// <summary>
        /// Obtiene los Indicadores
        /// </summary>
        /// Fecha corte >> Fecha de corte 
        /// <returns>Documentos</returns>
        public List<DTO_QueryIndicadores> plIndicadores(DateTime fechaCorte)
        {
            try
            {


                List<DTO_QueryIndicadores> result = new List<DTO_QueryIndicadores>();
                this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_Indicadores(fechaCorte);

                foreach (var item in result)
                {

                    switch (item.PresentaTipo.Value)
                    {
                        case "P":
                            item.Dato.Value = item.Indicador.Value.Value.ToString("n2");
                            break;
                        case "D":
                            item.Dato.Value = item.Factor.Value.Value.ToString("n2");//+" "+item.cUnidad.Value.TrimEnd();
                            break;
                        case "E":
                            item.Dato.Value = item.Valor.Value.Value.ToString("c0");
                            break;
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoDocu_GetAllProyectos");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el detalle de los proyectos
        /// </summary>
        /// Fecha corte >> Fecha de corte 
        /// <returns>Documentos</returns>
        public List<DTO_QueryEjecucionPresupuestal> plEjecucionPresupuestal(DateTime fechaCorte)
        {
            try
            {
                List<DTO_QueryEjecucionPresupuestal> result = new List<DTO_QueryEjecucionPresupuestal>();
                this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_EjecucionPresupuestal(fechaCorte);

                foreach (DTO_QueryEjecucionPresupuestal det in result)
                {
                    det.IngPresupuesto.Value = det.Detalle.Sum(x => x.IngPresupuesto.Value);
                    det.IngEjecucion.Value = det.Detalle.Sum(x => x.IngEjecucion.Value);
                    det.IngxEjecutar.Value = det.Detalle.Sum(x => x.IngxEjecutar.Value);
                    det.CostoPresupuesto.Value = det.Detalle.Sum(x => x.CostoPresupuesto.Value);
                    det.CostoPresAjustado.Value = det.Detalle.Sum(x => x.CostoPresAjustado.Value);
                    det.CostoEjecucion.Value = det.Detalle.Sum(x => x.CostoEjecucion.Value);
                    det.CostoxEjecutar.Value = det.Detalle.Sum(x => x.CostoxEjecutar.Value);
                    //det.RentaPresupuesto.Value = det.Detalle.Sum(x => x.RentaPresupuesto.Value);
                    if( det.CostoPresupuesto.Value  == 0)
                        det.RentaPresupuesto.Value=999;
                    else
                        det.RentaPresupuesto.Value=Math.Round(100 * (Convert.ToDecimal(det.IngPresupuesto.Value) - Convert.ToDecimal(det.CostoPresupuesto.Value))/Convert.ToDecimal(det.CostoPresupuesto.Value),2);

                    //det.RentaPresAjustado.Value = det.Detalle.Sum(x => x.RentaPresAjustado.Value);
                    if (det.CostoPresAjustado.Value == 0)
                        det.RentaPresAjustado.Value = 999;
                    else
                        det.RentaPresAjustado.Value = Math.Round(100 * (Convert.ToDecimal(det.IngPresupuesto.Value) - Convert.ToDecimal(det.CostoPresAjustado.Value)) / Convert.ToDecimal(det.CostoPresAjustado.Value), 2);

                    //det.RentaEjecucion.Value = det.Detalle.Sum(x => x.RentaEjecucion.Value);
                    if (det.CostoEjecucion.Value == 0)
                        det.RentaEjecucion.Value = 999;
                    else
                        det.RentaEjecucion.Value = Math.Round(100 * (Convert.ToDecimal(det.IngEjecucion.Value) - Convert.ToDecimal(det.CostoEjecucion.Value)) / Convert.ToDecimal(det.CostoEjecucion.Value), 2);


                    //det.FactINGxEjecutar.Value = det.Detalle.Sum(x => x.FactINGxEjecutar.Value);
                    if (det.IngPresupuesto.Value == 0)
                        det.FactINGxEjecutar.Value = 999;
                    else
                        det.FactINGxEjecutar.Value = Math.Round(100 * Convert.ToDecimal(det.IngEjecucion.Value)  / Convert.ToDecimal(det.IngPresupuesto.Value), 2);

                    //det.FactCTOxEjecutar.Value = det.Detalle.Sum(x => x.FactCTOxEjecutar.Value);

                    if (det.CostoPresAjustado.Value == 0)
                        det.FactCTOxEjecutar.Value = 999;
                    else
                        det.FactCTOxEjecutar.Value = Math.Round(100 * Convert.ToDecimal(det.CostoEjecucion.Value)/ Convert.ToDecimal(det.CostoPresAjustado.Value), 2);                    
                }
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "pyProyectoDocu_GetAllProyectos");
                throw exception;
            }
        }


        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Presupuesto consolidado</returns>
        public DTO_Presupuesto Presupuesto_GetConsolidadoTotal(int documentID, string proyecto, DateTime periodo, byte proyectoTipo, string contratoID, string actividadID, string areaFisica)
        {
            try
            {
                #region Variables
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_Presupuesto presupuesto = new DTO_Presupuesto();
                List<DTO_plPresupuestoDeta> presupuestoDet = new List<DTO_plPresupuestoDeta>();
                DTO_plPresupuestoTotal total = new DTO_plPresupuestoTotal();
                DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                Dictionary<string, DTO_glLocFisica> cacheLocFisicas = new Dictionary<string, DTO_glLocFisica>();
                DTO_glLocFisica _locFisica;
                DTO_glDocumentoControl docControl = null;
                #endregion
                #region Obtiene el documento Presupuesto
                filter.ProyectoID.Value = proyecto;
                filter.PeriodoDoc.Value = periodo;
                filter.DocumentoID.Value = AppDocuments.Presupuesto;
                var docsPresup = this._moduloGlobal.glDocumentoControl_GetByParameter(filter).ToList();
                #endregion
                #region Valida Tipo de Proyecto y Contrato
                if (proyectoTipo == (byte)ProyectoTipo.Opex || proyectoTipo == (byte)ProyectoTipo.Administracion)
                {
                    //Valida que el doc obtenido sea con proyecto tipo Opex o Admin
                    foreach (DTO_glDocumentoControl doc in docsPresup)
                    {
                        DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, doc.ProyectoID.Value, true, false);
                        DTO_coActividad actividad = (DTO_coActividad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, proy.ActividadID.Value, true, false);
                        if ((actividad.ProyectoTipo.Value == (byte)ProyectoTipo.Opex || actividad.ProyectoTipo.Value == (byte)ProyectoTipo.Administracion)
                            && proy.ContratoID.Value == contratoID)
                        {
                            docControl = doc;
                            break;
                        }
                    }
                }
                else
                    if (docsPresup.Count > 0) docControl = docsPresup.First();

                if (docControl == null) return null;
                #endregion
                #region Obtiene el detalle del Doc
                List<DTO_plPresupuestoDeta> detalleInicialPres = this.plPresupuestoDeta_GetByNumeroDoc(docControl.NumeroDoc.Value.Value);
                if (docControl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                {
                    presupuesto.Detalles = detalleInicialPres;
                    foreach (var deta in presupuesto.Detalles)
                    {
                        #region Carga LocFisica
                        if (cacheLocFisicas.ContainsKey(deta.LocFisicaID.Value))
                            _locFisica = cacheLocFisicas[deta.LocFisicaID.Value];
                        else
                        {
                            _locFisica = (DTO_glLocFisica)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, deta.LocFisicaID.Value, true, false);
                            cacheLocFisicas.Add(deta.LocFisicaID.Value, _locFisica);
                        }
                        #endregion
                        if (_locFisica != null)
                            deta.AreaFisicaID.Value = _locFisica.AreaFisica.Value;
                    }
                    #region Valida la actividad y AreaFisica(Campo) en el detalle
                    if (!string.IsNullOrEmpty(actividadID))
                        presupuesto.Detalles = presupuesto.Detalles.Where(x => x.ActividadID.Value == actividadID).ToList();
                    if (!string.IsNullOrEmpty(areaFisica))
                        presupuesto.Detalles = presupuesto.Detalles.Where(x => x.AreaFisicaID.Value == areaFisica).ToList();
                    #endregion
                    presupuesto.DocCtrl = docControl;
                    return presupuesto;
                }
                #endregion
                #region Obtiene el detalle Consolidado
                total.ProyectoID.Value = proyecto;
                total.Ano.Value = periodo.Year;
                List<DTO_plPresupuestoTotal> presupuestoTotal = this.plPresupuestoTotal_GetByParameter(total);
                foreach (DTO_plPresupuestoTotal tot in presupuestoTotal)
                {
                    DTO_plPresupuestoDeta deta = new DTO_plPresupuestoDeta(true);
                    DTO_plPresupuestoDeta detaPorcent = detalleInicialPres.Find(x => x.ProyectoID.Value == tot.ProyectoID.Value && x.CentroCostoID.Value == tot.CentroCostoID.Value && x.LineaPresupuestoID.Value == tot.LineaPresupuestoID.Value);
                    deta.ProyectoID.Value = tot.ProyectoID.Value;
                    #region Carga LocFisica
                    if (cacheLocFisicas.ContainsKey(deta.LocFisicaID.Value))
                        _locFisica = cacheLocFisicas[deta.LocFisicaID.Value];
                    else
                    {
                        _locFisica = (DTO_glLocFisica)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, deta.LocFisicaID.Value, true, false);
                        cacheLocFisicas.Add(deta.LocFisicaID.Value, _locFisica);
                    }

                    #endregion
                    if (_locFisica != null)
                        deta.AreaFisicaID.Value = _locFisica.AreaFisica.Value;
                    deta.CentroCostoID.Value = tot.CentroCostoID.Value;
                    deta.LineaPresupuestoID.Value = tot.LineaPresupuestoID.Value;
                    deta.ValorLoc00.Value = tot.ValorLoc00.Value;
                    deta.ValorLoc01.Value = tot.ValorLoc01.Value;
                    deta.ValorLoc02.Value = tot.ValorLoc02.Value;
                    deta.ValorLoc03.Value = tot.ValorLoc03.Value;
                    deta.ValorLoc04.Value = tot.ValorLoc04.Value;
                    deta.ValorLoc05.Value = tot.ValorLoc05.Value;
                    deta.ValorLoc06.Value = tot.ValorLoc06.Value;
                    deta.ValorLoc07.Value = tot.ValorLoc07.Value;
                    deta.ValorLoc08.Value = tot.ValorLoc08.Value;
                    deta.ValorLoc09.Value = tot.ValorLoc09.Value;
                    deta.ValorLoc10.Value = tot.ValorLoc10.Value;
                    deta.ValorLoc11.Value = tot.ValorLoc11.Value;
                    deta.ValorLoc12.Value = tot.ValorLoc12.Value;
                    deta.EquivExt00.Value = tot.EquivExt00.Value;
                    deta.EquivExt01.Value = tot.EquivExt01.Value;
                    deta.EquivExt02.Value = tot.EquivExt02.Value;
                    deta.EquivExt03.Value = tot.EquivExt03.Value;
                    deta.EquivExt04.Value = tot.EquivExt04.Value;
                    deta.EquivExt05.Value = tot.EquivExt05.Value;
                    deta.EquivExt06.Value = tot.EquivExt06.Value;
                    deta.EquivExt07.Value = tot.EquivExt07.Value;
                    deta.EquivExt08.Value = tot.EquivExt08.Value;
                    deta.EquivExt09.Value = tot.EquivExt09.Value;
                    deta.EquivExt10.Value = tot.EquivExt10.Value;
                    deta.EquivExt11.Value = tot.EquivExt11.Value;
                    deta.EquivExt12.Value = tot.EquivExt12.Value;
                    deta.ValorExt00.Value = tot.ValorExt00.Value;
                    deta.ValorExt01.Value = tot.ValorExt01.Value;
                    deta.ValorExt02.Value = tot.ValorExt02.Value;
                    deta.ValorExt03.Value = tot.ValorExt03.Value;
                    deta.ValorExt04.Value = tot.ValorExt04.Value;
                    deta.ValorExt05.Value = tot.ValorExt05.Value;
                    deta.ValorExt06.Value = tot.ValorExt06.Value;
                    deta.ValorExt07.Value = tot.ValorExt07.Value;
                    deta.ValorExt08.Value = tot.ValorExt08.Value;
                    deta.ValorExt09.Value = tot.ValorExt09.Value;
                    deta.ValorExt10.Value = tot.ValorExt10.Value;
                    deta.ValorExt11.Value = tot.ValorExt11.Value;
                    deta.ValorExt12.Value = tot.ValorExt12.Value;
                    deta.EquivLoc00.Value = tot.EquivLoc00.Value;
                    deta.EquivLoc01.Value = tot.EquivLoc01.Value;
                    deta.EquivLoc02.Value = tot.EquivLoc02.Value;
                    deta.EquivLoc03.Value = tot.EquivLoc03.Value;
                    deta.EquivLoc04.Value = tot.EquivLoc04.Value;
                    deta.EquivLoc05.Value = tot.EquivLoc05.Value;
                    deta.EquivLoc06.Value = tot.EquivLoc06.Value;
                    deta.EquivLoc07.Value = tot.EquivLoc07.Value;
                    deta.EquivLoc08.Value = tot.EquivLoc08.Value;
                    deta.EquivLoc09.Value = tot.EquivLoc09.Value;
                    deta.EquivLoc10.Value = tot.EquivLoc10.Value;
                    deta.EquivLoc11.Value = tot.EquivLoc11.Value;
                    deta.EquivLoc12.Value = tot.EquivLoc12.Value;
                    deta.VlrSaldoAntLoc.Value = tot.VlrSaldoAntLoc.Value;
                    deta.VlrSaldoAntExtr.Value = tot.VlrSaldoAntExtr.Value;
                    deta.VlrNuevoSaldoLoc.Value = tot.VlrNuevoSaldoLoc.Value;
                    deta.VlrNuevoSaldoExtr.Value = tot.VlrNuevoSaldoExtr.Value;
                    if (detaPorcent != null)
                    {
                        //Obtiene los porcentajes de distribucion mensual
                        deta.Porcentaje01.Value = detaPorcent.Porcentaje01.Value;
                        deta.Porcentaje02.Value = detaPorcent.Porcentaje02.Value;
                        deta.Porcentaje03.Value = detaPorcent.Porcentaje03.Value;
                        deta.Porcentaje04.Value = detaPorcent.Porcentaje04.Value;
                        deta.Porcentaje05.Value = detaPorcent.Porcentaje05.Value;
                        deta.Porcentaje06.Value = detaPorcent.Porcentaje06.Value;
                        deta.Porcentaje07.Value = detaPorcent.Porcentaje07.Value;
                        deta.Porcentaje08.Value = detaPorcent.Porcentaje08.Value;
                        deta.Porcentaje09.Value = detaPorcent.Porcentaje09.Value;
                        deta.Porcentaje10.Value = detaPorcent.Porcentaje10.Value;
                        deta.Porcentaje11.Value = detaPorcent.Porcentaje11.Value;
                        deta.Porcentaje12.Value = detaPorcent.Porcentaje12.Value;
                    }
                    presupuestoDet.Add(deta);
                }
                #endregion

                presupuesto.Detalles = presupuestoDet;
                #region Valida la actividad y AreaFisica(Campo) en el detalle
                if (!string.IsNullOrEmpty(actividadID))
                    presupuesto.Detalles = presupuesto.Detalles.Where(x => x.ActividadID.Value == actividadID).ToList();
                if (!string.IsNullOrEmpty(areaFisica))
                    presupuesto.Detalles = presupuesto.Detalles.Where(x => x.AreaFisicaID.Value == areaFisica).ToList();
                #endregion
                presupuesto.DocCtrl = docControl;

                return presupuesto;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_GetConsolidado");
                return null;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoTotal> plPresupuestoTotal_GetByParameter(DTO_plPresupuestoTotal filter)
        {
            try
            {
                this._dal_plPresupuestoTotal = (DAL_plPresupuestoTotal)base.GetInstance(typeof(DAL_plPresupuestoTotal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_plPresupuestoTotal> presupuestoTotal = this._dal_plPresupuestoTotal.DAL_plPresupuestoTotal_GetByParameter(filter);
                return presupuestoTotal;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plPresupuestoTotal_GetByParameter");
                return null;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos de un usuario por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public DTO_Presupuesto Presupuesto_GetNuevo(int documentoID, string proyectoID, DateTime periodoID, bool orderByAsc = true)
        {
            try
            {
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_Presupuesto presupuesto = this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_GetNuevo(documentoID, proyectoID, periodoID, orderByAsc);

                return presupuesto;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_GetNuevos");
                return null;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public List<DTO_PresupuestoAprob> Presupuesto_GetNuevosForAprob(int documentoID, string actividadFlujo)
        {
            List<DTO_PresupuestoAprob> docs = new List<DTO_PresupuestoAprob>();
            try
            {
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                string usuarioID = seUsuario.ID.Value;
                docs = this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_GetNuevosForAprob(documentoID, actividadFlujo, usuarioID);
                foreach (var item in docs)
                {
                    item.TotalML.Value += item.Detalle.Sum(x => Math.Round(x.ValorML.Value.Value));
                    item.TotalME.Value += item.Detalle.Sum(x => Math.Round(x.ValorME.Value.Value));
                }
                return docs;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_GetNuevosForAprob");
                return docs;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <param name="areaPresupuestalID">Area presupuestal</param>
        /// <param name="centroCostoID">Centro de costo</param>
        /// <returns></returns>
        public List<DTO_PresupuestoDetalle> Presupuesto_GetNuevosForAprobDetails(string proyectoID, DateTime periodoID, string areaPresupuestalID, string centroCostoID)
        {
            List<DTO_PresupuestoDetalle> docs = new List<DTO_PresupuestoDetalle>();
            try
            {
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return null;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_GetNuevosForAprobDetails");
                return docs;
            }
        }

        /// <summary>
        /// Aprueba una lista de presupuestos
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="proyectoID">Identificador del proyecto</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <param name="areasIDs">Lista de areas para aprobar</param>
        /// <returns>Retorna una lista de resultados o  alarmas</returns>
        public List<DTO_SerializedObject> Presupuesto_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_PresupuestoAprob> docs, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                int i = 0;
                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100;

                foreach (DTO_PresupuestoAprob item in docs)
                {
                    #region Variables
                    porcTemp = (porcParte * i) / docs.Count;
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

                    DateTime periodoDoc = item.PeriodoDoc.Value.Value;
                    DateTime fechaDoc = DateTime.Now;
                    if (fechaDoc.Year != periodoDoc.Year || fechaDoc.Month != periodoDoc.Month)
                        fechaDoc = new DateTime(periodoDoc.Year, periodoDoc.Month, DateTime.DaysInMonth(periodoDoc.Year, periodoDoc.Month));

                    #endregion
                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            //if (documentID != AppDocuments.GenerarPresupuestoAprob)
                            //    this.AsignarFlujo(documentID, item.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                            //else
                                result = this.Presupuesto_Aprobar(documentID, actividadFlujoID, item, periodoDoc, fechaDoc, false);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Presupuesto_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_AprobarDoc + "&&" + item.PeriodoDoc.Value.Value.Date.ToString() + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString();
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, item.NumeroDoc.Value.Value, EstadoDocControl.Anulado, item.Observacion.Value, true);
                            this.AsignarFlujo(documentID, item.NumeroDoc.Value.Value, actividadFlujoID, true, item.Observacion.Value);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Presupuesto_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_RechazarDoc + "&&" + item.PeriodoDoc.Value.Value.Date.ToString() + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString();
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_Aprobar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        ///  Trae la informacion de planeacion proveedores
        /// </summary>
        /// <param name="consActLinea"> Consecutivo filtro</param>
        /// <returns>Lista</returns>
        public List<DTO_plPlaneacion_Proveedores> plPlaneacion_Proveedores_GetByConsActLinea(int consActLinea)
        {
            try
            {
                this._dal_plPlaneacionProveedor = (DAL_plPlaneacion_Proveedores)base.GetInstance(typeof(DAL_plPlaneacion_Proveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_plPlaneacion_Proveedores> presupuestoDeta = this._dal_plPlaneacionProveedor.DAL_plPlaneacion_Proveedores_GetByConsActLinea(consActLinea);
                return presupuestoDeta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plPresupuestoDeta_GetByNumeroDoc");
                return null;
            }
        }

        /// <summary>
        /// Aprueba Un Presupuesto contable
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="numeroDoc">Identificador del doc</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <returns></returns>
        public DTO_TxResult PresupuestoContable_Aprobar(int documentID, int numeroDoc, DateTime periodoDoc, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            bool validateSaldoMensual = true;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrlPrincipal = new DTO_glDocumentoControl();
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoTotal = (DAL_plPresupuestoTotal)base.GetInstance(typeof(DAL_plPresupuestoTotal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables por defecto
                //string ctaRecDisp = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_CuentaRecursoDisp);
                string lugGeoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string concCargXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string linPresXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string concSaldoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);
                string mdaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExtranj = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                ctrlPrincipal = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                List<DTO_plPresupuestoDeta> presupuestoDeta = this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_Get(numeroDoc);
                #region Carga y Guarda la info de Presupuesto Total
                foreach (DTO_plPresupuestoDeta plDeta in presupuestoDeta)
                {
                    DTO_plPresupuestoTotal total = new DTO_plPresupuestoTotal();
                    total.Ano.Value = periodoDoc.Year;
                    total.ProyectoID.Value = plDeta.ProyectoID.Value;
                    total.CentroCostoID.Value = plDeta.CentroCostoID.Value;
                    total.LineaPresupuestoID.Value = plDeta.LineaPresupuestoID.Value;
                    total.ValorLoc00.Value = plDeta.ValorLoc00.Value;
                    total.ValorLoc01.Value = plDeta.ValorLoc01.Value;
                    total.ValorLoc02.Value = plDeta.ValorLoc02.Value;
                    total.ValorLoc03.Value = plDeta.ValorLoc03.Value;
                    total.ValorLoc04.Value = plDeta.ValorLoc04.Value;
                    total.ValorLoc05.Value = plDeta.ValorLoc05.Value;
                    total.ValorLoc06.Value = plDeta.ValorLoc06.Value;
                    total.ValorLoc07.Value = plDeta.ValorLoc07.Value;
                    total.ValorLoc08.Value = plDeta.ValorLoc08.Value;
                    total.ValorLoc09.Value = plDeta.ValorLoc09.Value;
                    total.ValorLoc10.Value = plDeta.ValorLoc10.Value;
                    total.ValorLoc11.Value = plDeta.ValorLoc11.Value;
                    total.ValorLoc12.Value = plDeta.ValorLoc12.Value;
                    total.EquivExt00.Value = plDeta.EquivExt00.Value;
                    total.EquivExt01.Value = plDeta.EquivExt01.Value;
                    total.EquivExt02.Value = plDeta.EquivExt02.Value;
                    total.EquivExt03.Value = plDeta.EquivExt03.Value;
                    total.EquivExt04.Value = plDeta.EquivExt04.Value;
                    total.EquivExt05.Value = plDeta.EquivExt05.Value;
                    total.EquivExt06.Value = plDeta.EquivExt06.Value;
                    total.EquivExt07.Value = plDeta.EquivExt07.Value;
                    total.EquivExt08.Value = plDeta.EquivExt08.Value;
                    total.EquivExt09.Value = plDeta.EquivExt09.Value;
                    total.EquivExt10.Value = plDeta.EquivExt10.Value;
                    total.EquivExt11.Value = plDeta.EquivExt11.Value;
                    total.EquivExt11.Value = plDeta.EquivExt11.Value;
                    total.EquivExt12.Value = plDeta.EquivExt11.Value;
                    total.ValorExt00.Value = plDeta.ValorExt00.Value;
                    total.ValorExt01.Value = plDeta.ValorExt01.Value;
                    total.ValorExt02.Value = plDeta.ValorExt02.Value;
                    total.ValorExt03.Value = plDeta.ValorExt03.Value;
                    total.ValorExt04.Value = plDeta.ValorExt04.Value;
                    total.ValorExt05.Value = plDeta.ValorExt05.Value;
                    total.ValorExt06.Value = plDeta.ValorExt06.Value;
                    total.ValorExt07.Value = plDeta.ValorExt07.Value;
                    total.ValorExt08.Value = plDeta.ValorExt08.Value;
                    total.ValorExt09.Value = plDeta.ValorExt09.Value;
                    total.ValorExt10.Value = plDeta.ValorExt10.Value;
                    total.ValorExt11.Value = plDeta.ValorExt11.Value;
                    total.ValorExt12.Value = plDeta.ValorExt12.Value;
                    total.EquivLoc00.Value = plDeta.EquivLoc00.Value;
                    total.EquivLoc01.Value = plDeta.EquivLoc01.Value;
                    total.EquivLoc02.Value = plDeta.EquivLoc02.Value;
                    total.EquivLoc03.Value = plDeta.EquivLoc03.Value;
                    total.EquivLoc04.Value = plDeta.EquivLoc04.Value;
                    total.EquivLoc05.Value = plDeta.EquivLoc05.Value;
                    total.EquivLoc06.Value = plDeta.EquivLoc06.Value;
                    total.EquivLoc07.Value = plDeta.EquivLoc07.Value;
                    total.EquivLoc08.Value = plDeta.EquivLoc08.Value;
                    total.EquivLoc09.Value = plDeta.EquivLoc09.Value;
                    total.EquivLoc10.Value = plDeta.EquivLoc10.Value;
                    total.EquivLoc11.Value = plDeta.EquivLoc11.Value;
                    total.EquivLoc12.Value = plDeta.EquivLoc12.Value;
                    this._dal_plPresupuestoTotal.DAL_plPresupuestoTotal_Add(total, ref validateSaldoMensual);
                    if (!validateSaldoMensual)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Pl_SaldoMensualNotAvailable + "&&" + total.CentroCostoID.Value + "&&" + total.LineaPresupuestoID.Value;
                        return result;
                    }
                }
                #endregion                                
                #region Guarda el Presupuesto en plCierreLegalizacion
                result = this.GeneraPresupuesto(ctrlPrincipal.DocumentoID.Value.Value, numeroDoc);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Aprueba el documento y asigna el flujo
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ctrlPrincipal.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                //if (ctrlSecundario != null)
                //    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ctrlSecundario.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                //result = this.AsignarFlujo(documentID, ctrlPrincipal.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Presupuesto_Aprobar");
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

                        //ctrlPrincipal.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrlPrincipal.PrefijoID.Value, ctrlPrincipal.PeriodoDoc.Value.Value, ctrlPrincipal.DocumentoNro.Value.Value);
                        //this._moduloGlobal.ActualizaConsecutivos(ctrlPrincipal, false, true, false);
                        //this._moduloContabilidad.ActualizaComprobanteNro(ctrlPrincipal.NumeroDoc.Value.Value, ctrlPrincipal.ComprobanteIDNro.Value.Value, false);
                        //if (ctrlSecundario != null)
                        //{
                        //    ctrlSecundario.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrlSecundario.PrefijoID.Value, ctrlSecundario.PeriodoDoc.Value.Value, ctrlSecundario.DocumentoNro.Value.Value);
                        //    this._moduloGlobal.ActualizaConsecutivos(ctrlSecundario, false, true, false);
                        //    this._moduloContabilidad.ActualizaComprobanteNro(ctrlSecundario.NumeroDoc.Value.Value, ctrlSecundario.ComprobanteIDNro.Value.Value, false);
                        //}
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

        #region Presupuesto PxQ

        /// <summary>
        /// Carga la información de un nuevo presupuesto
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta el proceso</param>
        /// <param name="periodoPresupuesto">periodo del presupuesto</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="presupuesto">Presupuesto que se va a generar</param>
        /// <param name="isAnotherTx">Revisa si se esta ejecutando en otar transaccion</param>
        /// <returns>Resultado</returns>
        public DTO_SerializedObject PresupuestoPxQ_Add(int documentoID, DateTime periodoPresupuesto, string proyectoID, decimal tc, DTO_Presupuesto presupuesto, bool onlySave, bool isAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            if (!isAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            DTO_Alarma alarma = null;
            try
            {
                #region Dals y Modulos
                this._dal_plPresupuestoPxQDeta = (DAL_plPresupuestoPxQDeta)base.GetInstance(typeof(DAL_plPresupuestoPxQDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoDocu = (DAL_plPresupuestoDocu)base.GetInstance(typeof(DAL_plPresupuestoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion

                ctrl = presupuesto.DocCtrl;
                if (ctrl == null)
                {
                    #region Variables
                    //Variables de operacion
                    string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    //Variables por defecto
                    string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                    string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                    string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    string terceroDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    string ctoCostoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);

                    //Periodo documento
                    string periodoStr = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.co_Periodo);
                    DateTime periodoDoc = Convert.ToDateTime(periodoStr);

                    decimal valorMdaLocal = presupuesto.DetallesPxQ.Sum(x => x.PresupuestoLoc.Value.Value);
                    decimal valorMdaExtr = presupuesto.DetallesPxQ.Sum(x => x.PresupuestoExt.Value.Value);

                    #endregion
                    #region Carga y Guarda glDocumentoControl

                    ctrl = new DTO_glDocumentoControl();
                    ctrl.DocumentoNro.Value = 0;
                    ctrl.DocumentoID.Value = documentoID;
                    ctrl.NumeroDoc.Value = 0;
                    ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                    ctrl.Fecha.Value = DateTime.Now;
                    ctrl.FechaDoc.Value = DateTime.Now;
                    ctrl.PeriodoDoc.Value = periodoPresupuesto;
                    ctrl.PeriodoUltMov.Value = periodoPresupuesto;
                    ctrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser(); ;
                    ctrl.PrefijoID.Value = prefijoDef;
                    ctrl.ProyectoID.Value = proyectoID;
                    ctrl.CentroCostoID.Value = ctoCostoDef;
                    ctrl.LugarGeograficoID.Value = lugGeoDef;
                    ctrl.LineaPresupuestoID.Value = linPresDef;
                    ctrl.TerceroID.Value = terceroDef;
                    ctrl.MonedaID.Value = valorMdaLocal != 0 ? mdaLoc : mdaExt;
                    ctrl.Valor.Value = valorMdaLocal != 0 ? valorMdaLocal : valorMdaExtr;

                    ctrl.TasaCambioCONT.Value = tc;
                    ctrl.TasaCambioDOCU.Value = tc;
                    if (documentoID == AppDocuments.Presupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Nuevo";
                    if (documentoID == AppDocuments.AdicionPresupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Adicion";
                    if (documentoID == AppDocuments.ReclasifPresupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Reclasificacion";
                    if (documentoID == AppDocuments.TrasladoPresupuesto)
                        ctrl.Descripcion.Value = "Presupuesto - Traslado";
                    ctrl.Estado.Value = onlySave ? (byte)EstadoDocControl.SinAprobar : (byte)EstadoDocControl.ParaAprobacion;
                    ctrl.seUsuarioID.Value = this.UserId;
                    ctrl.Iva.Value = 0;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }
                    ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    #endregion
                    #region Guarda en plPresupuestoDocu
                    DTO_plPresupuestoDocu docu = new DTO_plPresupuestoDocu();
                    docu.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    if (documentoID == AppDocuments.Presupuesto)
                        docu.NumeroDocPresup.Value = ctrl.NumeroDoc.Value;
                    else
                        docu.NumeroDocPresup.Value = presupuesto.NumeroDocPresup.Value;
                    this._dal_plPresupuestoDocu.DAL_plPresupuestoDocu_Add(docu);
                    #endregion
                    #region Guarda detalle (plPresupuestoPxQDeta)
                    foreach (DTO_plPresupuestoPxQDeta det in presupuesto.DetallesPxQ)
                    {
                        foreach (DTO_plPresupuestoPxQDeta pxq in det.Detalle)
                        {
                            if (documentoID != AppDocuments.Presupuesto)
                            {
                                pxq.CantidadPRELoc.Value = pxq.NuevaCantidadPRELoc.Value;
                                pxq.CantidadPREExt.Value = pxq.NuevaCantidadPREExt.Value;
                            }
                            pxq.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                            this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_Add(pxq);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Actualiza docControl
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentoID, ctrl.NumeroDoc.Value.Value, (onlySave ? EstadoDocControl.SinAprobar : EstadoDocControl.ParaAprobacion), ctrl.Observacion.Value, true);
                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentoID);
                    if (act.Count > 0)
                        this.AsignarFlujo(documentoID, ctrl.NumeroDoc.Value.Value, act[0], false, string.Empty);
                    #endregion
                    #region Actualiza la info del presupuesto
                    this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_Delete(ctrl.NumeroDoc.Value.Value);
                    foreach (DTO_plPresupuestoPxQDeta det in presupuesto.DetallesPxQ)
                    {
                        foreach (DTO_plPresupuestoPxQDeta pxq in det.Detalle)
                        {
                            //if (pxq.PresupuestoLoc.Value != 0 || pxq.PresupuestoExt.Value != 0)
                            //{
                            pxq.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                            this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_Add(pxq);
                            //}
                        }
                    }
                    #endregion
                }

                #region Asigna las alarmas
                alarma = this.GetFirstMailInfo(ctrl.NumeroDoc.Value.Value, false);
                alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                return alarma;
                #endregion

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PresupuestoPxQ_Add");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!isAnotherTx)
                    {
                        #region Genera el consecutivo
                        base._mySqlConnectionTx.Commit();

                        if (ctrl.DocumentoNro.Value == 0)
                        {
                            base._mySqlConnectionTx = null;
                            this._moduloGlobal._mySqlConnectionTx = null;

                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentoID, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, false);
                            alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !isAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos PxQ con un filtro
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="proyecto"></param>
        /// <param name="periodo"></param>
        /// <param name="proyectoTipo"></param>
        /// <param name="contratoID"></param>
        /// <param name="actividadID"></param>
        /// <param name="areaFisica"></param>
        /// <param name="validState"></param>
        /// <returns></returns>
        public DTO_Presupuesto PresupuestoPxQ_GetPresupuestoPxQConsolidado(int documentID, string proyecto, DateTime periodo, byte proyectoTipo, string contratoID, string actividadID, string areaFisica, byte estadoDocCtrl)
        {
            try
            {
                #region Variables
                this._dal_plPresupuestoTotal = (DAL_plPresupuestoTotal)base.GetInstance(typeof(DAL_plPresupuestoTotal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_Presupuesto presupuesto = new DTO_Presupuesto();
                DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                Dictionary<string, DTO_glLocFisica> cacheLocFisicas = new Dictionary<string, DTO_glLocFisica>();
                DTO_glLocFisica _locFisica;
                Dictionary<string, DTO_plLineaPresupuesto> cacheLineasPres = new Dictionary<string, DTO_plLineaPresupuesto>();
                DTO_plLineaPresupuesto _lineaPres;
                DTO_glDocumentoControl docControl = null;
                #endregion
                #region Obtiene el documento Presupuesto
                filter.ProyectoID.Value = proyecto;
                filter.PeriodoDoc.Value = periodo;
                filter.Estado.Value = estadoDocCtrl;
                filter.DocumentoID.Value = documentID;
                var docsPresup = this._moduloGlobal.glDocumentoControl_GetByParameter(filter).ToList();
                #endregion
                #region Valida Tipo de Proyecto y Contrato
                if (proyectoTipo == (byte)ProyectoTipo.Opex || proyectoTipo == (byte)ProyectoTipo.Administracion)
                {
                    //Valida que el doc obtenido sea con proyecto tipo Opex o Admin
                    foreach (DTO_glDocumentoControl doc in docsPresup)
                    {
                        DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, doc.ProyectoID.Value, true, false);
                        DTO_coActividad actividad = (DTO_coActividad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, proy.ActividadID.Value, true, false);
                        if ((actividad.ProyectoTipo.Value == (byte)ProyectoTipo.Opex || actividad.ProyectoTipo.Value == (byte)ProyectoTipo.Administracion)
                            && proy.ContratoID.Value == contratoID)
                        {
                            docControl = doc;
                            break;
                        }
                    }
                }
                else
                    if (docsPresup.Count > 0) docControl = docsPresup.First();

                if (docControl == null) return null;
                #endregion
                #region Obtiene el detalle del Doc
                List<DTO_plPresupuestoPxQDeta> detallePresupPxQ = this.plPresupuestoPxQDeta_GetByNumeroDoc(docControl.NumeroDoc.Value.Value, documentID == AppDocuments.Presupuesto? true : false);
                presupuesto.DetallesPxQ = detallePresupPxQ;
                foreach (var deta in presupuesto.DetallesPxQ)
                {
                    #region Carga LocFisica
                    if (cacheLocFisicas.ContainsKey(deta.LocFisicaID.Value))
                        _locFisica = cacheLocFisicas[deta.LocFisicaID.Value];
                    else
                    {
                        _locFisica = (DTO_glLocFisica)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, deta.LocFisicaID.Value, true, false);
                        cacheLocFisicas.Add(deta.LocFisicaID.Value, _locFisica);
                    }
                    #endregion
                    if (_locFisica != null)
                        deta.AreaFisicaID.Value = _locFisica.AreaFisica.Value;
                    #region Asigna RecursoID
                    if (proyectoTipo == (byte)ProyectoTipo.Opex || proyectoTipo == (byte)ProyectoTipo.Administracion)
                    {
                        #region Carga LineaPresupuesto
                        if (cacheLineasPres.ContainsKey(deta.LineaPresupuestoID.Value))
                            _lineaPres = cacheLineasPres[deta.LineaPresupuestoID.Value];
                        else
                        {
                            _lineaPres = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, deta.LineaPresupuestoID.Value, true, false);
                            cacheLineasPres.Add(deta.LineaPresupuestoID.Value, _lineaPres);
                        }
                        #endregion
                        deta.RecursoID.Value = _lineaPres.RecursoID.Value;
                    }
                    else
                    {
                        Dictionary<string, string> pks = new Dictionary<string, string>();
                        pks.Add("ActividadID", deta.ActividadID.Value);
                        pks.Add("LineaPresupuestoID", deta.LineaPresupuestoID.Value);
                        DTO_plActividadLineaPresupuestal actLinea = (DTO_plActividadLineaPresupuestal)this.GetMasterComplexDTO(AppMasters.plActividadLineaPresupuestal, pks, true);
                        deta.RecursoID.Value = actLinea != null ? actLinea.RecursoID.Value : string.Empty;
                    }
                    #endregion
                    deta.PresupuestoLoc.Value = deta.ValorUniLoc.Value * deta.CantidadPRELoc.Value;
                    deta.PresupuestoExt.Value = deta.ValorUniExt.Value * deta.CantidadPREExt.Value;
                }
                #endregion
                presupuesto.DocCtrl = docControl;
                return presupuesto;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PresupuestoPxQ_GetPresupuestoPxQ");
                return null;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos PxQ con un filtro
        /// </summary>
        /// <param name="DocumentID">documento Actual</param>
        /// <param name="Proyecto">proyecto</param>
        /// <param name="Periodo">periodo actual</param>
        /// <param name="ProyectoTipo">tipo de Proyecto</param>
        /// <param name="ContratoID">Contrato</param>
        /// <param name="ActividadID">Actividad</param>
        /// <returns>Presupuesto detallado</returns>
        public DTO_Presupuesto PresupuestoPxQ_GetDataPxQ(int documentID, byte tipoProyecto, string proyectoID, DateTime periodo, string contratoID, string actividadID, string areaFisicaID, string lineaPresupID, string recursoID)
        {
            try
            {
                #region Variables
                this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterHierarchy = (DAL_MasterHierarchy)base.GetInstance(typeof(DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProveedores = (ModuloProveedores)base.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string centroCostoIDxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                DTO_Presupuesto presupuestoPxQ = new DTO_Presupuesto();

                List<DTO_plPresupuestoPxQDeta> detListFinal = new List<DTO_plPresupuestoPxQDeta>();
                List<DTO_plPresupuestoPxQDeta> detalleOriginal = new List<DTO_plPresupuestoPxQDeta>();
                #endregion

                #region Capex / Inversion
                if (tipoProyecto == (byte)ProyectoTipo.Capex || tipoProyecto == (byte)ProyectoTipo.Inversion)
                {
                    DTO_coProyecto dtoProy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, proyectoID, true, false);
                    #region Filtra plActividadLineaPresupuestal
                    DTO_glConsulta consultaActLinea = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtrosActLinea = new List<DTO_glConsultaFiltro>();
                    filtrosActLinea.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ActividadID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = dtoProy.ActividadID.Value,
                    });
                    if (!string.IsNullOrEmpty(lineaPresupID))
                    {
                        filtrosActLinea.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "LineaPresupuestoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = lineaPresupID
                        });
                    }
                    consultaActLinea.Filtros.AddRange(filtrosActLinea);
                    this._dal_MasterComplex.DocumentID = AppMasters.plActividadLineaPresupuestal;
                    long countActLinea = this._dal_MasterComplex.DAL_MasterComplex_Count(consultaActLinea, true);
                    var actLineas = this._dal_MasterComplex.DAL_MasterComplex_GetPaged(countActLinea, 1, consultaActLinea, true).ToList();
                    #endregion
                    #region Asigna Detalle
                    DTO_glLocFisica dtoLocFisica = (DTO_glLocFisica)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, dtoProy.LocFisicaID.Value, true, false);
                    DTO_glAreaFisica dtoAreaFisica = (DTO_glAreaFisica)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, dtoLocFisica.AreaFisica.Value, true, false);

                    //Recorre las lineas para obtener detalle
                    foreach (var linea in actLineas)
                    {
                        #region Variables
                        DTO_plPresupuestoPxQDeta detalle = new DTO_plPresupuestoPxQDeta(true);
                        DTO_plActividadLineaPresupuestal dtoActLinea = (DTO_plActividadLineaPresupuestal)linea;
                        DTO_plLineaPresupuesto dtoLineaPres = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, dtoActLinea.LineaPresupuestoID.Value, true, false);
                        DTO_plRecurso dtoRecurso = (DTO_plRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecurso, dtoActLinea.RecursoID.Value, true, false);
                        #endregion
                        //Valida si trae la info adicional de Proveedores
                        if (dtoLineaPres.ControlEjecucionPxQ.Value == 2 || dtoLineaPres.ControlEjecucionPxQ.Value == 3)
                        {
                            List<DTO_plPlaneacion_Proveedores> planProveedor = this.plPlaneacion_Proveedores_GetByConsActLinea(dtoActLinea.ReplicaID.Value.Value);
                            foreach (var plan in planProveedor)
                            {
                                //Trae Codigos BS y Referencia
                                DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, plan.CodigoBSID.Value, true, false);
                                detalle.AreaFisicaID.Value = dtoAreaFisica.ID.Value;
                                detalle.AreaFisicaDesc.Value = dtoAreaFisica.Descriptivo.Value;
                                detalle.RecursoID.Value = dtoActLinea.RecursoID.Value;
                                detalle.RecursoDesc.Value = dtoRecurso != null ? dtoRecurso.Descriptivo.Value : string.Empty;
                                detalle.LineaPresupuestoID.Value = dtoLineaPres.ID.Value;
                                detalle.LineaPresDesc.Value = dtoLineaPres.Descriptivo.Value;
                                detalle.CodigoBSID.Value = plan.CodigoBSID.Value;
                                detalle.CodigoBSDesc.Value = bs.Descriptivo.Value;
                                detalle.inReferenciaID.Value = plan.inReferenciaID.Value;
                                detalle.UnidadInvID.Value = bs.UnidadInvID.Value;
                                detalle.ProyectoID.Value = dtoProy.ID.Value;
                                detalle.ActividadID.Value = dtoProy.ActividadID.Value;
                                detalle.ContratoID.Value = dtoProy.ContratoID.Value;
                                detalle.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                                detalle.CentroCostoID.Value = centroCostoIDxDef;
                                detalle.Ano.Value = periodo.Year;
                                detalle.NumeroDoc.Value = 0;

                                //Trae Costos unitarios Orden Compra   
                                List<DTO_prCierreMesCostos> cierres = this._moduloProveedores.prCierreMesCostos_GetByParameter(periodo, plan.CodigoBSID.Value, plan.inReferenciaID.Value, null);
                                if (cierres.Count > 0)
                                {
                                    DTO_glDocumentoControl dtoControlOC = this._moduloGlobal.glDocumentoControl_GetByID(detalle.NumeroDocOC.Value.Value);
                                    detalle.PrefijoIDOC.Value = dtoControlOC.PrefijoID.Value;
                                    detalle.NroOC.Value = dtoControlOC.DocumentoNro.Value;
                                    detalle.ValorUniOCLoc.Value = cierres.Sum(x => x.VlrLocal.Value);
                                    detalle.ValorUniOCExt.Value = cierres.Sum(x => x.VlrExtra.Value);
                                    detalle.NumeroDocOC.Value = cierres.First().NumeroDoc.Value;
                                }
                            }
                            if (planProveedor.Count == 0)
                            {
                                detalle.AreaFisicaID.Value = dtoAreaFisica.ID.Value;
                                detalle.AreaFisicaDesc.Value = dtoAreaFisica.Descriptivo.Value;
                                detalle.RecursoID.Value = dtoActLinea.RecursoID.Value;
                                detalle.RecursoDesc.Value = dtoRecurso != null ? dtoRecurso.Descriptivo.Value : string.Empty;
                                detalle.LineaPresupuestoID.Value = dtoActLinea.LineaPresupuestoID.Value;
                                detalle.LineaPresDesc.Value = dtoLineaPres.Descriptivo.Value;
                                detalle.ProyectoID.Value = dtoProy.ID.Value;
                                detalle.ActividadID.Value = dtoProy.ActividadID.Value;
                                detalle.ContratoID.Value = dtoProy.ContratoID.Value;
                                detalle.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                                detalle.CentroCostoID.Value = centroCostoIDxDef;
                                detalle.ValorUniOCLoc.Value = 0;
                                detalle.ValorUniOCExt.Value = 0;
                                detalle.PorcentajeVar.Value = 0;
                                detalle.Ano.Value = periodo.Year;
                                detalle.NumeroDoc.Value = 0;
                            }
                        }
                        else
                        {
                            detalle.AreaFisicaID.Value = dtoAreaFisica.ID.Value;
                            detalle.AreaFisicaDesc.Value = dtoAreaFisica.Descriptivo.Value;
                            detalle.RecursoID.Value = dtoActLinea.RecursoID.Value;
                            detalle.RecursoDesc.Value = dtoRecurso != null ? dtoRecurso.Descriptivo.Value : string.Empty;
                            detalle.LineaPresupuestoID.Value = dtoLineaPres.ID.Value;
                            detalle.LineaPresDesc.Value = dtoLineaPres.Descriptivo.Value;
                            detalle.ProyectoID.Value = dtoProy.ID.Value;
                            detalle.ActividadID.Value = dtoProy.ActividadID.Value;
                            detalle.ContratoID.Value = dtoProy.ContratoID.Value;
                            detalle.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                            detalle.CentroCostoID.Value = centroCostoIDxDef;
                            detalle.ValorUniOCLoc.Value = 0;
                            detalle.ValorUniOCExt.Value = 0;
                            detalle.PorcentajeVar.Value = 0;
                            detalle.Ano.Value = periodo.Year;
                            detalle.NumeroDoc.Value = 0;
                        }
                        detalleOriginal.Add(detalle);
                    }
                    #endregion
                }
                #endregion
                #region Opex/Administrativo
                else
                {
                    #region Filtra y recorre Areas Fisicas(Campos)
                    DTO_glConsulta consulta = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ContratoID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = contratoID,
                        OperadorSentencia = "AND"
                    });
                    if (!string.IsNullOrEmpty(areaFisicaID))
                    {
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "AreaFisica",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = areaFisicaID,

                        });
                    }
                    consulta.Filtros.AddRange(filtros);
                    this._dal_MasterSimple.DocumentID = AppMasters.glAreaFisica;
                    long count = this._dal_MasterSimple.DAL_MasterSimple_Count(consulta, null, true);
                    var areasFis = this._dal_MasterSimple.DAL_MasterSimple_GetPaged(count, 1, consulta, null, true).ToList();
                    #endregion
                    foreach (var area in areasFis)
                    {
                        DTO_glAreaFisica dtoAreaFisica = (DTO_glAreaFisica)area;
                        #region Filtra y recorre Localizacion Fisica
                        consulta = new DTO_glConsulta();
                        filtros = new List<DTO_glConsultaFiltro>();
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "AreaFisica",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = dtoAreaFisica.ID.Value,

                        });
                        consulta.Filtros.AddRange(filtros);
                        this._dal_MasterHierarchy.DocumentID = AppMasters.glLocFisica;
                        count = this._dal_MasterHierarchy.DAL_MasterSimple_Count(consulta, null, true);
                        var locFisicas = this._dal_MasterHierarchy.DAL_MasterSimple_GetPaged(count, 1, consulta, null, true).ToList();
                        #endregion
                        foreach (var locFis in locFisicas)
                        {
                            DTO_glLocFisica dtoLocFisica = (DTO_glLocFisica)locFis;
                            #region Filtra y recorre Proyectos
                            consulta = new DTO_glConsulta();
                            filtros = new List<DTO_glConsultaFiltro>();
                            filtros.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "LocFisicaID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = dtoLocFisica.ID.Value,
                                OperadorSentencia = "AND"
                            });
                            filtros.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "ContratoID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = contratoID,
                                OperadorSentencia = "AND"
                            });
                            if (!string.IsNullOrEmpty(proyectoID))
                            {
                                filtros.Add(new DTO_glConsultaFiltro()
                                {
                                    CampoFisico = "ProyectoID",
                                    OperadorFiltro = OperadorFiltro.Igual,
                                    ValorFiltro = "1",
                                    OperadorSentencia = "AND"
                                });
                            }
                            if (!string.IsNullOrEmpty(actividadID))
                            {
                                filtros.Add(new DTO_glConsultaFiltro()
                                {
                                    CampoFisico = "ActividadID",
                                    OperadorFiltro = OperadorFiltro.Igual,
                                    ValorFiltro = actividadID,
                                });
                            }
                            consulta.Filtros.AddRange(filtros);
                            this._dal_MasterHierarchy.DocumentID = AppMasters.coProyecto;
                            count = this._dal_MasterHierarchy.DAL_MasterSimple_Count(consulta, null, true);
                            var proyectos = this._dal_MasterHierarchy.DAL_MasterSimple_GetPaged(count, 1, consulta, null, true).ToList();
                            #endregion
                            foreach (var proy in proyectos)
                            {
                                DTO_coProyecto dtoProy = (DTO_coProyecto)proy;
                                DTO_coActividad dtoActividad = (DTO_coActividad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, dtoProy.ActividadID.Value, true, false);
                                #region Llena el detalle con la info obtenida SI es de Presupuesto y SI es Opex o Admin
                                if (dtoProy.PresupuestalInd.Value.Value && (dtoActividad.ProyectoTipo.Value == (byte)ProyectoTipo.Opex ||
                                                                            dtoActividad.ProyectoTipo.Value == (byte)ProyectoTipo.Administracion))
                                {
                                    #region Filtra plActividadLineaPresupuestal
                                    DTO_glConsulta consultaActLinea = new DTO_glConsulta();
                                    filtros = new List<DTO_glConsultaFiltro>();
                                    filtros.Add(new DTO_glConsultaFiltro()
                                    {
                                        CampoFisico = "ActividadID",
                                        OperadorFiltro = OperadorFiltro.Igual,
                                        ValorFiltro = dtoProy.ActividadID.Value,
                                        OperadorSentencia = "AND"
                                    });
                                    if (!string.IsNullOrEmpty(lineaPresupID))
                                    {
                                        filtros.Add(new DTO_glConsultaFiltro()
                                        {
                                            CampoFisico = "LineaPresupuestoID",
                                            OperadorFiltro = OperadorFiltro.Igual,
                                            ValorFiltro = lineaPresupID
                                        });
                                    }
                                    consultaActLinea.Filtros.AddRange(filtros);
                                    this._dal_MasterComplex.DocumentID = AppMasters.plActividadLineaPresupuestal;
                                    long countActLinea = this._dal_MasterComplex.DAL_MasterComplex_Count(consultaActLinea, true);
                                    var actLineas = this._dal_MasterComplex.DAL_MasterComplex_GetPaged(countActLinea, 1, consultaActLinea, true).ToList();
                                    #endregion
                                    #region Asigna Detalle

                                    //Recorre las lineas para obtener info requerida
                                    foreach (var linea in actLineas)
                                    {
                                        DTO_plPresupuestoPxQDeta detalle = new DTO_plPresupuestoPxQDeta(true);
                                        DTO_plActividadLineaPresupuestal dtoActLinea = (DTO_plActividadLineaPresupuestal)linea;
                                        DTO_plLineaPresupuesto dtoLineaPres = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, dtoActLinea.LineaPresupuestoID.Value, true, false);
                                        DTO_plRecurso dtoRecurso = (DTO_plRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecurso, dtoLineaPres.RecursoID.Value, true, false);

                                        //Valida si trae la info existente a partir de la linea Presup
                                        if (dtoLineaPres.ControlEjecucionPxQ.Value == 2 || dtoLineaPres.ControlEjecucionPxQ.Value == 3)
                                        {
                                            List<DTO_plPlaneacion_Proveedores> planProveedor = this.plPlaneacion_Proveedores_GetByConsActLinea(dtoActLinea.ReplicaID.Value.Value);
                                            foreach (var plan in planProveedor)
                                            {
                                                #region Trae Codigos BS y Referencia y Costos
                                                DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, plan.CodigoBSID.Value, true, false);
                                                detalle.AreaFisicaID.Value = area.ID.Value;
                                                detalle.AreaFisicaDesc.Value = dtoAreaFisica.Descriptivo.Value;
                                                detalle.RecursoID.Value = dtoLineaPres.RecursoID.Value;
                                                detalle.RecursoDesc.Value = dtoRecurso != null ? dtoRecurso.Descriptivo.Value : string.Empty;
                                                detalle.LineaPresupuestoID.Value = dtoActLinea.LineaPresupuestoID.Value;
                                                detalle.LineaPresDesc.Value = dtoLineaPres.Descriptivo.Value;
                                                detalle.CodigoBSID.Value = plan.CodigoBSID.Value;
                                                detalle.CodigoBSDesc.Value = bs.Descriptivo.Value;
                                                detalle.inReferenciaID.Value = plan.inReferenciaID.Value;
                                                detalle.UnidadInvID.Value = bs.UnidadInvID.Value;
                                                detalle.ProyectoID.Value = dtoProy.ID.Value;
                                                detalle.ActividadID.Value = dtoProy.ActividadID.Value;
                                                detalle.ContratoID.Value = dtoProy.ContratoID.Value;
                                                detalle.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                                                detalle.CentroCostoID.Value = centroCostoIDxDef;
                                                detalle.Ano.Value = periodo.Year;
                                                detalle.NumeroDoc.Value = 0;

                                                //Trae Costos unitarios Orden Compra   
                                                List<DTO_prCierreMesCostos> cierres = this._moduloProveedores.prCierreMesCostos_GetByParameter(periodo, plan.CodigoBSID.Value, plan.inReferenciaID.Value, null);
                                                if (cierres.Count > 0)
                                                {
                                                    detalle.ValorUniOCLoc.Value = cierres.Sum(x => x.VlrLocal.Value);
                                                    detalle.ValorUniOCExt.Value = cierres.Sum(x => x.VlrExtra.Value);
                                                    detalle.NumeroDocOC.Value = cierres.First().NumeroDoc.Value;
                                                    DTO_glDocumentoControl dtoControlOC = this._moduloGlobal.glDocumentoControl_GetByID(detalle.NumeroDocOC.Value.Value);
                                                    detalle.PrefijoIDOC.Value = dtoControlOC.PrefijoID.Value;
                                                    detalle.NroOC.Value = dtoControlOC.DocumentoNro.Value;
                                                }
                                                #endregion
                                            }

                                            if (planProveedor.Count == 0)
                                            {
                                                detalle.AreaFisicaID.Value = area.ID.Value;
                                                detalle.AreaFisicaDesc.Value = dtoAreaFisica.Descriptivo.Value;
                                                detalle.RecursoID.Value = dtoLineaPres.RecursoID.Value;
                                                detalle.RecursoDesc.Value = dtoRecurso != null ? dtoRecurso.Descriptivo.Value : string.Empty;
                                                detalle.LineaPresupuestoID.Value = dtoActLinea.LineaPresupuestoID.Value;
                                                detalle.LineaPresDesc.Value = dtoLineaPres.Descriptivo.Value;
                                                detalle.ProyectoID.Value = dtoProy.ID.Value;
                                                detalle.ActividadID.Value = dtoProy.ActividadID.Value;
                                                detalle.ContratoID.Value = dtoProy.ContratoID.Value;
                                                detalle.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                                                detalle.CentroCostoID.Value = centroCostoIDxDef;
                                                detalle.ValorUniOCLoc.Value = 0;
                                                detalle.ValorUniOCExt.Value = 0;
                                                detalle.PorcentajeVar.Value = 0;
                                                detalle.Ano.Value = periodo.Year;
                                                detalle.NumeroDoc.Value = 0;
                                            }
                                        }
                                        else
                                        {
                                            detalle.AreaFisicaID.Value = area.ID.Value;
                                            detalle.AreaFisicaDesc.Value = dtoAreaFisica.Descriptivo.Value;
                                            detalle.RecursoID.Value = dtoLineaPres.RecursoID.Value;
                                            detalle.RecursoDesc.Value = dtoRecurso != null ? dtoRecurso.Descriptivo.Value : string.Empty;
                                            detalle.LineaPresupuestoID.Value = dtoActLinea.LineaPresupuestoID.Value;
                                            detalle.LineaPresDesc.Value = dtoLineaPres.Descriptivo.Value;
                                            detalle.ProyectoID.Value = dtoProy.ID.Value;
                                            detalle.ActividadID.Value = dtoProy.ActividadID.Value;
                                            detalle.ContratoID.Value = dtoProy.ContratoID.Value;
                                            detalle.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                                            detalle.CentroCostoID.Value = centroCostoIDxDef;
                                            detalle.ValorUniOCLoc.Value = 0;
                                            detalle.ValorUniOCExt.Value = 0;
                                            detalle.PorcentajeVar.Value = 0;
                                            detalle.Ano.Value = periodo.Year;
                                            detalle.NumeroDoc.Value = 0;
                                        }
                                        detalleOriginal.Add(detalle);
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                }

                #endregion
                #region Distinct x AreaFisica(Campo)
                List<string> distinctAreaFisica = (from c in detalleOriginal select c.AreaFisicaID.Value).Distinct().ToList();
                foreach (string area in distinctAreaFisica)
                {
                    DTO_plPresupuestoPxQDeta deta = new DTO_plPresupuestoPxQDeta(true);
                    deta.AreaFisicaID.Value = area;
                    deta.AreaFisicaDesc.Value = detalleOriginal.Find(x => x.AreaFisicaID.Value == area).AreaFisicaDesc.Value;
                    deta.PresupuestoLoc.Value = detalleOriginal.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.PresupuestoLoc.Value);
                    deta.PresupuestoExt.Value = detalleOriginal.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.PresupuestoExt.Value);
                    deta.Detalle.AddRange(detalleOriginal.Where(x => x.AreaFisicaID.Value == area));
                    deta.Detalle = deta.Detalle.OrderBy(x => x.RecursoID.Value).ToList();
                    detListFinal.Add(deta);
                }
                detListFinal = detListFinal.OrderBy(x => x.AreaFisicaID.Value).ToList();
                #endregion

                presupuestoPxQ.DetallesPxQ = detListFinal;
                return presupuestoPxQ;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PresupuestoPxQ_GetPresupuestoPxQ");
                return null;
            }
        }

        /// <summary>
        /// Envia para aprobacion 
        /// </summary>
        /// <param name="currentMod">Modulo que esta ejecutando la operacion</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="userId">Usuario que ejecuta la transaccion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject PresupuestoPxQ_SendToAprob(int documentID, int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PresupuestoPxQ_SendToAprob");
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
        /// Trae la informacion de los presupuestos PxQ con un filtro
        /// </summary>
        /// <param name="numeroDoc">identificador del documento</param>
        /// <param name="validCantConsolid">indica si valida la cantidad consolidada</param>
        /// <returns>lista de detalles</returns>
        private List<DTO_plPresupuestoPxQDeta> plPresupuestoPxQDeta_GetByNumeroDoc(int numeroDoc, bool validCantConsolid)
        {
            try
            {
                this._dal_plPresupuestoPxQDeta = (DAL_plPresupuestoPxQDeta)base.GetInstance(typeof(DAL_plPresupuestoPxQDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_plPresupuestoPxQDeta> presupuestoDeta = this._dal_plPresupuestoPxQDeta.DAL_plPresupuestoPxQDeta_Get(numeroDoc);
                if (validCantConsolid)
                {
                    foreach (DTO_plPresupuestoPxQDeta pxqDeta in presupuestoDeta)
                    {
                        DTO_plPresupuestoPxQ pxqFilter = new DTO_plPresupuestoPxQ();
                        pxqFilter.Ano.Value = pxqDeta.Ano.Value;
                        pxqFilter.ProyectoID.Value = pxqDeta.ProyectoID.Value;
                        pxqFilter.CentroCostoID.Value = pxqDeta.CentroCostoID.Value;
                        pxqFilter.LineaPresupuestoID.Value = pxqDeta.LineaPresupuestoID.Value;
                        pxqFilter.CodigoBSID.Value = pxqDeta.CodigoBSID.Value;
                        pxqFilter.inReferenciaID.Value = pxqDeta.inReferenciaID.Value;
                        if ( pxqDeta.CantidadPRELoc.Value != 0 || pxqDeta.CantidadPREExt.Value != 0)
                        {
                            //Obtiene las cantidades consolidadas(plPresupuestoPxQ)
                            List<DTO_plPresupuestoPxQ> presupuestoPxQ = this.plPresupuestoPxQ_GetByParameter(pxqFilter);
                            if(presupuestoPxQ.Count > 0)
                            {
                                pxqDeta.CantidadPRELoc.Value = presupuestoPxQ.Sum(x => x.CantidadPRELoc.Value);
                                pxqDeta.CantidadPREExt.Value = presupuestoPxQ.Sum(x => x.CantidadPREExt.Value);
                            }
                        }
                    }
                }
                return presupuestoDeta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plPresupuestoPxQDeta_GetByNumeroDoc");
                return null;
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoPxQ> plPresupuestoPxQ_GetByParameter(DTO_plPresupuestoPxQ filter)
        {
            try
            {
                this._dal_plPresupuestoPxQ = (DAL_plPresupuestoPxQ)base.GetInstance(typeof(DAL_plPresupuestoPxQ), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_plPresupuestoPxQ> presupuestoPxQ = this._dal_plPresupuestoPxQ.DAL_plPresupuestoPxQ_GetByParameter(filter);

                return presupuestoPxQ;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_plPresupuestoPxQ_GetByParameter");
                return null;
            }
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
        public DTO_TxResult Planeacion_Revertir(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls, ref List<DTO_coComprobante> coComps, bool insideAnotherTx)
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
            List<DTO_plPresupuestoDeta> listPresDeta = null;
            DTO_plPresupuestoDeta presDeta = null;

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
                this._dal_plPresupuestoDocu = (DAL_plPresupuestoDocu)base.GetInstance(typeof(DAL_plPresupuestoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Trae detalle del Documento con un filtro
                DTO_glDocumentoControl ctrlSelect = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                #endregion

                if (ctrlSelect.DocumentoID.Value == AppDocuments.Presupuesto)
                {
                    #region Trae documentos relacionados con el Presupuesto Inicial
                    List<DTO_plPresupuestoDocu> listNumeroDocRel = this._dal_plPresupuestoDocu.DAL_plPresupuestoDocu_GetByNumeroDocPresup(numeroDoc);
                    //Recorre los docs relacionados con el Presupuesto
                    foreach (DTO_plPresupuestoDocu num in listNumeroDocRel)
                    {
                        if (num.NumeroDoc.Value != num.NumeroDocPresup.Value)
                        {
                            DTO_glDocumentoControl ctrlRel = this._moduloGlobal.glDocumentoControl_GetByID(num.NumeroDoc.Value.Value);
                            if (ctrlRel.Estado.Value != (byte)EstadoDocControl.Revertido && ctrlRel.Estado.Value != (byte)EstadoDocControl.Anulado)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = ctrlRel.Descripcion.Value;
                                rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrlRel.NumeroDoc.Value.ToString();
                                rd.DetailsFields.Add(rdF);
                            }
                        }
                    }
                    #endregion
                    if (rd.DetailsFields.Count > 0)
                    {
                        result.Details.Add(rd);
                        result.Result = ResultValue.NOK;
                        return result;
                    }
                }
                #region Revierte el documento
                result = this._moduloGlobal.glDocumentoControl_Revertir(ctrlSelect.DocumentoID.Value.Value, numeroDoc, consecutivoPos, ref ctrlsReversion, ref coCompsReversion, true);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion

                #region Guarda el Docu y el Detalle consolidando el Total del Presupuesto
                this._dal_plPresupuestoDeta = (DAL_plPresupuestoDeta)base.GetInstance(typeof(DAL_plPresupuestoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plPresupuestoTotal = (DAL_plPresupuestoTotal)base.GetInstance(typeof(DAL_plPresupuestoTotal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                foreach (DTO_glDocumentoControl doc in ctrlsReversion)
                {
                    #region Guarda en plPresupuestoDocu
                    DTO_plPresupuestoDocu docu = new DTO_plPresupuestoDocu();
                    docu.NumeroDoc.Value = doc.NumeroDoc.Value;
                    docu.NumeroDocPresup.Value = numeroDoc;
                    this._dal_plPresupuestoDocu.DAL_plPresupuestoDocu_Add(docu);
                    #endregion
                    listPresDeta = this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_Get(numeroDoc);
                    bool validateSaldoMensual = true;
                    foreach (DTO_plPresupuestoDeta det in listPresDeta)
                    {
                        #region Guarda detalle (plPresupuestoDeta)
                        det.NumeroDoc.Value = doc.NumeroDoc.Value;
                        det.ValorLoc00.Value = det.ValorLoc00.Value * (-1);
                        det.ValorLoc01.Value = det.ValorLoc01.Value * (-1);
                        det.ValorLoc02.Value = det.ValorLoc02.Value * (-1);
                        det.ValorLoc03.Value = det.ValorLoc03.Value * (-1);
                        det.ValorLoc04.Value = det.ValorLoc04.Value * (-1);
                        det.ValorLoc05.Value = det.ValorLoc05.Value * (-1);
                        det.ValorLoc06.Value = det.ValorLoc06.Value * (-1);
                        det.ValorLoc07.Value = det.ValorLoc07.Value * (-1);
                        det.ValorLoc08.Value = det.ValorLoc08.Value * (-1);
                        det.ValorLoc09.Value = det.ValorLoc09.Value * (-1);
                        det.ValorLoc10.Value = det.ValorLoc10.Value * (-1);
                        det.ValorLoc11.Value = det.ValorLoc11.Value * (-1);
                        det.ValorLoc12.Value = det.ValorLoc12.Value * (-1);
                        det.EquivExt00.Value = det.EquivExt00.Value * (-1);
                        det.EquivExt01.Value = det.EquivExt01.Value * (-1);
                        det.EquivExt02.Value = det.EquivExt02.Value * (-1);
                        det.EquivExt03.Value = det.EquivExt03.Value * (-1);
                        det.EquivExt04.Value = det.EquivExt04.Value * (-1);
                        det.EquivExt05.Value = det.EquivExt05.Value * (-1);
                        det.EquivExt06.Value = det.EquivExt06.Value * (-1);
                        det.EquivExt07.Value = det.EquivExt07.Value * (-1);
                        det.EquivExt08.Value = det.EquivExt08.Value * (-1);
                        det.EquivExt09.Value = det.EquivExt09.Value * (-1);
                        det.EquivExt10.Value = det.EquivExt10.Value * (-1);
                        det.EquivExt11.Value = det.EquivExt11.Value * (-1);
                        det.EquivExt12.Value = det.EquivExt12.Value * (-1);
                        det.ValorExt00.Value = det.ValorExt00.Value * (-1);
                        det.ValorExt01.Value = det.ValorExt01.Value * (-1);
                        det.ValorExt02.Value = det.ValorExt02.Value * (-1);
                        det.ValorExt03.Value = det.ValorExt03.Value * (-1);
                        det.ValorExt04.Value = det.ValorExt04.Value * (-1);
                        det.ValorExt05.Value = det.ValorExt05.Value * (-1);
                        det.ValorExt06.Value = det.ValorExt06.Value * (-1);
                        det.ValorExt07.Value = det.ValorExt07.Value * (-1);
                        det.ValorExt08.Value = det.ValorExt08.Value * (-1);
                        det.ValorExt09.Value = det.ValorExt09.Value * (-1);
                        det.ValorExt10.Value = det.ValorExt10.Value * (-1);
                        det.ValorExt11.Value = det.ValorExt11.Value * (-1);
                        det.ValorExt12.Value = det.ValorExt12.Value * (-1);
                        det.EquivLoc00.Value = det.EquivLoc00.Value * (-1);
                        det.EquivLoc01.Value = det.EquivLoc01.Value * (-1);
                        det.EquivLoc02.Value = det.EquivLoc02.Value * (-1);
                        det.EquivLoc03.Value = det.EquivLoc03.Value * (-1);
                        det.EquivLoc04.Value = det.EquivLoc04.Value * (-1);
                        det.EquivLoc05.Value = det.EquivLoc05.Value * (-1);
                        det.EquivLoc06.Value = det.EquivLoc06.Value * (-1);
                        det.EquivLoc07.Value = det.EquivLoc07.Value * (-1);
                        det.EquivLoc08.Value = det.EquivLoc08.Value * (-1);
                        det.EquivLoc09.Value = det.EquivLoc09.Value * (-1);
                        det.EquivLoc10.Value = det.EquivLoc10.Value * (-1);
                        det.EquivLoc11.Value = det.EquivLoc11.Value * (-1);
                        det.EquivLoc12.Value = det.EquivLoc12.Value * (-1);
                        this._dal_plPresupuestoDeta.DAL_plPresupuestoDeta_AddItem(det);
                        #endregion
                        #region Guarda el detalle consolidado (PresupuestoTotal)
                        DTO_plPresupuestoTotal total = new DTO_plPresupuestoTotal();
                        total.Ano.Value = doc.PeriodoDoc.Value.Value.Year;
                        total.ProyectoID.Value = det.ProyectoID.Value;
                        total.CentroCostoID.Value = det.CentroCostoID.Value;
                        total.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                        total.ValorLoc00.Value = det.ValorLoc00.Value;
                        total.ValorLoc01.Value = det.ValorLoc01.Value;
                        total.ValorLoc02.Value = det.ValorLoc02.Value;
                        total.ValorLoc03.Value = det.ValorLoc03.Value;
                        total.ValorLoc04.Value = det.ValorLoc04.Value;
                        total.ValorLoc05.Value = det.ValorLoc05.Value;
                        total.ValorLoc06.Value = det.ValorLoc06.Value;
                        total.ValorLoc07.Value = det.ValorLoc07.Value;
                        total.ValorLoc08.Value = det.ValorLoc08.Value;
                        total.ValorLoc09.Value = det.ValorLoc09.Value;
                        total.ValorLoc10.Value = det.ValorLoc10.Value;
                        total.ValorLoc11.Value = det.ValorLoc11.Value;
                        total.ValorLoc12.Value = det.ValorLoc12.Value;
                        total.EquivExt00.Value = det.EquivExt00.Value;
                        total.EquivExt01.Value = det.EquivExt01.Value;
                        total.EquivExt02.Value = det.EquivExt02.Value;
                        total.EquivExt03.Value = det.EquivExt03.Value;
                        total.EquivExt04.Value = det.EquivExt04.Value;
                        total.EquivExt05.Value = det.EquivExt05.Value;
                        total.EquivExt06.Value = det.EquivExt06.Value;
                        total.EquivExt07.Value = det.EquivExt07.Value;
                        total.EquivExt08.Value = det.EquivExt08.Value;
                        total.EquivExt09.Value = det.EquivExt09.Value;
                        total.EquivExt10.Value = det.EquivExt10.Value;
                        total.EquivExt11.Value = det.EquivExt11.Value;
                        total.EquivExt11.Value = det.EquivExt11.Value;
                        total.EquivExt12.Value = det.EquivExt11.Value;
                        total.ValorExt00.Value = det.ValorExt00.Value;
                        total.ValorExt01.Value = det.ValorExt01.Value;
                        total.ValorExt02.Value = det.ValorExt02.Value;
                        total.ValorExt03.Value = det.ValorExt03.Value;
                        total.ValorExt04.Value = det.ValorExt04.Value;
                        total.ValorExt05.Value = det.ValorExt05.Value;
                        total.ValorExt06.Value = det.ValorExt06.Value;
                        total.ValorExt07.Value = det.ValorExt07.Value;
                        total.ValorExt08.Value = det.ValorExt08.Value;
                        total.ValorExt09.Value = det.ValorExt09.Value;
                        total.ValorExt10.Value = det.ValorExt10.Value;
                        total.ValorExt11.Value = det.ValorExt11.Value;
                        total.ValorExt12.Value = det.ValorExt12.Value;
                        total.EquivLoc00.Value = det.EquivLoc00.Value;
                        total.EquivLoc01.Value = det.EquivLoc01.Value;
                        total.EquivLoc02.Value = det.EquivLoc02.Value;
                        total.EquivLoc03.Value = det.EquivLoc03.Value;
                        total.EquivLoc04.Value = det.EquivLoc04.Value;
                        total.EquivLoc05.Value = det.EquivLoc05.Value;
                        total.EquivLoc06.Value = det.EquivLoc06.Value;
                        total.EquivLoc07.Value = det.EquivLoc07.Value;
                        total.EquivLoc08.Value = det.EquivLoc08.Value;
                        total.EquivLoc09.Value = det.EquivLoc09.Value;
                        total.EquivLoc10.Value = det.EquivLoc10.Value;
                        total.EquivLoc11.Value = det.EquivLoc11.Value;
                        total.EquivLoc12.Value = det.EquivLoc12.Value;
                        this._dal_plPresupuestoTotal.DAL_plPresupuestoTotal_Add(total, ref validateSaldoMensual);
                        if (!validateSaldoMensual)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Pl_SaldoMensualNotAvailable + "&&" + total.CentroCostoID.Value + "&&" + total.LineaPresupuestoID.Value;
                            return result;
                        }
                        #endregion
                    }

                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Planeacion_Revertir");
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

        #region Procesos

        /// <summary>
        /// Ejecutar proceso de cierre Legalizacion en Planeacion 
        /// </summary>
        /// <param name="periodo">Periodo de Cierre</param>
        public DTO_TxResult Proceso_plCierreLegalizacion_Cierre(DateTime periodo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this._dal_plCierreLegalizacion = (DAL_plCierreLegalizacion)base.GetInstance(typeof(DAL_plCierreLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_plCierreLegalizacion.DAL_plCierreLegalizacion_ProcesoCierre(periodo);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_plCierreLegalizacion_Cierre");
                return result;
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de presupuesto de cierre mensual
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="tipoInforme">Tipo de Informe</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryInformeMensualCierre> plCierreLegalizacion_GetInfoMensual(int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            try
            {
                this._dal_plCierreLegalizacion = (DAL_plCierreLegalizacion)base.GetInstance(typeof(DAL_plCierreLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_QueryInformeMensualCierre> listCierreLegFinal = new List<DTO_QueryInformeMensualCierre>();
                List<string> distinctRomp1 = new List<string>();
                List<string> distinctRomp2 = new List<string>();
                List<string> distinctRomp3 = new List<string>();

                List<DTO_QueryInformeMensualCierre> listCierreLegalizacion = this._dal_plCierreLegalizacion.DAL_plCierreLegalizacion_GetInfoMensual(filter, tipoInforme, proType, tipoMda, mdaOrigen);

                #region Asigna valores validados con el ProyectoTipo
                if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                {
                    this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (DTO_QueryInformeMensualCierre cierre in listCierreLegalizacion)
                    {
                        #region Trae el RecursoID(cambia el que viene en la lista(plLineaPresupuesto.RecursoID))
                        #region Crea consulta para la tabla plActividadLineaPresupuestal
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
                    foreach (DTO_QueryInformeMensualCierre cierre in listCierreLegalizacion)
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

                #region Realiza el 1º Rompimiento
                #region Obtiene IDs no duplicados
                if (proType == ProyectoTipo.Opex)
                    distinctRomp1 = (from c in listCierreLegalizacion select c.ActividadID.Value).Distinct().ToList();
                else if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                    distinctRomp1 = (from c in listCierreLegalizacion select c.Grupo.Value).Distinct().ToList();
                else
                    distinctRomp1 = (from c in listCierreLegalizacion select c.ProyectoID.Value).Distinct().ToList();
                #endregion

                foreach (string IDRomp1 in distinctRomp1.Where(x => x != string.Empty))
                {
                    #region Declara variables
                    List<DTO_QueryInformeMensualCierre> detalleNivel1 = new List<DTO_QueryInformeMensualCierre>();
                    DTO_QueryInformeMensualCierre dtoCierreLegFinal = new DTO_QueryInformeMensualCierre(true);
                    #endregion
                    #region Asigna valores y detalle 1º Rompimiento
                    if (proType == ProyectoTipo.Opex)
                    {
                        DTO_coActividad dtoActiv = (DTO_coActividad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, IDRomp1, true, false);
                        dtoCierreLegFinal.ActividadID.Value = dtoActiv.ID.Value;
                        dtoCierreLegFinal.Descriptivo.Value = dtoActiv.Descriptivo.Value;
                        detalleNivel1.AddRange(listCierreLegalizacion.Where(x => x.ActividadID.Value == IDRomp1));
                    }
                    else if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                    {
                        DTO_MasterBasic dtoRecursoGrupo = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecursoGrupo, IDRomp1, true, false);
                        dtoCierreLegFinal.Grupo.Value = dtoRecursoGrupo.ID.Value;
                        dtoCierreLegFinal.Descriptivo.Value = dtoRecursoGrupo.Descriptivo.Value;
                        detalleNivel1.AddRange(listCierreLegalizacion.Where(x => x.Grupo.Value == IDRomp1));
                    }
                    else
                    {
                        DTO_coProyecto dtoProyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, IDRomp1, true, false);
                        dtoCierreLegFinal.ProyectoID.Value = dtoProyecto.ID.Value;
                        dtoCierreLegFinal.Descriptivo.Value = dtoProyecto.Descriptivo.Value;
                        detalleNivel1.AddRange(listCierreLegalizacion.Where(x => x.ProyectoID.Value == IDRomp1));
                    }

                    #endregion
                    #region  Realiza el 2º Rompimiento
                    List<DTO_QueryInformeMensualCierre> detalleNivel1Tmp = new List<DTO_QueryInformeMensualCierre>();
                    #region Opex-Capex-Inversion
                    if (proType == ProyectoTipo.Opex || proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                    {
                        distinctRomp2 = (from c in detalleNivel1 select c.RecursoID.Value).Distinct().ToList();
                        foreach (string IDRomp2 in distinctRomp2.Where(x => x != string.Empty))
                        {
                            DTO_QueryInformeMensualCierre dtoDetalleNivel1Tmp = new DTO_QueryInformeMensualCierre(true);
                            foreach (DTO_QueryInformeMensualCierre detRomp2 in detalleNivel1.Where(x => x.RecursoID.Value == IDRomp2))
                            {
                                #region Acumula valores
                                dtoDetalleNivel1Tmp.RecursoID.Value = detRomp2.RecursoID.Value;
                                dtoDetalleNivel1Tmp.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                dtoDetalleNivel1Tmp.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                dtoDetalleNivel1Tmp.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                dtoDetalleNivel1Tmp.SaldoInicial.Value += detRomp2.SaldoInicial.Value;
                                dtoDetalleNivel1Tmp.SaldoEnero.Value += detRomp2.SaldoEnero.Value.Value;
                                dtoDetalleNivel1Tmp.SaldoFebrero.Value += detRomp2.SaldoFebrero.Value;
                                dtoDetalleNivel1Tmp.SaldoMarzo.Value += detRomp2.SaldoMarzo.Value;
                                dtoDetalleNivel1Tmp.SaldoAbril.Value += detRomp2.SaldoAbril.Value;
                                dtoDetalleNivel1Tmp.SaldoMayo.Value += detRomp2.SaldoMayo.Value;
                                dtoDetalleNivel1Tmp.SaldoJunio.Value += detRomp2.SaldoJunio.Value;
                                dtoDetalleNivel1Tmp.SaldoJulio.Value += detRomp2.SaldoJulio.Value;
                                dtoDetalleNivel1Tmp.SaldoAgosto.Value += detRomp2.SaldoAgosto.Value;
                                dtoDetalleNivel1Tmp.SaldoSeptiembre.Value += detRomp2.SaldoSeptiembre.Value;
                                dtoDetalleNivel1Tmp.SaldoOctubre.Value += detRomp2.SaldoOctubre.Value;
                                dtoDetalleNivel1Tmp.SaldoNoviembre.Value += detRomp2.SaldoNoviembre.Value;
                                dtoDetalleNivel1Tmp.SaldoDiciembre.Value += detRomp2.SaldoDiciembre.Value;
                                dtoDetalleNivel1Tmp.SaldoFinal.Value += detRomp2.SaldoFinal.Value;
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel1.Where(x => x.RecursoID.Value == IDRomp2).ToList();
                                #endregion
                                #region Realiza el 3º Rompimiento
                                List<DTO_QueryInformeMensualCierre> detalleNivel2Tmp = new List<DTO_QueryInformeMensualCierre>();
                                distinctRomp3 = (from c in dtoDetalleNivel1Tmp.DetalleNivel2 select c.LineaPresupuestoID.Value).Distinct().ToList();
                                foreach (string IDRomp3 in distinctRomp3.Where(x => x != string.Empty))
                                {
                                    DTO_QueryInformeMensualCierre dtoDetalleNivel2Tmp = new DTO_QueryInformeMensualCierre(true);
                                    foreach (DTO_QueryInformeMensualCierre detRomp3 in dtoDetalleNivel1Tmp.DetalleNivel2.Where(x => x.LineaPresupuestoID.Value == IDRomp3))
                                    {
                                        #region Acumula valores
                                        dtoDetalleNivel2Tmp.RecursoID.Value = detRomp3.RecursoID.Value;
                                        dtoDetalleNivel2Tmp.ProyectoID.Value = detRomp3.ProyectoID.Value;
                                        dtoDetalleNivel2Tmp.CentroCostoID.Value = detRomp3.CentroCostoID.Value;
                                        dtoDetalleNivel2Tmp.LineaPresupuestoID.Value = detRomp3.LineaPresupuestoID.Value;
                                        dtoDetalleNivel2Tmp.SaldoInicial.Value += detRomp3.SaldoInicial.Value;
                                        dtoDetalleNivel2Tmp.SaldoEnero.Value += detRomp3.SaldoEnero.Value.Value;
                                        dtoDetalleNivel2Tmp.SaldoFebrero.Value += detRomp3.SaldoFebrero.Value;
                                        dtoDetalleNivel2Tmp.SaldoMarzo.Value += detRomp3.SaldoMarzo.Value;
                                        dtoDetalleNivel2Tmp.SaldoAbril.Value += detRomp3.SaldoAbril.Value;
                                        dtoDetalleNivel2Tmp.SaldoMayo.Value += detRomp3.SaldoMayo.Value;
                                        dtoDetalleNivel2Tmp.SaldoJunio.Value += detRomp3.SaldoJunio.Value;
                                        dtoDetalleNivel2Tmp.SaldoJulio.Value += detRomp3.SaldoJulio.Value;
                                        dtoDetalleNivel2Tmp.SaldoAgosto.Value += detRomp3.SaldoAgosto.Value;
                                        dtoDetalleNivel2Tmp.SaldoSeptiembre.Value += detRomp3.SaldoSeptiembre.Value;
                                        dtoDetalleNivel2Tmp.SaldoOctubre.Value += detRomp3.SaldoOctubre.Value;
                                        dtoDetalleNivel2Tmp.SaldoNoviembre.Value += detRomp3.SaldoNoviembre.Value;
                                        dtoDetalleNivel2Tmp.SaldoDiciembre.Value += detRomp3.SaldoDiciembre.Value;
                                        dtoDetalleNivel2Tmp.SaldoFinal.Value += detRomp3.SaldoFinal.Value;
                                        #endregion
                                    }
                                    detalleNivel2Tmp.Add(dtoDetalleNivel2Tmp);
                                }
                                #endregion
                                //Reasigna el detalle 3 Rompimiento
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel2Tmp.OrderBy(x => x.LineaPresupuestoID.Value).ToList();
                            }
                            detalleNivel1Tmp.Add(dtoDetalleNivel1Tmp);
                        }
                        //Reasigna el detalle del 2º Rompimiento
                        detalleNivel1 = detalleNivel1Tmp.OrderBy(x => x.RecursoID.Value).ToList();
                    }
                    #endregion
                    #region Otros TiposProyecto
                    else
                    {
                        distinctRomp2 = (from c in detalleNivel1 select c.CentroCostoID.Value).Distinct().ToList();
                        foreach (string IDRomp2 in distinctRomp2.Where(x => x != string.Empty))
                        {
                            DTO_QueryInformeMensualCierre dtoDetalleNivel1Tmp = new DTO_QueryInformeMensualCierre(true);
                            foreach (DTO_QueryInformeMensualCierre detRomp2 in detalleNivel1.Where(x => x.CentroCostoID.Value == IDRomp2))
                            {
                                #region Acumula valores
                                dtoDetalleNivel1Tmp.RecursoID.Value = detRomp2.RecursoID.Value;
                                dtoDetalleNivel1Tmp.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                dtoDetalleNivel1Tmp.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                dtoDetalleNivel1Tmp.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                dtoDetalleNivel1Tmp.SaldoInicial.Value += detRomp2.SaldoInicial.Value;
                                dtoDetalleNivel1Tmp.SaldoEnero.Value += detRomp2.SaldoEnero.Value.Value;
                                dtoDetalleNivel1Tmp.SaldoFebrero.Value += detRomp2.SaldoFebrero.Value;
                                dtoDetalleNivel1Tmp.SaldoMarzo.Value += detRomp2.SaldoMarzo.Value;
                                dtoDetalleNivel1Tmp.SaldoAbril.Value += detRomp2.SaldoAbril.Value;
                                dtoDetalleNivel1Tmp.SaldoMayo.Value += detRomp2.SaldoMayo.Value;
                                dtoDetalleNivel1Tmp.SaldoJunio.Value += detRomp2.SaldoJunio.Value;
                                dtoDetalleNivel1Tmp.SaldoJulio.Value += detRomp2.SaldoJulio.Value;
                                dtoDetalleNivel1Tmp.SaldoAgosto.Value += detRomp2.SaldoAgosto.Value;
                                dtoDetalleNivel1Tmp.SaldoSeptiembre.Value += detRomp2.SaldoSeptiembre.Value;
                                dtoDetalleNivel1Tmp.SaldoOctubre.Value += detRomp2.SaldoOctubre.Value;
                                dtoDetalleNivel1Tmp.SaldoNoviembre.Value += detRomp2.SaldoNoviembre.Value;
                                dtoDetalleNivel1Tmp.SaldoDiciembre.Value += detRomp2.SaldoDiciembre.Value;
                                dtoDetalleNivel1Tmp.SaldoFinal.Value += detRomp2.SaldoFinal.Value;
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel1.Where(x => x.CentroCostoID.Value == IDRomp2).ToList();
                                #endregion
                                #region Realiza el 3º Rompimiento
                                List<DTO_QueryInformeMensualCierre> detalleNivel2Tmp = new List<DTO_QueryInformeMensualCierre>();
                                distinctRomp3 = (from c in dtoDetalleNivel1Tmp.DetalleNivel2 select c.LineaPresupuestoID.Value).Distinct().ToList();
                                foreach (string IDRomp3 in distinctRomp3.Where(x => x != string.Empty))
                                {
                                    DTO_QueryInformeMensualCierre dtoDetalleNivel2Tmp = new DTO_QueryInformeMensualCierre(true);
                                    foreach (DTO_QueryInformeMensualCierre detRomp3 in dtoDetalleNivel1Tmp.DetalleNivel2.Where(x => x.LineaPresupuestoID.Value == IDRomp3))
                                    {
                                        #region Acumula valores
                                        dtoDetalleNivel2Tmp.RecursoID.Value = detRomp3.RecursoID.Value;
                                        dtoDetalleNivel2Tmp.ProyectoID.Value = detRomp3.ProyectoID.Value;
                                        dtoDetalleNivel2Tmp.CentroCostoID.Value = detRomp3.CentroCostoID.Value;
                                        dtoDetalleNivel2Tmp.LineaPresupuestoID.Value = detRomp3.LineaPresupuestoID.Value;
                                        dtoDetalleNivel2Tmp.SaldoInicial.Value += detRomp3.SaldoInicial.Value;
                                        dtoDetalleNivel2Tmp.SaldoEnero.Value += detRomp3.SaldoEnero.Value.Value;
                                        dtoDetalleNivel2Tmp.SaldoFebrero.Value += detRomp3.SaldoFebrero.Value;
                                        dtoDetalleNivel2Tmp.SaldoMarzo.Value += detRomp3.SaldoMarzo.Value;
                                        dtoDetalleNivel2Tmp.SaldoAbril.Value += detRomp3.SaldoAbril.Value;
                                        dtoDetalleNivel2Tmp.SaldoMayo.Value += detRomp3.SaldoMayo.Value;
                                        dtoDetalleNivel2Tmp.SaldoJunio.Value += detRomp3.SaldoJunio.Value;
                                        dtoDetalleNivel2Tmp.SaldoJulio.Value += detRomp3.SaldoJulio.Value;
                                        dtoDetalleNivel2Tmp.SaldoAgosto.Value += detRomp3.SaldoAgosto.Value;
                                        dtoDetalleNivel2Tmp.SaldoSeptiembre.Value += detRomp3.SaldoSeptiembre.Value;
                                        dtoDetalleNivel2Tmp.SaldoOctubre.Value += detRomp3.SaldoOctubre.Value;
                                        dtoDetalleNivel2Tmp.SaldoNoviembre.Value += detRomp3.SaldoNoviembre.Value;
                                        dtoDetalleNivel2Tmp.SaldoDiciembre.Value += detRomp3.SaldoDiciembre.Value;
                                        dtoDetalleNivel2Tmp.SaldoFinal.Value += detRomp3.SaldoFinal.Value;
                                        #endregion
                                    }
                                    detalleNivel2Tmp.Add(dtoDetalleNivel2Tmp);
                                }
                                #endregion
                                //Reasigna el detalle 3 Rompimiento
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel2Tmp.OrderBy(x => x.LineaPresupuestoID.Value).ToList();
                            }
                            detalleNivel1Tmp.Add(dtoDetalleNivel1Tmp);
                        }
                        //Reasigna el detalle del 2º Rompimiento
                        detalleNivel1 = detalleNivel1Tmp.OrderBy(x => x.CentroCostoID.Value).ToList();
                    }
                    #endregion
                    #endregion

                    #region Asigna valores de saldos segun detalle
                    foreach (DTO_QueryInformeMensualCierre saldo in detalleNivel1)
                    {
                        #region Acumula saldos
                        dtoCierreLegFinal.SaldoInicial.Value += saldo.SaldoInicial.Value;
                        dtoCierreLegFinal.SaldoEnero.Value += saldo.SaldoEnero.Value;
                        dtoCierreLegFinal.SaldoFebrero.Value += saldo.SaldoFebrero.Value;
                        dtoCierreLegFinal.SaldoMarzo.Value += saldo.SaldoMarzo.Value;
                        dtoCierreLegFinal.SaldoAbril.Value += saldo.SaldoAbril.Value;
                        dtoCierreLegFinal.SaldoMayo.Value += saldo.SaldoMayo.Value;
                        dtoCierreLegFinal.SaldoJunio.Value += saldo.SaldoJunio.Value;
                        dtoCierreLegFinal.SaldoJulio.Value += saldo.SaldoJulio.Value;
                        dtoCierreLegFinal.SaldoAgosto.Value += saldo.SaldoAgosto.Value;
                        dtoCierreLegFinal.SaldoSeptiembre.Value += saldo.SaldoSeptiembre.Value;
                        dtoCierreLegFinal.SaldoOctubre.Value += saldo.SaldoOctubre.Value;
                        dtoCierreLegFinal.SaldoNoviembre.Value += saldo.SaldoNoviembre.Value;
                        dtoCierreLegFinal.SaldoDiciembre.Value += saldo.SaldoDiciembre.Value;
                        dtoCierreLegFinal.SaldoFinal.Value += saldo.SaldoFinal.Value;
                        #endregion
                        #region Asigna Descripcion 2º Rompimiento
                        if (proType == ProyectoTipo.Opex || proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                        {
                            DTO_plRecurso dtoRecurso = (DTO_plRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecurso, saldo.RecursoID.Value, true, false);
                            saldo.Descriptivo.Value = dtoRecurso.Descriptivo.Value;
                        }
                        else
                        {
                            DTO_coCentroCosto dtoCentroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, saldo.CentroCostoID.Value, true, false);
                            saldo.Descriptivo.Value = dtoCentroCosto.Descriptivo.Value;
                        }
                        #endregion
                        #region Asigna Descripcion 3º Rompimiento
                        foreach (DTO_QueryInformeMensualCierre saldoRomp3 in saldo.DetalleNivel2)
                        {
                            DTO_plLineaPresupuesto dtoLineaPresup = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, saldoRomp3.LineaPresupuestoID.Value, true, false);
                            saldoRomp3.Descriptivo.Value = dtoLineaPresup.Descriptivo.Value;
                        }
                        #endregion
                    }
                    #endregion
                    #region Asigna Detalle final
                    if (proType == ProyectoTipo.Opex)
                        dtoCierreLegFinal.DetalleNivel1 = detalleNivel1.OrderBy(x => x.ActividadID.Value).ToList();
                    else if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                        dtoCierreLegFinal.DetalleNivel1 = detalleNivel1.OrderBy(x => x.Grupo.Value).ToList();
                    else
                        dtoCierreLegFinal.DetalleNivel1 = detalleNivel1.OrderBy(x => x.ProyectoID.Value).ToList();
                    listCierreLegFinal.Add(dtoCierreLegFinal);
                    #endregion
                }
                #endregion

                return listCierreLegFinal;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plCierreLegalizacion_GetInfoMensual");
                return null;
            }
        }

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de Estado de presupuesto por periodo
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryEstadoEjecucion> plCierreLegalizacion_GetEstadoEjecByPeriodo(int documentID, DTO_QueryEstadoEjecucion filter, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            try
            {
                this._dal_plCierreLegalizacion = (DAL_plCierreLegalizacion)base.GetInstance(typeof(DAL_plCierreLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_QueryEstadoEjecucion> listCierreLegFinal = new List<DTO_QueryEstadoEjecucion>();
                List<string> distinctRomp1 = new List<string>();
                List<string> distinctRomp2 = new List<string>();
                List<string> distinctRomp3 = new List<string>();

                List<DTO_QueryEstadoEjecucion> listCierreLegalizacion = this._dal_plCierreLegalizacion.DAL_plCierreLegalizacion_GetEstadoEjecByPeriodo(filter, proType, tipoMda, mdaOrigen);

                #region Asigna valores validados con el ProyectoTipo
                if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                {
                    this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (DTO_QueryEstadoEjecucion cierre in listCierreLegalizacion)
                    {
                        #region Trae el RecursoID(cambia el que viene en la lista(plLineaPresupuesto.RecursoID))
                        #region Crea consulta para la tabla plActividadLineaPresupuestal
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
                        #region Calcula el % de Ejecucion
                        if (cierre.Presupuesto.Value != 0)
                            cierre.EjeVsPre.Value = (cierre.Compromisos.Value + cierre.Recibidos.Value + cierre.Ejecutado.Value) * 100 / cierre.Presupuesto.Value;
                        else
                            cierre.EjeVsPre.Value = 100;
                        #endregion
                    }
                }
                else
                {
                    #region El Grupo es igual a la actividad del Proyecto
                    foreach (DTO_QueryEstadoEjecucion cierre in listCierreLegalizacion)
                    {
                        cierre.Grupo.Value = cierre.ActividadID.Value;
                        #region  Calcula el % de Ejecucion
                        if (cierre.Presupuesto.Value != 0)
                            cierre.EjeVsPre.Value = (cierre.Compromisos.Value + cierre.Recibidos.Value + cierre.Ejecutado.Value) * 100 / cierre.Presupuesto.Value;
                        else
                            cierre.EjeVsPre.Value = 100;
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region Realiza el 1º Rompimiento
                #region Obtiene IDs no duplicados
                distinctRomp1 = (from c in listCierreLegalizacion select c.ProyectoID.Value).Distinct().ToList();
                #endregion

                foreach (string IDRomp1 in distinctRomp1.Where(x => x != string.Empty))
                {
                    #region Declara variables
                    List<DTO_QueryEstadoEjecucion> detalleNivel1 = new List<DTO_QueryEstadoEjecucion>();
                    DTO_QueryEstadoEjecucion dtoCierreLegFinal = new DTO_QueryEstadoEjecucion(true);
                    #endregion
                    #region Asigna valores y detalle 1º Rompimiento
                    DTO_coProyecto dtoProyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, IDRomp1, true, false);
                    dtoCierreLegFinal.ProyectoID.Value = dtoProyecto.ID.Value;
                    dtoCierreLegFinal.Descriptivo.Value = dtoProyecto.Descriptivo.Value;
                    detalleNivel1.AddRange(listCierreLegalizacion.Where(x => x.ProyectoID.Value == IDRomp1));
                    #endregion
                    #region  Realiza el 2º Rompimiento
                    List<DTO_QueryEstadoEjecucion> detalleNivel1Tmp = new List<DTO_QueryEstadoEjecucion>();
                    #region Opex-Capex-Inversion
                    if (proType == ProyectoTipo.Opex || proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                    {
                        distinctRomp2 = (from c in detalleNivel1 select c.RecursoID.Value).Distinct().ToList();
                        foreach (string IDRomp2 in distinctRomp2.Where(x => x != string.Empty))
                        {
                            DTO_QueryEstadoEjecucion dtoDetalleNivel1Tmp = new DTO_QueryEstadoEjecucion(true);
                            foreach (DTO_QueryEstadoEjecucion detRomp2 in detalleNivel1.Where(x => x.RecursoID.Value == IDRomp2))
                            {
                                #region Acumula valores
                                dtoDetalleNivel1Tmp.RecursoID.Value = detRomp2.RecursoID.Value;
                                dtoDetalleNivel1Tmp.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                dtoDetalleNivel1Tmp.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                dtoDetalleNivel1Tmp.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                dtoDetalleNivel1Tmp.Disponible.Value += detRomp2.Disponible.Value;
                                dtoDetalleNivel1Tmp.Presupuesto.Value += detRomp2.Presupuesto.Value.Value;
                                dtoDetalleNivel1Tmp.Compromisos.Value += detRomp2.Compromisos.Value;
                                dtoDetalleNivel1Tmp.Recibidos.Value += detRomp2.Recibidos.Value;
                                dtoDetalleNivel1Tmp.Ejecutado.Value += detRomp2.Ejecutado.Value;
                                dtoDetalleNivel1Tmp.EjeVsPre.Value += detRomp2.EjeVsPre.Value;
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel1.Where(x => x.RecursoID.Value == IDRomp2).ToList();
                                #endregion
                                #region Realiza el 3º Rompimiento
                                List<DTO_QueryEstadoEjecucion> detalleNivel2Tmp = new List<DTO_QueryEstadoEjecucion>();
                                distinctRomp3 = (from c in dtoDetalleNivel1Tmp.DetalleNivel2 select c.LineaPresupuestoID.Value).Distinct().ToList();
                                foreach (string IDRomp3 in distinctRomp3.Where(x => x != string.Empty))
                                {
                                    DTO_QueryEstadoEjecucion dtoDetalleNivel2Tmp = new DTO_QueryEstadoEjecucion(true);
                                    foreach (DTO_QueryEstadoEjecucion detRomp3 in dtoDetalleNivel1Tmp.DetalleNivel2.Where(x => x.LineaPresupuestoID.Value == IDRomp3))
                                    {
                                        #region Acumula valores
                                        dtoDetalleNivel2Tmp.RecursoID.Value = detRomp3.RecursoID.Value;
                                        dtoDetalleNivel2Tmp.ProyectoID.Value = detRomp3.ProyectoID.Value;
                                        dtoDetalleNivel2Tmp.CentroCostoID.Value = detRomp3.CentroCostoID.Value;
                                        dtoDetalleNivel2Tmp.LineaPresupuestoID.Value = detRomp3.LineaPresupuestoID.Value;
                                        dtoDetalleNivel2Tmp.Disponible.Value += detRomp3.Disponible.Value;
                                        dtoDetalleNivel2Tmp.Presupuesto.Value += detRomp3.Presupuesto.Value.Value;
                                        dtoDetalleNivel2Tmp.Compromisos.Value += detRomp3.Compromisos.Value;
                                        dtoDetalleNivel2Tmp.Recibidos.Value += detRomp3.Recibidos.Value;
                                        dtoDetalleNivel2Tmp.Ejecutado.Value += detRomp3.Ejecutado.Value;
                                        dtoDetalleNivel2Tmp.EjeVsPre.Value += detRomp3.EjeVsPre.Value;
                                        #endregion
                                    }
                                    detalleNivel2Tmp.Add(dtoDetalleNivel2Tmp);
                                }
                                #endregion
                                //Reasigna el detalle 3 Rompimiento
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel2Tmp.OrderBy(x => x.LineaPresupuestoID.Value).ToList();
                            }
                            detalleNivel1Tmp.Add(dtoDetalleNivel1Tmp);
                        }
                        //Reasigna el detalle 2' rompimiento
                        detalleNivel1 = detalleNivel1Tmp.OrderBy(x => x.RecursoID.Value).ToList();
                    }
                    #endregion
                    #region Otros TiposProyecto
                    else
                    {
                        distinctRomp2 = (from c in detalleNivel1 select c.CentroCostoID.Value).Distinct().ToList();
                        foreach (string IDRomp2 in distinctRomp2.Where(x => x != string.Empty))
                        {
                            DTO_QueryEstadoEjecucion dtoDetalleNivel1Tmp = new DTO_QueryEstadoEjecucion(true);
                            foreach (DTO_QueryEstadoEjecucion detRomp2 in detalleNivel1.Where(x => x.CentroCostoID.Value == IDRomp2))
                            {
                                #region Acumula valores
                                dtoDetalleNivel1Tmp.RecursoID.Value = detRomp2.RecursoID.Value;
                                dtoDetalleNivel1Tmp.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                dtoDetalleNivel1Tmp.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                dtoDetalleNivel1Tmp.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                dtoDetalleNivel1Tmp.Disponible.Value += detRomp2.Disponible.Value;
                                dtoDetalleNivel1Tmp.Presupuesto.Value += detRomp2.Presupuesto.Value.Value;
                                dtoDetalleNivel1Tmp.Compromisos.Value += detRomp2.Compromisos.Value;
                                dtoDetalleNivel1Tmp.Recibidos.Value += detRomp2.Recibidos.Value;
                                dtoDetalleNivel1Tmp.Ejecutado.Value += detRomp2.Ejecutado.Value;
                                dtoDetalleNivel1Tmp.EjeVsPre.Value += detRomp2.EjeVsPre.Value;
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel1.Where(x => x.CentroCostoID.Value == IDRomp2).ToList();
                                #endregion
                                #region Realiza el 3º Rompimiento
                                List<DTO_QueryEstadoEjecucion> detalleNivel2Tmp = new List<DTO_QueryEstadoEjecucion>();
                                distinctRomp3 = (from c in dtoDetalleNivel1Tmp.DetalleNivel2 select c.LineaPresupuestoID.Value).Distinct().ToList();
                                foreach (string IDRomp3 in distinctRomp3.Where(x => x != string.Empty))
                                {
                                    DTO_QueryEstadoEjecucion dtoDetalleNivel2Tmp = new DTO_QueryEstadoEjecucion(true);
                                    foreach (DTO_QueryEstadoEjecucion detRomp3 in dtoDetalleNivel1Tmp.DetalleNivel2.Where(x => x.LineaPresupuestoID.Value == IDRomp3))
                                    {
                                        #region Acumula valores
                                        dtoDetalleNivel2Tmp.RecursoID.Value = detRomp3.RecursoID.Value;
                                        dtoDetalleNivel2Tmp.ProyectoID.Value = detRomp3.ProyectoID.Value;
                                        dtoDetalleNivel2Tmp.CentroCostoID.Value = detRomp3.CentroCostoID.Value;
                                        dtoDetalleNivel2Tmp.LineaPresupuestoID.Value = detRomp3.LineaPresupuestoID.Value;
                                        dtoDetalleNivel2Tmp.Disponible.Value += detRomp3.Disponible.Value;
                                        dtoDetalleNivel2Tmp.Presupuesto.Value += detRomp3.Presupuesto.Value.Value;
                                        dtoDetalleNivel2Tmp.Compromisos.Value += detRomp3.Compromisos.Value;
                                        dtoDetalleNivel2Tmp.Recibidos.Value += detRomp3.Recibidos.Value;
                                        dtoDetalleNivel2Tmp.Ejecutado.Value += detRomp3.Ejecutado.Value;
                                        dtoDetalleNivel2Tmp.EjeVsPre.Value += detRomp3.EjeVsPre.Value;
                                        #endregion
                                    }
                                    detalleNivel2Tmp.Add(dtoDetalleNivel2Tmp);
                                }
                                #endregion
                                //Reasigna el detalle 3 Rompimiento
                                dtoDetalleNivel1Tmp.DetalleNivel2 = detalleNivel2Tmp.OrderBy(x => x.LineaPresupuestoID.Value).ToList();
                            }
                            detalleNivel1Tmp.Add(dtoDetalleNivel1Tmp);
                        }
                        //Reasigna el detalle 2' rompimiento
                        detalleNivel1 = detalleNivel1Tmp.OrderBy(x => x.CentroCostoID.Value).ToList();
                    }
                    #endregion
                    #endregion

                    #region Asigna valores de saldos segun detalle
                    foreach (DTO_QueryEstadoEjecucion saldo in detalleNivel1)
                    {
                        #region Acumula saldos
                        dtoCierreLegFinal.Disponible.Value += saldo.Disponible.Value;
                        dtoCierreLegFinal.Presupuesto.Value += saldo.Presupuesto.Value;
                        dtoCierreLegFinal.Compromisos.Value += saldo.Compromisos.Value;
                        dtoCierreLegFinal.Recibidos.Value += saldo.Recibidos.Value;
                        dtoCierreLegFinal.Ejecutado.Value += saldo.Ejecutado.Value;
                        dtoCierreLegFinal.EjeVsPre.Value += saldo.EjeVsPre.Value;
                        #endregion
                        #region Asigna Descripcion 2º Rompimiento
                        if (proType == ProyectoTipo.Opex || proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                        {
                            DTO_plRecurso dtoRecurso = (DTO_plRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecurso, saldo.RecursoID.Value, true, false);
                            saldo.Descriptivo.Value = dtoRecurso.Descriptivo.Value;
                        }
                        else
                        {
                            DTO_coCentroCosto dtoCentroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, saldo.CentroCostoID.Value, true, false);
                            saldo.Descriptivo.Value = dtoCentroCosto.Descriptivo.Value;
                        }
                        #endregion
                        #region Asigna Descripcion 3º Rompimiento
                        foreach (DTO_QueryEstadoEjecucion saldoRomp3 in saldo.DetalleNivel2)
                        {
                            DTO_plLineaPresupuesto dtoLineaPresup = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, saldoRomp3.LineaPresupuestoID.Value, true, false);
                            saldoRomp3.Descriptivo.Value = dtoLineaPresup.Descriptivo.Value;
                        }
                        #endregion
                    }
                    #endregion
                    #region Asigna Detalle final 1' Rompimiento
                    dtoCierreLegFinal.DetalleNivel1 = detalleNivel1.OrderBy(x => x.ProyectoID.Value).ToList();
                    listCierreLegFinal.Add(dtoCierreLegFinal);
                    #endregion
                }
                #endregion

                return listCierreLegFinal;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plCierreLegalizacion_GetEstadoEjecByPeriodo");
                return null;
            }
        }

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de plSobreEjecucion
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="estado">Estado de los documentos</param>
        /// <param name="areaAprob">Area de aprobacion</param>
        /// <returns>Lista de informe </returns>
        public List<DTO_plSobreEjecucion> plSobreEjecucion_GetOrdenCompraSobreEjec(int documentID, int estado, string areaAprob)
        {
            try
            {
                this._dal_plSobreEjecucion = (DAL_plSobreEjecucion)base.GetInstance(typeof(DAL_plSobreEjecucion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_plSobreEjecucion> listSobreEjecucion = this._dal_plSobreEjecucion.DAL_plSobreEjecucion_GetOrdenCompraSobreEjec(estado, areaAprob);

                foreach (DTO_plSobreEjecucion sobreEjec in listSobreEjecucion)
                {
                    //Suma el detalle 
                    sobreEjec.SolicitadoOrigenLoc.Value = sobreEjec.Detalle.Sum(x => x.SolicitadoOrigenLoc.Value);
                    sobreEjec.SolicitadoOrigenExt.Value = sobreEjec.Detalle.Sum(x => x.SolicitadoOrigenExt.Value);
                    sobreEjec.DisponibleOrigenLoc.Value = sobreEjec.Detalle.Sum(x => x.DisponibleOrigenLoc.Value);
                    sobreEjec.DisponibleOrigenExt.Value = sobreEjec.Detalle.Sum(x => x.DisponibleOrigenExt.Value);
                    sobreEjec.EnProcesoOrigenLoc.Value = sobreEjec.Detalle.Sum(x => x.EnProcesoOrigenLoc.Value);
                    sobreEjec.EnProcesoOrigenExt.Value = sobreEjec.Detalle.Sum(x => x.EnProcesoOrigenExt.Value);
                    sobreEjec.PorAprobarOrigenLoc.Value = sobreEjec.Detalle.Sum(x => x.PorAprobarOrigenLoc.Value);
                    sobreEjec.PorAprobarOrigenExt.Value = sobreEjec.Detalle.Sum(x => x.PorAprobarOrigenExt.Value);

                    //Valida la moneda para mostrar el valor correspondiente 
                    if (sobreEjec.MonedaOrigen.Value == (byte)TipoMoneda.Local)
                    {
                        sobreEjec.Solicita.Value = sobreEjec.SolicitadoOrigenLoc.Value;
                        sobreEjec.Disponible.Value = sobreEjec.DisponibleOrigenLoc.Value;
                        sobreEjec.EnProceso.Value = sobreEjec.EnProcesoOrigenLoc.Value;
                        sobreEjec.PorAprobar.Value = sobreEjec.PorAprobarOrigenLoc.Value;
                    }
                    else
                    {
                        sobreEjec.Solicita.Value = sobreEjec.SolicitadoOrigenExt.Value;
                        sobreEjec.Disponible.Value = sobreEjec.DisponibleOrigenExt.Value;
                        sobreEjec.EnProceso.Value = sobreEjec.EnProcesoOrigenExt.Value;
                        sobreEjec.PorAprobar.Value = sobreEjec.PorAprobarOrigenExt.Value;
                    }

                }
                return listSobreEjecucion;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plSobreEjecucion_GetOrdenCompraSobreEjec");
                return null;
            }
        }

        #endregion

        #region Reportes

        #region Cierre Legalizacion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="Periodo">Periodo que se desea verificar</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>        
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesPlaneacion_CierreLegalizacion(DateTime Periodo, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso)
        {
            this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_CierreLegalizacion(Periodo, contrato, bloque, campo, pozo, proyecto, actividad, lineaPresupuesto, centroCosto,
                recurso);
        }

        #endregion

        #region Presupuesto
        /// <summary>
        /// Funcion q se encarga de traer los datos para el presuepuesto
        /// </summary>
        /// <param name="periodo">Año q se va generar</param>
        /// <param name="proyecto">Proyecto que se ver</param>
        /// <returns>Listado de DTO</returns>
        public DataTable ReportesPlaneacion_Presupuesto(DateTime periodo, string proyecto, bool isAcumulado, bool tipoMoneda, bool isConsololidado)
        {
            #region NO BORRAR
            //List<DTO_PlaneacionTotal> presupuesto = new List<DTO_PlaneacionTotal>();

            //try
            //{
            //    List<DTO_PlaneacionTotal> result = new List<DTO_PlaneacionTotal>();
            //    DTO_PlaneacionTotal presupuestos = new DTO_PlaneacionTotal();
            //    presupuestos.DetallesPresupuesto = new List<DTO_ReportPresupuesto>();
            //    this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            //    presupuestos.DetallesPresupuesto = this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_Presupuesto(periodo, proyecto, isAcumulado);
            //    List<string> distint = (from c in presupuestos.DetallesPresupuesto select c.ProyectoID.Value).Distinct().ToList();

            //    foreach (var proyec in distint)
            //    {
            //        DTO_PlaneacionTotal pres = new DTO_PlaneacionTotal();
            //        pres.DetallesPresupuesto = new List<DTO_ReportPresupuesto>();

            //        pres.DetallesPresupuesto = presupuestos.DetallesPresupuesto.Where(x => x.ProyectoID.Value == proyec).ToList();
            //        result.Add(pres);
            //    }

            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesPlaneacion_Presupuesto");
            //    return presupuesto;
            //} 
            #endregion
            this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_Presupuesto(periodo, proyecto, isAcumulado, tipoMoneda, isConsololidado);
        }

        /// <summary>
        /// Funcion que se encarga de consultar la ejecucion presupuestal para la empresa consultada
        /// </summary>
        /// <param name="Periodo">Periodo que se desea presupuestar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto que se desea ver (1 = Capex, 2 = Opex, 3 = Inversion, 4 = Administrativo, 5 = Inventarios, 6 = Capital de Trabajo, 7 = Distribucion) </param>
        /// <param name="TipoReporte">Tipo Reporte a mostrar (ProyAct = Proyecto x Actividad, LinRecur = Lineas x Recurso, RecurAct = Recurso x Actividad, LineCosto = Lineas x Centro Costo)</param>
        /// <param name="ProyectoID">Filtra un Proyecto  ------------------	(Se utiliza en el reporte de ProyAct)</param>
        /// <param name="ActividadID">Filtra una Actividad -----------------	(Se utiliza en el reporte de ProyAct y RecurAct)</param>
        /// <param name="LineaPresupuestalID">Filtra una Linea Presupuestal ---	(Se utiliza en el reporte de  LinRecur y LineCosto)	</param>
        /// <param name="CentroCostoID">Filtra un Centro de Costo --------- (Se utiliza en el reporte de LineCosto)</param>
        /// <param name="RecursoGrupoID">Filtra un Resurso Grupo -----------	(Se utiliza en el reporte de LinRecur y RecurAct)</param>
        /// <returns>Listado de DTO con la ejecion presupuestal</returns>
        public List<DTO_PlaneacionTotal> ReportesPlaneacion_EjecucionPresupuestalxMoneda(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID)
        {
            List<DTO_PlaneacionTotal> ejecucionPresupuestal = new List<DTO_PlaneacionTotal>();

            try
            {
                List<DTO_PlaneacionTotal> result = new List<DTO_PlaneacionTotal>();
                DTO_PlaneacionTotal ejePre = new DTO_PlaneacionTotal();
                ejePre.DetallesEjePresupuestal = new List<DTO_ReportEjecucionPresupuestal>();
                List<string> distinct = new List<string>();
                this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                ejePre.DetallesEjePresupuestal = this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_EjecucionPresupuestalxMoneda(Periodo, ProyectoTipo, TipoReporte, ProyectoID,
                    ActividadID, LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

                switch (TipoReporte)
                {
                    case "LinRecur":
                        #region Agrupa el reporte Recurso por Linea Presupuestal
                        distinct = (from c in ejePre.DetallesEjePresupuestal select c.RecursoID.Value).Distinct().ToList();

                        foreach (var recursoID in distinct)
                        {
                            DTO_PlaneacionTotal ejePresupuestal = new DTO_PlaneacionTotal();
                            ejePresupuestal.DetallesEjePresupuestal = new List<DTO_ReportEjecucionPresupuestal>();

                            ejePresupuestal.DetallesEjePresupuestal = ejePre.DetallesEjePresupuestal.Where(x => x.RecursoID.Value == recursoID).ToList();
                            result.Add(ejePresupuestal);
                        }
                        #endregion
                        break;

                    case "RecurAct":
                        #region Agrupa el reporte Actividad por Recurso

                        distinct = (from c in ejePre.DetallesEjePresupuestal select c.ActividadID.Value).Distinct().ToList();

                        foreach (var recursoID in distinct)
                        {
                            DTO_PlaneacionTotal ejePresupuestal = new DTO_PlaneacionTotal();
                            ejePresupuestal.DetallesEjePresupuestal = new List<DTO_ReportEjecucionPresupuestal>();

                            ejePresupuestal.DetallesEjePresupuestal = ejePre.DetallesEjePresupuestal.Where(x => x.ActividadID.Value == recursoID).ToList();
                            result.Add(ejePresupuestal);
                        }
                        #endregion
                        break;

                    case "LineCosto":
                        #region Agrupa el reporte Lineas por Centro de Costo

                        distinct = (from c in ejePre.DetallesEjePresupuestal select c.CentroCostoID.Value).Distinct().ToList();

                        foreach (var centroCosto in distinct)
                        {
                            DTO_PlaneacionTotal ejePreActivi = new DTO_PlaneacionTotal();
                            ejePreActivi.DetallesEjePresupuestal = new List<DTO_ReportEjecucionPresupuestal>();

                            ejePreActivi.DetallesEjePresupuestal = ejePre.DetallesEjePresupuestal.Where(x => x.CentroCostoID.Value == centroCosto).ToList();
                            result.Add(ejePreActivi);
                        }
                        #endregion
                        break;

                    default:
                        #region Agrupa el reporte Proyecto por Actividad

                        distinct = (from c in ejePre.DetallesEjePresupuestal select c.ActividadID.Value).Distinct().ToList();

                        foreach (var actividad in distinct)
                        {
                            DTO_PlaneacionTotal ejePreActivi = new DTO_PlaneacionTotal();
                            ejePreActivi.DetallesEjePresupuestal = new List<DTO_ReportEjecucionPresupuestal>();

                            ejePreActivi.DetallesEjePresupuestal = ejePre.DetallesEjePresupuestal.Where(x => x.ActividadID.Value == actividad).ToList();
                            result.Add(ejePreActivi);
                        }

                        #endregion
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesPlaneacion_EjecucionPresupuestal");
                return ejecucionPresupuestal;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer los datos para generar el reportes de Ejecucion Presupuestal por Origen
        /// </summary>
        /// <param name="Periodo">Periodo a consultar</param>
        /// <param name="ProyectoTipo">Tipo de Proyecto a consultar</param>
        /// <returns>Tabla con la ejecucion presupuestal</returns>
        public DataTable ReportesPlaneacion_EjecucionPresupuestalXOrigen(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID,
            string ActividadID, string LineaPresupuestalID, string CentroCostoID, string RecursoGrupoID)
        {
            this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_EjecucionPresupuestaXOrigen(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID,
                LineaPresupuestalID, CentroCostoID, RecursoGrupoID);
        }
        #endregion

        #region SobreEjecucion

        /// <summary>
        /// Funcion que se encarga de traer los datos de la sobre Ejecucion
        /// </summary>
        /// <param name="year">Año que se desea ver</param>
        /// <param name="contrato">Filtra un Contrato</param>
        /// <param name="bloque">Filtra un bloque</param>
        /// <param name="campo">Filtra un campo</param>
        /// <param name="pozo">Filtra un Pozo</param>
        /// <param name="proyecto">Filtra un Proyecto</param>
        /// <param name="actividad">Filtra una Actividad</param>
        /// <param name="lineaPresupuesto">Filtra una Linea Presupuesto</param>
        /// <param name="centroCosto">Filtra un Centro Costo</param>
        /// <param name="recurso">Filtra un recurso</param>
        /// <param name="usuario">Filtra un usuario</param>
        /// <param name="prefijo">Filtra un prefijo </param>
        /// <param name="numeroDoc">Filtra un numero de Documento Especifico</param>
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesPlaneacion_SobreEjecucion(int year, string contrato, string bloque, string campo, string pozo, string proyecto, string actividad,
            string lineaPresupuesto, string centroCosto, string recurso, string usuario, string prefijo, string numeroDoc)
        {
            this._dal_reportesPlaneacion = (DAL_ReportesPlaneacion)this.GetInstance(typeof(DAL_ReportesPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_reportesPlaneacion.DAL_ReportesPlaneacion_SobreEjecucion(year, contrato, bloque, campo, pozo, proyecto, actividad, lineaPresupuesto, centroCosto,
                recurso, usuario, prefijo, numeroDoc);
        }

        #endregion

        #endregion
    }
}
