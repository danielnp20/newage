using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloTesoreria : ModuloBase
    {
        #region Variables

        #region Dals

        
        private DAL_Tesoreria _dal_Tesoreria = null;
        private DAL_ReportesTesoreria _dal_ReporteTesoreria = null;
        private DAL_tsBancosCuenta _dal_tsBancosCuenta = null;
        private DAL_tsBancosDocu _dal_tsBancosDocu = null;
        private DAL_tsReciboCajaDocu _dal_tsReciboCajaDocu = null;

        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloCuentasXPagar _moduloCuentasXPagar = null;
        private ModuloGlobal _moduloGlobal = null;

        #endregion

        #endregion
        /// <summary>
        /// Constructor Módulo Tesorería
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tx"></param>
        /// <param name="emp"></param>
        /// <param name="userID"></param>
        public ModuloTesoreria(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }



        /// <summary>
        /// Obtiene el detalle de los proyectos
        /// </summary>
        /// Fecha corte >> Fecha de corte 
        /// <returns>Documentos</returns>
        public List<DTO_QueryFlujoFondos> tsFlujoFondos(DateTime fechaCorte)
        {
            try
            {

                List<DTO_QueryFlujoFondos> result = new List<DTO_QueryFlujoFondos>();
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_Tesoreria.DAL_Tesoreria_tsFlujoFondos(fechaCorte);

                foreach (DTO_QueryFlujoFondos det in result)
                {
                    det.PerA.Value = det.Detalle.Sum(x => (x.PerA.Value*x.Factor.Value));
                    det.Per0.Value = det.Detalle.Sum(x => (x.Per0.Value*x.Factor.Value));
                    det.Per1.Value = det.Detalle.Sum(x => (x.Per1.Value*x.Factor.Value));
                    det.Per2.Value = det.Detalle.Sum(x => (x.Per2.Value*x.Factor.Value));
                    det.Per3.Value = det.Detalle.Sum(x => (x.Per3.Value*x.Factor.Value));
                    det.Per4.Value = det.Detalle.Sum(x => (x.Per4.Value*x.Factor.Value));
                    det.Per5.Value = det.Detalle.Sum(x => (x.Per5.Value*x.Factor.Value));
                    det.Per6.Value = det.Detalle.Sum(x => (x.Per6.Value*x.Factor.Value));
                    det.PerM.Value = det.Detalle.Sum(x => (x.PerM.Value*x.Factor.Value));
                }
                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "tsFlujoFondos");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el detalle de los proyectos
        /// </summary>
        /// Fecha corte >> Fecha de corte 
        /// <returns>Documentos</returns>
        public List<DTO_QueryFlujoFondosTareas> tsFlujoFondos_Tareas(DateTime fechaCorte, string proyecto, bool? recaudosInd)
        {
            try
            {
                List<DTO_QueryFlujoFondosTareas> result = new List<DTO_QueryFlujoFondosTareas>();
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_Tesoreria.DAL_Tesoreria_tsFlujoFondosTarea(fechaCorte,proyecto, recaudosInd);

                foreach (DTO_QueryFlujoFondosTareas det in result)
                {
                    det.PerA.Value = det.DetalleTarea.Sum(x => (x.PerA.Value*x.Factor.Value));
                    det.Per0.Value = det.DetalleTarea.Sum(x => (x.Per0.Value*x.Factor.Value));
                    det.Per1.Value = det.DetalleTarea.Sum(x => (x.Per1.Value*x.Factor.Value));
                    det.Per2.Value = det.DetalleTarea.Sum(x => (x.Per2.Value*x.Factor.Value));
                    det.Per3.Value = det.DetalleTarea.Sum(x => (x.Per3.Value*x.Factor.Value));
                    det.Per4.Value = det.DetalleTarea.Sum(x => (x.Per4.Value*x.Factor.Value));
                    det.Per5.Value = det.DetalleTarea.Sum(x => (x.Per5.Value*x.Factor.Value));
                    det.Per6.Value = det.DetalleTarea.Sum(x => (x.Per6.Value*x.Factor.Value));
                    det.PerM.Value = det.DetalleTarea.Sum(x => (x.PerM.Value*x.Factor.Value));
                    
                }
                return result;

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "tsFlujoFondos_Tareas");
                throw exception;
            }
        }


        /// <summary>
        /// Consulta El Flujo de Caja
        /// </summary>
        /// <returns>Lista del Flujo de Caja</returns>
        public List<DTO_QueryFlujoCaja> tsFlujoCaja()
        {
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_QueryFlujoCaja> list = _dal_Tesoreria.DAL_Tesoreria_tsFlujoCaja();
            return list;
        }

        /// <summary>
        /// Consulta El Flujo de Caja
        /// </summary>
        /// <returns>Lista del Flujo de Caja</returns>
        public List<DTO_QueryFlujoCajaDetalle> tsFlujoCajaDetalle( string Documento)
        {
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_QueryFlujoCajaDetalle> list = _dal_Tesoreria.DAL_Tesoreria_tsFlujoCajaDetalle(Documento);
            return list;
        }

        /// <summary>
        /// Consulta Semana
        /// </summary>
        /// <returns>Semana</returns>
        public string Global_DiaSemana(int Semana)
        {
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            string  list = _dal_Tesoreria.DAL_Tesoreria_Global_DiaSemana(Semana);
            return list;
        }

        /// <summary>
        /// Consulta Mes
        /// </summary>
        /// <returns>Mes</returns>
        public string Global_Mes(int Mes)
        {
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            string list = _dal_Tesoreria.DAL_Tesoreria_Global_Mes(Mes);
            return list;
        }

        #region PagosElectronicos

        /// <summary>
        /// Consulta las facturas para transmitir al banco
        /// </summary>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosSinTransmitir()
        {
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_PagosElectronicos> list = _dal_Tesoreria.DAL_Tesoreria_GetPagosElectronicosSinTransmitir();
            return list;
        }

        /// <summary>
        /// Guarda el estado actual de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_Guardar(int documentID, List<DTO_PagosElectronicos> pagosElectronicos, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int i = 0;
                foreach (DTO_PagosElectronicos pagoElectronico in pagosElectronicos)
                {
                    int percent = ((i + 1) * 100) / pagosElectronicos.Count;
                    DTO_tsBancosDocu bancoDocu = this._dal_tsBancosDocu.DAL_tsBancosDocu_Get(pagoElectronico.NumeroDoc.Value.Value);

                    bool transmitido = string.IsNullOrWhiteSpace(bancoDocu.Dato4.Value) ? false : true;

                    if (pagoElectronico.PagosElectronicosInd.Value.Value != transmitido)
                    {
                        bancoDocu.Dato4.Value = pagoElectronico.PagosElectronicosInd.Value.Value ? Convert.ToByte(true).ToString() : null;
                        this._dal_tsBancosDocu.DAL_tsBancosDocu_Upd(bancoDocu);
                    }

                    batchProgress[tupProgress] = percent;
                    i++;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PagosElectronicos_Guardar");

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
        /// Transmite los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_Transmitir(int documentID, List<DTO_PagosElectronicos> pagosElectronicos, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int i = 0;
                foreach (DTO_PagosElectronicos pagoElectronico in pagosElectronicos)
                {
                    int percent = ((i + 1) * 100) / pagosElectronicos.Count;
                    DTO_tsBancosDocu bancoDocu = this._dal_tsBancosDocu.DAL_tsBancosDocu_Get(pagoElectronico.NumeroDoc.Value.Value);

                    bool transmitido = string.IsNullOrWhiteSpace(bancoDocu.Dato4.Value) ? false : true;

                    DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, pagoElectronico.BancoCuentaID.Value, true, false);
                    bancoDocu.Dato1.Value = pagoElectronico.Banco.Value;
                    bancoDocu.Dato2.Value = pagoElectronico.CuentaNro.Value;
                    bancoDocu.Dato3.Value = DateTime.Today.ToString(FormatString.Date);
                    this._dal_tsBancosDocu.DAL_tsBancosDocu_Upd(bancoDocu);

                    batchProgress[tupProgress] = percent;
                    i++;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PagosElectronicos_Transmitir");

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
        /// Consulta los pagos transmitidos al banco, buscando por tercero y fecha de transmicion
        /// </summary>
        /// <param name="terceroID">Tercero al que se le realizó el pago</param>
        /// <param name="fechaTransmicion">Fecha en la que se realizó la transmición</param>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosTransmitidos(string terceroID, DateTime fechaTransmicion)
        {
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_PagosElectronicos> list = _dal_Tesoreria.DAL_Tesoreria_GetPagosElectronicosTransmitidos(terceroID, fechaTransmicion);
            return list;
        }

        /// <summary>
        /// Revierte la transmicion de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos a revertir</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_RevertirTransmicion(int documentID, List<DTO_PagosElectronicos> pagosElectronicos, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int i = 0;
                foreach (DTO_PagosElectronicos pagoElectronico in pagosElectronicos)
                {
                    int percent = ((i + 1) * 100) / pagosElectronicos.Count;
                    DTO_tsBancosDocu bancoDocu = this._dal_tsBancosDocu.DAL_tsBancosDocu_Get(pagoElectronico.NumeroDoc.Value.Value);

                    bancoDocu.Dato1.Value = string.Empty;
                    bancoDocu.Dato2.Value = string.Empty;
                    bancoDocu.Dato3.Value = string.Empty;
                    bancoDocu.Dato4.Value = string.Empty;
                    this._dal_tsBancosDocu.DAL_tsBancosDocu_Upd(bancoDocu);

                    batchProgress[tupProgress] = percent;
                    i++;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PagosElectronicos_RevertirTransmicion");

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

        #region Pagos

        #region Funciones Privadas

        /// <summary>
        /// Obtiene los detalles de las facturas con esos valores
        /// </summary>
        /// <param name="TerceroID">El Tercero al cual se le va a pagar las facturas</param>
        /// <param name="BancoCuentaID">La cuenta con la que se van a pagar las facturas</param>
        /// <param name="monedaBancoCuenta">Origen de la moneda del banco</param>
        /// <returns>Facturas a pagar relacionadas</returns>
        //private List<DTO_DetalleFactura> PagoFacturas_GetDetallesFacturas(bool multiMoneda, string terceroID, string bancoCuentaID, int monedaBancoCuenta)
        private List<DTO_DetalleFactura> PagoFacturas_GetDetallesFacturas(bool multiMoneda, string beneficiarioID, string bancoCuentaID, int monedaBancoCuenta)
        {
            try
            {
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //List<DTO_DetalleFactura> list = this._dal_Tesoreria.DAL_Pagos_GetDetallesFacturas(terceroID, bancoCuentaID);
                List<DTO_DetalleFactura> list = this._dal_Tesoreria.DAL_Pagos_GetDetallesFacturasBeneficiaro(beneficiarioID, bancoCuentaID);
                string monedaLocalID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjeraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                decimal tasaCambio = 0;
                if (multiMoneda)
                {
                    DAL_glTasaDeCambio dalTasaCambio = (DAL_glTasaDeCambio)base.GetInstance(typeof(DAL_glTasaDeCambio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    tasaCambio = dalTasaCambio.DAL_TasaDeCambio_Get(monedaExtranjeraID, DateTime.Today);
                }

                list.ForEach(f =>
                {
                    switch (monedaBancoCuenta)
                    {
                        case (int)TipoMoneda_CoDocumento.Local:
                            f.ValorPagoLocal.Value = f.ValorPago.Value;
                            if (f.MonedaID.Value == monedaExtranjeraID)
                            {
                                f.ValorPago.Value = Math.Round(f.ValorPago.Value.Value * tasaCambio, 2);
                                f.ValorPagoExtra.Value = f.ValorPago.Value;
                                f.MonedaID.Value = monedaLocalID;
                            }
                            else
                            {
                                f.ValorPagoExtra.Value = multiMoneda ? Math.Round(f.ValorPago.Value.Value / tasaCambio, 2) : 0;
                            }
                            break;
                        case (int)TipoMoneda_CoDocumento.Foreign:
                            f.ValorPagoExtra.Value = f.ValorPago.Value;
                            if (f.MonedaID.Value == monedaLocalID)
                            {
                                f.ValorPago.Value = Math.Round(f.ValorPago.Value.Value / tasaCambio, 2);
                                f.ValorPagoLocal.Value = f.ValorPago.Value;
                                f.MonedaID.Value = monedaExtranjeraID;
                            }
                            else
                            {
                                f.ValorPagoLocal.Value = Math.Round(f.ValorPago.Value.Value * tasaCambio, 2);
                            }
                            break;
                    }
                });

                return list;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "PagoFacturas_GetDetallesFacturas");
                throw exception;
            }
        }

        /// <summary>
        /// Separa las facturas por cheque
        /// </summary>
        /// <param name="pagoFacturaTemp">Objeto que tiene el listado de detalles a separar</param>
        /// <param name="detalles">Listado que devuelve separado por cheques</param>
        /// <param name="index">Índice de la nueva lista separada</param>
        private void GetDetallesXCheques(bool multiMoneda, DTO_PagoFacturas pagoFacturaTemp, ref List<DTO_SerializedObject> detalles, ref int index)
        {
            try
            {
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DateTime periodoTs = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, pagoFacturaTemp.BancoCuentaID.Value, true, false);
                DTO_coComprobante comprobante = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, pagoFacturaTemp.ComprobanteID.Value, true, false);
                pagoFacturaTemp.ConsecutivoEgreso.Value = comprobante != null? this._moduloContabilidad.GenerarComprobanteNro(comprobante, string.Empty, periodoTs, 0, true) : 0;
                pagoFacturaTemp.ConsecutivoEgreso.Value++;
                //pagoFacturaTemp.DetallesFacturas = this.PagoFacturas_GetDetallesFacturas(multiMoneda, pagoFacturaTemp.TerceroID.Value, pagoFacturaTemp.BancoCuentaID.Value, Convert.ToInt32(pagoFacturaTemp.MonedaBancoCuenta.Value));
                pagoFacturaTemp.DetallesFacturas = this.PagoFacturas_GetDetallesFacturas(multiMoneda, pagoFacturaTemp.BeneficiarioID.Value, pagoFacturaTemp.BancoCuentaID.Value, Convert.ToInt32(pagoFacturaTemp.MonedaBancoCuenta.Value));
                List<List<DTO_DetalleFactura >> detallesXCheques = pagoFacturaTemp.DetallesFacturas
                                        .Select((x, i) => new { Index = i, Value = x })
                                        .GroupBy(x => x.Index / bancoCuenta.FactXCheque.Value)
                                        .Select(x => x.Select(v => v.Value).ToList())
                                        .ToList();

                foreach (List<DTO_DetalleFactura> detallesXCheque in detallesXCheques)
                {
                    DTO_PagoFacturas pagoFactura = new DTO_PagoFacturas();

                    pagoFactura.PagoFacturasInd.Value = pagoFacturaTemp.PagoFacturasInd.Value;
                    pagoFactura.BancoCuentaID.Value = pagoFacturaTemp.BancoCuentaID.Value;
                    pagoFactura.MonedaBancoCuenta.Value = pagoFacturaTemp.MonedaBancoCuenta.Value;
                    pagoFactura.MonedaID.Value = pagoFacturaTemp.MonedaID.Value;
                    pagoFactura.TerceroID.Value = pagoFacturaTemp.TerceroID.Value;
                    pagoFactura.Descriptivo.Value = pagoFacturaTemp.Descriptivo.Value;
                    pagoFactura.NumeroFacturas.Value = detallesXCheque.Count;
                    pagoFactura.TotalFacturas.Value = detallesXCheque.Sum(d => d.ValorPago.Value.Value);
                    pagoFactura.TotalFacturasLocal.Value = detallesXCheque.Sum(d => d.ValorPagoLocal.Value.Value);
                    pagoFactura.TotalFacturasExtra.Value = multiMoneda ? detallesXCheque.Sum(d => d.ValorPagoExtra.Value.Value) : 0;
                    pagoFactura.DetallesFacturas = detallesXCheque;
                    pagoFactura.BeneficiarioID.Value = pagoFacturaTemp.BeneficiarioID.Value;
                    pagoFactura.BeneficiarioDesc.Value = pagoFacturaTemp.BeneficiarioDesc.Value;
                    pagoFactura.ComprobanteID.Value = pagoFacturaTemp.ComprobanteID.Value;
                    pagoFactura.ConsecutivoEgreso.Value = pagoFacturaTemp.ConsecutivoEgreso.Value;
                    pagoFactura.Index = index;

                    detalles.Add(pagoFactura);
                    index++;
                }
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SepararXNroCheques");
            }
        }

        /// <summary>
        /// Carga la informacion para el registro de pago de una factura
        /// </summary>
        /// <param name="documentID">Documento que realiza la transaccion</param>
        /// <param name="areaFuncionalID">Area funcional de donde se ejecuta la operacion</param>
        /// <param name="bancoCuenta">Cuenta del banco</param>
        /// <param name="documento">Documento de contabilidad</param>
        /// <param name="comprobante">Comprobante (maestra)</param>
        /// <param name="pagoFactura">Pago factura</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="monedaExtranjeraID">Moneda extranjera</param>
        /// <param name="tasaCambio">Tasa de cambio</param>
        /// <param name="cacheCtrls">glDocumentoControl consultados</param>
        /// <param name="cacheCtas">Cuentas consultadas</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult RegistroPagoFactura(int documentID, string actividadFlujoID, string areaFuncionalID, DTO_tsBancosCuenta bancoCuenta, 
            DTO_coDocumento documento, DateTime fechaPago, DTO_coComprobante comprobante, DTO_PagoFacturas pagoFactura, string monedaExtranjeraID, 
            decimal tasaCambio, Dictionary<int, DTO_glDocumentoControl> cacheCtrls,Dictionary<string, DTO_coPlanCuenta> cacheCtas, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_RegistroPagoFactura registro = null;
            DTO_glDocumentoControl docCtrl = null;
            DTO_Comprobante comp = null;
            try
            {
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                registro = new DTO_RegistroPagoFactura();
                DTO_tsBancosCuenta bancoCtaTmp = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, bancoCuenta.ID.Value, true, false);
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Carga la info de las fechas

                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                DateTime periodoMod = Convert.ToDateTime(periodoStr);

                #endregion
                #region Carga el glDocumentoControl

                docCtrl = new DTO_glDocumentoControl();
                docCtrl.TerceroID.Value = pagoFactura.TerceroID.Value;
                docCtrl.MonedaID.Value = pagoFactura.MonedaID.Value;
                docCtrl.CuentaID.Value = ((int)documento.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documento.CuentaLOC.Value : documento.CuentaEXT.Value;
                docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                docCtrl.ProyectoID.Value = bancoCuenta.ProyectoID.Value;
                docCtrl.CentroCostoID.Value = bancoCuenta.CentroCostoID.Value;
                docCtrl.FechaDoc.Value = fechaPago;
                docCtrl.PeriodoDoc.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                docCtrl.PrefijoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                docCtrl.ComprobanteID.Value = documento.ComprobanteID.Value;
                docCtrl.DocumentoNro.Value = 0;
                docCtrl.ComprobanteIDNro.Value = 0;
                docCtrl.DocumentoTercero.Value = bancoCtaTmp.ChequeInicial.Value.Value.ToString();
                docCtrl.TasaCambioCONT.Value = tasaCambio;
                docCtrl.TasaCambioDOCU.Value = tasaCambio;
                docCtrl.DocumentoID.Value = documentID;
                docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                docCtrl.PeriodoUltMov.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                docCtrl.seUsuarioID.Value = this.UserId;
                docCtrl.AreaFuncionalID.Value = areaFuncionalID;
                docCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                docCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                docCtrl.Descripcion.Value = "CONT. PAGOS";
                docCtrl.Valor.Value = pagoFactura.TotalFacturasLocal.Value;;
                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    return result;
                }
                docCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                registro.DocumentoControl = docCtrl;
                #endregion
                #region Carga la info del banco
                DTO_tsBancosDocu banco = new DTO_tsBancosDocu();

                if (pagoFactura.Descriptivo.Value.Length > 50)
                    banco.Beneficiario.Value = pagoFactura.BeneficiarioDesc.Value.Substring(0, 50);
                else
                    banco.Beneficiario.Value = pagoFactura.BeneficiarioDesc.Value;

                banco.BeneficiarioÌD.Value = pagoFactura.BeneficiarioID.Value;
                banco.EmpresaID.Value = this.Empresa.ID.Value;
                banco.BancoCuentaID.Value = pagoFactura.BancoCuentaID.Value;
                banco.NroCheque.Value = bancoCtaTmp.ChequeInicial.Value;
                banco.Valor.Value = pagoFactura.TotalFacturas.Value;
                banco.IVA.Value = 0;
                banco.MonedaPago.Value = pagoFactura.MonedaID.Value;
                banco.ValorLocal.Value = pagoFactura.TotalFacturasLocal.Value;
                banco.ValorExtra.Value = pagoFactura.TotalFacturasExtra.Value;
                banco.NumeroDoc.Value = docCtrl.NumeroDoc.Value;

                this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(banco);

                registro.BancosDocu = banco;
                #endregion
                #region Actualiza datos de Programación de Pagos en cpCuentasXPagar
                foreach (DTO_DetalleFactura det in pagoFactura.DetallesFacturas)
                {
                    DTO_ProgramacionPagos programacionPago = new DTO_ProgramacionPagos();

                    programacionPago.NumeroDoc.Value = det.NumeroDoc.Value;
                    programacionPago.PagoInd.Value = false;
                    programacionPago.BancoCuentaID.Value = pagoFactura.BancoCuentaID.Value;
                    programacionPago.ValorPago.Value = null;

                    this._dal_Tesoreria.DAL_CuentaXPagar_Update(programacionPago, false);
                }
                #endregion
                #region Crea el comprobante y lo contabiliza
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                comp = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                #region Carga el cabezote

                header.ComprobanteID.Value = docCtrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.Fecha.Value = fechaPago;
                header.MdaTransacc.Value = docCtrl.MonedaID.Value;
                header.MdaOrigen.Value = header.MdaTransacc.Value == monedaExtranjeraID ? (byte)TipoMoneda_LocExt.Foreign : (byte)TipoMoneda_LocExt.Local;
                header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                header.PeriodoID.Value = docCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = docCtrl.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = header.TasaCambioBase.Value;

                comp.Header = header;
                #endregion
                #region Carga los detalles

                DTO_glDocumentoControl ctrl;
                DTO_coPlanCuenta cta;
                foreach (DTO_DetalleFactura det in pagoFactura.DetallesFacturas)
                {
                    #region Carga el documento de la factura
                    int identificadorTR = det.NumeroDoc.Value.Value;
                    if (cacheCtrls.ContainsKey(identificadorTR))
                        ctrl = cacheCtrls[identificadorTR];
                    else
                    {
                        ctrl = this._moduloGlobal.glDocumentoControl_GetByID(identificadorTR);
                        cacheCtrls.Add(identificadorTR, ctrl);
                    }
                    #endregion
                    #region Carga la cuenta
                    string ctaID = ctrl.CuentaID.Value;
                    if (cacheCtas.ContainsKey(ctaID))
                        cta = cacheCtas[ctaID];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaID, true, false);
                        cacheCtas.Add(ctaID, cta);
                    }
                    #endregion
                    #region Crea el detalle del comprobante
                    DTO_ComprobanteFooter footerDet = this.CrearComprobanteFooter(ctrl, header.TasaCambioBase.Value, concCargoDef, lgDef, lineaDef,
                        det.ValorPagoLocal.Value.Value, det.ValorPagoExtra.Value.Value, false);
                    footer.Add(footerDet);
                    #endregion
                }
                #endregion
                #region Crea la contrapartida
                DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(registro.DocumentoControl, header.TasaCambioBase.Value, concCargoDef, lgDef, lineaDef,
                    pagoFactura.TotalFacturasLocal.Value.Value * -1, pagoFactura.TotalFacturasExtra.Value.Value * -1, true);
                contraP.DocumentoCOM.Value = registro.DocumentoControl.DocumentoTercero.Value;
                footer.Add(contraP);

                comp.Footer = footer;
                #endregion
                #region Contabiliza el comprobante
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #endregion
                #region Incrementa el cheque inicial
                result = this.IncrementarChequeInicial(pagoFactura.BancoCuentaID.Value);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion

                result.ExtraField = resultGLDC.Key;
                return result;

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "RegistroPagoFactura");

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
                        docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comprobante, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

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

        /// <summary>
        /// Incrementa el cheque Inical en tsBancosCuenta
        /// </summary>
        /// <returns>Resultado de la operación</returns>
        private DTO_TxResult IncrementarChequeInicial(string bancoCuentaID)
        {
            DTO_TxResult result = new DTO_TxResult();
            try
            {
                this._dal_tsBancosCuenta = (DAL_tsBancosCuenta)this.GetInstance(typeof(DAL_tsBancosCuenta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, bancoCuentaID, true, false);

                this._dal_tsBancosCuenta.DAL_tsBancosCuenta_IncrementarChequeInicial(bancoCuentaID, bancoCuenta.ChequeInicial.Value.Value);
                if (bancoCuenta.ChequeInicial.Value.Value >= bancoCuenta.ChequeFinal.Value.Value)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ts_ChequesFinalizados;

                    return result;
                }

                //Guarda en bitácora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, AppDocuments.DesembolsoFacturas, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, bancoCuentaID, string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);

                result.Result = ResultValue.OK;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "IncrementarChequeInicial");

                return result;
            }
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Obtiene la lista de pagos para su programación
        /// </summary>
        /// /// <returns>Lista de pagos para su programación</returns>
        public List<DTO_ProgramacionPagos> ProgramacionPagos_GetProgramacionPagos()
        {
            try
            {
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                List<DTO_ProgramacionPagos> list = this._dal_Tesoreria.DAL_ProgramacionPagos_Get(periodo, libroFunc);
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ProgramacionPagos_GetProgramacionPagos");
                return new List<DTO_ProgramacionPagos>();
            }
        }

        /// <summary>
        /// Programa pagos
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult ProgramacionPagos_ProgramarPagos(int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos, bool pagoAprobacionInd, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                int i = 0;
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                foreach (DTO_ProgramacionPagos programacion in programacionesPagos)
                {
                    int percent = ((i + 1) * 100) / programacionesPagos.Count;
                    batchProgress[tupProgress] = percent;

                    if (programacion.PagoInd.Value.Value || programacion.PagoIndInicial.Value.Value)
                    {
                        #region Realiza la programacion validando el Saldo
                        //Trae el saldo
                        DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(programacion.NumeroDoc.Value.Value);
                        ctrl.Observacion.Value = programacion.Observacion.Value;
                        this._moduloGlobal.glDocumentoControl_Update(ctrl,true,true);
                        decimal saldoML = this._moduloContabilidad.Saldo_GetByDocumentoCuenta(true, periodo, ctrl.NumeroDoc.Value.Value, ctrl.CuentaID.Value, libroFunc);


                        //Revisa si el saldo es el mismo
                        if (Math.Abs(saldoML) == Math.Abs(programacion.SaldoML.Value.Value))
                        {
                            //Actualiza el flujo
                            this._dal_Tesoreria.DAL_CuentaXPagar_Update(programacion, pagoAprobacionInd);
                            this.AsignarFlujo(documentID, programacion.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);

                            //Guarda en bitácora
                            this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Edit, DateTime.Now,
                                this.UserId, this.Empresa.ID.Value, programacion.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                                string.Empty, string.Empty, 0, 0, 0);
                        }
                        else
                        {
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = i;
                            rd.Message = DictionaryMessages.Err_Ts_SaldosMvto;

                            result.Result = ResultValue.NOK;
                            result.Details.Add(rd);
                            return result;
                        }
                        #endregion
                    }
                    else if (!string.IsNullOrEmpty(programacion.BancoCuentaID.Value) || (programacion.ValorPago.Value.HasValue && programacion.ValorPago.Value != 0))
                    {
                        //Actualiza la CxP
                        programacion.BancoCuentaID.Value = string.Empty;
                        programacion.ValorPago.Value = null;
                        this._dal_Tesoreria.DAL_CuentaXPagar_Update(programacion, pagoAprobacionInd);
                    }

                    i++;
                }


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
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Consulta las facturas programadas previamente para pagar
        /// </summary>
        /// <returns>Lista de facturas por tercero para pagar</returns>
        public List<DTO_SerializedObject> PagoFacturas_GetPagoFacturas()
        {
            try
            {
                string monedaLocalID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjeraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                // Lista de DTO_txResult o DTO_PagoFacturas
                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                decimal tasaCambio = 0;
                bool isMultiMoneda = this.Multimoneda();
                if (isMultiMoneda)
                {
                    DAL_glTasaDeCambio dalTasaCambio = (DAL_glTasaDeCambio)base.GetInstance(typeof(DAL_glTasaDeCambio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    tasaCambio = dalTasaCambio.DAL_TasaDeCambio_Get(monedaExtranjeraID, DateTime.Today);
                }

                if (!isMultiMoneda || tasaCambio > 0)
                {
                    this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    List<DTO_PagoFacturas> tempList = this._dal_Tesoreria.DAL_PagoFacturas_Get();

                    List<int> indexUsed = new List<int>();
                    int index = 0;

                    if (tempList.Exists(p => (int)p.MonedaBancoCuenta.Value == (int)TipoMoneda_CoDocumento.Both || (int)p.MonedaBancoCuenta.Value == (int)TipoMoneda_CoDocumento.NA))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Ts_MonedaOrigenInvalida;

                        results = new List<DTO_SerializedObject>();
                        results.Add(result);
                        return results;
                    }
                    //ps.TerceroID.Value == p.TerceroID.Value &&
                    tempList.ForEach(p =>
                    {
                        if (!indexUsed.Contains(p.Index))
                        {
                            List<DTO_PagoFacturas> pagosASumar = tempList.FindAll(ps =>
                                                                        (
                                                                            ps.BancoCuentaID.Value == p.BancoCuentaID.Value &&                                                                            
                                                                            ps.BeneficiarioID.Value == p.BeneficiarioID.Value &&
                                                                            ps.Descriptivo.Value == p.Descriptivo.Value
                                                                        ));

                            if (pagosASumar.Count > 1)
                            {
                                pagosASumar.ForEach(ps => indexUsed.Add(ps.Index));
                                                    pagosASumar[0].NumeroFacturas.Value = pagosASumar.Sum(ts => ts.NumeroFacturas.Value.Value);
                                                    pagosASumar[0].TotalFacturas.Value = pagosASumar.Sum(ts => ts.TotalFacturas.Value.Value);
                                                    this.GetDetallesXCheques(isMultiMoneda, pagosASumar[0], ref results, ref index);
                            }
                            else if (pagosASumar.Count == 1)
                                this.GetDetallesXCheques(isMultiMoneda, pagosASumar[0], ref results, ref index);
                        }
                    });

                    return results;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ts_NoTasaCambioFechaActual;

                    results = new List<DTO_SerializedObject>();
                    results.Add(result);
                    return results;
                }
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PagoFacturas_GetPagoFacturas");

                List<DTO_SerializedObject> nl = new List<DTO_SerializedObject>();
                nl.Add(result);
                return nl;
            }
        }

        /// <summary>
        /// Registra el pago de una factura en un documento
        /// </summary>
        /// <param name="pagoFacturas">Pagos a registrar</param>
        /// <param name="areaFuncionalID">Código del área funcional</param>
        /// <returns></returns>
        public List<DTO_TxResult> PagoFacturas_RegistrarPagoFacturas(int documentID, string actividadFlujoID, List<DTO_PagoFacturas> pagoFacturas, DateTime fechaPago,
            string areaFuncionalID, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                #region Carga las variables de todos los pagos
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, pagoFacturas[0].BancoCuentaID.Value, true, false);
                DTO_coDocumento documento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, bancoCuenta.coDocumentoID.Value, true, false);
                DTO_coComprobante comprobante = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, documento.ComprobanteID.Value, true, false);
                string monedaExtranjeraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                #endregion
                #region Carga la tasa de cambio
                decimal tasaCambio = 0;
                if (this.Multimoneda())
                {
                    DAL_glTasaDeCambio dalTasaCambio = (DAL_glTasaDeCambio)base.GetInstance(typeof(DAL_glTasaDeCambio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    tasaCambio = dalTasaCambio.DAL_TasaDeCambio_Get(monedaExtranjeraID, DateTime.Today);
                }
                #endregion

                int i = 0;
                Dictionary<int, DTO_glDocumentoControl> cacheCtrls = new Dictionary<int, DTO_glDocumentoControl>();
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();

                foreach (DTO_PagoFacturas pagoFactura in pagoFacturas)
                {
                    int percent = ((i + 1) * 100) / pagoFacturas.Count;
                    batchProgress[tupProgress] = percent;

                    DTO_TxResult objResut = this.RegistroPagoFactura(documentID, actividadFlujoID, areaFuncionalID, bancoCuenta, 
                        documento, fechaPago, comprobante, pagoFactura, monedaExtranjeraID, tasaCambio, cacheCtrls, cacheCtas, false);

                    results.Add(objResut);
                    i++;
                }
                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PagoFacturas_RegistrarPagoFacturas");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Transferencias bancarias
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public List<DTO_TxResult> TransferenciasBancarias_Transferencias(int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos,
            DateTime fecha, decimal tc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            List<DTO_glDocumentoControl> docsTransfer = new List<DTO_glDocumentoControl>();
            List<DTO_TxResult> results = new List<DTO_TxResult>();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_Comprobante comp = null;
            DTO_coComprobante coComp = null;
            try
            {
                #region Variables

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                //Variables por defecto
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                DTO_coTercero dtoTercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroxDef, true, false);

                //Periodo
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);

                //Variables de operación
                bool isMultimoneda = this.Multimoneda();
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, programacionesPagos[0].BancoCuentaID.Value, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, bancoCuenta.coDocumentoID.Value, true, false);
                DTO_glDocumentoControl docCtrl = null;
                #endregion
                #region Validaciones

                //Valida que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    results.Add(result);
                    return results;
                }
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                //Valida que tenga cuenta
                if (string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                    results.Add(result);
                    return results;
                }

                #endregion

                if (bancoCuenta.FormaPago.Value == 1 || bancoCuenta.FormaPago.Value == 2) //Giro Individual
                {
                    int i = 0;
                    foreach (DTO_ProgramacionPagos prog in programacionesPagos)
                    {
                        int percent = ((i + 1) * 100) / programacionesPagos.Count;
                        batchProgress[tupProgress] = percent >= 80 ? 80 : percent;
                        List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                        bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, prog.BancoCuentaID.Value, true, false);
                        decimal vlrContraLoc = 0;
                        decimal vlrContraExt = 0;

                        #region Carga el glDocumentoControl

                        docCtrl = new DTO_glDocumentoControl();
                        docCtrl.TerceroID.Value = terceroxDef;//QUE TERCERO VA ACA
                        docCtrl.MonedaID.Value = (short)coDoc.MonedaOrigen.Value != (short)TipoMoneda_CoDocumento.Foreign ? mdaLoc : mdaExt;
                        docCtrl.CuentaID.Value = coDoc.CuentaLOC.Value;
                        docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                        docCtrl.ProyectoID.Value = bancoCuenta.ProyectoID.Value;
                        docCtrl.CentroCostoID.Value = bancoCuenta.CentroCostoID.Value;
                        docCtrl.FechaDoc.Value = fecha;
                        docCtrl.PeriodoDoc.Value = periodo;
                        docCtrl.PrefijoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                        docCtrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                        docCtrl.DocumentoNro.Value = 0;
                        docCtrl.ComprobanteIDNro.Value = 0;
                        docCtrl.DocumentoTercero.Value = bancoCuenta.ChequeInicial.Value.ToString();
                        docCtrl.TasaCambioCONT.Value = tc;
                        docCtrl.TasaCambioDOCU.Value = tc;
                        docCtrl.DocumentoID.Value = documentID;
                        docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        docCtrl.PeriodoUltMov.Value = periodo;
                        docCtrl.seUsuarioID.Value = this.UserId;
                        docCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        docCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                        docCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        docCtrl.Descripcion.Value = "CONT. TRANSFERENCIAS BANCARIAS INDIVIDUAL";

                        DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                        if (resultGLDC.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(resultGLDC);
                            results.Add(result);
                            return results;
                        }
                        docCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                        #endregion
                        #region Carga la info del banco
                        DTO_tsBancosDocu banco = new DTO_tsBancosDocu();

                        banco.EmpresaID.Value = this.Empresa.ID.Value;
                        banco.BancoCuentaID.Value = bancoCuenta.ID.Value;
                        banco.Beneficiario.Value = dtoTercero.Descriptivo.Value;//QUE BENEFICIARIO VA
                        banco.NroCheque.Value = bancoCuenta.ChequeInicial.Value;
                        banco.Valor.Value = prog.ValorPago.Value;
                        banco.IVA.Value = 0;
                        banco.MonedaPago.Value = docCtrl.MonedaID.Value;
                        banco.NumeroDoc.Value = docCtrl.NumeroDoc.Value;

                        if (isMultimoneda)
                        {
                            banco.ValorLocal.Value = docCtrl.MonedaID.Value == mdaLoc ? banco.Valor.Value : Math.Round(banco.Valor.Value.Value * tc, 2);
                            banco.ValorExtra.Value = docCtrl.MonedaID.Value == mdaLoc ? Math.Round(banco.Valor.Value.Value / tc, 2) : banco.Valor.Value;
                        }
                        else
                        {
                            banco.ValorLocal.Value = banco.Valor.Value;
                            banco.ValorExtra.Value = 0;
                        }

                        this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(banco);

                        #endregion
                        #region Revisa los saldos, actualiza la CxP y carga la info del comprobante
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
                            results.Add(result);
                            return results;

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

                            vlrContraLoc += vlrPagoLoc;
                            vlrContraExt += vlrPagoExt;
                            DTO_ComprobanteFooter footerDet = this.CrearComprobanteFooter(ctrlCxP, docCtrl.TasaCambioDOCU.Value, concCargoDef, lgDef, lineaDef,vlrPagoLoc, vlrPagoExt, false);
                            footerDet.TerceroID.Value = prog.TerceroID.Value;
                            footer.Add(footerDet);
                            #endregion
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

                        DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(docCtrl, header.TasaCambioBase.Value, concCargoDef, lgDef, lineaDef,
                            vlrContraLoc * -1, vlrContraExt * -1, true);
                        contraP.DocumentoCOM.Value = bancoCuenta.ChequeInicial.Value.ToString();
                        footer.Add(contraP);

                        comp.Footer = footer;
                        #endregion

                        //Contabiliza
                        result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                        if (result.Result == ResultValue.NOK)
                        {
                            results.Add(result);
                            return results;
                        }
                        #endregion
                        #region Incrementa el cheque inicial
                        result = this.IncrementarChequeInicial(bancoCuenta.ID.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            results.Add(result);
                            return results;
                        }
                        #endregion

                        //Asigna el numero doc para generar el reporte
                        result.ExtraField = docCtrl.NumeroDoc.Value.ToString();
                        results.Add(result);
                        docsTransfer.Add(docCtrl);
                        i++;
                    }
                }
                else if (bancoCuenta.FormaPago.Value == 3) //Giro masivo
                {
                    decimal vlrContraLoc = 0;
                    decimal vlrContraExt = 0;
                    #region Carga el glDocumentoControl

                    docCtrl = new DTO_glDocumentoControl();
                    docCtrl.TerceroID.Value = terceroxDef;
                    docCtrl.MonedaID.Value = (short)coDoc.MonedaOrigen.Value != (short)TipoMoneda_CoDocumento.Foreign ? mdaLoc : mdaExt;
                    docCtrl.CuentaID.Value = coDoc.CuentaLOC.Value;
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.ProyectoID.Value = bancoCuenta.ProyectoID.Value;
                    docCtrl.CentroCostoID.Value = bancoCuenta.CentroCostoID.Value;
                    docCtrl.FechaDoc.Value = fecha;
                    docCtrl.PeriodoDoc.Value = periodo;
                    docCtrl.PrefijoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                    docCtrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.ComprobanteIDNro.Value = 0;
                    docCtrl.DocumentoTercero.Value = bancoCuenta.ChequeInicial.Value.Value.ToString();
                    docCtrl.TasaCambioCONT.Value = tc;
                    docCtrl.TasaCambioDOCU.Value = tc;
                    docCtrl.DocumentoID.Value = documentID;
                    docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrl.PeriodoUltMov.Value = periodo;
                    docCtrl.seUsuarioID.Value = this.UserId;
                    docCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                    docCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                    docCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    docCtrl.Descripcion.Value = "CONT. TRANSFERENCIAS BANCARIAS MASIVO";

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        results.Add(result);
                        return results;
                    }
                    docCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                    #endregion
                    #region Carga la info del banco
                    DTO_tsBancosDocu banco = new DTO_tsBancosDocu();

                    banco.EmpresaID.Value = this.Empresa.ID.Value;
                    banco.BancoCuentaID.Value = bancoCuenta.ID.Value;
                    banco.Beneficiario.Value = dtoTercero.Descriptivo.Value;
                    banco.NroCheque.Value = bancoCuenta.ChequeInicial.Value;
                    banco.Valor.Value = (from p in programacionesPagos select p.ValorPago.Value.Value).Sum();
                    banco.IVA.Value = 0;
                    banco.MonedaPago.Value = docCtrl.MonedaID.Value;
                    banco.NumeroDoc.Value = docCtrl.NumeroDoc.Value;

                    if (isMultimoneda)
                    {
                        banco.ValorLocal.Value = docCtrl.MonedaID.Value == mdaLoc ? banco.Valor.Value : Math.Round(banco.Valor.Value.Value * tc, 2);
                        banco.ValorExtra.Value = docCtrl.MonedaID.Value == mdaLoc ? Math.Round(banco.Valor.Value.Value / tc, 2) : banco.Valor.Value;
                    }
                    else
                    {
                        banco.ValorLocal.Value = banco.Valor.Value;
                        banco.ValorExtra.Value = 0;
                    }

                    this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(banco);

                    #endregion
                    #region Revisa los saldos, actualiza la CxP y carga la info del comprobante

                    int i = 0;
                    List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                    foreach (DTO_ProgramacionPagos prog in programacionesPagos)
                    {
                        int percent = ((i + 1) * 100) / programacionesPagos.Count;
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
                            results.Add(result);
                            return results;

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

                            vlrContraLoc += vlrPagoLoc;
                            vlrContraExt += vlrPagoExt;
                            DTO_ComprobanteFooter footerDet = this.CrearComprobanteFooter(ctrlCxP, docCtrl.TasaCambioDOCU.Value, concCargoDef, lgDef, lineaDef,
                                vlrPagoLoc, vlrPagoExt, false);

                            if (!string.IsNullOrWhiteSpace(prog.TerceroID.Value))
                                footerDet.TerceroID.Value = prog.TerceroID.Value;
                            footer.Add(footerDet);
                            #endregion
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

                    DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(docCtrl, header.TasaCambioBase.Value, concCargoDef, lgDef, lineaDef,
                        vlrContraLoc * -1, vlrContraExt * -1, true);
                    contraP.DocumentoCOM.Value = bancoCuenta.ChequeInicial.Value.ToString();
                    footer.Add(contraP);

                    comp.Footer = footer;
                    #endregion

                    //Contabiliza
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        results.Add(result);
                        return results;
                    }
                    #endregion
                    #region Incrementa el cheque inicial
                    result = this.IncrementarChequeInicial(bancoCuenta.ID.Value);
                    if (result.Result == ResultValue.NOK)
                    {
                        results.Add(result);
                        return results;
                    }
                    #endregion

                    //Asigna el numero doc para generar el reporte
                    result.ExtraField = docCtrl.NumeroDoc.Value.ToString();
                    results.Add(result);
                    docsTransfer.Add(docCtrl); 
                }

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "TransferenciasBancarias_Transferencias");
                results.Add(result);

                return results;
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

                        foreach (DTO_glDocumentoControl doc in docsTransfer)
                        {
                            doc.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, doc.PrefijoID.Value);
                            doc.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, doc.PrefijoID.Value, doc.PeriodoDoc.Value.Value, doc.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(doc, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(doc.NumeroDoc.Value.Value, doc.ComprobanteIDNro.Value.Value, false);

                        }                      
                        #endregion
                    }
                    else
                        throw new Exception("RegistroPagoFactura - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Revierte una transferencia bancaria
        /// </summary>
        /// <param name="documentID">documento</param>
        /// <param name="comprobanteEgreso">comprobante a revertir</param>
        /// <returns></returns>
        public DTO_TxResult TransferenciasBancarias_Revertir(int documentID,DTO_Comprobante comprobanteEgreso,bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Inicia las variables globales

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            DTO_glDocumentoControl ctrlAnula = new DTO_glDocumentoControl();
            DTO_coComprobante coCompAnula = null;
           
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            try
            {
                #region Variables de datos
                DTO_ComprobanteFooter compFooterEgreso = comprobanteEgreso.Footer.First();
                int numDocCxP = Convert.ToInt32(compFooterEgreso.IdentificadorTR.Value.Value);
                int numDocEgreso = comprobanteEgreso.Header.NumeroDoc.Value.Value;
                DTO_glDocumentoControl ctrlEgreso = this._moduloGlobal.glDocumentoControl_GetByID(numDocEgreso);
                int consecAuxEgreso = compFooterEgreso.Consecutivo.Value.Value;
                #endregion

                #region Validaciones

                //Valida que el documento exista
                if (ctrlEgreso == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                    return result;
                }
                //Valida el estado
                EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), ctrlEgreso.Estado.Value.Value.ToString());
                DTO_glDocumento glDoc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, ctrlEgreso.DocumentoID.Value.Value.ToString(), true, true);
                if (estado != EstadoDocControl.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_DocNoApr;
                    return result;
                }
                //Verifica que tenga documento de reversion
                if (string.IsNullOrWhiteSpace(glDoc.DocAnula.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_NoDocAnula + "&&(" + glDoc.ID.Value + ")" + glDoc.Descriptivo.Value;
                    return result;
                }
                //Verifica que exista un comprobante de anulacion
                DTO_coComprobante coCompOld = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobanteEgreso.Header.ComprobanteID.Value, true, false);
                if (string.IsNullOrWhiteSpace(coCompOld.ComprobanteAnulID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_NoCompAnula + "&&" + coCompAnula.ID.Value;
                    return result;
                }

                //Comprobante de anulacion
                coCompAnula = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coCompOld.ComprobanteAnulID.Value, true, false);
                #endregion

                #region Crea el documento de Anulacion
                ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), glDoc.ModuloID.Value.ToLower());
                string periodoStr = this.GetControlValueByCompany(mod, AppControl.ts_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                DateTime fechaDoc = DateTime.Now;
                if (fechaDoc.Year != periodo.Year || fechaDoc.Month != periodo.Month)
                    fechaDoc = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                ctrlAnula = ObjectCopier.Clone(ctrlEgreso);
                ctrlAnula.NumeroDoc.Value = 0;
                ctrlAnula.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                ctrlAnula.DocumentoNro.Value = 0;
                ctrlAnula.DocumentoID.Value = Convert.ToInt32(glDoc.DocAnula.Value);
                ctrlAnula.PeriodoDoc.Value = periodo;
                ctrlAnula.FechaDoc.Value = fechaDoc;
                ctrlAnula.PeriodoAnula.Value = ctrlEgreso.PeriodoDoc.Value;
                ctrlAnula.DocumentoAnula.Value = ctrlEgreso.NumeroDoc.Value;
                ctrlAnula.ComprobanteID.Value = string.Empty;
                ctrlAnula.ComprobanteIDNro.Value = 0;
                ctrlAnula.Descripcion.Value = "REVERSION DE DOCUMENTO: " + ctrlEgreso.NumeroDoc.Value.Value.ToString(); 

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(ctrlAnula.DocumentoID.Value.Value, ctrlAnula, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }
                int numDoc = Convert.ToInt32(resultGLDC.Key);
                ctrlAnula.NumeroDoc.Value = numDoc;

                #endregion

                #region Revierte el comprobante

                if (!string.IsNullOrWhiteSpace(ctrlEgreso.ComprobanteID.Value))
                {
                    List<Tuple<string, int>> comprobantes = this._moduloContabilidad.Comprobante_GetComprobantesByNumeroDoc(ctrlEgreso.NumeroDoc.Value.Value);
                    foreach (Tuple<string, int> tupla in comprobantes)
                    { 
                        List<DTO_ComprobanteFooter> footerNew = new List<DTO_ComprobanteFooter>();
                        DTO_Comprobante compEgresos = this._moduloContabilidad.Comprobante_Get(true, false, ctrlEgreso.PeriodoDoc.Value.Value,tupla.Item1, tupla.Item2, null, null);
                        //Pago a Revertir
                        compFooterEgreso.DatoAdd9.Value = ctrlAnula.NumeroDoc.Value.ToString();
                        footerNew.Add(compFooterEgreso);

                        //Contrapartida
                        DTO_ComprobanteFooter contraP = compEgresos.Footer.Find(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString());
                        if(contraP != null)
                        {
                            contraP.vlrMdaLoc.Value = compFooterEgreso.vlrMdaLoc.Value.Value * -1;
                            footerNew.Add(contraP);                           
                        }                      
                        compEgresos.Footer = footerNew;
                        result = this._moduloContabilidad.Comprobante_Revertir(ctrlAnula, compEgresos, mod, ref coCompAnula);
                        if (result.Result == ResultValue.NOK)
                            return result;
                    }
                    ctrlAnula.ComprobanteID.Value = coCompAnula.ID.Value;
                    this._moduloGlobal.glDocumentoControl_Update(ctrlAnula, false, true);

                    //Actualiza DatoAdd9 del egreso
                    this._moduloContabilidad.Comprobante_Update(consecAuxEgreso, compFooterEgreso, false);
                }

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CarteraPagos_Revertir");

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

                        if (ctrlAnula.NumeroDoc.Value != null)
                        {
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
        /// Carga la informacion para la anulacion de un cheque
        /// </summary>
        /// <param name="documentID">Documento que realiza la transaccion</param>
        /// <param name="fechaPago">fecha de la operacion</param>
        /// <param name="pagoFactura">Pago factura</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_TxResult AnularCheques(int documentID, DateTime fechaPago, DTO_PagoFacturas pagoFactura, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl docCtrl = null;
            DTO_Comprobante comp = null;
            DTO_coComprobante comprobanteAnul = null;
            try
            {
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Carga las variables de todos los pagos
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, pagoFactura.BancoCuentaID.Value, true, false);
                DTO_coDocumento documento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, bancoCuenta.coDocumentoID.Value, true, false);
                DTO_coComprobante comprobante = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, documento.ComprobanteID.Value, true, false);
                comprobanteAnul = comprobante != null && !string.IsNullOrEmpty(comprobante.ComprobanteAnulID.Value)? (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobante.ComprobanteAnulID.Value, true, false) : comprobante;
                string monedaExtranjeraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                #endregion
                #region Validaciones
                DTO_tsBancosDocu docu = this._dal_tsBancosDocu.DAL_tsBancosDocu_GetByBancoCheque(bancoCuenta.ID.Value, bancoCuenta.ChequeInicial.Value);
                if (docu != null)
                {
                    DTO_glDocumentoControl ctrlExist = this._moduloGlobal.glDocumentoControl_GetByID(docu.NumeroDoc.Value.Value);
                    if (ctrlExist != null && ctrlExist.DocumentoID.Value == AppDocuments.DesembolsoFacturas)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "No es posible anular, el nro de cheque ya fue procesado verifique nuevamente";
                        return result;
                    }
                }
                #endregion
                #region Carga la tasa de cambio
                decimal tasaCambio = 0;
                if (this.Multimoneda())
                {
                    DAL_glTasaDeCambio dalTasaCambio = (DAL_glTasaDeCambio)base.GetInstance(typeof(DAL_glTasaDeCambio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    tasaCambio = dalTasaCambio.DAL_TasaDeCambio_Get(monedaExtranjeraID, DateTime.Today);
                }
                #endregion
                #region Carga la info de las fechas
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                DateTime periodoMod = Convert.ToDateTime(periodoStr);
                #endregion
                #region Carga el glDocumentoControl
                docCtrl = new DTO_glDocumentoControl();
                docCtrl.TerceroID.Value = pagoFactura.TerceroID.Value;
                docCtrl.MonedaID.Value = pagoFactura.MonedaID.Value;
                docCtrl.CuentaID.Value = ((int)documento.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documento.CuentaLOC.Value : documento.CuentaEXT.Value;
                docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                docCtrl.ProyectoID.Value = bancoCuenta.ProyectoID.Value;
                docCtrl.CentroCostoID.Value = bancoCuenta.CentroCostoID.Value;
                docCtrl.FechaDoc.Value = fechaPago;
                docCtrl.PeriodoDoc.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                docCtrl.PrefijoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                docCtrl.ComprobanteID.Value = comprobanteAnul.ID.Value;
                docCtrl.DocumentoNro.Value = 0;
                docCtrl.ComprobanteIDNro.Value = 0;
                docCtrl.DocumentoTercero.Value = bancoCuenta.ChequeInicial.Value.ToString();
                docCtrl.TasaCambioCONT.Value = tasaCambio;
                docCtrl.TasaCambioDOCU.Value = tasaCambio;
                docCtrl.DocumentoID.Value = AppDocuments.DesembolsoFacturas;
                docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                docCtrl.PeriodoUltMov.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                docCtrl.seUsuarioID.Value = this.UserId;
                docCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                docCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                docCtrl.Estado.Value = (byte)EstadoDocControl.Anulado;
                docCtrl.Descripcion.Value = "CHEQUE ANULADO ";
                docCtrl.Valor.Value = 0; 
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
                DTO_tsBancosDocu banco = new DTO_tsBancosDocu();
                if (pagoFactura.Descriptivo.Value.Length > 50)
                    banco.Beneficiario.Value = pagoFactura.BeneficiarioDesc.Value.Substring(0, 50);
                else
                    banco.Beneficiario.Value = pagoFactura.BeneficiarioDesc.Value;
                
                banco.EmpresaID.Value = this.Empresa.ID.Value;
                banco.BancoCuentaID.Value = pagoFactura.BancoCuentaID.Value;
                banco.NroCheque.Value = bancoCuenta.ChequeInicial.Value;
                banco.Valor.Value = 0;
                banco.IVA.Value = 0;
                banco.MonedaPago.Value = pagoFactura.MonedaID.Value;
                banco.ValorLocal.Value = 0;
                banco.ValorExtra.Value = 0;
                banco.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(banco);
                #endregion                
                #region Crea el comprobante y lo contabiliza
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                comp = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                #region Carga el cabezote
                header.ComprobanteID.Value = docCtrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.Fecha.Value = fechaPago;
                header.MdaTransacc.Value = docCtrl.MonedaID.Value;
                header.MdaOrigen.Value = header.MdaTransacc.Value == monedaExtranjeraID ? (byte)TipoMoneda_LocExt.Foreign : (byte)TipoMoneda_LocExt.Local;
                header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                header.PeriodoID.Value = docCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = docCtrl.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = header.TasaCambioBase.Value;
                comp.Header = header;
                #endregion
                #region Carga los detalles
                //Carga el documento de la factura
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(docCtrl.NumeroDoc.Value.Value);
                //Carga la cuenta
                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctrl.CuentaID.Value, true, false);                    
                #endregion
                // Crea la contrapartida
                DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(docCtrl, header.TasaCambioBase.Value, concCargoDef, lgDef, lineaDef, 0, 0, true);
                contraP.DocumentoCOM.Value = docCtrl.DocumentoTercero.Value;
                footer.Add(contraP);
                comp.Footer = footer;
                #region Contabiliza el comprobante
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #endregion
                #region Incrementa el cheque inicial
                result = this.IncrementarChequeInicial(pagoFactura.BancoCuentaID.Value);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                result.ExtraField = resultGLDC.Key;
                return result;

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "RegistroPagoFactura");

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
                        docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comprobanteAnul, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, docCtrl.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                    else
                        throw new Exception("AnularCheques - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Obtiene el docu de los pagos
        /// </summary>
        /// /// <returns>Lista de pagos para su programación</returns>
        public DTO_tsBancosDocu tsBancosDocu_Get(int numeroDoc)
        {
            try
            {
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_tsBancosDocu.DAL_tsBancosDocu_Get(numeroDoc);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "tsBancosDocu_Get");
                return null;
            }
        }

        #endregion

        #endregion

        #region Recibos de Caja

        #region Funciones Privadas

        /// <summary>
        /// Adiciona en tabla tsReciboCajaDocu 
        /// </summary>
        /// <param name="recibo">Recibo de Caja</param>
        /// <returns></returns>
        internal DTO_TxResult ReciboCaja_Add(DTO_tsReciboCajaDocu recibo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_tsReciboCajaDocu = (DAL_tsReciboCajaDocu)base.GetInstance(typeof(DAL_tsReciboCajaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsReciboCajaDocu.DAL_tsReciboCajaDocu_Add(recibo);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, AppDocuments.ReciboCaja, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, recibo.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReciboCaja_Add");
                return result;
            }
        }

        /// <summary>
        /// Crea un dto de reporte para Recibo de caja
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        /// <param name="comp">Datos de coAuxiliar</param>
        /// <returns></returns>
        public DTO_ReportReciboCaja DtoReciboCajaReport(int numeroDoc, DTO_Comprobante comp)
        {
            try
            {
                #region Obtener datos para el reporte

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                DTO_tsReciboCajaDocu recibo = this.ReciboCaja_Get(numeroDoc);

                DTO_ReportReciboCaja reportRecibo = new DTO_ReportReciboCaja();
                reportRecibo.CajaID = recibo.CajaID.Value;
                DTO_tsCaja cajaInfo = (DTO_tsCaja)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsCaja, recibo.CajaID.Value, true, false);
                reportRecibo.CajaDesc = ((DTO_tsCaja)cajaInfo).Descriptivo.Value;
                reportRecibo.ComprobanteID = ctrl.ComprobanteID.Value;
                reportRecibo.ComprobanteNro = ctrl.ComprobanteIDNro.Value.Value.ToString();
                reportRecibo.Fecha = ctrl.FechaDoc.Value.Value;
                reportRecibo.FechaConsigna = recibo.FechaConsignacion.Value.HasValue ? recibo.FechaConsignacion.Value.Value : ctrl.FechaDoc.Value.Value;
                reportRecibo.DocumentoDesc = ctrl.Observacion.Value;
                reportRecibo.MonedaID = ctrl.MonedaID.Value;
                reportRecibo.ReciboNro = ctrl.DocumentoNro.Value.Value.ToString();
                reportRecibo.TerceroID = ctrl.TerceroID.Value;
                DTO_coTercero terceroInfo = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, ctrl.TerceroID.Value, true, false);
                reportRecibo.TerceroDesc = ((DTO_coTercero)terceroInfo).Descriptivo.Value;
                reportRecibo.Valor = recibo.Valor.Value.Value;
                reportRecibo.Valor_letters = CurrencyFormater.GetCurrencyString("ES1", reportRecibo.MonedaID, reportRecibo.Valor);

                reportRecibo.ReciboDetail = new List<DTO_ReciboDetail>();

                foreach (DTO_ComprobanteFooter footer in comp.Footer)
                {
                    DTO_ReciboDetail reportReciboDet = new DTO_ReciboDetail();
                    reportReciboDet.CuentaID = footer.CuentaID.Value;
                    DTO_coPlanCuenta cuentaInfo = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, footer.CuentaID.Value, true, false);
                    reportReciboDet.CuentaDesc = ((DTO_coPlanCuenta)cuentaInfo).Descriptivo.Value;
                    reportReciboDet.TerceroID_cuenta = footer.TerceroID.Value;
                    terceroInfo = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, reportReciboDet.TerceroID_cuenta, true, false);
                    reportReciboDet.TerceroDesc = ((DTO_coTercero)terceroInfo).Descriptivo.Value;
                    reportReciboDet.DocumentoCOM = footer.DocumentoCOM.Value;
                    reportReciboDet.ValorML_cuenta = footer.vlrMdaLoc.Value.Value;
                    reportRecibo.ReciboDetail.Add(reportReciboDet);
                }
                #endregion
                return reportRecibo;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReciboCajaReport");
                return null;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Consulta una tabla tsReciboCajaDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Recibo</param>
        /// <returns></returns>
        public DTO_tsReciboCajaDocu ReciboCaja_Get(int NumeroDoc)
        {
            this._dal_tsReciboCajaDocu = (DAL_tsReciboCajaDocu)base.GetInstance(typeof(DAL_tsReciboCajaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_tsReciboCajaDocu.DAL_tsReciboCajaDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="cajaID">PrefijoID (corresponde a cajaID)</param>
        /// <param name="reciboNro">Numero de Documento interno</param>
        /// <returns>Retorna el recibo</returns>
        public DTO_ReciboCaja ReciboCaja_GetForLoad(int documentID, string cajaID, int reciboNro)
        {
            try
            {
                DTO_ReciboCaja res = new DTO_ReciboCaja();

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(documentID, cajaID, reciboNro);

                //Si no existe devuelve null
                if (docCtrl == null)
                    return null;

                //Verifica documento ID
                res.DocControl = docCtrl;
                //if (docCtrl.DocumentoID.Value.Value != AppDocuments.ReciboCaja)
                //    return null;

                //Carga Recibo
                DTO_tsReciboCajaDocu recibo = this.ReciboCaja_Get(docCtrl.NumeroDoc.Value.Value);
                res.ReciboCajaDoc = recibo;

                //Revisa si tiene comprobante
                if (!string.IsNullOrEmpty(docCtrl.ComprobanteID.Value))
                {
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                    bool isPre = estado == EstadoDocControl.ParaAprobacion || estado == EstadoDocControl.SinAprobar ? true : false;

                    this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_Comprobante comp = this._moduloContabilidad.Comprobante_Get(true, isPre, docCtrl.PeriodoDoc.Value.Value, docCtrl.ComprobanteID.Value, docCtrl.ComprobanteIDNro.Value.Value, null, null);

                    res.Comp = comp;
                }
                else
                    res.Comp = null;

                return res;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReciboCaja_GetForLoad");
                throw exception;
            }
        }

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="ajuste">Recibo de Caja</param>
        /// <param name="comp">Comprobante que se debe agregar</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        public DTO_TxResult ReciboCaja_Guardar(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsReciboCajaDocu recibo, DTO_Comprobante comp, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Variables Globales
            DTO_coComprobante coComp = null;
            #endregion
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                numeroDoc = 0;
                decimal porcTotal = 0;
                decimal porcParte = 100 / 4;

                #region Inicializacion Variables globales
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.Header.ComprobanteID.Value, true, false);
                #endregion
                #region Guardar en glDocumentoControl

                ctrl.DocumentoNro.Value = 0;
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
                numeroDoc = Convert.ToInt32(resultGLDC.Key);
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Guardar en tsReciboCajaDocu
                recibo.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                result = this.ReciboCaja_Add(recibo);
                if (result.Result == ResultValue.NOK)
                {
                    numeroDoc = 0;
                    return result;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion;
                #region Completa el comprobante
                comp.Header.NumeroDoc.Value = recibo.NumeroDoc.Value;
                comp.Footer.Last().DocumentoCOM.Value = ctrl.DocumentoNro.Value.ToString();
                comp.Footer.Last().IdentificadorTR.Value = ctrl.NumeroDoc.Value.Value;
                #endregion
                #region Contabiliza comprobante
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, comp.Header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);

                if (result.Result == ResultValue.NOK)
                    return result;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReciboCaja_Guardar");

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
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, comp.Header.PeriodoID.Value.Value, ctrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                    }
                    else
                        throw new Exception("ReciboCaja_Guardar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
            result.ResultMessage = DictionaryMessages.Ts_ReciboNro + "&&" + ctrl.ComprobanteIDNro.Value.Value.ToString();
            return result;
        }

        /// <summary>
        /// Migra una lista de recibos de caja
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="recibos">Lista de recibos</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> ReciboCaja_Migracion(int documentID, List<DTO_ReciboCaja> recibos, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            List<DTO_TxResult> results = new List<DTO_TxResult>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            List<DTO_glDocumentoControl> ctrls = new List<DTO_glDocumentoControl>();
            List<DTO_coComprobante> coComprobantes = new List<DTO_coComprobante>();

            #region Variables generales

            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            #endregion
            try
            {
                //Cache de comprobantes
                Dictionary<string, DTO_coComprobante> cacheComps = new Dictionary<string, DTO_coComprobante>();

                //Información por defecto
                string concCargoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                int i = 0; 
                foreach (DTO_ReciboCaja recibo in recibos)
                {
                    #region Variables

                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / recibos.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    //Variales por recibo
                    DTO_glDocumentoControl ctrl = recibo.DocControl;
                    DTO_tsReciboCajaDocu reciboDocu = recibo.ReciboCajaDoc;
                    DTO_Comprobante comprobante = recibo.Comp;

                    if (!cacheComps.ContainsKey(comprobante.Header.ComprobanteID.Value))
                    {
                        DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobante.Header.ComprobanteID.Value, true, false);
                        cacheComps[comprobante.Header.ComprobanteID.Value] = coComp;
                    }
  
                    #endregion
                    #region Guardar en glDocumentoControl

                    ctrl.DocumentoNro.Value = 0;
                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                    }
                    else
                        ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                    #endregion
                    #region Guardar en tsReciboCajaDocu
                    if (result.Result == ResultValue.OK)
                    {
                        reciboDocu.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                        result = this.ReciboCaja_Add(reciboDocu);
                    }
                    #endregion;
                    #region Completa el comprobante
                    if (result.Result == ResultValue.OK)
                    {
                        comprobante.Header.NumeroDoc.Value = ctrl.NumeroDoc.Value;

                        comprobante.Footer.ForEach(f => 
                        {
                            f.vlrMdaLoc.Value = f.vlrMdaLoc.Value * -1;
                            f.vlrMdaExt.Value = f.vlrMdaExt.Value * -1;
                        });

                        decimal vlrML = comprobante.Footer.Sum(f => f.vlrMdaLoc.Value.Value);
                        decimal vlrME = comprobante.Footer.Sum(f => f.vlrMdaExt.Value.Value);
                        DTO_ComprobanteFooter contra = this.CrearComprobanteFooter(ctrl, ctrl.TasaCambioCONT.Value, concCargoXdef, lgXdef, lineaXdef, vlrML * -1, vlrME * -1, true);
                        contra.Descriptivo.Value = "MIGRACIÓN RECIBOS DE CAJA";
                        comprobante.Footer.Add(contra);
                    }
                    #endregion
                    #region Contabiliza comprobante

                    if (result.Result == ResultValue.OK)
                        result = this._moduloContabilidad.ContabilizarComprobante(documentID, comprobante, comprobante.Header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);

                    #endregion

                    if (result.Result == ResultValue.OK)
                    {
                        ctrls.Add(ctrl);
                        coComprobantes.Add(cacheComps[comprobante.Header.ComprobanteID.Value]);
                    }

                    results.Add(result);
                }

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReciboCaja_Migracion");
                results.Add(result);

                return results;
            }
            finally
            {
                int count = results.Where(x => x.Result == ResultValue.NOK).Count();
                if (count == 0 && !insideAnotherTx)
                {
                    #region Genera el consecutivo
                    base._mySqlConnectionTx.Commit();

                    base._mySqlConnectionTx = null;
                    this._moduloGlobal._mySqlConnectionTx = null;
                    this._moduloContabilidad._mySqlConnectionTx = null;

                    int i = 0;
                    foreach(DTO_glDocumentoControl ctrl in ctrls)
                    {
                        DTO_coComprobante comp = coComprobantes[i];

                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(ctrl, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                   
                        ++i;
                    }

                    #endregion
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        #endregion

        #endregion

        #region Traslado de Fondos

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <param name="generaOrdenPago">Valor que indica si el documento genera orden de pago</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult TrasladoFondos_TrasladarFondos(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux, bool generaOrdenPago, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl contCtrl = null;
            DTO_Comprobante comp = null;
            DTO_coComprobante comprobante = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 4;

                #region Variables Globales
                //Info de caches
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                DTO_coPlanCuenta cta;
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                //Monedas
                string monedaLocalID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjeraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                //Documento
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string coDocTrasladoFondos = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_DocContableTrasladosBancaDirectos);
                if (string.IsNullOrEmpty(coDocTrasladoFondos))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.ts).ToString() + AppControl.ts_DocContableTrasladosBancaDirectos + "&&" + string.Empty;
                    return result;
                }
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, tblAux.BancoCuentaID.Value, true, false);
                DTO_coDocumento documentoTraslado = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocTrasladoFondos, true, false);
                DTO_coDocumento documento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, bancoCuenta.coDocumentoID.Value, true, false);

                DateTime periodoDoc = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));

                comprobante = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante,documentoTraslado !=null?documentoTraslado.ComprobanteID.Value : string.Empty, true, false);
                if (comprobante == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Co_NotExistComprobante;
                    return result;
                }
                #endregion
                #region Crea el documento
                ctrl.TerceroID.Value = bancoCuenta.TerceroBanco.Value;
                ctrl.MonedaID.Value = ((int)documento.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? monedaLocalID : monedaExtranjeraID;
                ctrl.CuentaID.Value = ((int)documento.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documento.CuentaLOC.Value : documento.CuentaEXT.Value;
                ctrl.EmpresaID.Value = this.Empresa.ID.Value;
                ctrl.ProyectoID.Value = bancoCuenta.ProyectoID.Value;
                ctrl.CentroCostoID.Value = bancoCuenta.CentroCostoID.Value;
                ctrl.PeriodoDoc.Value = periodoDoc;
                ctrl.PrefijoID.Value = prefijoDef;
                ctrl.DocumentoNro.Value = 0;
                ctrl.ComprobanteID.Value = documentoTraslado.ComprobanteID.Value;
                ctrl.ComprobanteIDNro.Value = 0;
                ctrl.DocumentoID.Value = documentID;
                ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                ctrl.PeriodoUltMov.Value = ctrl.PeriodoDoc.Value;
                ctrl.seUsuarioID.Value = this.UserId;
                ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                ctrl.LineaPresupuestoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                ctrl.Descripcion.Value = "CONT TRASLADO FONDOS";

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
                #region Crea la tabla auxiliar

                tblAux.EmpresaID.Value = this.Empresa.ID.Value;
                tblAux.NroCheque.Value = bancoCuenta.ChequeInicial.Value;
                tblAux.IVA.Value = 0;
                tblAux.MonedaPago.Value = ctrl.MonedaID.Value;

                tblAux.ValorLocal.Value = tblAux.Valor.Value;
                tblAux.ValorExtra.Value = 0;
                if (this.Multimoneda())
                {
                    tblAux.ValorLocal.Value = (tblAux.MonedaPago.Value == monedaLocalID) ? tblAux.Valor.Value : Math.Round(tblAux.Valor.Value.Value * ctrl.TasaCambioCONT.Value.Value, 2);
                    tblAux.ValorExtra.Value = (tblAux.MonedaPago.Value == monedaExtranjeraID) ? tblAux.Valor.Value :  Math.Round(tblAux.Valor.Value.Value / ctrl.TasaCambioCONT.Value.Value, 2);
                }

                tblAux.NumeroDoc.Value = ctrl.NumeroDoc.Value;

                this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(tblAux);
                
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Crear Comprobante y genera orden de pago
                #region Crea el header
                comp = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                header.ComprobanteID.Value = ctrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = ctrl.ComprobanteIDNro.Value.Value;
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.Fecha.Value = ctrl.FechaDoc.Value;
                header.MdaTransacc.Value = ctrl.MonedaID.Value;
                header.MdaOrigen.Value = header.MdaTransacc.Value == monedaExtranjeraID ? (byte)TipoMoneda_LocExt.Foreign : (byte)TipoMoneda_LocExt.Local;
                header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                header.PeriodoID.Value = ctrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = ctrl.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = header.TasaCambioBase.Value;

                comp.Header = header;
                #endregion
                #region Carga los detalles
                #region Carga la cuenta
                string ctaID = ctrl.CuentaID.Value;
                if (cacheCtas.ContainsKey(ctaID))
                    cta = cacheCtas[ctaID];
                else
                {
                    cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaID, true, false);
                    cacheCtas.Add(ctaID, cta);
                }
                #endregion
                #region Crea el detalle del comprobante
                DTO_ComprobanteFooter footerDet = this.CrearComprobanteFooter(ctrl, header.TasaCambioBase.Value.Value, concCargoDef, ctrl.LugarGeograficoID.Value, ctrl.LineaPresupuestoID.Value,
                    tblAux.ValorLocal.Value.Value, tblAux.ValorExtra.Value.Value, false);
                footer.Add(footerDet);
                #endregion
                #endregion
                #region Contrapartida y orden de pago
                #region Inicialización de variables para ambos procesos
                DTO_tsBancosCuenta bancoCuentaOrigen = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, tblAux.Dato1.Value, true, false);
                DTO_coDocumento documentoOrigen = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, bancoCuentaOrigen.coDocumentoID.Value, true, false);
                #endregion

                if (!generaOrdenPago)
                {
                    #region Sin generar Orden de Pago
                    string contrCtaID = ((int)documentoOrigen.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documentoOrigen.CuentaLOC.Value : documentoOrigen.CuentaEXT.Value;

                    if (cacheCtas.ContainsKey(contrCtaID))
                        cta = cacheCtas[contrCtaID];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, contrCtaID, true, false);
                        cacheCtas.Add(contrCtaID, cta);
                    }
                    contCtrl = ctrl;
                    contCtrl.CuentaID.Value = contrCtaID;
                    contCtrl.TerceroID.Value = bancoCuentaOrigen.TerceroBanco.Value;
                    #endregion
                }
                else
                {
                    #region Generando Orden de Pago
                    #region Carga las variables

                    //Definición Módulo CuentasXPagar
                    this._moduloCuentasXPagar = (ModuloCuentasXPagar)base.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    //Cuenta x Pagar
                    DTO_cpCuentaXPagar _ctaXPagar = new DTO_cpCuentaXPagar();
                    _ctaXPagar.ConceptoCxPID.Value = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_ConceptoCXPTrasladoFondos);

                    //Valida el concepto CxP
                    if (string.IsNullOrWhiteSpace(_ctaXPagar.ConceptoCxPID.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cc).ToString() + AppControl.ts_ConceptoCXPTrasladoFondos + "&&" + string.Empty;
                        return result;
                    }

                    //Obtiene Maestras requeridas
                    DTO_cpConceptoCXP conceptoCXP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, _ctaXPagar.ConceptoCxPID.Value, true, false);
                    DTO_coDocumento documentoCXP = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, conceptoCXP.coDocumentoID.Value, true, false);
                    DTO_coComprobante comprobanteCXP = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, documentoCXP.ComprobanteID.Value, true, false);

                    //Definición de la cuenta
                    string contrCtaID = ((int)documentoOrigen.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documentoCXP.CuentaLOC.Value : documentoCXP.CuentaEXT.Value;
                    if (cacheCtas.ContainsKey(contrCtaID))
                        cta = cacheCtas[contrCtaID];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, contrCtaID, true, false);
                        cacheCtas.Add(contrCtaID, cta);
                    }
                    #endregion
                    #region Carga el documento y la CxP

                    //Campos variables DTO_glDocumentoControl
                    contCtrl = new DTO_glDocumentoControl();
                    contCtrl.DocumentoID.Value = AppDocuments.CausarFacturas;
                    contCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                    contCtrl.FechaDoc.Value = ctrl.FechaDoc.Value;
                    contCtrl.PeriodoDoc.Value = ctrl.PeriodoDoc.Value;
                    contCtrl.PeriodoUltMov.Value = ctrl.PeriodoUltMov.Value;
                    contCtrl.AreaFuncionalID.Value = ctrl.AreaFuncionalID.Value;
                    contCtrl.PrefijoID.Value = ctrl.PrefijoID.Value;
                    contCtrl.Observacion.Value = ctrl.Observacion.Value;
                    contCtrl.ConsSaldo.Value = 0;
                    contCtrl.MonedaID.Value = ((int)documentoOrigen.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? monedaLocalID : monedaExtranjeraID;
                    contCtrl.TasaCambioCONT.Value = ctrl.TasaCambioCONT.Value;
                    contCtrl.TasaCambioDOCU.Value = ctrl.TasaCambioDOCU.Value;
                    contCtrl.TerceroID.Value = bancoCuentaOrigen.TerceroBanco.Value;
                    contCtrl.Estado.Value = (byte)EstadoDocControl.Radicado;
                    contCtrl.seUsuarioID.Value = ctrl.seUsuarioID.Value;
                    contCtrl.LineaPresupuestoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    contCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                    contCtrl.ProyectoID.Value = bancoCuentaOrigen.ProyectoID.Value;
                    contCtrl.CentroCostoID.Value = bancoCuentaOrigen.CentroCostoID.Value;
                    contCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    contCtrl.DocumentoNro.Value = 0;
                    contCtrl.CuentaID.Value = ((int)documentoOrigen.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documentoCXP.CuentaLOC.Value : documentoCXP.CuentaEXT.Value;
                    contCtrl.DocumentoTercero.Value = this.GetFactura(ctrl.PeriodoDoc.Value.Value, ctrl.NumeroDoc.Value.Value);
                    contCtrl.Descripcion.Value = "CONT TRASLADO FONDOS (CXP)";
                    contCtrl.Valor.Value = tblAux.Valor.Value;

                    //Campos variables DTO_cpCuentasXPagar
                    _ctaXPagar.Valor.Value = tblAux.Valor.Value;
                    _ctaXPagar.IVA.Value = 0;
                    _ctaXPagar.MonedaPago.Value = tblAux.MonedaPago.Value;
                    _ctaXPagar.FacturaFecha.Value = ctrl.FechaDoc.Value;
                    _ctaXPagar.ContabFecha.Value = ctrl.FechaDoc.Value;
                    _ctaXPagar.VtoFecha.Value = ctrl.FechaDoc.Value.Value.AddDays(2);
                    _ctaXPagar.DistribuyeImpLocalInd.Value = false;
                    _ctaXPagar.RadicaCodigo.Value = DateTime.Now.ToShortDateString();
                    #endregion
                    #region  Crear CuentaXPagar
                    int _cxpNumDoc;

                    object obj = this._moduloCuentasXPagar.CuentasXPagar_Radicar(documentID, contCtrl, _ctaXPagar, false, false, out _cxpNumDoc, batchProgress, true);
                    if (obj.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)obj;
                        return result;
                    }
                    #endregion
                    #region Asociar CxP al traslado
                    contCtrl = _moduloGlobal.glDocumentoControl_GetByID(_cxpNumDoc);
                    tblAux.NumeroDocCXP.Value = contCtrl.NumeroDoc.Value;
                    #endregion
                    #endregion
                }

                #region Crea el detalle de la Contrapartida
                DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(contCtrl, header.TasaCambioBase.Value, concCargoDef, contCtrl.LugarGeograficoID.Value, contCtrl.LineaPresupuestoID.Value,
                    tblAux.ValorLocal.Value.Value * -1, tblAux.ValorExtra.Value.Value * -1, true);
                footer.Add(contraP);

                comp.Footer = footer;
                #endregion
                #endregion

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Contabiliza el comprobante

                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "TrasladoFondos_TrasladarFondos");

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

                        ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comprobante, ctrl.PrefijoID.Value, comp.Header.PeriodoID.Value.Value, ctrl.DocumentoNro.Value.Value);

                        result.ExtraField = "Comprobante: "+ ctrl.ComprobanteID.Value + "-" + ctrl.ComprobanteIDNro.Value;
                        this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                    }
                    else
                        throw new Exception("TrasladoFondos_TrasladarFondos - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Consignaciones

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult Consignaciones_Consignar(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_Comprobante comp = null;
            DTO_coComprobante comprobante = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 3;

                #region Variables
                //Monedas
                string monedaLocalID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjeraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                //Valores por defecto
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                DateTime periodoDoc = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                //Info de documentos
                DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, tblAux.BancoCuentaID.Value, true, false);
                DTO_coDocumento documento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, bancoCuenta.coDocumentoID.Value, true, false);
                comprobante = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, documento.ComprobanteID.Value, true, false);
                //Caches
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                DTO_coPlanCuenta cta;
                #endregion
                #region Guarda el documento
                ctrl.TerceroID.Value = bancoCuenta.TerceroBanco.Value;
                ctrl.MonedaID.Value = ((int)documento.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? monedaLocalID : monedaExtranjeraID;
                ctrl.CuentaID.Value = ((int)documento.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documento.CuentaLOC.Value : documento.CuentaEXT.Value;
                ctrl.EmpresaID.Value = this.Empresa.ID.Value;
                ctrl.ProyectoID.Value = bancoCuenta.ProyectoID.Value;
                ctrl.CentroCostoID.Value = bancoCuenta.CentroCostoID.Value;
                ctrl.PeriodoDoc.Value = periodoDoc;
                ctrl.PrefijoID.Value = prefijoDef;
                ctrl.DocumentoNro.Value = 0;
                ctrl.ComprobanteID.Value = documento.ComprobanteID.Value;
                ctrl.ComprobanteIDNro.Value = 0;
                ctrl.DocumentoID.Value = documentID;
                ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                ctrl.PeriodoUltMov.Value = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                ctrl.seUsuarioID.Value = this.UserId;
                ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                ctrl.LineaPresupuestoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                ctrl.Descripcion.Value = "CONT CONSIGNACIONES";

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
                #region Guarda la tabla auxiliar
                tblAux.EmpresaID.Value = this.Empresa.ID.Value;
                tblAux.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                tblAux.NroCheque.Value = bancoCuenta.ChequeInicial.Value;
                tblAux.IVA.Value = 0;
                tblAux.MonedaPago.Value = ctrl.MonedaID.Value;

                tblAux.ValorLocal.Value = tblAux.Valor.Value;
                tblAux.ValorExtra.Value = 0;
                if (this.Multimoneda())
                {
                    tblAux.ValorLocal.Value = (tblAux.MonedaPago.Value == monedaLocalID) ? tblAux.Valor.Value : Math.Round(tblAux.Valor.Value.Value * ctrl.TasaCambioCONT.Value.Value, 2);
                    tblAux.ValorExtra.Value = (tblAux.MonedaPago.Value == monedaExtranjeraID) ? tblAux.Valor.Value : Math.Round(tblAux.Valor.Value.Value / ctrl.TasaCambioCONT.Value.Value, 2);
                }

                this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(tblAux);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region Contabiliza la consignación

                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                #region Crea el comprobante
                comp = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                header.ComprobanteID.Value = ctrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = ctrl.ComprobanteIDNro.Value.Value;
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.Fecha.Value = ctrl.FechaDoc.Value;
                header.MdaTransacc.Value = ctrl.MonedaID.Value;
                header.MdaOrigen.Value = header.MdaTransacc.Value == monedaExtranjeraID ? (byte)TipoMoneda_LocExt.Foreign : (byte)TipoMoneda_LocExt.Local;
                header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                header.PeriodoID.Value = ctrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = ctrl.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = header.TasaCambioBase.Value;

                comp.Header = header;
                #endregion
                #region Carga los detalles
                #region Carga la cuenta
                string ctaID = ctrl.CuentaID.Value;
                if (cacheCtas.ContainsKey(ctaID))
                    cta = cacheCtas[ctaID];
                else
                {
                    cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaID, true, false);
                    cacheCtas.Add(ctaID, cta);
                }
                #endregion
                #region Crea el detalle del comprobante
                DTO_ComprobanteFooter footerDet = this.CrearComprobanteFooter(ctrl, header.TasaCambioBase.Value, concCargoDef, ctrl.LugarGeograficoID.Value, ctrl.LineaPresupuestoID.Value,
                    tblAux.ValorLocal.Value.Value, tblAux.ValorExtra.Value.Value, false);
                footer.Add(footerDet);
                #endregion
                #endregion
                #region Contrapartida
                DTO_tsCaja caja = (DTO_tsCaja)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsCaja, tblAux.Dato1.Value, true, false);
                DTO_coDocumento documentoOrigen = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, caja.coDocumentoID.Value, true, false);

                string contrCtaID = ((int)documentoOrigen.MonedaOrigen.Value.Value == (int)TipoMoneda_CoDocumento.Local) ? documentoOrigen.CuentaLOC.Value : documentoOrigen.CuentaEXT.Value;

                if (cacheCtas.ContainsKey(contrCtaID))
                    cta = cacheCtas[contrCtaID];
                else
                {
                    cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, contrCtaID, true, false);
                    cacheCtas.Add(contrCtaID, cta);
                }

                DTO_glDocumentoControl contCtrl = ctrl;
                contCtrl.CuentaID.Value = contrCtaID;
                contCtrl.TerceroID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(contCtrl, header.TasaCambioBase.Value, concCargoDef, ctrl.LugarGeograficoID.Value, ctrl.LineaPresupuestoID.Value,
                    tblAux.ValorLocal.Value.Value * -1, tblAux.ValorExtra.Value.Value * -1, true);
                footer.Add(contraP);

                comp.Footer = footer;
                #endregion
                #region Contabiliza el comprobante
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                #endregion

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Consignaciones_Consignar");

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

                        ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comprobante, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                    }
                    else
                        throw new Exception("Consignaciones_Consignar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Notas Bancarias

        /// <summary>
        /// Crea una nota bancaria
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <returns></returns>
        public DTO_TxResult NotasBancarias_Radicar(int documentID, DTO_glDocumentoControl dtoCtrl, DTO_Comprobante comp, DTO_coDocumentoRevelacion revelacion, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                // Crea el glDocumentoControl
                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, dtoCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }
                int numeroDoc = Convert.ToInt32(resultGLDC.Key);
                dtoCtrl.NumeroDoc.Value = numeroDoc;
                comp.Header.NumeroDoc.Value = numeroDoc;

                //Crea el comprobante
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, dtoCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ts, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;

                //Agrega el documento de revelacion
                if (revelacion != null)
                {
                    revelacion.EmpresaID.Value = this.Empresa.ID.Value;
                    revelacion.NumeroDoc.Value = dtoCtrl.NumeroDoc.Value;
                    result = this._moduloContabilidad.DocumentoRevelacion_Add(revelacion);
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "NotasBancarias_Radicar");

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
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        DTO_coComprobante coComp = coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.Header.ComprobanteID.Value, true, false);
                        dtoCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, dtoCtrl.PrefijoID.Value);
                        dtoCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, dtoCtrl.PrefijoID.Value, comp.Header.PeriodoID.Value.Value, dtoCtrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(dtoCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(dtoCtrl.NumeroDoc.Value.Value, dtoCtrl.ComprobanteIDNro.Value.Value, false);

                        result.ResultMessage = DictionaryMessages.Co_NumberComp + "&&" + dtoCtrl.ComprobanteID.Value + "&&" + dtoCtrl.ComprobanteIDNro.Value.Value.ToString();

                        #endregion
                    }
                    else
                        throw new Exception("NotasBancarias_Radicar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que carga la información de cada cheque con sus respectivos movimientos
        /// </summary>
        /// <param name="bancoID">Filtro del bancoID</param>
        /// <param name="nit">TerceroDi</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="numCheque">Numero del cheque</param>
        /// <returns>Lista de cheques con sus respectivos detalles</returns>
        public List<DTO_ChequesGirados> GetCheques(string bancoID, string nit, DateTime fechaIni, DateTime fechaFin, string numCheque)
        {
            try
            {
                List<DTO_ChequesGirados> chequesGirados = new List<DTO_ChequesGirados>();
                List<DTO_ChequesGirados> chequesGiradosReturn = new List<DTO_ChequesGirados>();
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                //Trae los datos
                chequesGirados = this._dal_Tesoreria.DAL_Pagos_GetChequesGirados(bancoID, nit, fechaIni, fechaFin, numCheque);
                if (chequesGirados.Count == 0)
                    return chequesGiradosReturn;

                foreach (DTO_ChequesGirados dtoChequesGirados in chequesGirados)
                {
                    DTO_glLugarGeografico lugarGeo = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, dtoChequesGirados.LugarGeograficoId.Value, true, false);
                    DTO_coTercero tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                    DTO_tsBancosCuenta bancosCuenta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, dtoChequesGirados.BancoCuentaId.Value, true, false);
                    dtoChequesGirados.CuentaNumero.Value = bancosCuenta.CuentaNumero.Value;
                    dtoChequesGirados.Descriptivo.Value = bancosCuenta.Descriptivo.Value;
                    dtoChequesGirados.LugarGeografico.Value = lugarGeo.Descriptivo.Value;
                    dtoChequesGirados.Direccion.Value = tercero.Direccion.Value;
                    dtoChequesGirados.Detalle = this._dal_Tesoreria.DAL_Pagos_GetChequesGiradosDeta(dtoChequesGirados.NumDoc.Value.Value, fechaIni, fechaFin);
                    for (int i = 0; i < dtoChequesGirados.Detalle.Count; i++)
                        dtoChequesGirados.Detalle[i].Descriptivo.Value = dtoChequesGirados.Descriptivo.Value;
                    chequesGiradosReturn.Add(dtoChequesGirados);
                }
                return chequesGiradosReturn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetCheques");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga la información de los recibos de caja
        /// </summary>
        /// <param name="CajaID">Filtro de la caja</param>
        /// <param name="tercero">TerceroID</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="numReciboCaja">Numero del recibo</param>
        /// <returns>Lista de recibos</returns>
        public List<DTO_QueryReciboCaja> ReciboCaja_GetByParameter(string CajaID, string tercero, DateTime fechaIni, DateTime fechaFin, string numReciboCaja)
        {
            try
            {
                List<DTO_QueryReciboCaja> chequesGirados = new List<DTO_QueryReciboCaja>();
                this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr); 
                //Trae los datos
                chequesGirados = this._dal_Tesoreria.DAL_ReciboCaja_GetByParameter(CajaID, tercero, fechaIni, fechaFin, numReciboCaja);
                return chequesGirados;
             }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetCheques");
                throw ex;
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Funcion que retorna el detalle de los cheques de acuerdo a los filtros
        /// </summary>
        /// <param name="bancoID">Banco </param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_tsChequesGiradosTotales> GetChequesDetaAux(string bancoID, string terceroID, string orden, DateTime fechaIni, DateTime fechaFin)
        {
            #region Variables

            DTO_tsChequesGiradosTotales dtoTotalesReturn = new DTO_tsChequesGiradosTotales();
            List<DTO_tsChequesGiradosTotales> dtoTotales = new List<DTO_tsChequesGiradosTotales>();
            this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion

            try
            {
                //Trae los datos
                dtoTotalesReturn.Detalles = this._dal_Tesoreria.GetChequesDetaAux(bancoID, terceroID, fechaIni, fechaFin);

                if (orden == "1") //Romp Banco
                {
                    List<string> distinct = (from c in dtoTotalesReturn.Detalles select c.BancoCuentaId.Value).Distinct().ToList();
                    foreach (string item in distinct)
                    {
                        DTO_tsChequesGiradosTotales cheque = new DTO_tsChequesGiradosTotales();
                        cheque.Detalles = new List<DTO_ChequesGiradosDetaReport>();

                        cheque.Detalles = dtoTotalesReturn.Detalles.Where(x => x.BancoCuentaId.Value == item).ToList();
                        dtoTotales.Add(cheque);
                    }
                }
                else //Romp Tercero
                {
                    List<string> distinct = (from c in dtoTotalesReturn.Detalles select c.Nombre.Value).Distinct().ToList();
                    foreach (string item in distinct)
                    {
                        DTO_tsChequesGiradosTotales cheque = new DTO_tsChequesGiradosTotales();
                        cheque.Detalles = new List<DTO_ChequesGiradosDetaReport>();

                        cheque.Detalles = dtoTotalesReturn.Detalles.Where(x => x.Nombre.Value == item).ToList();
                        dtoTotales.Add(cheque);
                    }
                }

                return dtoTotales;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetChequesDetaAux");
                return null;
            }
        }

        /// <summary>
        /// Funcion que retorna el detalle de los cheques de acuerdo a los filtros
        /// </summary>
        /// <param name="bancoID">Banco </param>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <returns>Lista de detalles</returns>
        public List<DTO_tsChequesGiradosTotales> Report_Ts_ChequesGiradosoDetalle(string bancoID, string terceroID, string orden, DateTime fechaIni, DateTime fechaFin)
        {
            #region Variables

            DTO_tsChequesGiradosTotales dtoTotalesReturn = new DTO_tsChequesGiradosTotales();
            List<DTO_tsChequesGiradosTotales> dtoTotales = new List<DTO_tsChequesGiradosTotales>();
            this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            #endregion

            try
            {
                //Trae los datos
                dtoTotalesReturn.Detalles = new List<DTO_ChequesGiradosDetaReport>();
                dtoTotalesReturn.Detalles = this._dal_ReporteTesoreria.Report_Ts_ChequesGiradosoDetalle(bancoID, terceroID, fechaIni, fechaFin);

                List<string> distinct = (from c in dtoTotalesReturn.Detalles select c.BancoCuentaId.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    DTO_tsChequesGiradosTotales cheque = new DTO_tsChequesGiradosTotales();
                    cheque.Detalles = new List<DTO_ChequesGiradosDetaReport>();

                    cheque.Detalles = dtoTotalesReturn.Detalles.Where(x => x.BancoCuentaId.Value == item).ToList();
                    dtoTotales.Add(cheque);
                }
                return dtoTotales;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetChequesDetaAux");
                return null;
            }
        }

        /// <summary>
        /// Funcion que trae todos los recibos de caja de acuerdo al periodo
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha finl del reporte</param>
        /// <param name="nit">Tercero ID</param>
        /// <param name="caja">???</param>
        /// <returns>Lista de recibos de caja</returns>
        public List<DTO_RecibosDeCajaTotales> Report_Ts_RecibosDeCaja(DateTime fechaIni, DateTime fechaFin, string nit, string caja)
        {
            #region Variables

            DTO_RecibosDeCajaTotales dtoTotalesReturn = new DTO_RecibosDeCajaTotales();
            List<DTO_RecibosDeCajaTotales> dtoTotales = new List<DTO_RecibosDeCajaTotales>();
            this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            #endregion
            try
            {
                //Trae los datos
                dtoTotalesReturn.Detalles = new List<DTO_RecibosDeCaja>();
                dtoTotalesReturn.Detalles = this._dal_ReporteTesoreria.Report_Ts_RecibosDeCaja(fechaIni, fechaFin, nit, caja);

                List<string> distinct = (from c in dtoTotalesReturn.Detalles select c.CajaID.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    DTO_RecibosDeCajaTotales cheque = new DTO_RecibosDeCajaTotales();
                    cheque.Detalles = new List<DTO_RecibosDeCaja>();

                    cheque.Detalles = dtoTotalesReturn.Detalles.Where(x => x.CajaID.Value == item).ToList();
                    dtoTotales.Add(cheque);
                }
                return dtoTotales;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Ts_RecibosDeCaja");
                return null;
            }
        }

        /// <summary>
        /// Funcion que trae el los bancos con sus saldos
        /// </summary>
        /// <param name="fechaIni">Periodo inicial del reporte</param>
        /// <param name="fechaFin">Periodo Final del reporte</param>
        /// <param name="bancoID">BancoId como filtro</param>
        /// <returns>Lista de bancos</returns>
        public List<DTO_LibroBancos> Report_Ts_LibroBancos(DateTime fechaIni, DateTime fechaFin, string bancoID)
        {
            try
            {
                #region Variables
                List<DTO_LibroBancos> dtoLibroBancosReturn = new List<DTO_LibroBancos>();
                List<DTO_LibroBancos> dtoLibroBancos = new List<DTO_LibroBancos>();
                this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion

                dtoLibroBancos = this._dal_ReporteTesoreria.Report_Ts_LibroBancos(fechaIni, fechaFin, bancoID);
                if (dtoLibroBancos.Count != 0)
                {
                    foreach (DTO_LibroBancos item in dtoLibroBancos)
                    {
                        DTO_LibroBancos libro = new DTO_LibroBancos();
                        libro.Detalle = new List<DTO_LibroBancosDeta>();
                        List<DTO_LibroBancosDeta> details = new List<DTO_LibroBancosDeta>();
                        item.Detalle = this._dal_ReporteTesoreria.Report_Ts_LibroBancosDeta(fechaIni, fechaFin, item.IdentificadorTr.Value.Value);

                        libro = item;
                        dtoLibroBancosReturn.Add(libro);
                    }
                }
                return dtoLibroBancosReturn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Ts_LibroBancos");
                return null;
            }
        }

        /// <summary>
        /// Funcion que trae las facturas 
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="bancoID">Filtro del banco</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_RelacionPagosTotales> Report_Ts_RelacionPagos(DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque)
        {
            try
            {
                #region Variables
                List<DTO_RelacionPagosTotales> totalesReturn = new List<DTO_RelacionPagosTotales>();
                DTO_RelacionPagosTotales total = new DTO_RelacionPagosTotales();
                List<DTO_RelacionPagosDeta1> deta1 = new List<DTO_RelacionPagosDeta1>();
                List<DTO_RelacionPagosDeta2> deta2 = new List<DTO_RelacionPagosDeta2>();
                DTO_RelacionPagosDeta2 deta2Cu = new DTO_RelacionPagosDeta2();
                deta2Cu.Detalles = new List<DTO_RelacionPagosDeta1>();
                this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #endregion
                //Trae los datos
                total.Detalles = new List<DTO_RelacionPagosDeta2>();
                deta1 = this._dal_ReporteTesoreria.Report_Ts_RelacionPagos(fechaIni, fechaFin, bancoID, nit, numCheque);

                List<string> distinct = (from c in deta1 select c.Descriptivo.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    List<DTO_RelacionPagosDeta1> deta1ON = new List<DTO_RelacionPagosDeta1>();

                    deta1ON = deta1.Where(x => x.Descriptivo.Value == item).ToList();
                    //Diferencia por Número de Cheque

                    List<int> distinct2 = (from c in deta1ON select c.NroCheque.Value.Value).Distinct().ToList();
                    foreach (int item2 in distinct2)
                    {
                        List<DTO_RelacionPagosDeta1> deta1ON2 = new List<DTO_RelacionPagosDeta1>();
                        DTO_RelacionPagosDeta2 deta2ON = new DTO_RelacionPagosDeta2();
                        deta2ON.Detalles = new List<DTO_RelacionPagosDeta1>();
                        deta1ON2 = deta1ON.Where(x => x.NroCheque.Value.Value == item2).ToList();
                        deta2ON.Detalles.AddRange(deta1ON2);
                        deta2.Add(deta2ON);
                    }
                    total.Detalles.AddRange(deta2);
                }
                totalesReturn.Add(total);

                return totalesReturn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Ts_RelacionPagos");
                return null;
            }
        }

        /// <summary>
        /// Función que trae toda la relacion de pagos resumido por banco
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha Final del reporte</param>
        /// <param name="bancoID">Filtro de banco</param>
        /// <param name="nit">TerceroID</param>
        /// <param name="numCheque">Numero de cheque</param>
        /// <returns>Lista de pagos</returns>
        public List<DTO_RelacionPagosTotales> Report_Ts_RelacionPagosXBancos(DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque)
        {
            try
            {
                #region Variables
                List<DTO_RelacionPagosTotales> totalesReturn = new List<DTO_RelacionPagosTotales>();
                DTO_RelacionPagosTotales total = new DTO_RelacionPagosTotales();
                List<DTO_RelacionPagosDeta1> deta1 = new List<DTO_RelacionPagosDeta1>();
                List<DTO_RelacionPagosDeta2> deta2 = new List<DTO_RelacionPagosDeta2>();
                DTO_RelacionPagosDeta2 deta2Cu = new DTO_RelacionPagosDeta2();
                deta2Cu.Detalles = new List<DTO_RelacionPagosDeta1>();
                this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #endregion
                //Trae los datos
                total.Detalles = new List<DTO_RelacionPagosDeta2>();
                deta1 = this._dal_ReporteTesoreria.Report_Ts_RelacionPagos(fechaIni, fechaFin, bancoID, nit, numCheque);

                List<string> distinct = (from c in deta1 select c.BancoDesc.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    List<DTO_RelacionPagosDeta1> deta1ON = new List<DTO_RelacionPagosDeta1>();

                    deta1ON = deta1.Where(x => x.BancoDesc.Value == item).ToList();
                    //Diferencia por Número de Cheque

                    List<int> distinct2 = (from c in deta1ON select c.NroCheque.Value.Value).Distinct().ToList();
                    foreach (int item2 in distinct2)
                    {
                        List<DTO_RelacionPagosDeta1> deta1ON2 = new List<DTO_RelacionPagosDeta1>();
                        DTO_RelacionPagosDeta2 deta2ON = new DTO_RelacionPagosDeta2();
                        deta2ON.Detalles = new List<DTO_RelacionPagosDeta1>();
                        deta1ON2 = deta1ON.Where(x => x.NroCheque.Value.Value == item2).ToList();
                        deta2ON.Detalles.AddRange(deta1ON2);
                        deta2.Add(deta2ON);
                    }
                    total.Detalles.AddRange(deta2);
                }
                totalesReturn.Add(total);

                return totalesReturn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Ts_RelacionPagos");
                return null;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer y armar el listado de las facturas por pagar
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a Pagar</param>
        /// <returns>Listado DTO con las facturas a pagar</returns>
        public List<DTO_TesoriraTotales> ReportesTesoreria_PagosFactura(int numDoc)
        {
            List<DTO_TesoriraTotales> result = new List<DTO_TesoriraTotales>();
            DTO_TesoriraTotales pagoFac = new DTO_TesoriraTotales();
            this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            pagoFac.DetallePagosFactura = this._dal_ReporteTesoreria.DAL_ReportTesoreria_Ts_PagosFacturas(numDoc);
         
            result.Add(pagoFac);
            //}
            
            return result;
        }

        #endregion

        #region Excel

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        ///   <param name="NroCheque">ChequeNro</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_Ts_TesoreriaToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string nroCheque, string facturaNro,
                string bancoCuentaID, byte? agrup, byte? romp)
        {
            try
            {
                DataTable result;
                this._dal_ReporteTesoreria = (DAL_ReportesTesoreria)this.GetInstance(typeof(DAL_ReportesTesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_ReporteTesoreria.DAL_Reportes_Ts_TesoreriaToExcel(documentoID, tipoReporte, fechaIni, fechaFin, tercero, nroCheque,facturaNro, bancoCuentaID, agrup, romp);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_Cp_CxPToExcel");
                throw ex;
            }
        }


        #endregion
    }
}

