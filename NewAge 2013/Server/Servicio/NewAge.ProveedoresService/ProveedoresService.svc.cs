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

namespace NewAge.Server.ProveedoresService
{
    /// <summary>
    /// Clase ProveedoresService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProveedoresService : IProveedoresService, IDisposable
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
        public ProveedoresService()
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
        public ProveedoresService(string connString, string connLoggerString)
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

        #region Proveedores

        #region DetalleDocu

        /// <summary>
        /// Trae un detalle de proveedores 
        /// </summary>
        /// <param name="detalleID">Identificador del detalle</param>
        /// <returns>Retorna el detalle</returns>
        public DTO_prDetalleDocu prDetalleDocu_GetByID(Guid channel, int detalleID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.prDetalleDocu_GetByID(detalleID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la lista de prDetalleDocu segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <param name="isFactura">Valida si se filtra por identificador de FacturaDocuID</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetByNumeroDoc(Guid channel,int NumeroDoc, bool isFactura)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.prDetalleDocu_GetByNumeroDoc(NumeroDoc, isFactura);

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
        /// <param name="document">Documento a filtrar</param>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="consecutivoDeta">identificador del detalle si se requiere</param>
        /// <returns></returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetByDocument(Guid channel,int document, int numeroDoc, int consecutivoDeta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.prDetalleDocu_GetByDocument(document, numeroDoc, consecutivoDeta);

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
        /// <param name="filter">Filtro de la tabla</param>
        /// <returns>Lista Dto de Detalle Docu</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetParameter(Guid channel,DTO_prDetalleDocu filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.prDetalleDocu_GetParameter(filter);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene los items para cerrar de un documento
        /// </summary>
        /// <param name="documentFilter">Documento  a cerrar</param>
        /// <param name="prefijoID">Prefijo del doc</param>
        /// <param name="docNro">nro  del Doc</param>
        /// <param name="proveedorID">Proveedor</param>
        /// <param name="referenciaID">Referencia</param>
        /// <param name="codigoBS">Codigo BS</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetPendienteForCierre(Guid channel, int documentFilter, string prefijoID, int? docNro, string proveedorID, string referenciaID, string codigoBS)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.prDetalleDocu_GetPendienteForCierre(documentFilter, prefijoID, docNro, proveedorID, referenciaID, codigoBS);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Cierre Detalle
        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="ctrl">referencia a glDocumentoControl</param>
        /// <param name="footer">la lista de detalle</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject CierreDetalle_Guardar(Guid channel,int documentID, DTO_glDocumentoControl ctrl, List<DTO_prDetalleDocu> footer)
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
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CierreDetalle_Guardar(documentID, ctrl, footer, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        #endregion

        #region Solicitud

        /// <summary>
        /// Carga la informacion completa del a Solicitud
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero del Documento interno</param>
        /// <returns>Retorna la Solicitud</returns>
        public DTO_prSolicitud Solicitud_Load(Guid channel, int documentID, string prefijoID, int solNro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Solicitud_Load(documentID, prefijoID, solNro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guardar nueva Solicitud y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="header">SolicitudDocu</param>
        /// <param name="footer">SolicitudFooter</param>
        /// <returns></returns>
        public DTO_SerializedObject Solicitud_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_prSolicitudDocu header,DTO_prSolicitudDirectaDocu headerDirecta, List<DTO_prSolicitudFooter> footer, bool update, out int numDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Solicitud_Guardar(documentID, ctrl, header, headerDirecta,footer, update, out numDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Manda el documento a aprobacion
        /// </summary>
        /// <returns></returns>
        public DTO_SerializedObject Solicitud_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Solicitud_SendToAprob(documentID, numeroDoc, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudAprobacion> Solicitud_GetPendientesByModulo(Guid channel, ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                var response = modu.Solicitud_GetPendientesByModulo(mod, documentID, actividadFlujoID, usuario);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudAsignacion> Solicitud_GetPendientesForAssign(Guid channel, ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
             
                var response = modu.Solicitud_GetPendientesParaAsignar(mod, documentID, actividadFlujoID, usuario);
                return response;              
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de solicitudes para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Solicitud_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_prSolicitudAprobacion> sol, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Solicitud_AprobarRechazar(documentID, actividadFlujoID, sol, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        
        }

        /// <summary>
        /// Recibe una lista de solicitudes para asignar
        /// </summary>
        /// <param name="documentID">documento que relaciona la asignacion</param>
        /// <param name="sol">solicitud que se deben asignar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Solicitud_Asignar(Guid channel, int documentID, string actividadFlujoID, List<DTO_prSolicitudAsignacion> sol)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Solicitud_Asignar(documentID, actividadFlujoID, sol, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Trae un listado de solicitudes para orden de compra
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudResumen> Solicitud_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, TipoMoneda origenMonet)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                
                var response = modu.Solicitud_GetResumen(documentID, usuario, mod,origenMonet);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de las solicitudes direcctas pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes</returns>
        public List<DTO_prSolicitudDirectaAprob> SolicitudDirecta_GetPendientesByModulo(Guid channel, int documentoID, string actFlujoID, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                var response = modu.SolicitudDirecta_GetPendientesByModulo(documentoID, actFlujoID, usuario);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la lista de prSolicitudCargos segun el identificador del detalle
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        public List<DTO_prSolicitudCargos> prSolicitudCargos_GetByConsecutivoDetaID(Guid channel, int documentID, int ConsecutivoDetaID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.prSolicitudCargos_GetByConsecutivoDetaID(documentID, ConsecutivoDetaID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de solicitudes directas para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="actividadFlujoID">actividad del documento</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> SolicitudDirecta_AprobarRechazar(Guid channel,int documentID, string actividadFlujoID, List<DTO_prSolicitudDirectaAprob> sol, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.SolicitudDirecta_AprobarRechazar(documentID, actividadFlujoID, sol, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Orden de compra

        /// <summary>
        /// Carga la informacion completa del Orden de compra
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Consecutivo del Orden de Compra</param>
        /// <param name="numeroDoc">numero Doc de la OC</param>
        /// <returns>Retorna datos del Orden de compra</returns>
        public DTO_prOrdenCompra OrdenCompra_Load(Guid channel, int documentID, string prefijoID, int ordenNro, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.OrdenCompra_Load(documentID, prefijoID, ordenNro, numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guardar nuevo Orden de Compra y asociar en glDocumentoControl
        /// </summary>
        /// <param name="orden">Data completa</param>
        /// <returns></returns>
        public DTO_SerializedObject OrdenCompra_Guardar(Guid channel, int documentID, DTO_prOrdenCompra orden, bool update, out int numDoc)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.OrdenCompra_Guardar(documentID, orden, update, out numDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de ordenes de compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>la lista de los ordenes de compra pendientes </returns>
        public List<DTO_prOrdenCompraAprob> OrdenCompra_GetPendientesByModulo(Guid channel, int documentID,string actFlujoID, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.OrdenCompra_GetPendientesByModulo(documentID, actFlujoID,usuario);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de Ordenes de Compra para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="usuario">usuario que aprueba el orden de compra</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> OrdenCompra_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prOrdenCompraAprob> ord, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.OrdenCompra_AprobarRechazar(documentID, actividadFlujoID,usuario, ord, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Trae un listado de ordenes de compra para recibido
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraResumen> OrdenCompra_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, List<Tuple<string, string>> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                var response = modu.OrdenCompra_GetResumen(documentID, usuario, mod, filtros);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region prContratoPlanPagos

        /// <summary>
        /// Actualiza la lista de prContratoPlanPago en base de datos
        /// </summary>
        /// <param name="listContPlanPago">la lista de DTO_prContratoPlanPago</param>
        /// <returns></returns>
        public DTO_TxResult prContratoPlanPago_Upd(Guid channel, List<DTO_prContratoPlanPago> listContPlanPago, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.prContratoPlanPago_Upd(listContPlanPago, numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Contrato
        /// <summary>
        /// Trae un listado de los contratos pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes para aprobar</returns>
        public List<DTO_prContratoAprob> Contrato_GetPendientesByModulo(Guid channel, int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Contrato_GetPendientesByModulo(documentID, actFlujoID, usuario);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de contratos para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="contrato">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Contrato_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prContratoAprob> contrato, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Contrato_AprobarRechazar(documentID, actividadFlujoID, usuario, contrato, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }
        
        #endregion

        #region Recibido

        /// <summary>
        /// Guardar nuevo Recibido y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">Referencia documento</param>
        /// <param name="header">RecibidoDocu</param>
        /// <param name="footer">RecibidoFooter</param>
        /// <param name="numDoc">Numero doc del documento guardado</param>
        /// <param name="docTransporte">documento de transporte para inventarios</param>
        /// <param name="manifCarga">maminifiesto de carga para inventarios</param>
        /// <returns>Resultado</returns>
        public DTO_SerializedObject Recibido_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_prRecibidoDocu header, List<DTO_prOrdenCompraResumen> footer, out int numDoc, string docTransporte, string manifCarga)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                numDoc = 0;
                return r;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Recibido_Guardar(documentID, ctrl, header, footer, out numDoc,docTransporte,manifCarga, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        public DTO_prRecibido Recibido_Load(Guid channel,int documentID, string prefijoID, int recNro, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Recibido_Load(documentID, prefijoID, recNro,numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de Recibidos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>la lista de los Recibidos pendientes </returns>
        public List<DTO_prRecibidoAprob> Recibido_GetPendientesByModulo(Guid channel, int documentID,string actFlujoID, DTO_seUsuario usuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Recibido_GetPendientesByModulo(documentID,actFlujoID, usuario);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de los Recibidos no facturados ya aprobados
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="actFlujoID">Actividad Flujo</param>
        /// <param name="usuario">usuario actual</param>
        /// <param name="NroDocfacturaExist">Numero Doc del documento de la factura si existe</param>
        /// <param name="loadIVA">Indica si carga el porc del iva o no</param>, bool 
        /// <returns>Lista de Recibidos Aprobados</returns>
        public List<DTO_prRecibidoAprob> Recibido_GetRecibidoNoFacturado(Guid channel, int documentID, string actFlujoID, string proveedor, bool loadIVA, int NroDocfacturaExist)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Recibido_GetRecibidoNoFacturado(documentID, actFlujoID, proveedor, loadIVA, NroDocfacturaExist);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de Recibidos para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="usuario">usuario que aprueba el orden de compra</param>
        /// <param name="res">Recibidos que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Recibido_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID,DTO_seUsuario usuario, List<DTO_prRecibidoAprob> rec, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Recibido_AprobarRechazar(documentID, actividadFlujoID,usuario, rec, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Envia para aprobacion un recibido
        /// </summary>
        /// <param name="numeroDoc">Codigo del comprobante</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Recibido_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Recibido_SendToAprob(documentID, numeroDoc, createDoc,DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Radicar Factura o Devuelve en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="_dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_SerializedObject Recibido_RadicarDevolver(Guid channel,int documentID, List<DTO_prRecibidoAprob> recibidosNoFactSelect, DTO_glDocumentoControl _dtoCtrl, DTO_cpCuentaXPagar cta, bool update, out int numeroDoc, bool devolverInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Recibido_RadicarDevolver(documentID, recibidosNoFactSelect, _dtoCtrl, cta, update, out numeroDoc, devolverInd,DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera el detalle de cargos de los Recibidos aprobados pendientes
        /// </summary>
        /// <returns>lista de resultados<</returns>
        public List<DTO_SerializedObject> Recibido_GenerarDetalleCargosRecib(Guid channel, int documentID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Recibido_GenerarDetalleCargosRecib(documentID, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }
        #endregion

        #region Convenios

        #region Solicitud Despacho Convenios

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="data">data a guardar</param>
        /// <param name="numeroDoc">identificador interior del documento</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject Convenio_Add(Guid channel, int documentID, DTO_Convenios data, out int numeroDoc, bool update)
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
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Convenio_Add(documentID, data, out numeroDoc, update, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de los Convenios pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConvenioAprob> Convenio_GetPendientesByModulo(Guid channel, int documentID, string actFlujoID, DTO_seUsuario usuario, bool SolicitudInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Convenio_GetPendientesByModulo(documentID, actFlujoID, usuario, SolicitudInd);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="Nro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        public DTO_Convenios Convenio_GetByNroContrato(Guid channel, int documentID, string prefijoID, int Nro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Convenio_GetByNroContrato(documentID, prefijoID, Nro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="NroConsecutivo">Numero de Documento consecutivo</param>
        /// <returns>Retorna el Convenio</returns>
        public DTO_Convenios Convenio_Get(Guid channel, int documentID, string prefijoID, int NroConsecutivo, bool ConsumoProyectoInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Convenio_Get(documentID, prefijoID, NroConsecutivo, ConsumoProyectoInd);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de Convenios para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Convenio_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_ConvenioAprob> convAp, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Convenio_AprobarRechazar(documentID, actividadFlujoID, usuario, convAp, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        ///   Envia para aprobacion un convenio
        /// </summary>
        /// <param name="documentID">Documento de aprobacion</param>
        /// <param name="actividadFlujoID">actividad actual</param>
        /// <param name="numeroDoc">identificador del documento a aprobar</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Convenio_SendToAprob(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, bool createDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.Convenio_SendToAprob(documentID, actividadFlujoID, numeroDoc, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de Solicitudes de Despacho Convenios para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<DTO_ConveniosResumen> SolicitudDespachoConvenio_GetResumen(Guid channel,int documentID, DTO_seUsuario usuario, ModulesPrefix mod, string proveedor)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modulo = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.SolicitudDespachoConvenio_GetResumen(documentID, usuario, mod, proveedor);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion
        #region Consumo Proyecto
        /// <summary>
        /// Trae un listado de los Consumos de Proyecto para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConveniosResumen> ConsumoProyecto_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, DateTime fechaCorte, string proveedorID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modulo = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.ConsumoProyecto_GetResumen(documentID, usuario, mod, fechaCorte,proveedorID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega un recibido de consumo y Recibe los consumos de proyecto en los saldos 
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="docCtrl">Doc control de recibido consumo</param>
        /// <param name="recibCons">Lista de items a recibir</param>
        /// <param name="numeroDoc">Identificador del documento actual</param>
        /// <returns>Respuesta</returns>
        public DTO_SerializedObject RecibidoConvenios_Add(Guid channel, int documentID, List<DTO_ConveniosResumen> recibCons, string proveedorID, DateTime fechaDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.RecibidoConvenios_Add(documentID,recibCons,proveedorID,fechaDoc,DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        #endregion        
        
        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de PRoveedores
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proveedores_Reversion(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                List<DTO_glDocumentoControl> ctrls = null;
                List<DTO_coComprobante> coComps = null;

                var response = mod.Proveedores_Revertir(documentID, numeroDoc, null, ref ctrls, ref coComps, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }           
        }

        /// <summary>
        /// Anula Proveedores
        /// </summary>
        /// <param name="numDoc">nums de las Proveedores a anular</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult Proveedoress_Anular(Guid channel, int documentID, List<int> numDocs)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proveedoress_Anular(documentID, numDocs,DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Provision
        /// <summary>
        /// Guarda documento de Provision con un comprobante de los Recibidos Pendientes por facturar
        /// </summary>
        /// <param name="documentID">documento de provision</param>
        /// <returns>Lista de Resultados</returns>
        public DTO_TxResult Provision_RecibidoNotFacturadoAdd(Guid channel, int documentID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Provision_RecibidoNotFacturadoAdd(documentID, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        } 
        #endregion

        #region Consultas

        /// <summary>
        /// Trae un listado de las documentos con detalle como consulta
        /// </summary>
        /// <param name="documentID">documento relacionado</param>
        /// <param name="docs">Lista de documentos a consultar</param>
        /// <returns>Detalle de la consulta</returns>
        public List<DTO_ConsultaCompras> ConsultaCompras_Get(Guid channel,int documentID, List<DTO_glDocumentoControl> ctrls)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                var response = mod.ConsultaCompras_Get(documentID, ctrls);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene una lista de cierres
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <param name="codigoBSID">codigo Bien servicio</param>
        /// <param name="referenciaID">referencia</param>
        /// <param name="numeroDocOC">numero Doc Orden Compra</param>
        /// <returns>lista de cierres</returns>
        public List<DTO_prCierreMesCostos> prCierreMesCostos_GetByParameter(Guid channel,DateTime periodo, string codigoBSID, string referenciaID, int? numeroDocOC)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores mod = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                var response = mod.prCierreMesCostos_GetByParameter(periodo, codigoBSID, referenciaID, numeroDocOC);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region prComprasModificaFechas
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="aprobadoDocInd">Indica si trae un documento aprobado o sin aprobar</param>
        /// <returns>Lista de dtos</returns>
        public List<DTO_prComprasModificaFechas> prComprasModificaFechas_GetByNumeroDoc(Guid channel, int numeroDoc, bool aprobadoDocInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.prComprasModificaFechas_GetByNumeroDoc(numeroDoc, aprobadoDocInd);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        public DTO_TxResult prComprasModificaFechas_Add(Guid channel, DTO_prComprasModificaFechas datos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.prComprasModificaFechas_Add(datos, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        public DTO_TxResult prComprasModificaFechas_Upd(Guid channel, DTO_prComprasModificaFechas datos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.prComprasModificaFechas_Upd(datos, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        public DTO_prComprasModificaFechas prComprasModificaFechas_Load(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProveedores modu = (ModuloProveedores)facade.GetModule(ModulesPrefix.pr, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.prComprasModificaFechas_Load(numeroDoc);
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
