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
using NewAge.Reports.Dinamicos;
using NewAge.Reports.Fijos;

namespace NewAge.Server.CFTService
{
    /// <summary>
    /// Clase CFTService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CFTService : ICFTService, IDisposable
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
        public CFTService()
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
        public CFTService(string connString, string connLoggerString)
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

        #region Cuentas X Pagar

        #region cpAnticipos

        /// <summary>
        /// obtiene un documento de anticipo por identificador unico
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        public DTO_cpAnticipo cpAnticipos_GetByEstado(Guid channel, int numeroDoc, EstadoDocControl estado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpAnticipos_GetByEstado(numeroDoc, estado);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// radica o actualiza documento de anticipo
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_anticipo">anticipo</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject cpAnticipos_Guardar(Guid channel, int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpAnticipo _anticipo, bool update)
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
                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpAnticipos_Guardar(documentID, _dtoCtrl, _anticipo, update, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna el valor total para una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo los anticipos</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de los anticipos</returns>
        public decimal cpAnticipos_GetResumenVal(Guid channel, DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpAnticipos_GetResumenVal(periodo, tm, tc, terceroID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer los anticipos</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <param name="anticipoTarjeta">Indica si es anticipo de tarjeta de credito</param>
        /// <returns>Retorna una lista de anticipos</returns>
        public List<DTO_AnticiposResumen> cpAnticipos_GetResumen(Guid channel, DateTime periodo, TipoMoneda tipoMoneda, string terceroID, bool anticipoTarjeta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpAnticipos_GetResumen(periodo, tipoMoneda, terceroID, anticipoTarjeta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> cpAnticipos_GetPendientesByModulo(Guid channel, ModulesPrefix modulo, string actividadFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpAnticipos_GetPendientesByModulo(modulo, actividadFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Proceso que aprueba o rechaza un listado de anticipos
        /// </summary>
        /// <param name="comps">listado de anticipos</param>
        /// <param name="userId">usuario responsable</param>
        /// <returns></returns>
        public List<DTO_SerializedObject> cpAnticipos_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> comps, bool createDoc)
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
                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpAnticipos_AprobarRechazar(documentID, actividadFlujoID, comps, createDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion
        
        #region Cuentas X Pagar

        /// <summary>
        /// Agrega una lista de CxP
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo para las cuentas</param>
        /// <param name="ctrlList">Lista de documentos</param>
        /// <param name="cxpList">Lista de cuentas por pagar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXPagar_Migracion(Guid channel, int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrls, List<DTO_cpCuentaXPagar> cxpList)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.CuentasXPagar_Migracion(documentID, actividadFlujoID, concSaldoID, ctrls, cxpList, DictionaryProgress.BatchProgress);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Implementacion Cuentas X Pagar 
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <returns></returns>
        public DTO_cpCuentaXPagar CuentasXPagar_Get(Guid channel, int NumeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_Get(NumeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Radicar Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_SerializedObject CuentasXPagar_Radicar(Guid channel, int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cxp, bool mainWindow, bool update, out int numDoc)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_Radicar(documentID, dtoCtrl, cxp, mainWindow, update, out numDoc, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adiciona en tabla cpCuentaXPagar 
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Add(Guid channel, int documentID, DTO_cpCuentaXPagar cxp)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_Add(cxp);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion para devolver una factura radicada
        /// </summary>
        /// <param name="dtoCtrl">documento control</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Devolver(Guid channel, int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cxp, bool mainWindow)
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
                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_Devolver(documentID, dtoCtrl, cxp, mainWindow, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la informacion completa de una facturA
        /// </summary>
        /// <param name="terceroID">Identificador del tercero</param>
        /// <param name="documentoNro">Numero del documento(tercero)</param>
        /// <param name="documentoTercero">Documento del tercero</param>
        /// <returns>Retorna la factura</returns>
        public DTO_CuentaXPagar CuentasXPagar_GetForCausacion(Guid channel, int documentID, string terceroID, string documentoTercero, bool chekEstado = true)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_GetForCausacion(documentID, terceroID, documentoTercero,chekEstado);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="cxp">Cuenta por pagar</param>
        /// <param name="comp">Comprobante que se debe agregar</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Causar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_cpCuentaXPagar cxp, DTO_Comprobante comp)
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
                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_Causar(documentID, ctrl, cxp, comp, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="listCxP">Lista de Cuenta por pagar</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_CausarMasivo(Guid channel, int documentID, List<DTO_CuentaXPagar> listCxP)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_CausarMasivo(documentID, listCxP, DictionaryProgress.BatchProgress, false);

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
        public List<DTO_CausacionAprobacion> CuentasxPagar_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID, bool checkUser)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar modu = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.CuentasXPagar_GetPendientesByModulo(mod, actividadFlujoID, checkUser);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba o rechazas causacion de facturas
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="comps">lista comprobantes</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> CuentasXPagar_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc)
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
                DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXPagar_AprobarRechazar(documentID, actividadFlujoID, comps, updDocCtrl, createDoc, DictionaryProgress.BatchProgress);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Revierte una cuenta por pagar
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXPagar_Revertir(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar modu = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                List<DTO_glDocumentoControl> ctrls = null;
                List<DTO_coComprobante> coComps = null;
                var response = modu.CuentasXPagar_Revertir(documentID, numeroDoc, null, ref ctrls, ref coComps, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Generar los consecutivos de las Facturas Equivalentes
        /// </summary>
        /// <param name="periodo">periodo</param>
        public DTO_TxResult CuentasXPagar_ConsecutivoFactEquivalente(Guid channel, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar modu = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.CuentasXPagar_ConsecutivoFactEquivalente(periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Reclasificacion SaldosCxP
        /// </summary>
        /// <param name="facturas">Pagos a programar o desprogramar</param>
        /// <param name="cuenta">cuenta contrapartida</param>
        /// <param name="fecha">fecha Doc</param>
        /// <param name="tc">tasa de cambio</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult ReclasificacionSaldosCxP(Guid channel, int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> facturas, string cuenta,
            DateTime fecha, decimal tc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar modu = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.ReclasificacionSaldosCxP(documentID, actividadFlujoID, facturas, cuenta, fecha,tc,DictionaryProgress.BatchProgress,false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Legalización

        /// <summary>
        /// Adiciona en una legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject Legalizacion_Add(Guid channel, int documentID, DTO_Legalizacion leg, bool ComprobantePRE)
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
                 DictionaryProgress.IniciarProceso(_channels[channel].Item2,documentID);
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Legalizacion_Add(documentID, leg, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Implementacion Legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <returns>DTO_LegalizaHeader</returns>
        public DTO_Legalizacion Legalizacion_Get(Guid channel, int NumeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Legalizacion_Get(NumeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza una legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalizacion</param>
        /// <returns></returns>
        public DTO_TxResult Legalizacion_Update(Guid channel, int documentID, List<DTO_cpLegalizaFooter> leg, DTO_cpLegalizaDocu header)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Legalizacion_Update(documentID, leg, header,DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Envia para aprobacion una legalizacion
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la legalizacion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Legalizacion_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Legalizacion_SendToAprob(documentID, numeroDoc, createDoc, DictionaryProgress.BatchProgress,false);


                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        
        
        }

        /// <summary>
        /// Trae un listado de cajas menores pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_LegalizacionAprobacion> Legalizacion_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar modcxp = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modcxp.Legalizacion_GetPendientesByModulo(mod, actividadFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de cajas menores para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="leg">Cajas menores que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Legalizacion_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_LegalizacionAprobacion> leg, bool createDoc)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Legalizacion_AprobarRechazar(documentID, actividadFlujoID, leg, createDoc, DictionaryProgress.BatchProgress,false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guardar el Auxiliar Pre
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="leg">Legalizacion a agregar con la informacion necesaria</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Legalizacion_ComprobantePreAdd(Guid channel,int documentID, DTO_Legalizacion leg)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Legalizacion_ComprobantePreAdd(documentID, leg,DictionaryProgress.BatchProgress);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Tarjeta Credito

        /// <summary>
        /// Obtiene un objeto DTO_cpTarjetaDocu por numero de documento
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns></returns>
        public DTO_cpTarjetaDocu cpTarjetaDocu_GetByEstado(Guid channel, int numeroDoc, EstadoDocControl estado, out List<DTO_cpTarjetaPagos> lisTarjetaPago)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpTarjetaDocu_GetByEstado(numeroDoc, estado, out lisTarjetaPago);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guarda o actualiza documento de Tarjeta Credito
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_tarjetaPago">Tarjeta Docu</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject cpTarjetaDocu_Guardar(Guid channel, int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpTarjetaDocu _tarjetaPago, List<DTO_cpTarjetaPagos> _listTarjetaPago, bool update)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpTarjetaDocu_Guardar(documentID, _dtoCtrl, _tarjetaPago, _listTarjetaPago, update, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna el valor total para una lista de Tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo las tarjetas</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de las tarjetas</returns>
        public decimal cpTarjetaDocu_GetResumenVal(Guid channel, DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpTarjetaDocu_GetResumenVal(periodo, tm, tc, terceroID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una lista de tarjetas Pago 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las tarjetas</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna una lista de Tarjetas Docu</returns>
        public List<DTO_AnticiposResumen> cpTarjetaDocu_GetResumen(Guid channel, DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpTarjetaDocu_GetResumen(periodo, tipoMoneda, terceroID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de Tarjetas pago pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> cpTarjetaDocu_GetPendientesByModulo(Guid channel, ModulesPrefix modulo, string actividadFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpTarjetaDocu_GetPendientesByModulo(modulo, actividadFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Recibe una lista de Tarjetas Docu para aprobar o rechazar
        /// </summary>
        /// <param name="tarjetaPago">Tarjetas Pago que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> cpTarjetaDocu_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> comps, bool createDoc)
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
                ModuloCuentasXPagar mod = (ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.cpTarjetaDocu_AprobarRechazar(documentID, actividadFlujoID, comps, createDoc, DictionaryProgress.BatchProgress, false);

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
        /// Función que carga una lista de facrutas
        /// </summary>
        /// <param name="año">Año para filtrar</param>
        /// <param name="terceroId">Filtro del tercero</param>
        /// <param name="factNro">Numero de factura</param>
        /// <returns>Lista de Facturas</returns>
        public List<DTO_QueryFacturas> ConsultarFacturas(Guid channel, DateTime periodo, string terceroId, string conceptoCxP, string factNro, int tipoConsul, int? tipoFact)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloCuentasXPagar mod =(ModuloCuentasXPagar)facade.GetModule(ModulesPrefix.cp, this._mySqlConnections[opIndex], null, _channels[channel].Item1,_channels[channel].Item2, this._connLoggerString);
                var response = mod.ConsultarFacturas(periodo, terceroId,conceptoCxP, factNro, tipoConsul, tipoFact);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        public List<DTO_QueryHeadFactura> ConsultarFacturasXNota(Guid channel, DateTime año, string terceroId, int tipoConsulta, string Asesor, string Zona, string Proyecto, int TipoFact, string NumFact, string Prefijo, bool facturaFijaInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ConsultarFacturas(año, terceroId, tipoConsulta, Asesor, Zona, Proyecto, TipoFact, NumFact,Prefijo, facturaFijaInd);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Facturacion

        #region FacturaVenta

        /// <summary>
        /// Consulta una tabla faFacturaDocu segun el numero de documento
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        public DTO_faFacturaDocu faFacturaDocu_Get(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.faFacturaDocu_Get(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la informacion completa dela FacturaVenta
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="facturaNro">Numero del Documento interno</param>
        /// <returns>Retorna la Factura</returns>
        public DTO_faFacturacion FacturaVenta_Load(Guid channel, int documentID, string prefijoID, int facturaNro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaVenta_Load(documentID, prefijoID, facturaNro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guardar nueva Factura de Venta y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="header">FacturaDocu</param>
        /// <param name="footer">FacturaFooter</param>
        /// <returns></returns>
        public DTO_SerializedObject FacturaVenta_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_faFacturaDocu header, List<DTO_faFacturacionFooter> footer, bool update, out int numDoc)
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
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaVenta_Guardar(documentID, ctrl, header, footer, update, out numDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un listado de facturas pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_faFacturacionAprobacion> FacturaVenta_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion modu = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.FacturaVenta_GetPendientesByModulo(mod, actividadFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba o rechazas facturas
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="factList">lista facturas</param>
        /// <param name="updateDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> FacturaVenta_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_faFacturacionAprobacion> factList)
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
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaVenta_AprobarRechazar(documentID, actividadFlujoID, factList, DictionaryProgress.BatchProgress);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las facturas</param>
        /// <param name="terceroID">Tercero del Cliente</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturacionResumen> Facturacion_GetResumen(Guid channel, DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Facturacion_GetResumen(periodo, tipoMoneda, terceroID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="clienteID">Periodo de consulta</param>
        /// <param name="NotaEnvioEmptyInd">filtrar por nota de envio</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturaDocu> FacturaVenta_GetByCliente(Guid channel, int documentID, string clienteID, bool NotaEnvioEmptyInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaVenta_GetByCliente(documentID, clienteID, NotaEnvioEmptyInd);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza la tabla faFacturaDocu 
        /// </summary>
        /// <param name="fact">DTO_faFacturaDocu</param>
        /// <param name="OnlyFacturaFija">Indica si solo guarda el indicador de facturaFija</param>
        /// <returns></returns>
        public DTO_TxResult FacturaDocu_Upd(Guid channel, int documentoID, List<DTO_faFacturaDocu> fact, bool OnlyFacturaFija)
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
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaDocu_Upd(documentoID, fact, DictionaryProgress.BatchProgress, false, OnlyFacturaFija);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega una lista de facturas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="migrarList">Datos para migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult FacturaVenta_MigracionGeneral(Guid channel, int documentID, string actividadFlujoID, List<DTO_MigrarFacturaVenta> migrarList, ref List<int> numDocs)
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
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaVenta_MigracionGeneral(documentID, actividadFlujoID, migrarList, ref numDocs, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        } 

        /// <summary>
        /// Anula una factura de venta
        /// </summary>
        /// <param name="numDocFacts">nums de las facturas a anular</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult FacturaVenta_Anular(Guid channel, int documentID, List<int> numDocFacts)
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
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.FacturaVenta_Anular(documentID, numDocFacts, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Revierte una factura venta
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Facturacion_Revertir(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloFacturacion modu = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                List<DTO_glDocumentoControl> ctrls = null;
                List<DTO_coComprobante> coComps = null;
                var response = modu.Facturacion_Revertir(documentID, numeroDoc, null, ref ctrls, ref coComps, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region CxC

        /// <summary>
        /// Agrega una lista de CxC
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo para las cuentas</param>
        /// <param name="ctrlList">Lista de documentos</param>
        /// <param name="cxcFactList">Lista de cuentas por cobrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXCobrar_Migracion(Guid channel, int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrlList, List<DTO_faFacturaDocu> cxcFactList)
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
                ModuloFacturacion mod = (ModuloFacturacion)facade.GetModule(ModulesPrefix.fa, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CuentasXCobrar_Migracion(documentID, actividadFlujoID, concSaldoID, ctrlList, cxcFactList, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        } 
        
        #endregion

        #endregion

        #region Tesoreria

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="loadTareas">Carga las tareas del proyecto</param>
        /// <param name="loadDetalleTarea">carga el detalle por tarea</param>
        /// <param name="loadDetalle">carga el detalle por proyecto</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryFlujoFondos> tsFlujoFondos(Guid channel, DateTime fechaCorte)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria mod = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.tsFlujoFondos(fechaCorte);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        /// <summary>
        ///  Obtiene el detalle de las tareas
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="proyecto">proyecto</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryFlujoFondosTareas> tsFlujoFondos_Tareas(Guid channel, DateTime fechaCorte, string proyecto, bool? recaudosInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria mod = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.tsFlujoFondos_Tareas(fechaCorte,proyecto, recaudosInd);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #region Flujo de Caja
        /// <summary>
        /// Consulta el flujo de caja
        /// </summary>
        /// <returns>Lista de el flujo de caja</returns>
        //public List<DTO_QueryFlujoCaja> tsFlujoCaja(Guid channel)
        public List<DTO_QueryFlujoCaja> tsFlujoCaja(Guid channel, DateTime fechaCorte)

        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.tsFlujoCaja();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta el flujo de caja Detallado
        /// </summary>
        /// <returns>Lista de el flujo de caja</returns>
        
        public List<DTO_QueryFlujoCajaDetalle> tsFlujoCajaDetalle(Guid channel, String Documento)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.tsFlujoCajaDetalle(Documento);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }



        public string Global_DiaSemana(Guid channel, int Semana)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Global_DiaSemana(Semana);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        public string Global_Mes(Guid channel, int Mes)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Global_Mes(Mes);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion
        #region PagosElectronicos

        /// <summary>
        /// Consulta las facturas para transmitir al banco
        /// </summary>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosSinTransmitir(Guid channel)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.PagosElectronicos_GetPagosElectronicosSinTransmitir();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guarda el estado actual de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_Guardar(Guid channel, int documentID, List<DTO_PagosElectronicos> pagosElectronicos)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.PagosElectronicos_Guardar(documentID, pagosElectronicos, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Transmite los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_Transmitir(Guid channel, int documentID, List<DTO_PagosElectronicos> pagosElectronicos)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.PagosElectronicos_Transmitir(documentID, pagosElectronicos, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta los pagos transmitidos al banco, buscando por tercero y fecha de transmicion
        /// </summary>
        /// <param name="terceroID">Tercero al que se le realizó el pago</param>
        /// <param name="fechaTransmicion">Fecha en la que se realizó la transmición</param>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        public List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosTransmitidos(Guid channel, string terceroID, DateTime fechaTransmicion)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.PagosElectronicos_GetPagosElectronicosTransmitidos(terceroID, fechaTransmicion);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Revierte la transmicion de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos a revertir</param>
        /// <returns>Resultado de la operación</returns>
        public DTO_TxResult PagosElectronicos_RevertirTransmicion(Guid channel, int documentID, List<DTO_PagosElectronicos> pagosElectronicos)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.PagosElectronicos_RevertirTransmicion(documentID, pagosElectronicos, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Pagos

        /// <summary>
        /// Obtiene la lista de pagos para su programación
        /// </summary>
        /// <returns>Lista de pagos para su programación</returns>
        public List<DTO_ProgramacionPagos> ProgramacionPagos_GetProgramacionPagos(Guid channel)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.ProgramacionPagos_GetProgramacionPagos();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Programa pagos
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult ProgramacionPagos_ProgramarPagos(Guid channel, int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos, 
            bool pagoAprobacionInd)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.ProgramacionPagos_ProgramarPagos(documentID, actividadFlujoID, programacionesPagos, pagoAprobacionInd, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta las facturas programadas previamente para pagar
        /// </summary>
        /// <returns>Lista de facturas por tercero para pagar</returns>
        public List<DTO_SerializedObject> PagoFacturas_GetPagoFacturas(Guid channel)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.PagoFacturas_GetPagoFacturas();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Registra el pago de una factura en un documento
        /// </summary>
        /// <param name="pagoFacturas">Pagos a registrar</param>
        /// <param name="areaFuncionalID">Código del área funcional</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public List<DTO_TxResult> PagoFacturas_RegistrarPagoFacturas(Guid channel, int documentID, string actividadFlujoID, List<DTO_PagoFacturas> pagoFacturas, DateTime fechaPago, 
            string areaFuncionalID)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult r;
            if (_currentProcess.Contains(documentID))
            {
                r = new DTO_TxResult()
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

                Tuple<int, int> tupProgress = new Tuple<int, int>(_channels[channel].Item2, documentID);
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                results = module.PagoFacturas_RegistrarPagoFacturas(documentID, actividadFlujoID, pagoFacturas, fechaPago, areaFuncionalID, DictionaryProgress.BatchProgress);

                return results;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Transferencias bancarias
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public List<DTO_TxResult> TransferenciasBancarias_Transferencias(Guid channel, int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos, 
            DateTime fecha, decimal tc)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult r;
            if (_currentProcess.Contains(documentID))
            {
                r = new DTO_TxResult()
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.TransferenciasBancarias_Transferencias(documentID, actividadFlujoID, programacionesPagos, fecha, tc, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Revierte una transferencia bancaria
        /// </summary>
        /// <param name="documentID">documento</param>
        /// <param name="compTransf">comprobante a revertir</param>
        /// <returns></returns>
        public DTO_TxResult TransferenciasBancarias_Revertir(Guid channel, int documentID, DTO_Comprobante compTransf)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.TransferenciasBancarias_Revertir(documentID, compTransf,false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
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
        public DTO_TxResult AnularCheques(Guid channel, int documentID, DateTime fechaPago, DTO_PagoFacturas pagoFactura)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.AnularCheques(documentID, fechaPago,pagoFactura, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el docu de los pagos
        /// </summary>
        /// /// <returns>DTO de pagos</returns>
        public DTO_tsBancosDocu tsBancosDocu_Get(Guid channel, int numeroDoc)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.tsBancosDocu_Get(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Recibos de Caja

        /// <summary>
        /// Consulta una tabla tsReciboCajaDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Recibo</param>
        /// <returns></returns>
        public DTO_tsReciboCajaDocu ReciboCaja_Get(Guid channel, int NumeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria mod = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReciboCaja_Get(NumeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la informacion completa del Recibo de Caja
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="cajaID">PrefijoID (corresponde a cajaID)</param>
        /// <param name="reciboNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibo de Caja</returns>
        public DTO_ReciboCaja ReciboCaja_GetForLoad(Guid channel, int documentID, string cajaID, int reciboNro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria mod = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReciboCaja_GetForLoad(documentID, cajaID, reciboNro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Guardar nuevo Recibo de Caja y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="recibo">Recibo de Caja</param>
        /// <param name="comp">Comprobante</param>
        /// <returns></returns>
        public DTO_TxResult ReciboCaja_Guardar(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsReciboCajaDocu recibo, DTO_Comprobante comp, out int numDoc)
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
                ModuloTesoreria mod = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ReciboCaja_Guardar(documentID, actividadFlujoID, ctrl, recibo, comp, out numDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
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
        public List<DTO_TxResult> ReciboCaja_Migracion(Guid channel, int documentID, List<DTO_ReciboCaja> recibos)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.ReciboCaja_Migracion(documentID, recibos, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Traslado de Fondos

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <param name="generaOrdenPago">Valor que indica si el documento genra orden de pago o no</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult TrasladoFondos_TrasladarFondos(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux, 
            bool generaOrdenPago)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.TrasladoFondos_TrasladarFondos(documentID, actividadFlujoID, ctrl, tblAux, generaOrdenPago, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
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
        public DTO_TxResult Consignaciones_Consignar(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux)
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
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = module.Consignaciones_Consignar(documentID, actividadFlujoID, ctrl, tblAux, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region Notas Bancarias

        /// <summary>
        /// Crea una nota bancaria
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <returns></returns>
        public DTO_TxResult NotasBancarias_Radicar(Guid channel, int documentID, DTO_glDocumentoControl dtoCtrl, DTO_Comprobante comp, DTO_coDocumentoRevelacion revelacion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria mod = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.NotasBancarias_Radicar(documentID,  dtoCtrl, comp,  revelacion, false);

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
        /// Funcion que carga la información de cada cheque con sus respectivos movimientos
        /// </summary>
        /// <param name="bancoID">Filtro del bancoID</param>
        /// <param name="nit">TerceroDi</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="numCheque">Numero del cheque</param>
        /// <returns>Lista de cheques con sus respectivos detalles</returns>
        public List<DTO_ChequesGirados> GetCheques(Guid channel, string bancoID, string nit, DateTime fechaIni, DateTime fechaFin, string numCheque, out string reportUrl, int? numeroDoc)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                List<DTO_ChequesGirados> response = module.GetCheques(bancoID, nit, fechaIni, fechaFin, numCheque);

                Query_Ts_ChequesGirados cheque = new Query_Ts_ChequesGirados(this._connLoggerString, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, ExportFormatType.pdf, numeroDoc);
                reportUrl = cheque.GenerateReport(response);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
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
        public List<DTO_QueryReciboCaja> ReciboCaja_GetByParameter(Guid channel,string CajaID, string tercero, DateTime fechaIni, DateTime fechaFin, string numReciboCaja)
        {
            int opIndex = -1;

            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloTesoreria module = (ModuloTesoreria)facade.GetModule(ModulesPrefix.ts, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                List<DTO_QueryReciboCaja> response = module.ReciboCaja_GetByParameter(CajaID, tercero, fechaIni, fechaFin, numReciboCaja);

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
