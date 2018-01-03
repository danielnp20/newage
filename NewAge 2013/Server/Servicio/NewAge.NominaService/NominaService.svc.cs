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

namespace NewAge.Server.NominaService
{
    /// <summary>
    /// Clase NominaService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class NominaService : INominaService, IDisposable
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
        /// Get or sets the connection
        /// </summary>
        /// <summary>
        /// Cadena de conexion
        /// </summary>
        private string _connLoggerString = string.Empty;

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
        public NominaService()
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
        public NominaService(string connString, string connLoggerString)
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

        #region Nomina

        #region Aprobaciones - Pagos

        #region Aprobacion Nomina

        /// <summary>
        /// Aprueba ó Rechaza una solicitud de Pago de Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_AprobarLiquidacion(Guid channel, List<DTO_noNominaPreliminar> liquidaciones, string actividadFlujoID)
        {
            if (_currentProcess.Contains(AppDocuments.PagoNominaAprob))
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
                _currentProcess.Add(AppDocuments.PagoNominaAprob);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.PagoNominaAprob);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AprobarLiquidacion(liquidaciones, actividadFlujoID, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.PagoNominaAprob);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Aprobacion Otros

        /// <summary>
        /// Trae las liquidaciones hacia Terceros
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="documentID">Identificador de Documento</param>
        /// <param name="Periodo">Periodo</param>
        /// <returns></returns>
        public List<DTO_noLiquidacionOtro> Nomina_GetLiquidacionOtros(Guid channel, List<int> documents, DateTime Periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetLiquidacionOtros(documents, Periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba una solicitud de Pago a Otros
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="terceros">listado de terceros</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_AprobarPagosTerceros(Guid channel, List<DTO_NominaPlanillaContabilizacion> terceros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AprobarPagosTerceros(terceros, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Pagos Nomina

        /// <summary>
        /// Realiza el Pago de la Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        public DTO_TxResult Nomina_PagoNomina(Guid channel, int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones, string actividadFlujoID)
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
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_PagoNomina(documentoID, periodo, fechaDoc, liquidaciones, actividadFlujoID, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Procesa el Pago de la Nomina detallando el comprobante de tesoreria por empleado
        /// </summary>
        /// <param name="channel">canal</param>
        /// <param name="documentoID">documento de Pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="fechaDoc">Fecha Documento</param>
        /// <param name="liquidaciones">liquidaciones</param>
        /// <returns>resultado</returns>
        public List<DTO_TxResult> Nomina_PagoNominaDetallada(Guid channel, int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones)
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
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Proceso_PagoNominaDetallada(documentoID, periodo, fechaDoc, liquidaciones, DictionaryProgress.BatchProgress,false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #endregion

        #region Aumento Salario

        /// <summary>
        /// Actualiza el Ajuste de Salario en la tabla empleados y crea la novedad de contrato
        /// </summary>
        /// <param name="lSalarios">listado de ajustes en salarios</param>
        /// <param name="insideAnotherTx">verifica si viene de una transacción</param>
        /// <returns>listado de resultados</returns>
        public DTO_TxResult Nomina_UpdSalarioEmpleado(Guid channel, List<DTO_AumentoSalarial> lSalarios, bool insideAnotherTx)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_UpdSalarioEmpleado(lSalarios, insideAnotherTx);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Compensación Flexible


        /// <summary>
        /// Obtiene el listado de beneficios por Empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de beneficios</returns>
        public List<DTO_noBeneficiosxEmpleado> Nomina_GetBeneficioXEmpleado(Guid channel, string empleadoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetBeneficioXEmpleado(empleadoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        /// <summary>
        /// Adiciona un listado de beneficios por empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="lbeneficios">listado de beneficios</param>
        /// <param name="insideAnotherTx">verifica si viene ó no de una transacción</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_AddBeneficioXEmpleado(Guid channel, List<DTO_noBeneficiosxEmpleado> lbeneficios, bool insideAnotherTx)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AddBeneficioXEmpleado(lbeneficios, insideAnotherTx);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Compensatorios

        /// <summary>
        /// Obtiene el historico de compensatorios
        /// </summary>
        /// <returns>listado de compensatorios</returns>
        public List<DTO_noCompensatorios> Nomina_GetCompensatorios(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetCompensatorios();
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        // <summary>
        /// Actualiza la informacion del compensatorio
        /// </summary>
        /// <param name="compesatorio">objeto compensatorio</param>
        /// <returns>true si la operacion es exitosa</returns>
        public DTO_TxResult Nomina_UpdCompensatorio(Guid channel, List<DTO_noCompensatorios> compesatorio)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_UpdCompensatorio(compesatorio, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Contabilizacion

        /// <summary>
        /// Obtiene el consolidado de las liquidacion de nomina X periodo
        /// </summary>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <returns></returns>
        public List<DTO_NominaContabilizacion> noLiquidacionesDocu_GetTotal(Guid channel, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.noLiquidacionesDocu_GetTotal(periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Empleados

        /// <summary>
        /// Generar un listado de empleados de acuerdo al periodo y el estado de la liquidación
        /// </summary>
        /// <param name="documentoID">ID del documento</param>
        /// <param name="periodo">perido</param>
        /// <param name="estadoLiquidacion">estado de la liquidación</param>
        /// <param name="procesadaInd">indica si el documento ya fue procesada</param>
        /// <param name="estadoEmpleado">estado del empleado</param>
        /// <returns>listado de empleados</returns>
        public List<DTO_noEmpleado> Nomina_noEmpleadoGet(Guid channel, int documentoID, DateTime periodo, byte estadoLiquidacion, bool procesadaInd, byte estadoEmpleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_noEmpleadoGet(documentoID, periodo, estadoLiquidacion, procesadaInd, estadoEmpleado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Lista los empleados segun el estado
        /// </summary>
        /// <param name="activoInd">estado</param>
        /// <param name="empleado">empleado</param>
        /// <returns>lista de empleados</returns>
        public List<DTO_noEmpleado> Nomina_SearchEmpleados(Guid channel, bool activoInd, string empleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_SearchEmpleados(activoInd, empleado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Incorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>resultado de la transacción</returns>
        public DTO_TxResult Nomina_IncorporacionEmpleado(Guid channel, DTO_noEmpleado empleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_IncorporacionEmpleado(empleado, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Reincorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        public DTO_TxResult Nomina_ReinCorporacionEmpleado(Guid channel, DTO_noEmpleado empleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_ReinCorporacionEmpleado(empleado, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae el estado de las liquidaciones del empleado del periodo de liquidación en curso
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Estado Liquidaciones</returns>
        public DTO_noEstadoLiquidaciones Nomina_GetEstadoLiquidaciones(Guid channel, string empleadoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetEstadoLiquidaciones(empleadoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// dice si un tercero esta repetido o no 
        /// </summary>
        /// <param name="empleadoID">tercero</param>
        /// <returns>un contador con la cantidad de veces que el tercero se repitio</returns>
        public int noEmpleado_CountTerceroID(Guid channel, string tercero)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return modulo.noEmpleado_CountTerceroID(tercero);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Liquidacion Documento

        /// <summary>
        /// Obtiene la liquidaciones del empleado asociadas al tipo de documento a liquidar
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="documentoID">identificador del documento</param>
        /// <param name="empleado">empleado</param>
        /// <returns>listado de liquidaciones </returns>
        public List<DTO_noLiquidacionesDocu> Nomina_GetLiquidacionesDocu(Guid channel, int documentoID, DateTime periodo, DTO_noEmpleado empleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetLiquidacionesDocu(documentoID, periodo, empleado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <returns>listado de liquidaciones aprobadas para generar pago</returns>
        public List<DTO_noPagoLiquidaciones> Nomina_NominaPagosGet(Guid channel, string actividadFlujoID, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_NominaPagosGet(actividadFlujoID, periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el periodo de vacaciones a liquidar
        /// </summary>
        /// <param name="empleadoId">identificador del empleado</param>
        /// <param name="estado">estado de la liquidacion</param>
        public List<DTO_noLiquidacionVacacionesDeta> Nomina_GetPeriodoVacaciones(Guid channel, string empleadoId, bool estado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetPeriodoVacaciones(empleadoId, estado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Liquidacion Detalle

        /// <summary>
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <param name="empleado">empleado</param>
        /// <returns>liquidacion completa</returns>
        public DTO_noNominaDefinitiva Nomina_NominaDefinitivaGet(Guid channel, int documentoId, DateTime periodo, DTO_noEmpleado empleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_NominaDefinitivaGet(documentoId, periodo, empleado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Obtiene el detalle para efectos de Pago de Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Nomina</param>
        /// <param name="empleadoId">Identificador de Empleado</param>
        /// <returns>Listado Detalle</returns>
        public List<DTO_noLiquidacionesDetalle> Nomina_GetDetallePago(Guid channel, int documentoID, DateTime periodo, string empleadoId)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetDetallePago(documentoID, periodo, empleadoId);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Liquidacion Preliminar

        /// <summary>
        /// Obtiene listado de detalle liquidacion preliminar (Prenomina)
        /// </summary>
        /// <returns>Listado de detalles liquidacion</returns>
        public List<DTO_noLiquidacionPreliminar> Nomina_LiquidacionPreliminarGetAll(Guid channel, int documentoID, DateTime periodo, DTO_noEmpleado empleado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_LiquidacionPreliminarGetAll(documentoID, periodo, empleado);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <param name="empleado">empleado</param>
        /// <returns>liquidacion completa</returns>
        public List<DTO_noNominaPreliminar> Nomina_NominaPreliminarGet(Guid channel, string actividadFlujoID, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_NominaPreliminarGet(actividadFlujoID, periodo, true);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Novedades Contrato

        /// <summary>
        /// Lista las novedades de contrato por empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de novedades de contrato</returns>
        public List<DTO_noNovedadesContrato> Nomina_GetNovedadesContrato(Guid channel, string empleadoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetNovedadesContrato(empleadoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adicona una novedad de contrato 
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la operacion es exitosa</returns>
        public DTO_TxResult Nomina_AddNovedadesContrato(Guid channel, List<DTO_noNovedadesContrato> novedades)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AddNovedadesContrato(novedades, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Elimina una novedad de contrato
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la elimina</returns>
        public DTO_TxResult Nomina_noNovedadesContrato_Delete(Guid channel, DTO_noNovedadesContrato novedad)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_noNovedadesContrato_Delete(novedad);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Novedades Nomina

        /// <summary>
        /// lista las novedades de nomina por empleado
        /// </summary>
        /// <param name="empleadoID">identificador de empleado</param>
        /// <returns>listado de novedades</returns>
        public List<DTO_noNovedadesNomina> Nomina_GetNovedades(Guid channel, string empleadoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetNovedades(empleadoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        /// <summary>
        /// Agrega las novedades de nomina 
        /// </summary>
        /// <param name="novedades">listado de novedades</param>
        /// <returns></returns>
        public DTO_TxResult Nomina_AddNovedadNomina(Guid channel, List<DTO_noNovedadesNomina> novedades)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AddNovedadNomina(novedades, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Elimina una novedad de Nomina
        /// </summary>
        /// <param name="novedad">novedad de nomina</param>
        public void Nomina_DelNovedadesNomina(Guid channel, DTO_noNovedadesNomina novedad)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                modulo.Nomina_DelNovedadesNomina(novedad);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Planilla de Aportes

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public DTO_noPlanillaAportesDeta Nomina_GetPlanillaAportes(Guid channel, string empleadoID, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetPlanillaAportes(empleadoID, periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        public List<DTO_noPlanillaAportesDeta> Nomina_GetAllPlanillaAportes(Guid channel, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetAllPlanillaAportes(periodo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza listado de planillas de aportes modificadas
        /// </summary>
        /// <param name="lplanilla">listado de planillas</param>
        /// <param name="insideAnotherTx">si viene de alguna tx</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Nomina_PlanillaAportesDeta_Upd(Guid channel, List<DTO_noPlanillaAportesDeta> lplanilla)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_PlanillaAportesDeta_Upd(lplanilla, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Proceso trae valores planilla X tercero
        /// </summary>
        public List<DTO_NominaPlanillaContabilizacion> noPlanillaAportesDeta_GetValoreXTercero(Guid channel, bool isPlanilla)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.noPlanillaAportesDeta_GetValoreXTercero(isPlanilla);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Planillas diarias trabajo

        /// <summary>
        /// Obtiene las planillas diarias de trabajos asociadas al empleado
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de planillas</returns>
        public List<DTO_noPlanillaDiariaTrabajo> Nomina_GetPlanillaDiaria(Guid channel, string empleadoID, DateTime periodo, Int16 contratoNo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetPlanillaDiaria(empleadoID, periodo, _channels[channel].Item1.EmpresaGrupoID_.Value, contratoNo);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adiciona una planilla diaria de trabajo
        /// </summary>
        /// <param name="prestamo">planilla</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddPlanillaDiaria(Guid channel, List<DTO_noPlanillaDiariaTrabajo> planillas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AddPlanillaDiaria(planillas, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Prestamos

        /// <summary>
        /// Obtiene prestamos asociados al empleados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de prestamos</returns>
        public List<DTO_noPrestamo> Nomina_GetPrestamos(Guid channel, string empleadoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetPrestamos(empleadoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adiciona un Prestamo
        /// </summary>
        /// <param name="prestamo">objeto prestamo</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddPrestamo(Guid channel, List<DTO_noPrestamo> prestamos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AddPrestamo(prestamos, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Procesos

        /// <summary>
        /// Liquidación Nomina
        /// </summary>
        /// <param name="periodo">perido del documento</param>
        /// <param name="lEmpleados">listado de empleados a liquidar</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarNomina(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados)
        {
            if (_currentProcess.Contains(AppDocuments.Nomina))
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
                _currentProcess.Add(AppDocuments.Nomina);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.Nomina);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarNomina(periodo, fechaDoc, lEmpleados, DictionaryProgress.BatchProgress); //DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.Nomina);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Liquidación de Contrato 
        /// </summary>
        /// <param name="periodo">Periodo de Documento</param>
        /// <param name="lEmpleados">Listado de Empleados</param>
        /// <param name="fechaRetiro">Fecha de Retiro</param>
        /// <param name="causa">Causa Liquidación</param>
        /// <param name="batchProgress">barra de Progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarContrato(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaRetiro, int causa)
        {
            if (_currentProcess.Contains(AppDocuments.LiquidacionContrato))
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
                _currentProcess.Add(AppDocuments.LiquidacionContrato);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.LiquidacionContrato);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarContrato(periodo, fechaDoc, lEmpleados, fechaRetiro, causa, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.LiquidacionContrato);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Liquida las Cesantias
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodo actual</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="fechaIniLiq">fecha inicial corte prima</param>
        /// <param name="fechaFinLiq">fecha final corte prima</param>
        /// <param name="resolucion">resolución</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarCesantias(Guid channel, DateTime periodo, DateTime fechaDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, DateTime fechaPago, string resolucion, TipoLiqCesantias tLiqCesantias, List<DTO_noEmpleado> lEmpleados)
        {
            if (_currentProcess.Contains(AppDocuments.Cesantias))
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
                _currentProcess.Add(AppDocuments.Cesantias);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.Cesantias);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarCesantias(periodo, fechaDoc, lEmpleados, fechaIniLiq, fechaFinLiq, fechaPago, resolucion, tLiqCesantias, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.Cesantias);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Liquida la Prima 
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodo actual</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="fechaIniLiq">fecha inicial corte prima</param>
        /// <param name="fechaFinLiq">fecha final corte prima</param>
        /// <param name="incNomina">determina si se incluye en la Nomina</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarPrima(Guid channel, DateTime periodo, DateTime fechaDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, bool incNomina, List<DTO_noEmpleado> lEmpleados)
        {
            if (_currentProcess.Contains(AppDocuments.Prima))
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
                _currentProcess.Add(AppDocuments.Prima);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.Prima);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarPrima(periodo, fechaDoc, lEmpleados, fechaIniLiq, fechaFinLiq, incNomina, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.Prima);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Proceso para liquidar las vacaciones
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <param name="diasVacDinero">días vacaciones en dinero</param>
        /// <param name="indIncNomina">indica si incluye en nómina</param>
        /// <param name="indPrima">indica si se incluye la prima</param>
        /// <param name="resolucion">resolucion</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <returns></returns>
        public List<DTO_TxResult> LiquidarVacaciones(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaIniLiq, DateTime fechaFinLiq,
                                                     int diasVacTiempo, int diasVacDinero, bool indIncNomina, bool indPrima, string resolucion, DateTime fechaPagoLiq, DateTime fechaIniPagoLiq, DateTime fechaIniPendVacac, DateTime fechaFinPendVacac)
        {
            if (_currentProcess.Contains(AppDocuments.Vacaciones))
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
                _currentProcess.Add(AppDocuments.Vacaciones);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.Vacaciones);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarVacaciones(periodo, fechaDoc, lEmpleados, fechaIniLiq, fechaFinLiq, diasVacTiempo, diasVacDinero, indIncNomina, indPrima, resolucion,
                    fechaPagoLiq, fechaIniPagoLiq,fechaIniPendVacac,fechaFinPendVacac, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.Vacaciones);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        public List<DTO_TxResult> LiquidarProvisiones(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados)
        {
            if (_currentProcess.Contains(AppDocuments.Provisiones))
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
                _currentProcess.Add(AppDocuments.Provisiones);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.Provisiones);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarProvisiones(periodo, fechaDoc, lEmpleados, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.Provisiones);
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        public List<DTO_TxResult> LiquidarPlanilla(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados)
        {
            if (_currentProcess.Contains(AppDocuments.PlanillaAutoLiquidAportes))
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
                _currentProcess.Add(AppDocuments.PlanillaAutoLiquidAportes);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppDocuments.PlanillaAutoLiquidAportes);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.LiquidarPlanilla(periodo, fechaDoc, lEmpleados, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppDocuments.PlanillaAutoLiquidAportes);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Calculo los días reales de vacaciones del empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <returns>número de días de vacaciones</returns>
        public int CalcularDiasVacaciones(Guid channel, string empleadoID, DateTime fechaIni, DateTime fechaFin, out decimal diasCausados)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.CalcularDiasVacaciones(empleadoID, fechaIni, fechaFin, out diasCausados);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Contabiliza la Nomina para el Periodo Actual
        /// </summary>
        /// <param name="documentoID">identifidor del documento</param>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="liquidaciones">liquidaciones</param>
        /// <returns>Resultado</returns>
        public List<DTO_TxResult> Proceso_ContabilizarNomina(Guid channel, int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones,byte procesarSel)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            if (_currentProcess.Contains(AppProcess.ContabilizarNomina))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                results.Add(result);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(AppProcess.ContabilizarNomina);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppProcess.ContabilizarNomina);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                results = modulo.Proceso_ContabilizarNomina(documentoID, periodo,  fechaDoc, liquidaciones, procesarSel, DictionaryProgress.BatchProgress, false);
                return results;
            }
            finally
            {
                _currentProcess.Remove(AppProcess.ContabilizarNomina);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Proceso de contabilización de la Planilla de Aportes
        /// </summary>
        /// <param name="documentoID">identificador del documento de Pagos</param>
        /// <param name="periodo">Periodo actual de la Nomina</param>
        /// <param name="liquidaciones">liquidaciones de la Planilla</param>
        /// <returns>listado de resultados</returns>
        public List<DTO_TxResult> Proceso_ContabilizarPlanilla(Guid channel, int documentoID, DateTime periodo, List<DTO_noPlanillaAportesDeta> liquidaciones)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            if (_currentProcess.Contains(AppProcess.ContabilizarPlanilla))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                results.Add(result);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(AppProcess.ContabilizarPlanilla);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppProcess.ContabilizarPlanilla);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                results = modulo.Proceso_ContabilizarPlanilla(documentoID, periodo, liquidaciones, DictionaryProgress.BatchProgress, false);
                return results;
            }
            finally
            {
                _currentProcess.Remove(AppProcess.ContabilizarPlanilla);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Contabiliza las Provisiones para el Periodo Actual
        /// </summary>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="batchProgress">Barra de Progreso</param>
        /// <returns>Resultado</returns>
        public List<DTO_TxResult> Proceso_ContabilizarProvisiones(Guid channel, int documentoID, DateTime periodo, List<DTO_noProvisionDeta> liquidaciones)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            if (_currentProcess.Contains(AppProcess.ContabilizaProvisiones))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                results.Add(result);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(AppProcess.ContabilizaProvisiones);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, AppProcess.ContabilizaProvisiones);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Proceso_ContabilizarProvisiones(documentoID, periodo, liquidaciones, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                _currentProcess.Remove(AppProcess.ContabilizaProvisiones);
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lEmpleados">liquidaciones Empleados</param>
        /// <returns></returns>
        public List<DTO_TxResult> Proceso_MigracionNomina(Guid channel, int documentoID, List<DTO_noMigracionNomina> lEmpleados)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            if (_currentProcess.Contains(documentoID))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                results.Add(result);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.MigracionNomina(lEmpleados, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lProvisiones">Provisiones</param>
        public List<DTO_TxResult> Proceso_MigracionProvisiones(Guid channel, int documentoID, List<DTO_noProvisionDeta> lProvisiones)
        {

            List<DTO_TxResult> results = new List<DTO_TxResult>();
            if (_currentProcess.Contains(documentoID))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };
                results.Add(result);
                return results;
            }
            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.MigracionProvisiones(lProvisiones, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados por periodo
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="tipoLiquida">Tipo de liquidacion</param>
        /// <returns>listado de liquidaciones aprobadas para generar boletas pago</returns>
        public List<DTO_NominaEnvioBoleta> Proceso_noLiquidacionesDocu_GetNominaLiquida(Guid channel, int documentoID, DateTime periodo, byte tipoLiquida)
        {
          
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Proceso_noLiquidacionesDocu_GetNominaLiquida(documentoID,periodo,tipoLiquida);
                return response;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Provisiones

        /// <summary>
        /// Obtiene un listado de la liquidación de Provisiones del empleado en el periodo
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="periodo">periodo</param>
        /// <param name="contratoNOID">número contrato empleado</param>
        /// <returns>lista de provisiones</returns>
        public List<DTO_noProvisionDeta> Nomina_ProvisionDeta_Get(Guid channel, DateTime periodo, int contratoNOID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_ProvisionDeta_Get(periodo, contratoNOID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Traslados

        /// <summary>
        /// Obtiene los traslados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de traslados</returns>
        public List<DTO_noTraslado> Nomina_GetTraslados(Guid channel, string empleadoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_GetTraslados(empleadoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adiciona un traslado
        /// </summary>
        /// <param name="prestamo">objeto traslado</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        public DTO_TxResult Nomina_AddTraslado(Guid channel, List<DTO_noTraslado> traslados)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.Nomina_AddTraslado(traslados, false);
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
        /// Funcion q llama la funcion de imprimir la prenomina
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc">nmeroDoc</param>
        /// <param name="lEmpleados">lista de MasterBasic de empleados</param>
        /// <param name="_ldetalle">Lista de dto_noLiquidacionPreliminar</param>
        public void Print(Guid channel, int numeroDoc, DateTime periodo, List<DTO_MasterBasic> lEmpleados, List<DTO_noLiquidacionPreliminar> _ldetalle)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                modulo.PrintPrenomina(numeroDoc, lEmpleados, periodo, _ldetalle, true);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Funcion que llama la funcion para enviar a imprimir el reporte de Liquidacion de vacaciones
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc">Numero de documento</param>
        /// <param name="empleado">dto_noEmpleado</param>
        /// <param name="_ldetalle">Lista de detalles de DTO_noLiquidacionPreliminar</param>
        public void PrintVacaciones(Guid channel, int numeroDoc, DTO_noEmpleado empleado, DTO_noLiquidacionesDocu liquidacion, List<DTO_noLiquidacionPreliminar> _lDetalles, bool isApro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                modulo.PrintVacaciones(numeroDoc, empleado, liquidacion, _lDetalles, isApro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="empleadoID">Empleado</param>
        /// <param name="operacionNoID">Operacion Nomina</param>
        /// <param name="conceptoNoID">Concepto Nomina</param>
        /// <param name="areaFuncID">Area Funcional</param>
        /// <param name="fondoID">Fondo Nom</param>
        /// <param name="cajaID">Caja Nomina</param>
        /// <param name="otroFilter">Filtro adicional</param>
        /// <param name="agrup">Agrupar u ordenar</param>
        /// <param name="romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_No_NominaToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                         string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return modulo.Reportes_No_NominaToExcel(documentoID, tipoReporte, fechaIni, fechaFin, empleadoID, operacionNoID, conceptoNoID, areaFuncID, fondoID, cajaID, terceroID, otroFilter, agrup, romp);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera un archivo de las planillas de los empleados
        /// </summary>
        /// <param name="rutaArchivo">Ruta de archivo a guardar</param>
        /// <param name="planillas">Planillas a migrar</param>
        /// <returns>nombre del archivo</returns>
        public string Reportes_No_GenerarArchivoPlanilla(Guid channel, string rutaArchivo, List<DTO_noPlanillaAportesDeta> planillas)
        {
            int opIndex = -1;
            try
            {
                opIndex =   this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                return modulo.Reportes_No_GenerarArchivoPlanilla(rutaArchivo, planillas);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Reversiones

        /// <summary>
        /// Proceso para revertir la liquidación de la Nomina
        /// </summary>
        /// <param name="documentoID">Documento de Nomina</param>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <param name="lEmpleados">lista de Empleados</param>        
        public DTO_TxResult RevertirLiqNomina(Guid channel, int documentoID, DateTime periodo, List<DTO_noEmpleado> lEmpleados)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (_currentProcess.Contains(documentoID))
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = DictionaryMessages.ProcessRunning;
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentoID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentoID);
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                modulo.RevertirLiqNomina(documentoID, periodo, lEmpleados, DictionaryProgress.BatchProgress);
                return result;
            }
            finally
            {
                _currentProcess.Remove(documentoID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region Documento Vacaciones
        /// <summary>
        /// Consulta por empleado
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> Report_No_GetVacacionesByEmpleado(Guid channel, string _empleadoID)
        {
            int opIndex = -1;
            try
            {
                List<DTO_ReportVacacionesDocumento> result = new List<DTO_ReportVacacionesDocumento>();
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                result = modulo.Report_No_GetVacacionesByEmpleado(_empleadoID);
                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta de por empleado para traer las fechas y utilizarlas como filtro.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        public List<DTO_ReportVacacionesDocumento> Report_No_GetLiquidaContratoByEmpleado(Guid channel, string _empleadoID)
        {
            int opIndex = -1;
            try
            {
                List<DTO_ReportVacacionesDocumento> result = new List<DTO_ReportVacacionesDocumento>();
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                result = modulo.Report_No_GetLiquidaContratoByEmpleado(_empleadoID);
                return result;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Cesantias e Intereses de Cesantias

        public void UpdateCesantias(Guid channel, int numeroDoc, decimal valorCesantias, decimal valorIntereses, bool indCesantias)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloNomina modulo = (ModuloNomina)facade.GetModule(ModulesPrefix.no, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                modulo.UpdateCesantias(numeroDoc, valorCesantias, valorIntereses, indCesantias);
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
