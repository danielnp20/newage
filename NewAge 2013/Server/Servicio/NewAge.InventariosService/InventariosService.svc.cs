using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using NewAge.Librerias.ExceptionHandler;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using SentenceTransformer;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.Server.InventariosService
{
    /// <summary>
    /// Clase AppService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class InventariosService : IInventariosService, IDisposable
    {
        #region Variables

        /// <summary>
        /// Diccionario con la lista de conexiones (canales abiertos)
        /// </summary>
        private static Dictionary<Guid, Tuple<DTO_glEmpresa, int>> _channels = new Dictionary<Guid, Tuple<DTO_glEmpresa, int>>();

        /// <summary>
        /// Lista de procesos que se están corriendo
        /// </summary>
        private static List<int> _currentProcess = new List<int>();

        /// <summary>
        /// Cadena de conexion
        /// </summary>
        private string _connString = string.Empty;

        /// <summary>
        /// Cadena de conexion
        /// </summary>
        private string _connLoggerString = string.Empty;
        
        /// <summary>
        /// Get or sets the connection
        /// </summary>
        private List<SqlConnection> _mySqlConnections = new List<SqlConnection>();

        #endregion

        #region Modelos

        /// <summary>
        /// Fachada de acceso a los modulos
        /// </summary>
        ModuloFachada facade = new ModuloFachada();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventariosService()
        {
            this._connString = ConfigurationManager.ConnectionStrings["sqlConnectionString"].ToString();
            this._connLoggerString = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString();
            if (this._mySqlConnections.Count == 0)
            {
                SqlConnection conn = new SqlConnection(this._connString);
                this._mySqlConnections.Add(conn);
            }
        }

        /// <summary>
        /// Constructor con el nombre de una cadena de conexion determinada
        /// </summary>
        /// <param name="connecctionStringName">Cadena de conexion completa</param>
        public InventariosService(string connString, string connLoggerString)
        {
            this._connString = connString;
            this._connLoggerString = connLoggerString;
            if (this._mySqlConnections.Count == 0)
            {
                SqlConnection conn = new SqlConnection(this._connString);
                this._mySqlConnections.Add(conn);
            }
        }

        /// <summary>
        /// Crea una nueva sesion pra el usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        public void CrearCanal(Guid channel)
        {
            Tuple<DTO_glEmpresa, int> t = new Tuple<DTO_glEmpresa, int>(null, 0);
            if (!_channels.ContainsKey(channel))
                _channels.Add(channel, t);
        }

        /// <summary>
        /// Cierra la sesion de un usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        public void CerrarCanal(Guid channel)
        {
            if (_channels.ContainsKey(channel))
                _channels.Remove(channel);
        }

        /// <summary>
        /// Inicializa la empresa y el usuario
        /// </summary>
        public void IniciarEmpresaUsuario(Guid channel, DTO_glEmpresa emp, int userID)
        {
            Tuple<DTO_glEmpresa, int> t = new Tuple<DTO_glEmpresa, int>(emp, userID);
            _channels[channel] = t;
        }

        #endregion

        #region DBConnection

        /// <summary>
        /// Retorna el indice de una conexion que este disponible
        /// </summary>
        /// <returns>Retorna una conexion disponible</returns>
        private int GetConnectionIndex()
        {
            int result = -1;
            SqlConnection conn;
            for (int i = 0; i < this._mySqlConnections.Count; ++i)
            {
                conn = this._mySqlConnections[i];
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    result = i;
                }
            }

            if (result == -1)
            {
                conn = new SqlConnection(this._connString);
                this._mySqlConnections.Add(conn);
                result = this._mySqlConnections.Count - 1;
            }

            return result;
        }

        /// <summary>
        /// Conecta al provedor Sql
        /// </summary>
        /// <returns>Retorna el indice con la conexion que se esta usando</returns>
        private int ADO_ConnectDB()
        {
            int connIndex = -1;
            try
            {
                connIndex = this.GetConnectionIndex();
                this._mySqlConnections[connIndex].Open();
                return connIndex;
            }
            catch
            {
                this.ADO_CloseDBConnection(connIndex);
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión
        /// </summary>
        public void ADO_CloseDBConnection(int connIndex)
        {
            try
            {
                if (connIndex == -1)
                {
                    this._mySqlConnections = new List<SqlConnection>();

                    SqlConnection conn = new SqlConnection(this._connString);
                    this._mySqlConnections.Add(conn);
                }
                else
                {
                    if (this._mySqlConnections[connIndex].State != ConnectionState.Closed)
                        this._mySqlConnections[connIndex].Close();
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción
        /// </summary>
        /// <param name="DocumentID">Identificador del proceso (documento)</param>
        /// <returns>Retorna el porcentaje del progreso</returns>
        public int ConsultarProgreso(Guid channel, int documentID)
        {
            int prog = DictionaryProgress.ConsultarProgreso(_channels[channel].Item2, documentID);
            return prog == 0 ? 1 : prog;
        }

        #endregion

        #region Inventarios

        #region Comprobantes

        /// <summary>
        /// Proceso de posteo de comprobantes para el modulo de inventarios
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna la lista de resultados (uno por cada comprobante)</returns>
        public List<DTO_TxResult> PosteoComprobantes(Guid channel, int documentID)
        {
            if (_currentProcess.Contains(documentID))
            {
                List<DTO_TxResult> results = new List<DTO_TxResult>();
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                results.Add(r);
                return results;
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.PosteoComprobantes(documentID, DictionaryProgress.BatchProgress, false);

                return response;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba una lista de posteo decomprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento qu eejecuta la transaccion</param>
        /// <param name="mod">Modulo del cual se esta aprobando el posteo</param>
        /// <param name="docs">Lista de documentos a aprobar</param>
        /// <returns>Retorna el resultado dela operacion</returns>
        public List<DTO_TxResult> AprobarPosteo(Guid channel, int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_glDocumentoControl> docs)
        {
            if (_currentProcess.Contains(documentID))
            {
                List<DTO_TxResult> results = new List<DTO_TxResult>();
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                results.Add(r);
                return results;
            }
            int opIndex = -1;

            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.AprobarPosteo(documentID, actividadFlujoID, currentMod, docs, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Movimientos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject Transaccion_Add(Guid channel, int documentID, DTO_MvtoInventarios transaccion, bool update, out int numeroDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numeroDoc = 0;
                return r;
            }
            int opIndex = -1;

            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Transaccion_Add(documentID, transaccion, update, out numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///Obtiene una transaccion manual
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <param name="trasladoNotaEnvio">Indica si es traslado de Nota Envio</param>
        /// <returns>DTO_inControlSaldosCostos</returns>
        public DTO_MvtoInventarios Transaccion_Get(Guid channel, int documentID, int numeroDoc, bool trasladoNotaEnvio)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Transaccion_Get(numeroDoc, trasladoNotaEnvio);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Envia para aprobacion una transaccion manual
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la transacción</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Transaccion_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Transaccion_SendToAprob(documentID, numeroDoc, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///Obtiene la relacion de saldos y costos para las salidas por referencia
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="saldo">dto para filtrar</param>
        /// <param name="costoTotal">costoTotal</param>
        public decimal Transaccion_SaldoExistByReferencia(Guid channel, int documentID, DTO_inControlSaldosCostos saldo, ref DTO_inCostosExistencias costo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Transaccion_SaldoExistByReferencia(documentID, saldo, ref costo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrlSaldoCosto"></param>
        /// <returns>Dto de Control de saldos y costos</returns>
        public List<DTO_inControlSaldosCostos> inControlSaldosCostos_GetByParameter(Guid channel, int documentID, DTO_inControlSaldosCostos ctrlSaldoCosto)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.inControlSaldosCostos_GetByParameter(documentID, ctrlSaldoCosto);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una lista de notas envio 
        /// </summary>        
        /// <returns>Retorna una lista de notas envio</returns>
        public List<DTO_NotasEnvioResumen> NotasEnvio_GetResumen(Guid channel, int documentID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.NotasEnvio_GetResumen();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe o devuelve entradas de una Nota de Envio
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="notaEnvio">resumen de mvtos</param>
        /// <param name="actFlujoID">Actividad actual</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject NotaEnvio_RecibirDevolver(Guid channel, int documentID, DTO_MvtoInventarios notaEnvio, string actFlujoID, bool createDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.NotaEnvio_RecibirDevolver(documentID, notaEnvio,actFlujoID, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Obtiene una lista de movimientos Docu
        /// </summary>
        /// <param name="documentoID">Documento Asociado</param>
        /// <param name="header">Filtro para consulta</param>
        /// <returns>List DTO_inMovimientoDocu </returns>
        public List<DTO_inMovimientoDocu> inMovimientoDocu_GetByParameter(Guid channel, int documentoID, DTO_inMovimientoDocu header)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.inMovimientoDocu_GetByParameter(documentoID,header);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        /// <summary>
        /// Actualiza la tabla inMovimientoDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult inMovimientoDocu_Upd(Guid channel,int documentoID, DTO_inMovimientoDocu header)
        {
            if (_currentProcess.Contains(documentoID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.inMovimientoDocu_Upd(documentoID, header);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Inventario Fisico

        /// <summary>
        /// Guardar el inventario fisico
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="bodega">Bodega relacionada</param>
        /// <param name="periodo">Periodo del inventario fisico</param>
        /// <param name="numeroDoc">Numero Doc relacionado</param>
        /// <param name="fisicoInventario"></param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject InventarioFisico_Add(Guid channel, int documentID, string bodega, DateTime periodo, out int numeroDoc, List<DTO_inFisicoInventario> fisicoInventario)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numeroDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.InventarioFisico_Add(documentID, bodega, periodo, out numeroDoc, fisicoInventario, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///Obtiene un inventario fisico
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="fisicoInv">Dto filtro</param>
        /// <returns> lista de DTO_inFisicoInventario</returns>
        public List<DTO_inFisicoInventario> InventarioFisico_Get(Guid channel, int documentID, DTO_inFisicoInventario fisicoInv)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.InventarioFisico_Get(documentID, fisicoInv);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Envia para aprobacion un inventario fisico
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject InventarioFisico_SendToAprob(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, bool createDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.InventarioFisico_SendToAprob(documentID, actividadFlujoID, numeroDoc, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de inventario fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un listado</returns>
        public List<DTO_InvFisicoAprobacion> InventarioFisico_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.InventarioFisico_GetPendientesByModulo(mod, actividadFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba o rechaza Inventario Fisico
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="invFisico">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> InventarioFisico_AprobarRechazar(Guid channel, int documentID, List<DTO_InvFisicoAprobacion> invFisico, string actFlujoID, bool updDocCtrl, bool createDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                results.Add(r);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.InventarioFisico_AprobarRechazar(documentID, invFisico, actFlujoID,updDocCtrl, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        /// <summary>
        /// Elimina los saldos guardados de la bodega actual
        /// </summary>        
        public void InventarioFisico_Delete(Guid channel,int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                modulo.InventarioFisico_Delete(numeroDoc);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Distribucion Costos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="costosDist">Dto de Distribucion Costos</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject inDistribucionCostos_Add(Guid channel, int documentID, DTO_MvtoInventarios transaccion,List<DTO_inDistribucionCosto> costosDist, bool update, out int numeroDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numeroDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.inDistribucionCostos_Add(documentID, transaccion, costosDist, update, out numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        /// <summary>
        ///Obtiene una lista de Distribucion Costo
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="numeroDoc">numero Doc inv</param>
        /// <param name="byNroDocFact">Indica si filtra por numero doc de la factura</param>
        /// <returns> lista de DTO_inDistribucionCosto</returns>
        public List<DTO_inDistribucionCosto> inDistribucionCosto_GetByNumeroDoc(Guid channel, int documentID, int numeroDoc, bool byNroDocFact)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.inDistribucionCosto_GetByNumeroDoc(documentID, numeroDoc, byNroDocFact);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        #endregion

        #region Liquidacion de Importacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="importacion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject LiquidacionImportacion_Add(Guid channel,int documentID, DTO_LiquidacionImportacion importacion, bool update, out int numeroDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numeroDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.LiquidacionImportacion_Add(documentID, importacion, update, out numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Deterioro / Revalorizacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="deterioro">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject Deterioro_Add(Guid channel, int documentID, DTO_MvtoInventarios deterioro, bool update, out int numeroDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numeroDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Deterioro_Add(documentID, deterioro,update, out numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Envia para aprobacion un deterioro
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Deterioro_SendToAprob(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, bool createDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Deterioro_SendToAprob(documentID, actividadFlujoID, numeroDoc, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba o rechaza un Deterioro
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="deterioro">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DTO</returns>
        public List<DTO_SerializedObject> Deterioro_AprobarRechazar(Guid channel, int documentID, List<DTO_inDeterioroAprob> deterioro, string actFlujoID, bool createDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                results.Add(r);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Deterioro_AprobarRechazar(documentID, deterioro, actFlujoID, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de deterioro fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        ///  <param name="actFlujoID">Actividad Actual</param>
        /// <returns>Retorna un listado</returns>
        public List<DTO_inDeterioroAprob> inMovimientoDocu_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.inMovimientoDocu_GetPendientesByModulo(mod, actFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Traslado Saldos de inventario a un nuevo periodo
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="periodoID">periodo actual del modulo</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_TxResult Proceso_TrasladoSaldosInventario(Guid channel, int documentID, DateTime periodoOld)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Proceso_TrasladoSaldosInventario(documentID,periodoOld,DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region  Orden Salida

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="ordenSalida">Dto de la Orden Salida</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject OrdenSalida_Add(Guid channel, int documentID, DTO_OrdenSalida ordenSalida, bool update, out int numeroDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numeroDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.OrdenSalida_Add(documentID, ordenSalida, update, out numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene una Orden de Salida
        /// </summary>
        /// <param name="bodegaID">Bodega a Filtrar</param>
        /// <returns>Una orden</returns>
        public DTO_OrdenSalida OrdenSalida_GeyByBodega(Guid channel, string bodegaID, int? numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios modulo = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.OrdenSalida_GeyByBodega(bodegaID,numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba la Orden de Salida y cambia el estado
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        public DTO_SerializedObject OrdenSalida_ApproveOrden(Guid channel, int documentID, DTO_OrdenSalida orden)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.OrdenSalida_ApproveOrden(documentID, orden, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        } 

        /// <summary>
        /// Genera un  mvto de salida de Inventarios a partir de la ORden de Salida
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        public DTO_SerializedObject OrdenSalida_ApproveMvtoInv(Guid channel, int documentID, DTO_OrdenSalida orden)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.OrdenSalida_ApproveMvtoInv(documentID, orden, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        } 

        #endregion

        #region Consultas
        
        /// <summary>
        /// Funcion que obriene la informacion del serial
        /// </summary>
        /// <param name="serial">Filtro SerialID</param>
        /// <param name="bodegaID">Filtro de BodegaID</param>
        /// <param name="inReferenciaID">Filtro de Referencia</param>
        /// <param name="inCliente">Filtro de TerceroID</param>
        /// <returns>Lista de detalles de las consultas</returns>
        public List<DTO_inQuerySeriales> inSaldosExistencias_GetBySerial(Guid channel,string serial, string bodegaID, string inReferenciaID, string inCliente)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloInventarios mod = (ModuloInventarios)facade.GetModule(ModulesPrefix.@in, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.inSaldosExistencias_GetBySerial(serial, bodegaID, inReferenciaID, inCliente);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            this.ADO_CloseDBConnection(-1);
            GC.SuppressFinalize(this);
        }
    }
}
