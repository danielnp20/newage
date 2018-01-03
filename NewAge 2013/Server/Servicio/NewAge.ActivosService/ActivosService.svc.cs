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
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.Librerias.ExceptionHandler;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using SentenceTransformer;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.Server.ActivosService
{
    /// <summary>
    /// Clase AppService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ActivosService : IActivosService, IDisposable
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
        public ActivosService()
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
        public ActivosService(string connString, string connLoggerString)
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

        #region Activos Fijos

        #region Procesos

        /// <summary>
        /// Genera la depreciacion de los activos
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="periodo">Periodo de cierre</param>
        public DTO_TxResult Proceso_GenerarDepreciacionActivos(Guid channel, int documentID, DateTime periodo)
        {
            if (_currentProcess.Contains(documentID))
            {
                return new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Proceso_GenerarDepreciacionActivos(documentID, periodo, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera un Reproceso de la Depreciación por Unidades de Producción
        /// </summary>
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="fechaIni">fechaIni</param>
        /// <param name="fechaFinal">fechaFinal</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Proceso_ReProcesoDepreciacion(Guid channel, int documentID, DateTime fechaIni, DateTime fechaFinal)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            if (_currentProcess.Contains(documentID))
            {
                results.Add(new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                });
                return results;
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Proceso_ReProcesoDepreciacion(documentID, fechaIni, fechaFinal, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region acActivoControl

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        public DTO_TxResultDetail acActivoControl_Add(Guid channel, int documentID, DTO_acActivoControl acCtrl, string documento6ID, bool updateRecibido)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_Add(documentID, acCtrl, documento6ID, false, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega varios registros al control de activos
        /// </summary>
        public List<DTO_TxResult> acActivoControl_AddList(Guid channel, int documentoID, string actividadFlujoID, string prefijo, string tipoMvto, List<DTO_acActivoControl> docCtrls, decimal tasaCambio)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_AddList(documentoID, actividadFlujoID, prefijo, docCtrls, tipoMvto, tasaCambio, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega varios registros al control de activos
        /// </summary>
        public List<DTO_TxResult> acActivoControl_AddListActivos(Guid channel, int documentoID, int activoID, string actividadFlujoID, string prefijoID, string tipoMvto, List<DTO_acActivoControl> docCtrls, decimal tasaCambio)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_AddListActivos(documentoID, activoID, actividadFlujoID, prefijoID, docCtrls, tipoMvto, tasaCambio, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza varios registros en el acActivoControl
        /// </summary>       
        public List<DTO_TxResult> acActivoControl_Update(Guid channel, List<DTO_acActivoControl> acCtrl, string tipoMvto, int activoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                result = modulo.acActivoControl_Update(acCtrl, tipoMvto, activoID);

                results.Add(result);
                return results;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza la plaqueta
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="acActivoControl"></param>
        /// <param name="tipoMvto"></param>
        /// <param name="activoID"></param>
        /// <param name="prefijo"></param>
        /// <returns></returns>
        public DTO_TxResult acActivoControl_UpdatePlaqueta(Guid channel, List<DTO_acActivoControl> acActivoControl, string tipoMvto, int activoID, string prefijo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_UpdatePlaqueta(acActivoControl, tipoMvto, activoID, prefijo, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los activos fijos para la Empresa actual
        /// </summary>
        /// <returns></returns>
        public List<DTO_acActivoControl> acActivoControl_Get(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_Get();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene los activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public List<DTO_acActivoControl> acActivoControl_GetFilters(Guid channel, string serialID,
                                                                        string PlaquetaID,
                                                                        string locFisicaID,
                                                                        string referenciaID,
                                                                        string centroCosto,
                                                                        string proyecto,
                                                                        string clase,
                                                                        string tipo,
                                                                        string grupo,
                                                                        string responsable,
                                                                        bool isContenedor,  
                                                                        int pageSize,
                                                                        int pageNum)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetFilters(serialID, PlaquetaID, locFisicaID, referenciaID, centroCosto, proyecto, clase, tipo, grupo, responsable, isContenedor, pageSize, pageNum);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el numero de activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="centroCosto">identificador centro de costo</param>
        /// <param name="proyecto">identificador de proyecto</param>
        /// <param name="clase">identificador de clase</param>
        /// <param name="tipo">identificador de tipo</param>
        /// <param name="grupo">identificador de grupo</param>
        /// <param name="responsable">resposable</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public int acActivoControl_GetFiltersCount(Guid channel, string serialID,
                                                        string PlaquetaID,
                                                        string locFisicaID,
                                                        string referenciaID,
                                                        string centroCosto,
                                                        string proyecto,
                                                        string clase,
                                                        string tipo,
                                                        string grupo,
                                                        string responsable,
                                                        bool isContenedor
                                                       )
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetFiltersCount(serialID, PlaquetaID, locFisicaID, referenciaID, centroCosto, proyecto, clase, tipo, grupo, responsable, isContenedor);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_acActivoControl acActivoControl_GetByID(Guid channel, int activoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetByID(activoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un activo control
        /// </summary>
        /// <param name="plaqueta">Plaqueta</param>
        /// <returns>Retorna un activo</returns>
        public DTO_acActivoControl acActivoControl_GetByPlaqueta(Guid channel, string plaquetaID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetByPlaqueta(plaquetaID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// tare uno o  varios registros del acActivoControl de acuerdo al parametro.
        /// </summary>       
        public List<DTO_acActivoControl> acActivoControl_GetBy_Parameter(Guid channel, DTO_acActivoControl acCtrl)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetByParameter(acCtrl);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// tare uno o  varios registros del acActivoControl de acuerdo al parametro.
        /// </summary>       
        public List<DTO_acActivoControl> acActivoControl_GetByParameterForTranfer(Guid channel, DTO_acActivoControl acCtrl)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetByParameterForTranfer(acCtrl);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Alta de Activos

        /// <summary>
        /// Obtiene la lista de activos recibidos por numero de Factura
        /// </summary>
        /// <param name="numeroDoc">Numero de factura</param>
        /// <returns>Lista de activos</returns>
        public List<DTO_acActivoControl> AltaActivos_GetActivosByNumDoc(Guid channel, int numeroDoc)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos module = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.AltaActivos_GetActivosByNumDoc(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Traslado de Activos

        /// <summary>
        /// Actualiza varios registros en el acActivoControl de acuerdo al movimiento
        /// </summary>       
        public List<DTO_TxResult> acActivoControl_TrasladoActivos(Guid channel, int documentoID, List<DTO_acActivoControl> acCtrl, int activoID, string prefijoID, string tipoMvto, DateTime fechaDocu)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.TrasladoActivos(documentoID, acCtrl, activoID, prefijoID, tipoMvto, fechaDocu, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Saldos Activos

        /// <summary>
        /// Obtiene una lista de saldos de un activo
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <param name="periodo">Periodo en el q se encuetra</param>
        /// <returns>Lista de saldos de activos</returns>
        public List<DTO_acActivoSaldo> acActivoControl_CargarSaldos(Guid channel, int identificadorTR, DateTime periodo, string clase)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_CargarSaldos(identificadorTR, periodo, clase);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga una lista de  dto_ActivoControl con los saldos por meses de acuerdo al año del periodo
        /// </summary>
        /// <param name="periodo">Periodo de busqueda</param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <returns>Lista de saldos de un activo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_CargarSaldos_Meses(Guid channel, string año, int identificadorTR, string activoClaseID, bool mLocal)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_CargarSaldos_Meses(año, identificadorTR, activoClaseID, mLocal);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los movimientos por componente de cada activo 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <param name="periodo">Periodo actual</param>
        /// <param name="clase">Clase de activo</param>
        /// <returns>Lista de DTO_activoSaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_CargarMvtos(Guid channel, int identificadorTR, DateTime periodo, string clase)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_CargarMvtos(identificadorTR, periodo, clase);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que trae el comprobante de acuerdo al numeroDoc y el idTR
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna una lista de comprobanteFooter</returns>
        public List<DTO_ComprobanteFooter> acActivoControl_GetByIdentificadorTR(Guid channel, int numeroDoc, int identTR)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetByIdentificadorTR(numeroDoc, identTR);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion para actualiza saldos correspondiente a activos.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">int id del documento</param>
        /// <param name="prefijoID">string prefijo del modlo</param>
        /// <param name="tipoMvto">tipo de mov q viene de la pantalla e identifica la transaccion</param>
        /// <param name="dto_activoSaldo">lista de saldos a actualizar</param>
        /// <param name="acActivoCtrl">lista de activos a los cuales se les actualiza los saldos</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_UpdateSaldos(Guid channel, int documentoID, string actividadFlujoID, string prefijoID, string tipoMvto, List<DTO_acActivoSaldo> dto_activoSaldo, List<DTO_acActivoControl> acActivoCtrl)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                List<DTO_TxResult> response = modulo.acActivoControl_UpdateSaldos(documentoID, actividadFlujoID, prefijoID, tipoMvto, dto_activoSaldo, acActivoCtrl, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los compponentes de acContabiliza de acuerdo a la clase del activo
        /// </summary>
        /// <param name="clase">Activo clase </param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetComponentesPorClaseActivoID(Guid channel, string clase)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetComponentesPorClaseActivoID(clase);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que trae una lista de saldos por componente
        /// </summary>
        /// <param name="periodo">preiodo del modulo</param>
        /// <param name="componentes">Lista con info del componetne</param>
        /// <param name="concSaldo">Compnente</param>
        /// <param name="identificadorTR">Id del activo</param>
        /// <param name="activoClaseID">Clase del activoID</param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetSaldoXComponente(Guid channel, DateTime periodo, List<DTO_acActivoSaldo> componentes, int identificadorTR, string activoClaseID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetSaldoXComponente(periodo, componentes, identificadorTR, activoClaseID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Retiro Activos

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> RetiroActivos_GetComponenentes(Guid channel, int activoID, string tipoBalance)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.RetiroActivos_GetComponenentes(activoID, tipoBalance);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> acActivoFijos_GetComponenentes(Guid channel, int activoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoFijos_GetComponenentes(activoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Da de baja un activo
        /// </summary>
        /// <param name="documentoID">Identificador Documento</param>
        /// <param name="actividadFlujoID">Identificador Actividad</param>
        /// <param name="prefijoID">Prefijo</param>
        /// <param name="acActivoControlList">Lista activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="batchProgress">Indicador de Progreso</param>
        /// <param name="insideAnotherTx">Indica si viene de una Transacción</param>
        /// <returns></returns>
        public List<DTO_TxResult> acActivoControl_RetiroActivos(Guid channel, int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_RetiroActivos(documentoID, actividadFlujoID, prefijoID, acActivoControlList, tipoMvto, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Deterioro Activos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un listado de saldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetSaldoCompraActivo(Guid channel, string concSaldo, int identificadorTR, string tipoBalance)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetSaldoCompraActivo(concSaldo, identificadorTR, tipoBalance);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Crear el comprobante de Deterioro o Revalorización de un listado de Activos
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="documentoID">documento generador</param>
        /// <param name="actividadFlujoID">actividad Flujo</param>
        /// <param name="balanceID">identificador del balance</param>
        /// <param name="prefijoID">prefijo documento</param>
        /// <param name="acActivoControlList">listado de activos</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <param name="insideAnotherTx">determina si procede de una transacción</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_Deterioro(Guid channel, int documentoID, string actividadFlujoID, string balanceID, string prefijoID, bool deterioroInd, string tipoMov, DTO_coDocumentoRevelacion revelacion, List<DTO_acActivoControl> acActivoControlList)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_Deterioro(documentoID, actividadFlujoID, balanceID, prefijoID, deterioroInd, tipoMov, revelacion, acActivoControlList, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Contenedores

        /// <summary>
        /// Obtiene los activos hijos 
        /// </summary>
        /// <param name="activoID">activoID padre</param>
        /// <returns>listado de activos control</returns>
        public List<DTO_acActivoControl> acActivoControl_GetChildrenActivos(Guid channel, int activoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_GetChildrenActivos(activoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region acActivoGarantia

        /// <summary>
        /// Trae lista activo garantia para plantilla de importacion
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public List<DTO_acActivoGarantia> acActivoGarantia_GetForImport(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoGarantia_GetForImport();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega una lista de activo garantia
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_TxResult acActivoGarantia_Add(Guid channel, int documentoID, List<DTO_acActivoGarantia> acGar)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoGarantia_Add(documentoID,acGar,DictionaryProgress.BatchProgress,false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Movimientos Activos

        /// <summary>
        /// Movimientos Activos 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="activoID">activoID</param>
        /// <param name="actividadFlujoID">actividadFlujoID</param>
        /// <param name="prefijoID">prefijoID</param>
        /// <param name="acActivoControlList">Listado de Activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="proyectoID">proyectoID</param>
        /// <param name="centroCostoID">centroCostoID</param>
        /// <param name="locFisicaID">locFisicaID</param>
        /// <param name="responsable">responsable</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <param name="insideAnotherTx">indica si viene de una transaccion</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_MovActivos(Guid channel, int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto,
                                                             string proyectoID, string centroCostoID, string locFisicaID, string responsable)
        {
            if (_currentProcess.Contains(documentoID))
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
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.acActivoControl_MovActivos(documentoID, actividadFlujoID, prefijoID, acActivoControlList, tipoMvto, proyectoID, centroCostoID, locFisicaID, responsable, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que obtiene la información del activo de acuerdo al filtro
        /// </summary>
        /// <param name="plaqueta">Filtro de PLaquetaID</param>
        /// <param name="serial">Filtro de SerialID</param>
        /// <param name="referencia">Filtro de Referencia</param>
        /// <param name="clase">Filtro de ActivoClaseID</param>
        /// <param name="tipo">Filtro de ActivoTipoID</param>
        /// <param name="grupo">Filtro de ActivoGrupoID</param>
        /// <param name="locFisica">Filtro de LocfisicaIF</param>
        /// <param name="contenedor">Bool que dice si trae o no los activos contenidos</param>
        /// <param name="responsable">Filtro de Responsable - TerceroID</param>
        /// <returns>Lista de detalles para la consulta</returns>
        public List<DTO_acQueryActivoControl> ActivoGetByParameter(Guid channel, string plaqueta, string serial, string referencia, string clase, string tipo, string grupo, string locFisica, bool contenedor, string responsable)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ActivoGetByParameter(plaqueta, serial, referencia, clase, tipo, grupo, locFisica, contenedor, responsable);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcionque obrtinee los saldos correspondientes al activo
        /// </summary>
        /// <param name="identificadorTr">Activo ID del activo</param>
        /// <param name="balanceTipoID">Tipo de Libro (Funcional o IFRS)</param>
        /// <param name="año">Año que se va a consultar</param>
        /// <param name="mes">Mes que se quiere consultar</param>
        /// <returns>Lista de saldos por activo</returns>
        public List<DTO_acActivoQuerySaldos> ActivoControl_GetSaldosByMesYLibro(Guid channel, int identificadorTr, string balanceTipoID, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloActivosFijos modulo = (ModuloActivosFijos)facade.GetModule(ModulesPrefix.ac, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ActivoControl_GetSaldosByMesYLibro(identificadorTr, balanceTipoID, periodo);
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
