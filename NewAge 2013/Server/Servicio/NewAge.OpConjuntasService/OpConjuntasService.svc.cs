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

namespace NewAge.Server.OpConjuntasService
{
    /// <summary>
    /// Clase AppService:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class OpConjuntasService : IOpConjuntasService, IDisposable
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
        public OpConjuntasService()
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
        public OpConjuntasService(string connString, string connLoggerString)
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

        #region OpConjuntas

        #region Billing

        /// <summary>
        /// Proceso para hacer la particion del Billing
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Billing_Particion(Guid channel, int documentID, string actividadFlujoID)
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
                ModuloOpConjuntas mod = (ModuloOpConjuntas)facade.GetModule(ModulesPrefix.oc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Billing_Particion(documentID, actividadFlujoID, DictionaryProgress.BatchProgress, false);

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
        /// Procesa el billing y generas los comprobantes Gross
        /// </summary>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="periodo">Periodo del billing</param>
        /// <returns>Retirna al resultado de la operación</returns>
        public DTO_TxResult ProcesarBilling(Guid channel, int documentID, string actividadFlujoID, DateTime periodo)
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
                ModuloOpConjuntas mod = (ModuloOpConjuntas)facade.GetModule(ModulesPrefix.oc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ProcesarBilling(documentID, actividadFlujoID, periodo, DictionaryProgress.BatchProgress, false);

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

        #region CashCall

        /// <summary>
        /// Genera el proceso de cash call
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CashCall_Procesar(Guid channel, int documentID, string actividadFlujoID, DateTime periodoID)
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
                ModuloOpConjuntas mod = (ModuloOpConjuntas)facade.GetModule(ModulesPrefix.oc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CashCall_Procesar(documentID, actividadFlujoID, periodoID, DictionaryProgress.BatchProgress, false);

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

        #region Distribucion

        /// <summary>
        /// Distribuye entre los socios de acuerdo a los porcentajes dados
        /// </summary>
        /// <param name="periodo">PeriodoID</param>
        public DTO_TxResult Proceso_ocDetalleLegalizacion_Distribucion(Guid channel, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloOpConjuntas mod = (ModuloOpConjuntas)facade.GetModule(ModulesPrefix.oc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_ocDetalleLegalizacion_Distribucion(periodo);
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
        /// Obtiene de acuerdo a un filtro la info de Operaciones de detalle mensual
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="tipoInforme">Tipo de Informe</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryInformeMensualCierre> ocDetalleLegalizacion_GetInfoMensual(Guid channel,int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloOpConjuntas mod = (ModuloOpConjuntas)facade.GetModule(ModulesPrefix.oc, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ocDetalleLegalizacion_GetInfoMensual(documentID, filter, tipoInforme, proType, tipoMda, mdaOrigen);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Planeacion

        #region Presupuesto

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <returns>resultado</returns>

        public List<DTO_QueryEjecucionPresupuestal> plEjecucionPresupuestal(Guid channel, DateTime fechaCorte)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plEjecucionPresupuestal(fechaCorte);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Obtiene los Indicadores
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <returns>resultado</returns>

        public List<DTO_QueryIndicadores> plIndicadores(Guid channel, DateTime fechaCorte)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plIndicadores(fechaCorte);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Presupuesto consolidado</returns>
        public DTO_Presupuesto Presupuesto_GetConsolidadoTotal(Guid channel, int documentID, string proyectoID, DateTime periodoID, byte proyectoTipo, string contratoID, string actividadID, string campo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Presupuesto_GetConsolidadoTotal(documentID, proyectoID, periodoID, proyectoTipo, contratoID, actividadID, campo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos de un usuario por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public DTO_Presupuesto Presupuesto_GetNuevo(Guid channel, int documentoID, string proyectoID, DateTime periodoID, bool orderByAsc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Presupuesto_GetNuevo(documentoID, proyectoID, periodoID, orderByAsc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Carga la información de un nuevo presupuesto
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta el proceso</param>
        /// <param name="actividadFlujoID">Actuvidad de flujo relacionada</param>
        /// <param name="periodoPresupuesto">Periodo de presupuesto</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="presupuesto">Presupuesto que se va a geenrar</param>
        /// <param name="isAnotherTx">Revisa si se esta ejecutando en otar transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject Presupuesto_Add(Guid channel, int documentoID, DateTime periodoPresupuesto,string proyectoID, decimal tc,
            DTO_Presupuesto presupuesto,bool onlySave)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Presupuesto_Add(documentoID, periodoPresupuesto,proyectoID, tc, presupuesto, onlySave, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        public List<DTO_PresupuestoAprob> Presupuesto_GetNuevosForAprob(Guid channel,int documentoID, string actividadFlujo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Presupuesto_GetNuevosForAprob(documentoID,actividadFlujo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <param name="areaPresupuestalID">Area presupuestal</param>
        /// <param name="centroCostoID">Centro de costo</param>
        /// <returns></returns>
        public List<DTO_PresupuestoDetalle> Presupuesto_GetNuevosForAprobDetails(Guid channel, string proyectoID, DateTime periodoID, string areaPresupuestalID, string centroCostoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Presupuesto_GetNuevosForAprobDetails(proyectoID, periodoID, areaPresupuestalID, centroCostoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
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
        public List<DTO_SerializedObject> Presupuesto_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_PresupuestoAprob> docs)
        {
            if (_currentProcess.Contains(documentID))
            {
                DTO_TxResult result = new DTO_TxResult()
                {
                    Result = ResultValue.NOK,
                    ResultMessage = DictionaryMessages.ProcessRunning
                };

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                results.Add(result);
                return results;
            }

            int opIndex = -1;
            try
            {
                _currentProcess.Add(documentID);
                opIndex = this.ADO_ConnectDB();
                DictionaryProgress.IniciarProceso(_channels[channel].Item2, documentID);
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Presupuesto_AprobarRechazar(documentID, actividadFlujoID,docs, DictionaryProgress.BatchProgress);

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
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        public List<DTO_plPresupuestoTotal> plPresupuestoTotal_GetByParameter(Guid channel,DTO_plPresupuestoTotal filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plPresupuestoTotal_GetByParameter(filter);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Trae la informacion de planeacion proveedores
        /// </summary>
        /// <param name="consActLinea"> Consecutivo filtro</param>
        /// <returns>Lista</returns>
        public List<DTO_plPlaneacion_Proveedores> plPlaneacion_Proveedores_GetByConsActLinea(Guid channel,int consActLinea)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plPlaneacion_Proveedores_GetByConsActLinea(consActLinea);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba Un Presupuesto contable
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="numeroDoc">Identificador del doc</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <returns></returns>
        public DTO_TxResult PresupuestoContable_Aprobar(Guid channel, int documentID, int numeroDoc, DateTime periodoDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.PresupuestoContable_Aprobar(documentID,numeroDoc,periodoDoc,false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

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
        public DTO_SerializedObject PresupuestoPxQ_Add(Guid channel,int documentoID, DateTime periodoPresupuesto, string proyectoID, decimal tc, DTO_Presupuesto presupuesto, bool onlySave)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.PresupuestoPxQ_Add(documentoID, periodoPresupuesto, proyectoID, tc, presupuesto, onlySave, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
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
        public DTO_Presupuesto PresupuestoPxQ_GetPresupuestoPxQConsolidado(Guid channel,int documentID, string proyecto, DateTime periodo, byte proyectoTipo, string contratoID, string actividadID, string areaFisica, byte estadoDocCtrl)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.PresupuestoPxQ_GetPresupuestoPxQConsolidado(documentID, proyecto, periodo, proyectoTipo, contratoID, actividadID, areaFisica,estadoDocCtrl);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
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
        public DTO_Presupuesto PresupuestoPxQ_GetDataPxQ(Guid channel, int documentID, byte tipoProyecto, string proyectoID, DateTime periodo, string contratoID, string actividadID, string areaFisicaID, string lineaPresupID, string recursoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.PresupuestoPxQ_GetDataPxQ(documentID, tipoProyecto, proyectoID, periodo, contratoID, actividadID, areaFisicaID, lineaPresupID, recursoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
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
        public DTO_SerializedObject PresupuestoPxQ_SendToAprob(Guid channel,int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.PresupuestoPxQ_SendToAprob(documentID, numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de Planeacion
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Planeacion_Reversion(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                List<DTO_glDocumentoControl> ctrls = null;
                List<DTO_coComprobante> coComps = null;
                var response = mod.Planeacion_Revertir(documentID, numeroDoc, null, ref ctrls, ref coComps, false);
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
        /// Ejecutar proceso de cierre Legalizacion en Planeacion 
        /// </summary>
        /// <param name="periodo">Periodo de Cierre</param>
        public DTO_TxResult Proceso_plCierreLegalizacion_Cierre(Guid channel, DateTime periodo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proceso_plCierreLegalizacion_Cierre(periodo);
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
        /// Obtiene de acuerdo a un filtro la info de presupuesto de cierre mensual
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="tipoInforme">Tipo de Informe</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryInformeMensualCierre> plCierreLegalizacion_GetInfoMensual(Guid channel, int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plCierreLegalizacion_GetInfoMensual(documentID, filter,tipoInforme,proType,tipoMda,mdaOrigen);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
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
        public List<DTO_QueryEstadoEjecucion> plCierreLegalizacion_GetEstadoEjecByPeriodo(Guid channel, int documentID, DTO_QueryEstadoEjecucion filter, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plCierreLegalizacion_GetEstadoEjecByPeriodo(documentID, filter, proType, tipoMda, mdaOrigen);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de plSobreEjecucion
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="estado">Estado de los documentos</param>
        /// <param name="areaAprob">Area de aprobacion</param>
        /// <returns>Lista de informe </returns>
        public List<DTO_plSobreEjecucion> plSobreEjecucion_GetOrdenCompraSobreEjec(Guid channel,int documentID, int estado, string areaAprob)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloPlaneacion mod = (ModuloPlaneacion)facade.GetModule(ModulesPrefix.pl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.plSobreEjecucion_GetOrdenCompraSobreEjec(documentID, estado, areaAprob);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Proyectos
        
        #region Solicitud Proyecto     
       
        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyPreProyectoDocu pyPreProyectoDocu_Get(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyPreProyectoDocu_Get(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Proceso que genera la solicitud
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="prefijo">prefijo</param>
        /// <param name="numeroDoc">numero doc</param>
        /// <param name="areaFuncional">area funcional</param>
        /// <param name="claseServicioID">clase servicio</param>
        /// <param name="clienteID">cliente</param>
        /// <param name="proyectoID">proyecto</param>
        /// <returns></returns>
        public DTO_TxResult SolicitudProyecto_Add(Guid channel, int documentoID, ref int numeroDoc, string claseServicioID, string areaFuncional, string prefijo, string proyectoID, string centroCto, string observaciones, DTO_SolicitudTrabajo transaccion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.SolicitudProyecto_Add(documentoID, ref numeroDoc, claseServicioID, areaFuncional, prefijo, proyectoID, centroCto, observaciones, transaccion);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Enviar aprobación la solicitud de trabajo
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="numeroDoc">numero documento</param>
        public DTO_SerializedObject SolicitudProyecto_AprobarProy(Guid channel, int documentID, DTO_SolicitudTrabajo transaccion, DateTime fechaInicio)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.SolicitudProyecto_AprobarProy(documentID, transaccion,fechaInicio, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Proceso que guarda un detalle del Cliente
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="numeroDoc">numero doc</param>
        /// <param name="transaccion">info del proyecto</param>
        /// <returns></returns>
        public DTO_TxResult SolicitudProyecto_AddAPUCliente(Guid channel, int documentoID, int numeroDoc, DTO_SolicitudTrabajo transaccion, bool saveProyectoInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.SolicitudProyecto_AddAPUCliente(documentoID, numeroDoc, transaccion, saveProyectoInd);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Numero de documento</param>
        /// <param name="actividadFlujoID">Identificador del Documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl pyServicioDocu_GetInternalDoc(Guid channel, int documentID, string idPrefijo, int documentoNro, string actividadFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyPreProyectoDocu_GetInternalDoc(documentID, idPrefijo, documentoNro, actividadFlujoID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="loadDetaInd">Indica si carga detalle de un documento</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyPreProyectoDeta> pyPreProyectoDeta_GetByParameter(Guid channel, int documentID, string tareaID, string claseServidioID, int? numeroDoc, bool loadDetaInd, decimal tasaCambio)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyPreProyectoDeta_GetByParameter(documentID, tareaID, claseServidioID, numeroDoc, loadDetaInd, documentID == AppDocuments.PlaneacionTiempo ? true : false,tasaCambio);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <returns>Lista de resultados</returns>
        public List<DTO_pyPreProyectoTarea> pyPreProyectoTarea_Get(Guid channel, int? numeroDoc, string tareaID, string claseServicioID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyPreProyectoTarea_Get(numeroDoc, tareaID, claseServicioID);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene los datos  de la solicitud
        /// </summary>
        /// <param name="documentoID">documento</param>
        /// <param name="prefijoID">prefijo del doc</param>
        /// <param name="docNro">nro consecutivo</param>
        /// <param name="numeroDoc">identificador doc</param>
        /// <param name="claseServicioID">clase servicio</param>
        /// <param name="isPreProyecto">Indica si carga las tablas de solicitud</param>
        /// <param name="loadMvtos">Indica si carga las los mvtos del detalle</param>
        /// <param name="loadActas">Indica si carga las actas de trabajo</param>
        /// <param name="loadTrazabilidad">Indica si carga la consulta de trazabilidad del proyecto</param>
        /// <returns>Objeto con todos los datos</returns>
        public DTO_SolicitudTrabajo SolicitudProyecto_Load(Guid channel, int documentoID, string prefijoID, int? docNro, int? numeroDoc, string claseServicioID,
                                                           string proyectoID, bool isPreProyecto, bool loadMvtos, bool loadActas, bool loadAPUCliente, bool loadTrazabilidad)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.SolicitudProyecto_Load(documentoID, prefijoID, docNro, numeroDoc, claseServicioID,proyectoID, isPreProyecto,loadMvtos,loadActas,loadAPUCliente,loadTrazabilidad);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
         
        #endregion

        #region Orden de Servicio

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_pyProyectoDocu pyProyectoDocu_Get(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoDocu_Get(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Planeacion Compras

        /// <summary>
        /// Aprueba los recursos para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="listMvtos">Lista de recursos a aprobar</param>
        /// <returns>Result</returns>
        public DTO_SerializedObject CompraRecursos_ApproveSolicitudOC(Guid channel, int documentID, List<DTO_pyProyectoMvto> listMvtos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CompraRecursos_ApproveSolicitudOC(documentID, listMvtos, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene los recursos pendientes para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <returns>Lista de</returns>
        public List<DTO_pyProyectoMvto> CompraRecursos_GetPendientesForApprove(Guid channel, DateTime fechaTope)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.CompraRecursos_GetPendientesForApprove(fechaTope);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region pyProyectoTarea
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <param name="claseServicio">clase servicio</param>
        /// <param name="recursoTimeInd">valida si es recurso de tiempo</param>
        /// <returns>Lista de resultados</returns>
        public List<DTO_pyProyectoTarea> pyProyectoTarea_Get(Guid channel, int? numeroDoc, string tareaID, string claseServicioID, bool recursoTimeInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyProyectoTarea_Get(numeroDoc, tareaID, claseServicioID,recursoTimeInd);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region pyProyectoDeta
        /// <summary>
        /// Trae una lista de proyecto Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="tareaID">Identificador del prefijo</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="loadDetaExist">Indica si carga detalle de un documento</param>
        ///  <param name="recursoTimeInd">Indica si carga si carga los recursos de solo tiempo</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_pyProyectoDeta> pyProyectoDeta_GetByParameter(Guid channel, int documentID, string tareaID, string claseServicioID, int? numeroDoc, bool loadDetaExist, bool recursoTimeInd, decimal tasaCambio)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyProyectoDeta_GetByParameter(documentID, tareaID, claseServicioID, numeroDoc, loadDetaExist, documentID == AppDocuments.PlaneacionTiempo ? true : false, tasaCambio);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region pyProyectoMvto
        /// <summary>
        /// Actualiza la tabla de mvtos
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        public DTO_SerializedObject pyProyectoMvto_Upd(Guid channel, int documentID, List<DTO_pyProyectoMvto> listMvtos, bool SendToSolicitudInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoMvto_Upd(documentID, listMvtos, SendToSolicitudInd, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el consolidado de documentos relacionados al proyecto
        /// </summary>
        /// <param name="consProyMvto">identificador del mvto de proyecto</param>
        /// <returns>Documentos</returns>
        public List<DTO_glDocumentoControl> pyProyectoMvto_GetDocsAnexo(Guid channel, int? consProyMvto)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoMvto_GetDocsAnexo(consProyMvto);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        ///<summary>
        /// Trae los mvtos con el filtro
        /// </summary>
        /// <param name="filter">filtro adecuado</param>
        /// <returns>Detalle mvtos</returns>
        public List<DTO_pyProyectoMvto> pyProyectoMvto_GetParameter(Guid channel, DTO_pyProyectoMvto filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoMvto_GetParameter(filter);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        ///<summary>
        /// Trae un mvto de proyecto por consecutivo
        /// </summary>
        /// <param name="consecMvto">filtro consec</param>
        /// <returns>mvto</returns>
        public DTO_pyProyectoMvto pyProyectoMvto_GetByConsecutivo(Guid channel,int? consecMvto)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoMvto_GetByConsecutivo(consecMvto);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Acta de Trabajo

        /// <summary>
        /// Aprueba el Acta de Trabajo y genera un recibido de Proveedores
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="proyecto">Lista de actas a aprobar</param>
        /// <returns>Result</returns>
        public DTO_SerializedObject ActaTrabajo_ApproveRecibidoBS(Guid channel, int documentID, DTO_SolicitudTrabajo proyecto, DTO_glDocumentoControl ctrlActa)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ActaTrabajo_ApproveRecibidoBS(documentID, proyecto,ctrlActa, DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="actasList">lista de actas</param>
        /// <returns></returns>
        public DTO_SerializedObject ActaTrabajo_Add(Guid channel, int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyActaTrabajoDeta> actasList, DTO_coProyecto proy, bool update)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ActaTrabajo_Add(documentoID, docCtrl,actasList,proy, update,DictionaryProgress.BatchProgress, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene las actas de trabajo
        /// </summary>
        /// <param name="numeroDoc">identificador doc</param>
        /// <returns>Objeto con todos los datos</returns>
        public List<DTO_pyActaTrabajoDeta> ActasTrabajo_Load(Guid channel,int? numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ActasTrabajo_Load(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Otras

        /// <summary>
        /// Trae las tareas(pyTarea) con un filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista</returns>
        public List<DTO_TareasFilter> TareasFilter_Get(Guid channel, DTO_TareasFilter filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.TareasFilter_Get(filter);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">FEcha corte</param>
        /// <param name="loadTareas">Carga las tareas y recursos del proyecto</param>
        /// <param name="loadCompras">carga las compras del proyecto</param>
        /// <param name="loadFinanciero">carga las transacciones financieras</param>
        /// <returns>resultado</returns>
        public List<DTO_QueryComiteTecnico> pyProyectoDocu_GetAllProyectos(Guid channel, DateTime fechaCorte,bool loadTareas, bool loadCompras,bool loadFinanciero)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoDocu_GetAllProyectos(fechaCorte,loadTareas,loadCompras,loadFinanciero);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Entregas Cliente

        /// <summary>
        ///  Guarda las tareas Cliente para realizar entregas posteriores
        /// </summary>
        /// <param name="documentoID">doc actual</param>
        /// <param name="numDocProy">identificador del proy</param>
        /// <param name="tareasCliente">lista de tareas</param>
        /// <param name="tareas">tareas del proyecto</param>
        /// <param name="tareasAdic">tareas adicionales del proyecto</param>
        /// <returns></returns>
        public DTO_TxResult EntregasCliente_Add(Guid channel, int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> tareasCliente, List<DTO_pyProyectoTarea> tareas, List<DTO_pyProyectoTarea> tareasAdic, List<int> entregasDelete)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.EntregasCliente_Add(documentoID, numDocProy, tareasCliente, tareas, tareasAdic,entregasDelete,false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae las tareas Cliente asignadas a un proyecto
        /// </summary>
        /// <param name="numDocProy">identificador del proyecto</param>
        /// <param name="tareaCliente">tarea Cliente</param>
        /// <param name="desc">descripcion de la tarea</param>
        /// <returns></returns>
        public List<DTO_pyProyectoTareaCliente> pyProyectoTareaCliente_GetByNumeroDoc(Guid channel, int numDocProy, string tareaCliente, string desc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyProyectoTareaCliente_GetByNumeroDoc(numDocProy, tareaCliente,desc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="listEntregables">lista de actas</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_Add(Guid channel, int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyProyectoTareaCliente> listEntregables, DTO_coProyecto proy, bool update)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ActaEntrega_Add(documentoID, docCtrl, listEntregables,proy,update,DictionaryProgress.BatchProgress,false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Aprueba un docum de Acta de Entrega
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="proy">proyecto actual</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_Aprobar(Guid channel, int documentoID, DTO_glDocumentoControl docCtrl, DTO_coProyecto proy)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ActaEntrega_Aprobar(documentoID, docCtrl, proy,false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene las actas de un proyecto con un filtro
        /// </summary>
        /// <param name="filter">filtros</param>
        /// <returns>lista</returns>
        public List<DTO_pyActaEntregaDeta> pyActaEntregaDeta_GetByParameter(Guid channel, DTO_pyActaEntregaDeta filter)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.pyActaEntregaDeta_GetByParameter(filter);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Acta de entrega
        /// </summary>
        /// <param name="documentID">Doc</param>
        /// <param name="ctrlProy">ctrl</param>
        /// <param name="docuProy">header</param>
        /// <param name="listActaDeta">lista</param>
        /// <returns>resultado</returns>
        public DTO_SerializedObject ActaEntrega_ApprovePreFactura(Guid channel, int documentID, DTO_glDocumentoControl ctrlProy, DTO_pyProyectoDocu docuProy, List<DTO_pyProyectoTareaCliente> listActaDeta, List<DTO_faFacturacionFooter> listDetalleFact)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ActaEntrega_ApprovePreFactura(documentID, ctrlProy, docuProy, listActaDeta,listDetalleFact,DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Guarda las tareas Cliente para realizar entregas posteriores
        /// </summary>
        /// <param name="documentoID">doc actual</param>
        /// <param name="numDocProy">identificador del proy</param>
        /// <param name="entregables">lista de entregables</param>
        /// <param name="deleteEntregable">Valida si borra entregables</param>
        /// <param name="deleteProgramacion">Valida si borra programacion</param>
        /// <param name="deleteActas">Valida si borra actas</param>
        /// <param name="anulaPreFacturas">Valida si anula prefacturas</param>
        /// <returns>Resultados</returns>
        public DTO_TxResult EntregasCliente_Delete(Guid channel, int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> entregables, bool deleteEntregable, bool deleteProgramacion, bool deleteActas, bool anulaPreFacturas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.EntregasCliente_Delete(documentoID, numDocProy, entregables, deleteEntregable,deleteProgramacion,deleteActas,anulaPreFacturas);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Migrar Insumos

        /// <summary>
        /// Agrega o actualiza una lista de insumos o proveedores
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="listInsumos">Lista de insumos</param>
        /// <param name="listProveedores">Lista de proveedores</param>
        /// <param name="listAnalisis">Lista de analisis</param>
        /// <param name="listAPU">Lista de Analisis Precios Unitarios</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult MigracionInsumos(Guid channel, int documentID, List<DTO_MigracionInsumos> listInsumos, List<DTO_MigracionProveedor> listProveedores, List<DTO_MigracionGrupos> listAnalisis,List<DTO_MigracionAPU> listAPU)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.MigracionInsumos(documentID, listInsumos, listProveedores, listAnalisis, listAPU, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega o actualiza una lista de locaciones y entregas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="listLocaciones">Lista de Locaciones</param>
        /// <param name="listLocTareas">Lista de tareas</param>
        /// <param name="listLocRecursos">Lista de recursos</param>
        /// <param name="listEntregas">Lista de entregas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult MigracionLocacionEntregas(Guid channel, int documentID, int numDocProy, List<DTO_pyProyectoLocacion> listLocaciones, List<DTO_pyProyectoIngDetalleTarea> listLocTareas, List<DTO_pyProyectoIngDetalleDeta> listLocRecursos, List<DTO_pyProyectoEntregasxMes> listEntregas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.MigracionLocacionEntregas(documentID, numDocProy, listLocaciones, listLocTareas, listLocRecursos, listEntregas, DictionaryProgress.BatchProgress);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trea la lista de locaciones y entregas de un proyecto
        /// </summary>
        /// <param name="numDocProy">Documnto que inicia la tx</param>
        /// <param name="listLocaciones">Lista de Locaciones</param>
        /// <param name="listLocTareas">Lista de tareas</param>
        /// <param name="listLocRecursos">Lista de recursos</param>
        /// <param name="listEntregas">Lista de entregas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult LocacionEntregasGetByProyecto(Guid channel, int documentID, int numDocProy, ref List<DTO_pyProyectoLocacion> listLocaciones, ref List<DTO_pyProyectoIngDetalleTarea> listLocTareas, ref List<DTO_pyProyectoIngDetalleDeta> listLocRecursos, ref List<DTO_pyProyectoEntregasxMes> listEntregas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.LocacionEntregasGetByProyecto(documentID, numDocProy, ref listLocaciones, ref listLocTareas, ref listLocRecursos, ref listEntregas);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de Proyectos
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proyectos_Revertir(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                List<DTO_glDocumentoControl> ctrls = null;
                List<DTO_coComprobante> coComps = null;
                ModuloProyectos mod = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.Proyectos_Revertir(documentID, numeroDoc, null, ref ctrls, ref coComps, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region pyProyectoModificaFechas
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="aprobadoDocInd">Indica si trae un documento aprobado o sin aprobar</param>
        /// <returns>Lista de dtos</returns>
        public List<DTO_pyProyectoModificaFechas> pyProyectoModificaFechas_GetByNumeroDoc(Guid channel, int numeroDoc, string tarea)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyProyectoModificaFechas_GetByNumeroDoc(numeroDoc, tarea);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        public DTO_TxResult pyProyectoModificaFechas_Add(Guid channel, DTO_pyProyectoModificaFechas datos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyProyectoModificaFechas_Add(datos, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        public DTO_TxResult pyProyectoModificaFechas_Upd(Guid channel, DTO_pyProyectoModificaFechas datos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyProyectoModificaFechas_Upd(datos, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        public DTO_pyProyectoModificaFechas pyProyectoModificaFechas_Load(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloProyectos modu = (ModuloProyectos)facade.GetModule(ModulesPrefix.py, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modu.pyProyectoModificaFechas_Load(numeroDoc);
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
