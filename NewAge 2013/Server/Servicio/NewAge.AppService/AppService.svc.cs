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

namespace NewAge.Server.AppService
{
    /// <summary>
    /// Clase AppService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AppService : IAppService, IDisposable
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
        public AppService()
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
        public AppService(string connString, string connLoggerString)
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

        #region Aplicacion

        #region Alarmas

        /// <summary>
        /// Dice si un usuario tiene alarmas pendientes
        /// </summary>
        /// <returns>Devuelve verdadero si el usuario tiene alarmas</returns>
        public bool Alarmas_HasAlarms(Guid channel, string userName)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.Alarmas_HasAlarms(userName);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el listado de tareas pendientes para envio de correos
        /// </summary>
        /// <returns>Retorna el listado de tareas pendientes</returns>
        public IEnumerable<DTO_Alarma> Alarmas_GetAll(Guid channel, string userName = null)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_Alarma> response = mod.Alarmas_GetAll(userName);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region aplBitacora

        /// <summary>
        /// Consulta la bitácora dado un filtro
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public IEnumerable<DTO_aplBitacora> aplBitacoraGetFiltered(Guid channel, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_aplBitacora> response = mod.aplBitacoraGetFiltered(consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta la bitácora dado un filtro y devuelve la cantidad de resultados
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public long aplBitacoraCountFiltered(Guid channel, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                long response = mod.aplBitacoraCountFiltered(consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta la bitácora dado un filtro por páginas
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public IEnumerable<DTO_aplBitacora> aplBitacoraGetFilteredPaged(Guid channel, int pageSize, int pageNum, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_aplBitacora> response = mod.aplBitacoraGetFilteredPaged(pageSize, pageNum, consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region aplIdioma

        /// <summary>
        /// Trae todos los Idiomas
        /// </summary>
        /// <returns>Devuelve los Idiomas</returns>
        public IEnumerable<DTO_aplIdioma> aplIdioma_GetAll(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                var module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_aplIdioma> response = module.aplIdioma_GetAll();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region aplIdiomaTraduccion

        /// <summary>
        /// Trae la versión de un idioma
        /// </summary>
        /// <returns>Devuelve la versión de un Idioma</returns>
        public IEnumerable<DTO_aplIdiomaTraduccion> aplIdiomaTraduccion_GetResources(Guid channel, string idiomId)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                var module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_aplIdiomaTraduccion> response = module.aplIdiomaTraduccion_GetByIdiomaId(idiomId);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region aplMaestraPropiedad

        /// <summary>
        /// Trae toda la informacion de las maestras
        /// </summary>
        /// <returns>Devuelve la lista de maestras</returns>
        public IEnumerable<DTO_aplMaestraPropiedades> aplMaestraPropiedad_GetAll(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                var module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_aplMaestraPropiedades> response = module.aplMaestraPropiedades_GetAll();

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region aplModulo

        /// <summary>
        /// Trae todos los modulos segun si están activos
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        public IEnumerable<DTO_aplModulo> aplModulo_GetByVisible(Guid channel, short visible, bool onlyOperative)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                var module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_aplModulo> response = module.aplModulo_GetByVisible(visible, onlyOperative);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region AplReporte

        /// <summary>
        /// Obtiene un resporte predefinido para una empresa
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public byte[] aplReporte_GetByID(Guid channel, int documentoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                var module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                byte[] response = module.aplReporte_GetByID(documentoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Ingresa o actualiza un reporte predefinido para un usuario
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        public void aplReporte_Update(Guid channel, DTO_aplReporte report)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                var module = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                module.aplReporte_Update(report);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Operaciones Documentos

        /// <summary>
        /// Verifica si se ha realizado algun proceso de cierre de un modulo para un oerdioo determinado
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo ha tenido cierres</returns>
        public bool PeriodoHasCierre(Guid channel, DateTime periodo, ModulesPrefix mod)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion modulo = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = modulo.PeriodoHasCierre(periodo, mod);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Verifica si un periodo existe y el mes esta abierto
        /// </summary>
        /// <param name="empresaID">Identificador de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo se puede usar</returns>
        public EstadoPeriodo CheckPeriod(Guid channel, DateTime periodo, ModulesPrefix mod)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion modulo = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                EstadoPeriodo response = modulo.CheckPeriod(periodo, mod);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el prefijo de un documento dado
        /// </summary>
        /// <param name="areaFuncionalID">Codigo del area funcional</param>
        /// <param name="documentoID">Codigo del documento</param>
        /// <param name="empresaGrupoID">Codigo de grupo de empresas</param>
        /// <returns>Retorna el codigo del prefijo</returns>
        public string PrefijoDocumento_Get(Guid channel, string areaFuncionalID, int documentoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion modulo = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                string response = modulo.PrefijoDocumento_Get(areaFuncionalID, documentoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el consecutivo para un numero de documento
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="onlyGet">Indica si solo puede traer la info o tambien crear un nuevo numero</param>
        /// <returns>Retorna el consecutivo</returns>
        public int DocumentoNro_Get(Guid channel, int documentID, string prefijoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion modulo = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.GenerarDocumentoNro(documentID, prefijoID, true);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Trae la data para un reporte
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public List<DTO_BasicReport> GetReportData(Guid channel, int reportId, DTO_glConsulta consulta, byte[] bItems)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                List<DTO_BasicReport> response = mod.GetReportData(reportId, consulta, bItems);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Temporales

        /// <summary>
        /// Revisa si existe un temporal segun el origen y el usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public bool aplTemporales_HasTemp(Guid channel, string origen, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.aplTemporales_HasTemp(origen, usuario);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el temporal de un origen determinado y luego lo borra de los temporales
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        public byte[] aplTemporales_GetByOrigen(Guid channel, string origen, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.aplTemporales_GetByOrigen(origen, usuario);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guarda un objeto en temporales. También borra un objeto que anteriormente estuviese bajo el mismo origen para ese usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario</param>
        /// <param name="objeto">objeto a guardar</param>
        public void aplTemporales_Save(Guid channel, string origen, DTO_seUsuario usuario, object objeto)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.aplTemporales_Save(origen, usuario, objeto);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Elimina los temporales de un usuario
        /// </summary>
        /// <param name="origen">Origen de los datos</param>
        /// <param name="usuario">Usuario que esta buscando temporales</param>
        public void aplTemporales_Clean(Guid channel, string origen, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB(); ;
                ModuloAplicacion mod = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.aplTemporales_Clean(origen, usuario);
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
