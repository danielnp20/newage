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

namespace NewAge.Server.ContabilidadService
{
    /// <summary>
    /// Clase ContabilidadService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ContabilidadService : IContabilidadService, IDisposable
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
        private ModuloFachada facade = new ModuloFachada();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContabilidadService()
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
        public ContabilidadService(string connString, string connLoggerString)
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

        #region Contabilidad

        #region Cont - General y Procesos

        /// <summary>
        /// Genera los comprobantes y saldos para el ajuste en cambio
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="AreaFuncionalID">Area funcional desde ka cual se ejecuta el proceso (la del usuario)</param>
        /// <param name="ndML">Numero de documento de glDocumentoControl ML</param>
        /// <param name="ndME">Numero de documento de glDocumentoControl ME</param>
        /// <returns></returns>
        public Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> Proceso_AjusteEnCambio(Guid channel, int documentID, string actividadFlujoID, string areaFuncionalID,
            DateTime periodo, string libroID)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(r, new List<DTO_ComprobanteAprobacion>());
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_AjusteEnCambio(documentID, actividadFlujoID, areaFuncionalID, periodo, libroID, DictionaryProgress.BatchProgress, false);

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
        /// Procesa el ajuste en cambio para un periodo seleccionado
        /// </summary>
        /// <param name="comps">Comprobantes para aprobar</param>
        /// <param name="periodo">Periodo del ajuste</param>
        /// <returns>Retorna el resultado de las operaciones</returns>
        public List<DTO_TxResult> Proceso_ProcesarBalancePreliminar(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, 
            DateTime periodo, string libroID)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_ProcesarBalancePreliminar(documentID, actividadFlujoID, comps, periodo, libroID, DictionaryProgress.BatchProgress, false);

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
        /// Actualiza el valor de la cuenta alterna en las tablas de coBalance coCuentaSaldo y coAuxiliar
        /// </summary>
        public DTO_TxResult Proceso_CuentaAlterna(Guid channel, int documentID, string actividadFlujoID)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult res = mod.Proceso_CuentaAlterna(documentID, actividadFlujoID, false);

                return res;
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
        /// Proceso de prorrateo IVA
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_ProrrateoIVA(Guid channel, int documentID, string actividadFlujoID)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_ProrrateoIVA(documentID, actividadFlujoID, DictionaryProgress.BatchProgress, false);

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
        /// Proceso para consolidar balances entre empresas
        /// </summary>
        /// <param name="documentID">Identificador del documento que genera el proceso</param>
        /// <param name="list">Lista de empresas a consolidar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Proceso_ConsolidacionBalances(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteConsolidacion> list)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_ConsolidacionBalances(documentID, actividadFlujoID, list, DictionaryProgress.BatchProgress);

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
        /// Reclasifica un libro fiscal 
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="libroID">Identificador del libro fiscal</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult Proceso_ReclasificacionLibros(Guid channel, int documentID, DateTime periodoID, string libroID)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return result;
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_ReclasificacionLibros(documentID, periodoID, libroID, false);

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

        #endregion

