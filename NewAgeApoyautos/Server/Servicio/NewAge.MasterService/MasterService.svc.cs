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
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Server.MasterService
{
    /// <summary>
    /// Clase MasterService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MasterService : IMasterService, IDisposable
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

        /// <summary>
        /// Master Complex (Multiple PK)
        /// </summary>
        private DAL_MasterComplex _masterComplex;

        /// <summary>
        /// Master Hierarchy basic
        /// </summary>
        private DAL_MasterHierarchy _masterHierarchy;

        /// <summary>
        /// Master Simple
        /// </summary>
        private DAL_MasterSimple _masterSimple;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MasterService()
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
        public MasterService(string connString, string connLoggerString)
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

        #region Maestras

        #region MasterComplex - Llaves multiples

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="pks">Identificador de la maestra</param>
        /// <param name="EmpresaGrupoID">Identificador por el cual se filtra</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterComplex MasterComplex_GetByID(Guid channel, int documentID, Dictionary<string, string> pks, bool active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                DTO_MasterComplex response = this._masterComplex.DAL_MasterComplex_GetByID(pks, active);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Cantidad de registros de una maestra
        /// </summary>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros de la consulta</returns>
        public long MasterComplex_Count(Guid channel, int documentID, DTO_glConsulta consulta, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                long response = this._masterComplex.DAL_MasterComplex_Count(consulta, active);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los registros de una maestra compleja
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtro</param>
        /// <returns>Devuelve los registros de una maestra compleja</returns>
        public IEnumerable<DTO_MasterComplex> MasterComplex_GetPaged(Guid channel, int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                IEnumerable<DTO_MasterComplex> data = this._masterComplex.DAL_MasterComplex_GetPaged(pageSize, pageNum, consulta, active);
                //byte[] response = CompressedSerializer.Compress<IEnumerable<DTO_MasterComplex>>(data);

                return data;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adiciona una lista a la maestra
        /// </summary>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de insercion de los datos</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterComplex_Add(Guid channel, int documentID, byte[] bItems, int accion)
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
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                this._masterComplex.DocumentID = documentID;
                DTO_TxResult response = this._masterComplex.DAL_MasterComplex_Add(bItems, accion, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza una maestra
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterComplex_Update(Guid channel, int documentID, DTO_MasterComplex item)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                DTO_TxResult response = this._masterComplex.DAL_MasterComplex_Update(item, false);
                
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        public DTO_TxResult MasterComplex_Delete(Guid channel, int documentID, Dictionary<string, string> pks)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                DTO_TxResult response = this._masterComplex.DAL_MasterComplex_Delete(pks, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        public string MasterComplex_Export(Guid channel, int documentID, string colsRsx, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                string response = this._masterComplex.DAL_MasterComplex_Export(colsRsx, consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el numero de la fila de una lista de Pks
        /// </summary>
        /// <param name="pks">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        public long MasterComplex_Rownumber(Guid channel, int documentID, Dictionary<string, string> pks, DTO_glConsulta consulta, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterComplex = new DAL_MasterComplex(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterComplex.DocumentID = documentID;
                long response = this._masterComplex.DAL_MasterComplex_Rownumber(pks, consulta, active);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region MasterHierarchy

        #region Funciones que llaman a MasterSimple

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        public IEnumerable<DTO_MasterHierarchyBasic> MasterHierarchy_GetPaged(Guid channel, int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterHierarchy.DocumentID = documentID;
                IEnumerable<DTO_MasterBasic> list = this._masterHierarchy.DAL_MasterSimple_GetPaged(pageSize, pageNum, consulta, FiltrosExtra, active);
                IEnumerable<DTO_MasterHierarchyBasic> data = this._masterHierarchy.CompleteHierarchyList(list);

                return data;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Funciones Maestra jerarquica

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra jerarquica</returns>
        public DTO_MasterHierarchyBasic MasterHierarchy_GetByID(Guid channel, int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterHierarchy.DocumentID = documentID;
                DTO_MasterBasic dto = this._masterHierarchy.DAL_MasterSimple_GetByID(id, active, filtros);
                if (dto != null)
                {
                    DTO_MasterHierarchyBasic response = this._masterHierarchy.CompleteHierarchy(dto);
                    return response;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la cantidad de hijos de un padre determinado
        /// </summary>
        /// <param name="parentId">id del padre</param>
        /// <param name="idFilter">filtro de id para los hijos</param>
        /// <param name="descFilter">filtro de descripcion</param>
        /// <param name="active">filtro de activo</param>
        /// <returns>Retorna la cantidad de hijos</returns>
        public long MasterHierarchy_CountChildren(Guid channel, int documentID, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                this._masterHierarchy.DocumentID = documentID;
                long result = this._masterHierarchy.DAL_MasterHierarchy_CountChildren(parentId, idFilter, descFilter, active, filtros);

                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una lista de hijos de una maestra de jerarquica
        /// </summary>
        /// <param name="pageSize">Número de registros por página</param>
        /// <param name="pageNum">Número de página</param>
        /// <param name="orderDirection">Ordenamiento</param>
        /// <param name="parentId">Identificador del padre</param>
        /// <param name="idFilter">Filtro para la columna del ID</param>
        /// <param name="descFilter">Filtro para la columna de la descripción</param>
        /// <param name="active">Indicador si se pueden traer solo datos activos</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna la lista de resultados</returns>
        public IEnumerable<DTO_MasterHierarchyBasic> MasterHierarchy_GetChindrenPaged(Guid channel, int documentID, int pageSize, int pageNum, OrderDirection orderDirection, 
            UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                this._masterHierarchy.DocumentID = documentID;
                IEnumerable<DTO_MasterBasic> list = this._masterHierarchy.DAL_MasterHierarchy_GetChindrenPaged(pageSize, pageNum, orderDirection, parentId, idFilter, descFilter, active, filtros);
                IEnumerable<DTO_MasterHierarchyBasic> response = this._masterHierarchy.CompleteHierarchyList(list);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adiciona una lista de dtos
        /// </summary>
        /// <param name="bItems">Lista de datos</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="progress">Información con el progreso de la transacción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterHierarchy_Add(Guid channel, int documentID, byte[] bItems, int accion)
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
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                this._masterHierarchy.DocumentID = documentID;
                DTO_TxResult response = this._masterHierarchy.DAL_MasterHierarchy_Add(bItems, accion, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza un registro de una maestra jerarquica
        /// </summary>
        /// <param name="item">Registro para actualizar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterHierarchy_Update(Guid channel, int documentID, DTO_MasterHierarchyBasic item)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterHierarchy.DocumentID = documentID;
                DTO_TxResult response = this._masterHierarchy.DAL_MasterHierarchy_Update(item, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Borra un registro
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterHierarchy_Delete(Guid channel, int documentID, UDT_BasicID id)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterHierarchy.DocumentID = documentID;
                DTO_TxResult response = this._masterHierarchy.DAL_MasterHierarchy_Delete(id, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Valida si los padres de un id existen
        /// </summary>
        /// <param name="documentID">Identificador de la maestra</param>
        /// <param name="id">id del hijo</param>
        /// <returns>True si existen los padres</returns>
        public bool MasterHierarchy_CheckParents(Guid channel, int documentID, UDT_BasicID id)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterHierarchy.DocumentID = documentID;
                var result = this._masterHierarchy.DAL_MasterHierarchy_CheckParents(id);

                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }   
        }

        #endregion

        #endregion

        #region MasterSimple

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterBasic MasterSimple_GetByID(Guid channel, int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                this._masterSimple.DocumentID = documentID;
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_MasterBasic response = this._masterSimple.DAL_MasterSimple_GetByID(id, active, filtros);

                if (response is DTO_MasterHierarchyBasic)
                {
                    this._masterHierarchy = new DAL_MasterHierarchy(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                    this._masterHierarchy.DocumentID = documentID;
                    response = (DTO_MasterBasic)this._masterHierarchy.CompleteHierarchy(response);
                }

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una maestra básica a partir de su descripcion
        /// En caso de encontrar mas de un resultado devolvera el primero
        /// </summary>
        /// <param name="desc">Descriptivo de la maestra</param>
        /// <returns>Devuelve la maestra basica</returns>
        public DTO_MasterBasic MasterSimple_GetByDesc(Guid channel, int documentID, string desc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                DTO_MasterBasic response = this._masterSimple.DAL_MasterSimple_GetByDesc(desc);
                if (response is DTO_MasterHierarchyBasic)
                {
                    this._masterHierarchy.DocumentID = documentID;
                    response = (DTO_MasterBasic)this._masterHierarchy.CompleteHierarchy(response);
                }

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Cuenta la cantidad de resultados dado un filtro
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        public long MasterSimple_Count(Guid channel, int documentID, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                long response = this._masterSimple.DAL_MasterSimple_Count(consulta, FiltrosExtra, active);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        public IEnumerable<DTO_MasterBasic> MasterSimple_GetPaged(Guid channel, int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                IEnumerable<DTO_MasterBasic> data = this._masterSimple.DAL_MasterSimple_GetPaged(pageSize, pageNum, consulta, FiltrosExtra, active);

                return data;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Adiciona una lista a la maestra básica
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de la operacion "usuario,progreso"</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterSimple_Add(Guid channel, int documentID, byte[] bItems, int accion)
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
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                this._masterSimple.DocumentID = documentID;
                DTO_TxResult response = this._masterSimple.DAL_MasterSimple_Add(bItems, accion, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza una maestra básica
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult MasterSimple_Update(Guid channel, int documentID, DTO_MasterBasic item)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                DTO_TxResult response = this._masterSimple.DAL_MasterSimple_Update(item, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Borra una maestra básica a partir de su id
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <returns>Resultado</returns>  
        public DTO_TxResult MasterSimple_Delete(Guid channel, int documentID, UDT_BasicID id)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                DTO_TxResult response = this._masterSimple.DAL_MasterSimple_Delete(id, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        public string MasterSimple_Export(Guid channel, int documentID, string colsRsx, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> filtrosExtra)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                string response = this._masterSimple.DAL_MasterSimple_Export(colsRsx, consulta, filtrosExtra);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el numero de la fila de un ID
        /// </summary>
        /// <param name="id">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        public long MasterSimple_Rownumber(Guid channel, int documentID, UDT_BasicID id, DTO_glConsulta consulta, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                this._masterSimple.DocumentID = documentID;
                long response = this._masterSimple.DAL_MasterSimple_Rownumber(id, consulta, active);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Masters Common

        /// <summary>
        /// Metodo de TODAS las maestras para traer un campo de imagen
        /// </summary>
        /// <param name="replicaId">Replica Id de la fila a buscar</param>
        /// <param name="fieldName">Nombre del campo que contiene la imagen</param>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] Master_GetImage(Guid channel, int docId, int replicaId, string fieldName)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                this._masterSimple = new DAL_MasterSimple(this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                this._masterSimple.DocumentID = docId;
                byte[] response = this._masterSimple.DAL_MasterSimple_GetImage(replicaId, fieldName);

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