        #region Ajuste Comprobantes

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante AjusteComprobante_Get(Guid channel, DateTime periodo, string comprobanteID, int compNro)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.AjusteComprobante_Get(periodo, comprobanteID, compNro);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Ajusta un comprobante existente
        /// </summary>
        /// <param name="documentID">documento Id</param>
        /// <param name="comp">Comprobante a ajustar</param>
        /// <param name="insideAnotherTx">determina si viene de una transaccion</param>
        public DTO_TxResult AjusteComprobante_Generar(Guid channel, int documentID, string actividadFlujoID, DTO_Comprobante comp)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.AjusteComprobante_Generar(documentID, actividadFlujoID, comp, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Elimina un ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public DTO_TxResult AjusteComprobante_Eliminar(Guid channel, int documentID, string actividadFlujoID, int numeroDoc)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.AjusteComprobante_Eliminar(documentID, actividadFlujoID, numeroDoc, true, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de los ajustes pendientes de aprobar
        /// </summary>
        /// <returns>Retorna una lista de comprobantes de ajuste</returns>
        public List<DTO_ComprobanteAprobacion> AjusteComprobante_GetPendientes(Guid channel, string actividadFlujoID)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.AjusteComprobante_GetPendientes(actividadFlujoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> AjusteComprobante_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.AjusteComprobante_AprobarRechazar(documentID, actividadFlujoID, comps, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Comprobantes

        #region AuxiliarPre

        /// <summary>
        /// Indica si hay un auxiliarPre
        /// </summary>
        /// <param name="empresaID">Codigo de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public bool ComprobantePre_Exists(Guid channel, int documentID, DateTime periodo, string comprobanteID, int compNro)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobantePre_Exists(documentID, periodo, comprobanteID, compNro);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Indica si hay un comprobante en auxiliarPre
        /// </summary>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <returns>Retorna un entero con la cantidad de comprobantes que hay en aux con un comprobanteID</returns>
        public bool ComprobanteExistsInAuxPre(Guid channel,string comprobanteID)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.ComprobanteExistsInAuxPre(comprobanteID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega un comprobante
        /// </summary>
        /// <param name="comprobante">Comprobante con los datos</param>
        /// <param name="areaFuncionalID">Identificador del area funcional</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="numeroDoc">Pk de glDocumentoControl (Null: El comprobante es el encargado de generar el registro)</param>
        /// <param name="numComp">Numero del comprobante generado (en caso de ser nuevo)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ComprobantePre_Add(Guid channel, int documentoID, ModulesPrefix currMod, DTO_Comprobante comprobante, string areaFuncionalID, string prefijoID, int? numeroDoc, DTO_coDocumentoRevelacion revelacion)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobantePre_Add(documentoID, currMod, comprobante, areaFuncionalID, prefijoID, true, numeroDoc, revelacion, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Elimina un auxiliar (pre) y crea el registro vacio
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public void ComprobantePre_Delete(Guid channel, int documentID, string actividadFlujoID, DateTime periodo, string comprobanteID, int compNro)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.ComprobantePre_Delete(documentID, actividadFlujoID, periodo, comprobanteID, compNro, false);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Envia para aprobacion un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public DTO_SerializedObject ComprobantePre_SendToAprob(Guid channel, int documentID, string actividadFlujoID, ModulesPrefix currentMod, DateTime periodo, string comprobanteID, int compNro, bool createDoc)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobantePre_SendToAprob(documentID, actividadFlujoID, currentMod, periodo, comprobanteID, compNro, createDoc, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_ComprobanteAprobacion> ComprobantePre_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.ComprobantePre_GetPendientesByModulo(mod, actividadFlujoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Auxiliar

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual a correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_BitacoraSaldo> Comprobante_GetByIdentificadorTR(Guid channel, DateTime periodo, long identTR)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_GetByIdentificadorTR(periodo, identTR);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el numero de registros de un comprobante
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public int Comprobante_Count(Guid channel, bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, DTO_glConsulta consulta = null)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_Count(allData, isPre, periodo, comprobanteID, compNro, consulta);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves (si tiene el numero de documento busca las CxP)
        /// </summary>
        /// <param name="numDoc">Numero de documento de busqueda</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante Comprobante_GetAll(Guid channel, int numDoc, bool isPre, DateTime periodo, string comprobanteID, int? compNro)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_GetAll(numDoc, isPre, periodo, comprobanteID, compNro);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante Comprobante_Get(Guid channel, bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, int? pageSize, int? pageNum, DTO_glConsulta consulta = null)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_Get(allData, isPre, periodo, comprobanteID, compNro, pageSize, pageNum, consulta);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Comprobante_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_AprobarRechazar(documentID, actividadFlujoID, currentMod, comps, updDocCtrl, createDoc, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera una lista de comprobantes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="comps">Lista de comprobantes</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="borraInfoPeriodo">Inidca si se debe borrar la información del periodo</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Comprobante_Migracion(Guid channel, int documentID, DateTime periodo, List<DTO_Comprobante> comps, string areaFuncionalID, 
            string prefijoID, bool borraInfoPeriodo)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_Migracion(documentID, periodo, comps, areaFuncionalID, prefijoID, borraInfoPeriodo, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Realiza la reclasificacion de saldos
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="numeroDoc">Numero de documento (PK)</param>
        /// <param name="proyectoID">Identificador del nuevo proyecto</param>
        /// <param name="ctoCostoID">Identificador del centro de costo</param>
        /// <param name="lgID">Identificador del lugar geografico</param>
        /// <param name="obs">Observacion del documento</param>
        /// <returns>Retorna el resultado del proceso</returns>
        public DTO_TxResult Comprobante_ReclasificacionSaldos(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, string proyectoID, string ctoCostoID, string lgID, string obs)
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
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Comprobante_ReclasificacionSaldos(documentID, actividadFlujoID, numeroDoc, proyectoID, ctoCostoID, lgID, obs, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        /// <summary>
        /// Trae el valor de las cuentas de costo
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        public decimal Comprobante_GetValorByCuentaCosto(Guid channel, int numeroDoc)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_GetValorByCuentaCosto(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae las transferencias bancarias por tercero
        /// </summary>
        /// <param name="terceroID">tercero a validar</param>
        /// <param name="docTercero">numero de la factura</param>
        /// <returns>lista de comp</returns>
        public DTO_Comprobante Comprobante_GetTransfBancariaByTercero(Guid channel, string terceroID, string docTercero)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_GetTransfBancariaByTercero(terceroID, docTercero);
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
        /// <param name="periodoInicial">periodo Inicial</param>
        /// <param name="periodoFinal">periodo final</param>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_QueryMvtoAuxiliar> Comprobante_GetAuxByParameter(Guid channel, DateTime? periodoInicial, DateTime? periodoFinal, DTO_QueryMvtoAuxiliar filter)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_GetAuxByParameter(periodoInicial, periodoFinal,filter);
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
        /// <param name="periodoInicial">periodo</param>
        /// <param name="lugarxDef">lugarGeo</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_PagoImpuesto> Comprobante_GetAuxForImpuesto(Guid channel, DateTime periodoFilter)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Comprobante_GetAuxForImpuesto(periodoFilter);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Cierres

        /// <summary>
        /// Obtiene ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <returns>Retorna periodo o null si no existe alguno</returns>
        public DateTime? GetUltimoMesCerrado(Guid channel,ModulesPrefix mod)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modulo = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.GetUltimoMesCerrado(mod);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Indica si el periodo enviado es el ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna True si el aperiodo se puede abrir de lo contrario false</returns>
        public bool UltimoMesCerrado(Guid channel, ModulesPrefix modulo, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool exists = false;
                var response = mod.UltimoMesCerrado(modulo, periodo, ref exists);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Abre un nuevo mes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo para abrir</param>
        /// <param name="modulo">Modulo que se desea abrir</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_AbrirMes(Guid channel, int documentID, DateTime periodo, ModulesPrefix modulo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_AbrirMes(documentID, periodo, modulo, false);

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
        /// Hace el cierre mensual
        /// </summary>
        /// <param name="empresa">Empresa</param>
        /// <param name="periodo">Periodo a cerrar</param>
        /// <param name="modulo">Módulo a cerrar</param>
        /// <param name="userId">Id del usuario</param>
        /// <returns></returns>
        public DTO_TxResult Proceso_CierrePeriodo(Guid channel, int documentID, DateTime periodo, ModulesPrefix modulo)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_CierrePeriodo(documentID, periodo, modulo, DictionaryProgress.BatchProgress, false);

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
        /// Hace el cierre anual
        /// </summary>
        /// <param name="empresa">Empresa</param>
        /// <param name="year">Año a cerrar</param>
        /// <param name="userId">Usuario que hace el cierre</param>
        /// <returns></returns>
        public Tuple<DTO_TxResult, DTO_ComprobanteAprobacion> Proceso_CierreAnual(Guid channel, int documentID, string actividadFlujoID, string areaFuncionalID, 
            int year, string libroID)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(r, new DTO_ComprobanteAprobacion());
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_CierreAnual(documentID, actividadFlujoID, areaFuncionalID, year, libroID, DictionaryProgress.BatchProgress, false);

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
        /// Carga la informacion del Cierre Mensual
        /// </summary>
        /// <param name="channel">Cannal de trasnsmicion de Datos</param>
        /// <param name="año">Periodo de Cierre</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> coCierreMes_GetAll(Guid channel, Int16 año)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modulo = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.coCierreMes_GetAll(año);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Carga la información para hacer un cierre Mensual
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> coCierreMes_GetByParameter(Guid channel, DTO_coCierreMes filter, RompimientoSaldos? romp1, RompimientoSaldos? romp2)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad modulo = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.coCierreMes_GetByParameter(filter,romp1,romp2);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region CruceCuentas(Ajuste Saldos)

        /// <summary>
        /// Creal el documento Ajuste y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="ajuste">documento Ajuste</param>
        /// <param name="comp">Comprobante</param>
        /// <returns></returns>
        public DTO_TxResult CruceCuentas_Ajustar(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_coDocumentoAjuste ajuste, 
            DTO_Comprobante comp)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CruceCuentas_Ajustar(documentID, actividadFlujoID, ctrl, ajuste, comp, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Distribucion Comprobante

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetDistribucion(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobanteDistribucion_GetDistribucion();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeExcluye> ComprobanteDistribucion_GetExclusiones(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobanteDistribucion_GetExclusiones();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza la informacion para la distribucion de comprobantes
        /// </summary>
        /// <param name="documentoId">Identificador del documento</param>
        /// <param name="tablas">Registros de distribucion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        public DTO_TxResult ComprobanteDistribucion_Update(Guid channel, int documentID, List<DTO_coCompDistribuyeTabla> tablas, List<DTO_coCompDistribuyeExcluye> excluyen)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobanteDistribucion_Update(documentID, tablas, excluyen, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetForProcess(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobanteDistribucion_GetForProcess();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera los preliminares y revierto los comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="origenes">Lista de comprobantes que se deben distribuir</param>
        /// <param name="periodoIni">Periodo Inicial</param>
        /// <param name="periodoFin">Periodo Final</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> ComprobanteDistribucion_GenerarPreliminar(Guid channel, int documentID, string actividadFlujoID,
            List<DTO_coCompDistribuyeTabla> origenes, DateTime periodoIni, DateTime periodoFin, string libroID)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult r = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(r, new List<DTO_ComprobanteAprobacion>());
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ComprobanteDistribucion_GenerarPreliminar(documentID, actividadFlujoID, origenes, periodoIni, periodoFin, libroID, 
                    DictionaryProgress.BatchProgress, false);

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

        #endregion

        #region Impuestos

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="modulo">Modulo que realiza la consulta</param>
        /// <param name="tercero">Tercero que esta ejecutando la consulta</param>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una lista de cuentas</returns>
        public List<DTO_SerializedObject> LiquidarImpuestos(Guid channel, ModulesPrefix modulo, DTO_coTercero tercero, string cuentaCosto, string conceptoCargoID, string operacionID, string lugarGeoID, string lineaPresID, decimal valor)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.LiquidarImpuestos(modulo, tercero, cuentaCosto, conceptoCargoID, operacionID, lugarGeoID, lineaPresID, valor);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la lista de declaracion de impuestos para un periodo
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna la lista de declaraciones</returns>
        public List<DTO_DeclaracionImpuesto> DeclaracionesImpuestos_Get(Guid channel, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.DeclaracionesImpuestos_Get(periodo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la lista de renglones de una declaracion
        /// </summary>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <returns>Retorna la lista de renglones</returns>
        public List<DTO_coImpDeclaracionDetaRenglon> DeclaracionesRenglones_Get(Guid channel, int numeroDoc, string impuestoID, short mesDeclaracion, short añoDeclaracion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.DeclaracionesRenglones_Get(numeroDoc, impuestoID, mesDeclaracion, añoDeclaracion);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Procesa una declaracion
        /// </summary>
        /// <param name="documentID">Identificador del documnto que genera el proceso</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ProcesarDeclaracion(Guid channel, int documentID, string impuestoID, short periodoCalendario, short mesDeclaracion, short añoDeclaracion, int? numeroDoc)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ProcesarDeclaracion(documentID, impuestoID, periodoCalendario, mesDeclaracion, añoDeclaracion, numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los detalles de un renglon
        /// </summary>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="renglon">Renglon</param>
        /// <param name="mesDeclaracion">Mes de la declaracion</param>
        /// <param name="añoDeclaracion">Año de la declaracion</param>
        /// <returns>Retorna la lista de cuentas del detalle</returns>
        public List<DTO_DetalleRenglon> DetallesRenglon_Get(Guid channel, string impuestoID, string renglon, short mesDeclaracion, short añoDeclaracion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.DetallesRenglon_Get(impuestoID, renglon, mesDeclaracion, añoDeclaracion);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Mayorizacion

        /// <summary>
        /// Realiza la mayorización de balances de acuerdo a saldos
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="balance">Tipo de balance</param>
        /// <param name="userId">Usuario que realiza la mayorizacion</param>
        /// <param name="empresa">Empresa</param>
        public DTO_TxResult Proceso_Mayorizar(Guid channel, int documentID, DateTime periodo, string tipoBalance)
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

                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_Mayorizar(documentID, periodo, tipoBalance, DictionaryProgress.BatchProgress, false);

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

        #endregion

        #region Reclasificaciones fiscales

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetDistribucion(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReclasificacionFiscal_GetDistribucion();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalExcluye> ReclasificacionFiscal_GetExclusiones(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReclasificacionFiscal_GetExclusiones();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza la informacion para la reclasificacion de comprobantes
        /// </summary>
        /// <param name="documentoId">Identificador del documento</param>
        /// <param name="tablas">Registros de reclasificacion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        public DTO_TxResult ReclasificacionFiscal_Update(Guid channel, int documentID, List<DTO_coReclasificaBalance> tablas, List<DTO_coReclasificaBalExcluye> excluyen)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReclasificacionFiscal_Update(documentID, tablas, excluyen, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetForProcess(Guid channel, string tipoBalanceID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReclasificacionFiscal_GetForProcess(tipoBalanceID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Procesa la reclasificacion
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ReclasificacionFiscal_Procesar(Guid channel, int documentID, string actividadFlujoID, string tipoBalanceID)
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
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReclasificacionFiscal_Procesar(documentID, actividadFlujoID, tipoBalanceID, DictionaryProgress.BatchProgress, false);

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

        #endregion

        #region Saldos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        public DTO_coCuentaSaldo Saldo_GetByDocumento(Guid channel, string cuentaID, string concSaldo, long identificadorTR, string balanceTipo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_coCuentaSaldo response = mod.Saldo_GetByDocumento(cuentaID, concSaldo, identificadorTR, balanceTipo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal Saldo_GetByDocumentoCuenta(Guid channel, bool isML, DateTime PeriodoID, long identificadorTR, string cuentaID, string libroID)
        {

            {
                int opIndex = -1;
                try
                {
                    opIndex = this.ADO_ConnectDB();
                    ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                    decimal response = mod.Saldo_GetByDocumentoCuenta(isML, PeriodoID, identificadorTR, cuentaID, libroID);

                    return response;
                }
                finally
                {
                    this.ADO_CloseDBConnection(opIndex);
                }
            }
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="ConceptoSaldoIDNew">Id de concepto saldo nuevo</param>
        /// <param name="conceptoSaldoIDOld">Id de concepto saldo anterior</param>
        /// <param name="cuentaID">Id de la cuenta anterior</param>
        /// <returns>true si existe</returns>
        public bool Saldo_ExistsByCtaConcSaldo(Guid channel, string ConceptoSaldoIDNew, string conceptoSaldoIDOld, string cuentaID)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Saldo_ExistsByCtaConcSaldo(ConceptoSaldoIDNew,conceptoSaldoIDOld, cuentaID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal Saldo_GetByPeriodoCuenta(Guid channel, bool isML, DateTime PeriodoID, string cuentaID, string libroID)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Saldo_GetByPeriodoCuenta(isML, PeriodoID, cuentaID, libroID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Revisa si un documento ha tenido movimientos de saldos despues de su creación
        /// </summary>
        /// <param name="idTR">Identificador del documento</param>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta del documento</param>
        /// <param name="libroID">Libro de consulta</param>
        /// <returns>Retorna true si ha tenido nuevos movimientos, de lo contrario false</returns>
        public bool Saldo_HasMovimiento(Guid channel, int idTR, DateTime periodoID, DTO_coPlanCuenta cta, string libroID)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Saldo_DocumentoConMovimiento(idTR, periodoID, cta, libroID);
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
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        public List<DTO_coCuentaSaldo> Saldos_GetByParameter(Guid channel, DTO_coCuentaSaldo filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad module = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Saldos_GetByParameter(filter);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Revelaciones

        /// <summary>
        /// Documento Revelacion
        /// </summary>
        /// <param name="revelacion">objeto Revelacion</param>
        /// <returns></returns>
        public DTO_TxResult DocumentoRevelacion_Add(Guid channel, DTO_coDocumentoRevelacion revelacion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.DocumentoRevelacion_Add(revelacion);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene un documento revelación por numero de documento
        /// </summary>
        ///<param name="numeroDoc">número de documento</param>
        ///<returns>Revelación</returns>
        public DTO_coDocumentoRevelacion DocumentoRevelacion_Get(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloContabilidad mod = (ModuloContabilidad)facade.GetModule(ModulesPrefix.co, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var  response = mod.DocumentoRevelacion_Get(numeroDoc);
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
