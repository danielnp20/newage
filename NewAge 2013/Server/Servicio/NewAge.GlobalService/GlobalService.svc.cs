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
using NewAge.DTO.Resultados;
using NewAge.Server;
using NewAge.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using SentenceTransformer;

namespace NewAge.Server.GlobalService
{
    /// <summary>
    /// Clase Service:
    /// Provee la lista de operaciones para exponer o manejar la información del sistema
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class GlobalService : IGlobalService, IDisposable
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
        public GlobalService()
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
        public GlobalService(string connString, string connLoggerString)
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

                GC.Collect();
                GC.WaitForPendingFinalizers();
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

        #region Operaciones Globales

        #region Maestras

        #region ccCarteraComponente

        /// <summary>
        /// Trae los componentes de  cartera dependiendo de la linea de credito
        /// </summary>
        /// <param name="pagaduriaID">Linea de credito</param>
        /// <returns></returns>
        public List<DTO_ccCarteraComponente> ccCarteraComponente_GetByLineaCredito(Guid channel, string lineaCreditoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccCarteraComponente_GetByLineaCredito(lineaCreditoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region ccAnexosLista

        /// <summary>
        /// Trae los documentos anexos dependiendo de la pagaduria
        /// </summary>
        /// <param name="pagaduriaID">Id de la pagaduria</param>
        /// <returns></returns>
        public List<DTO_MasterBasic> ccAnexosLista_GetByPagaduria(Guid channel, string pagaduriaID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccPagaduriaAnexos_GetByPagaduria(pagaduriaID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        #endregion

        #region ccCliente

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="bItems">Lista de empresas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ccCliente_Add(Guid channel, int documentoID, byte[] bItems)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccCliente_Add(documentoID, bItems, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza un cliente
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult ccCliente_Update(Guid channel, DTO_ccCliente cliente)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccCliente_Update(cliente, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="data">data a guardar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResultDetail ccCliente_AddFromSource(Guid channel, int documentoID, object data)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccCliente_AddFromSource(documentoID,data, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region ccFasecolda
        /// <summary>
        ///  Adiciona una lista de fasecoldas y modelos
        /// </summary>
        /// <param name="fasecoldas">fasecoldas</param>
        /// <param name="modelos">modelos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ccFasecolda_Migracion(Guid channel, byte[] fasecoldas, byte[] modelos)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccFasecolda_Migracion(fasecoldas, modelos, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region ccChequeoLista

        /// <summary>
        /// Trae la lista de tareas asociados a un documento
        /// </summary>
        /// <param name="documentoID">Id de la pagaduria</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_MasterBasic> ccChequeoLista_GetByDocumento(Guid channel, int documentoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ccChequeoLista_GetByDocumento(documentoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region glActividadChequeoLista
        /// <summary>
        /// Trae la lista de chequeos de un flujo
        /// </summary>
        /// <param name="actividadFlujo">Flujo</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public List<DTO_MasterBasic> glActividadChequeoLista_GetByActividad(Guid channel, string actividadFlujo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glActividadChequeoLista_GetByActividad(actividadFlujo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region coCargoCosto

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="ConceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o null si no existe</returns>
        public string coCargoCosto_GetCuentaIDByCargoOper(Guid channel, string ConceptoCargoID, string proyID, string ctoCostoID, string lineaPresID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                string response = mod.coCargoCosto_GetCuentaIDByCargoOper(ConceptoCargoID, proyID, ctoCostoID, lineaPresID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o string.Empty si no existe</returns>
        public DTO_CuentaValor coCargoCosto_GetCuentaByCargoOper(Guid channel, string conceptoCargoID, string operacionID, string lineaPresID, decimal valor)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_CuentaValor response = mod.coCargoCosto_GetCuentaByCargoOper(conceptoCargoID, operacionID, lineaPresID, valor);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region coComprobantePrefijo

        /// <summary>
        /// Trae el identificador de un comprobante segun el documento y el prefijo
        /// </summary>
        /// <param name="coDocumentoID">Identificador del documento de la PK</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="compAnulacion">Indica si debe traer el comprobante de anulacion</param>
        /// <returns>Retorna el identificador de un comprobante o null si no existe</returns>
        public string coComprobantePrefijo_GetComprobanteByDocPref(Guid channel, int documentID, int coDocumentoID, string prefijoID, bool compAnulacion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                string response = mod.coComprobantePrefijo_GetComprobanteByDocPref(coDocumentoID, prefijoID, compAnulacion);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region coDocumento

        /// <summary>
        /// Dice si un prefijo ya esta asignado en la tabla coDocumento
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna Verdadero si el prefijo ya existe, de lo contrario retorna falso</returns>
        public bool coDocumento_PrefijoExists(Guid channel, string prefijoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.coDocumento_PrefijoExists(prefijoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region coPlanCuenta

        /// <summary>
        /// Trae una lista de tarifas para una cuenta
        /// </summary>
        /// <param name="impTipoID">Tipo de impuesto</param>
        public List<decimal> coPlanCuenta_TarifasImpuestos(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.coPlanCuenta_TarifasImpuestos();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la cuenta de la cuenta alterna
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna un registro de la cuenta alterna</returns>
        public DTO_coPlanCuenta coPlanCuenta_GetCuentaAlterna(Guid channel, int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.coPlanCuenta_GetCuentaAlterna(documentID, id, active, filtros);

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
        public long coPlanCuenta_CountChildren(Guid channel, int documentID, UDT_BasicID parentId, string idFilter, string descFilter, bool? active,
            List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.coPlanCuenta_CountChildren(documentID, parentId, idFilter, descFilter, active, filtros);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae los registros de un plan de cuentas corporativo 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de un plan de cuentas corporativo</returns>
        public IEnumerable<DTO_MasterHierarchyBasic> coPlanCuenta_GetPagedChildren(Guid channel, int documentID, int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId,
            string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.coPlanCuenta_GetPagedChildren(documentID, pageSize, pageNum, orderDirection, parentId, idFilter, descFilter, active, filtros);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region coPlanillaConsolidacion

        /// <summary>
        /// Trae la lista de empresas a consilidar
        /// </summary>
        /// <returns>Retorna la lista de empresas a consilidar</returns>
        public List<DTO_ComprobanteConsolidacion> coPlanillaConsolidacion_GetEmpresas(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.coPlanillaConsolidacion_GetEmpresas();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glActividadFlujo

        /// <summary>
        /// Obtiene la lista de tareas de un documento
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<string> glActividadFlujo_GetActividadesByDocumentID(Guid channel, int documentoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal modulo = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.glActividadFlujo_GetActividadesByDocumentID(documentoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la lista de padres de una actividad de flujo
        /// </summary>
        /// <param name="actFlujoID">Actividad hija</param>
        /// <returns>Retorna la lista de tareas</returns>
        public List<DTO_glActividadFlujo> glActividadFlujo_GetParents(Guid channel, string actFlujoID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal modulo = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.glActividadFlujo_GetParents(actFlujoID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glActividadPermiso

        /// <summary>
        /// Consulta un listado de tareas de glTareaPermiso para usuarios
        /// </summary>
        /// <returns>Listado de TareaID(int)</returns>
        public List<string> glActividadPermiso_GetActividadesByUser(Guid channel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal modulo = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.glActividadPermiso_GetActividadesByUser();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glEmpresa

        /// <summary>
        /// Trae la imagen del logo de la empresa
        /// </summary>
        /// <param name="empresaId">Id de la empresa</param>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        public byte[] glEmpresaLogo(Guid channel, DTO_glEmpresa empresa)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                byte[] response = mod.glEmpresaLogo();

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="empresa">Empresa sobre la que se esta trabajando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult glEmpresa_Add(Guid channel, int documentoID, byte[] bItems, int accion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.glEmpresa_Add(documentoID, bItems, accion, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Elimina una empresa
        /// </summary>
        /// <param name="empresaDel">Empresa que de desea eliminar</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="empresa">Empresa sobre la que se esta trabajando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult glEmpresa_Delete(Guid channel, int documentoID, DTO_glEmpresa empresaDel)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.glEmpresa_Delete(documentoID, empresaDel, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glEmpresaGrupo

        /// <summary>
        /// Agrega un nuevo grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool glEmpresaGrupo_Add(Guid channel, byte[] bItems)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.glEmpresaGrupo_Add(bItems, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Elimina un grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        public bool glEmpresaGrupo_Delete(Guid channel, string egID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.glEmpresaGrupo_Delete(egID, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glTabla

        /// <summary>
        /// Retorna todas las gl tablas de un grupo de empresas
        /// </summary>
        /// <param name="empGrupo">Nombre del grupo de empresas</param>
        /// <returns>Lista de glTabla</returns>
        public IEnumerable<DTO_glTabla> glTabla_GetAllByEmpresaGrupo(Guid channel, Dictionary<int, string> empGrupo, bool jerarquicaInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glTabla> response = mod.glTabla_GetAllByEmpresaGrupo(empGrupo, jerarquicaInd);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Indica si una tabla tiene datos para un grupo empresa
        /// </summary>
        /// <param name="tablaNombre">Nombre de la tabla</param>
        /// <param name="empGrupo">Empresa Grupo</param>
        /// <returns>True si tiene datos, False si no tiene datos</returns>
        public bool glTabla_HasData(Guid channel, string tablaNombre, string empresaGrupo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.glTabla_HasData(tablaNombre, empresaGrupo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna la info de una tabla segun el nombre y el grupo de empresas
        /// </summary>
        /// <param name="tablaNombre">Tabla Nombre</param>
        /// <param name="empGrupo">Grupo de empresas</param>
        /// <returns>Retorna la informacion de una tabla</returns>
        public DTO_glTabla glTabla_GetByTablaNombre(Guid channel, string tablaNombre, string empGrupo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glTabla_GetByTablaNombre(tablaNombre, empGrupo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glTasaDeCambio

        /// <summary>
        /// Obtiene el la tasa de cambio
        /// </summary>
        /// <param name="monedaID">Identificador de la moneda</param>
        /// <param name="fecha">Fecha</param>
        /// <returns>Retorna la tasa de canbio</returns>
        public decimal TasaDeCambio_Get(Guid channel, string monedaID, DateTime fecha)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                decimal response = mod.TasaDeCambio_Get(monedaID, fecha);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region seDelegacionTarea

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        public List<DTO_seDelegacionHistoria> seDelegacionHistoria_Get(Guid channel, string userId)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.seDelegacionHistoria_Get(userId);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega una delegacion
        /// </summary>
        /// <param name="del">Delegacion</param>
        public bool seDelegacionHistoria_Add(Guid channel, int documentID, DTO_seDelegacionHistoria del)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.seDelegacionHistoria_Add(documentID, del, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza el estado de un delegado
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="enabled">Nuevo estado</param>
        public bool seDelegacionHistoria_UpdateStatus(Guid channel, int documentID, string userID, DateTime fechaIni, bool enabled)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.seDelegacionHistoria_UpdateStatus(documentID, userID, fechaIni, enabled, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region seGrupoDocumento

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        public IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByUsuarioId(Guid channel, string userEmpDef, int userId, bool isGroupActive = true)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_seGrupoDocumento> response = mod.seGrupoDocumento_GetByUsuarioID(userEmpDef, userId, isGroupActive);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene las seguridades del sistema para un grupo dado el modulo y el tipo de documento
        /// </summary>
        /// <param name="grupo">Grupo de seguridades</param>
        /// <param name="tipo">Tipo de documento</param>
        /// <returns>Retorna las seguridades de un grupo</returns>
        public IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByType(Guid channel, string grupo, string tipo)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_seGrupoDocumento> response = mod.seGrupoDocumento_GetByType(grupo, tipo);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza una lista de seguridades
        /// </summary>
        /// <param name="bItems">Lista de seguridades comprimidas</param>
        /// <param name="seUsuario">Identificador del usuario</param>
        /// <returns>Retorna la lista de seguridades comprimidas</returns>
        public DTO_TxResult seGrupoDocumento_UpdateSecurity(Guid channel, byte[] bItems, int seUsuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.seGrupoDocumento_UpdateSecurity(bItems, seUsuario, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region seLAN

        /// <summary>
        /// Trae la configuración de una LAN
        /// </summary>
        /// <param name="lan">Nombre de la LAN</param>
        /// <returns>Retorna la configuracion de una LAN</returns>
        public DTO_seLAN seLAN_GetLanByID(Guid channel, string lan, int documentID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_seLAN response = mod.seLAN_GetLanByID(lan, documentID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae todas las configuraciones de LAN
        /// </summary>
        /// <returns>Retorna la lista de LANs y sus configuraciones</returns>
        public List<DTO_seLAN> seLAN_GetLanAll(Guid channel, int documentID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                List<DTO_seLAN> response = mod.seLAN_GetLanAll(documentID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region seMaquina

        /// <summary>
        /// valida que la maquina pueda ingresar al sistema
        /// </summary>
        /// <param name="pcMAC">MACs posibles</param>
        /// <returns>Retorna verdadero si la maquina tiene permiso</returns>
        /// <returns>Devuelve si el usuarios es valido Usuarios</returns>
        public bool seMaquina_ValidatePC(Guid channel, List<string> macs)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.seMaquina_ValidatePC(macs);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region seUsuario

        /// <summary>
        /// Trae un usuario valido
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="password">Contraseña de usuario</param>
        /// <returns>Returna un usuario valido</returns>
        public UserResult seUsuario_ValidateUserCredentials(Guid channel, string userId, string password)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                UserResult response = mod.seUsuario_ValidateUserCredentials(AppMasters.seUsuario, userId, password);

                DTO_seUsuario user = this.seUsuario_GetUserbyID(channel, userId);
                if (user != null)
                {
                    ModuloAplicacion modApl = (ModuloAplicacion)facade.GetModule(ModulesPrefix.apl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                    modApl.aplBitacora_Add(user.EmpresaID.Value, Convert.ToInt32(AppForms.LognIn), (short)0, DateTime.Now, Convert.ToInt32(user.ReplicaID.Value), response.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                }
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }

        }

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <returns>Retorna un usuario</returns>
        public DTO_seUsuario seUsuario_GetUserbyID(Guid channel, string userId)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_seUsuario response = mod.seUsuario_GetUserbyID(userId);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un usuario de acuerdo con el id de la replica (pk)
        /// </summary>
        /// <param name="userID">Identificador del usuario (ReplicaID)</param>
        /// <returns>Retorna el Usuario</returns>
        public DTO_seUsuario seUsuario_GetUserByReplicaID(Guid channel, int replicaID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_seUsuario response = mod.seUsuario_GetUserByReplicaID(replicaID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la lista de usuarios
        /// </summary>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve la lista de usuarios </returns>
        public IEnumerable<DTO_MasterBasic> seUsuario_GetAll(Guid channel, int documentoID, bool? active)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_MasterBasic> response = mod.seUsuario_GetAll(documentoID, active);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <param name="oldPwd">Contraseña vieja</param>
        /// <param name="oldPwdDate">Fecha en que fue modificada por ultima vez la contraseña</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool seUsuario_UpdatePassword(Guid channel, int userID, string pwd, string oldPwd, string oldPwdDate)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.seUsuario_UpdatePassword(userID, pwd, oldPwd, oldPwdDate, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        public bool seUsuario_ResetPassword(Guid channel, int userID, string pwd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                bool response = mod.seUsuario_ResetPassword(userID, pwd);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Devuelve las empresas a las que tiene permiso un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna una lista de empresas</returns>
        public IEnumerable<DTO_glEmpresa> seUsuario_GetUserCompanies(Guid channel, string userID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glEmpresa> response = mod.seUsuario_GetUserCompanies(userID);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="empresa">Empresa sobre la que se esta trabajando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult seUsuario_Add(Guid channel, int documentoID, byte[] bItems, int seUsuario, int accion)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.seUsuario_Add(documentoID, bItems, seUsuario, accion, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="usr">registro donde se realiza la acción</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <returns>Resultado TxResult</returns>
        public virtual DTO_TxResult seUsuario_Update(Guid channel, int documentoID, DTO_seUsuario usr, int seUsuario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.seUsuario_Update(documentoID, usr, seUsuario, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region glActividadControl

        /// <summary>
        /// Trae los registros de glTareasControl
        /// </summary>
        /// <param name="numeroDoc">concecutivo documento</param>
        /// <returns></returns>
        public IEnumerable<DTO_glActividadControl> glActividadControl_Get(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glActividadControl> response = mod.glActividadControl_Get(numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glActividadEstado

        /// <summary>
        /// Obtiene lista de actividades con llamada pendientes
        /// </summary>
        /// <param name="documentoID">documento filtro</param>
        /// <param name="actFlujoID">actividad o tarea filtro</param>
        /// <param name="fechaIni">fecha inicial de consulta</param>
        /// <param name="fechaFin">fecha final de consulta</param>
        /// <param name="terceroID">tercero filtro</param>
        /// <param name="prefijoID">prefijo filtro</param>
        /// <param name="docNro">nro de documento filtro</param>
        ///  <param name="tipo">Filtra vencidas, no vencidas o todas</param>
        ///  <param name="LLamadaInd">Si asigna valores de llamadas</param>
        /// <param name="estadoTareaInd">Indica si esta cerrada o no</param>
        /// <returns></returns>
        public List<DTO_InfoTarea> glActividadEstado_GetPendientesByParameter(Guid channel, int? numeroDoc, int? documentoID, string actFlujoID, DateTime? fechaIni,
            DateTime? fechaFin, string terceroID, string prefijoID, int? docNro, EstadoTareaIncumplimiento tipo, bool llamadaInd, bool? vencidas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glActividadEstado_GetPendientesByParameter(numeroDoc, documentoID, actFlujoID, fechaIni, fechaFin, terceroID, prefijoID, docNro,
                    tipo, llamadaInd, vencidas);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega registros a actividadEstado
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="notas">lista de notas</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult glActividadEstado_AddNotas(Guid channel,int documentID, List<DTO_InfoTarea> notas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glActividadEstado_AddNotas(documentID, notas, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene la actividad actual del documento solicitado y filtra las que solo estan abiertas si requiere
        /// </summary>
        /// <param name="NumeroDoc">id del documento</param>
        /// <param name="onlyAbiertas">valida solo las actividades abiertas</param>
        /// <returns>ActividadFlujo</returns>
        public string glActividadEstado_GetActFlujoByNumeroDoc(Guid channel, int numeroDoc, bool onlyAbiertas)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glActividadEstado_GetActFlujoByNumeroDoc(numeroDoc, onlyAbiertas);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region glGarantiaControl

        /// <summary>
        /// Agrega un registro al control de garantias
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="garantias">data a guardar o actualizar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult glGarantiaControl_Add(Guid channel, int documentID, List<DTO_glGarantiaControl> garantias, List<int> itemsDelete)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glGarantiaControl_Add(documentID, garantias,itemsDelete,DictionaryProgress.BatchProgress,false);

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
        /// <param name="filter"> Filtro</param>
        /// <param name="prefijoID"> Prefijo filtro</param>
        /// <param name="docNro"> Doc nro filtro</param>
        /// <param name="estado"> Estado de la garantia</param>
        /// <returns>Lista </returns>
        public List<DTO_glGarantiaControl> glGarantiaControl_GetByParameter(Guid channel,DTO_glGarantiaControl filter, string prefijoID, int? docNro, byte? estado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glGarantiaControl_GetByParameter(filter, prefijoID, docNro, estado);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region glIncumpleCambioEstado

        /// <summary>
        /// Agrega info a glIncumpleCambioEstado
        /// </summary>
        /// <param name="documentID">documento Actual</param>
        /// <param name="gestionList">gestionList</param>
        /// <returns></returns>
        public DTO_TxResult glIncumpleCambioEstado_Update(Guid channel,int documentID, List<DTO_GestionCobranza> gestionList)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glIncumpleCambioEstado_Update(documentID, gestionList, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glControl

        /// <summary>
        /// Actualiza glControl
        /// </summary>
        /// <param name="control">control</param> 
        /// <returns>Retorna una respuesta TxResult</returns>
        public DTO_TxResult glControl_Update(Guid channel, DTO_glControl control)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.glControl_Update(control);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae todos los glControl de una empresa
        /// </summary>
        /// <param name="isBasic">Indica si solo trae la informacion basica</param>
        /// <param name="numEmpresa">Numero de control de una empresa</param>
        /// <returns>enumeracion de glControl</returns>
        public IEnumerable<DTO_glControl> glControl_GetByNumeroEmpresa(Guid channel, bool isBasic, string numEmpresa)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glControl> response = mod.glControl_GetByNumeroEmpresa(isBasic, numEmpresa);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        ///  Trae una fila de la tabla de control de acuerdo a un id
        /// </summary>
        /// <param name="controlId">ID de control</param>
        /// <returns>dto control encontrado</returns>
        public DTO_glControl glControl_GetById(Guid channel, int controlId)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glControl response = mod.GetControlByID(controlId);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza una lista de registros 
        /// </summary>
        /// <param name="data">Diccionario con la lista de datos "Llave,Valor"</param>
        public void glControl_UpdateModuleData(Guid channel, Dictionary<string, string> data)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.glControl_UpdateModuleData(data);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glDocAnexoControl

        /// <summary>
        /// Retorna la lista de anexos de un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <returns>Retorna la lista de anexos</returns>
        public List<DTO_glDocAnexoControl> glDocAnexoControl_GetAnexosByNumeroDoc(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocAnexoControl_GetAnexosByNumeroDoc(numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Retorna un anexo de un documento
        /// </summary>
        /// <param name="">replica</param>
        /// <returns>Retorna un anexo</returns>
        public DTO_glDocAnexoControl glDocAnexoControl_GetAnexosByReplica(Guid channel,int replica)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocAnexoControl_GetAnexosByReplica(replica);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualiza los anexos de un documento
        /// </summary>
        /// <param name="mod">Modulo que guarda los anexos</param>
        /// <param name="list">lista de anexos</param>
        /// <returns></returns>
        public DTO_TxResult glDocAnexoControl_Update(Guid channel, ModulesPrefix modu, List<DTO_glDocAnexoControl> list)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocAnexoControl_Update(modu, list);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        #endregion

        #region glDocumentoControl

        /// <summary>
        /// Anula un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        public DTO_TxResult glDocumentoControl_Anular(Guid channel, int documentID, List<int> numeroDoc)
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
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_TxResult response = mod.glDocumentoControl_Anular(documentID, numeroDoc, DictionaryProgress.BatchProgress, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Revierte un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        public DTO_TxResult glDocumentoControl_Revertir(Guid channel, int documentID, int numeroDoc)
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
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);

                List<DTO_glDocumentoControl> ctrls = new List<DTO_glDocumentoControl>();
                List<DTO_coComprobante> coComps = new List<DTO_coComprobante>();
                DTO_TxResult response = mod.glDocumentoControl_Revertir(documentID, numeroDoc, null, ref ctrls, ref coComps, false);

                return response;
            }
            finally
            {
                _currentProcess.Remove(documentID);
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Le cambia el estado a un documentoControl y guarda en la bitacora
        /// </summary>
        /// <param name="documentoID">Documento que esta ejecutando la transaccion (TAREA ACTUAL) y del cual se busca la siguiente tarea</param>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) de glDocumentoControl</param>
        /// <param name="estado">Nuevo estado</param>
        /// <param name="obs">Observaciones</param>
        /// <returns>Retorna el identificador de la bitacora con que se guardo la info</returns>
        public int glDocumentoControl_ChangeDocumentStatus(Guid channel,int documentoID, int numeroDoc, EstadoDocControl estado, string obs)
        {

            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_ChangeDocumentStatus(documentoID, numeroDoc, estado, obs,false);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public DTO_glDocumentoControl glDocumentoControl_GetByID(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetByID(numeroDoc);

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
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetInternalDoc(Guid channel, int documentId, string idPrefijo, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetInternalDoc(documentId, idPrefijo, numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetExternalDoc(Guid channel, int documentId, string idTercero, string numeroDocTercero)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetExternalDoc(documentId, idTercero, numeroDocTercero);

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
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        public DTO_glDocumentoControl glDocumentoControl_GetInternalDocByCta(Guid channel, string cuentaID, string idPrefijo, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetInternalDocByCta(cuentaID, idPrefijo, numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetExternalDocByCta(Guid channel, string cuentaID, string idTercero, string numeroDocTercero)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetExternalDocByCta(cuentaID, idTercero, numeroDocTercero);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        public DTO_glDocumentoControl glDocumentoControl_GetByComprobante(Guid channel, int documentId, DateTime periodo, string comprobanteID, int compNro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetByComprobante(documentId, periodo, comprobanteID, compNro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene el documento relacionado con una libranza
        /// </summary>
        /// <param name="libranza">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="cerradoInd">Indica si trae la actividad con estado cerrado o abierto</param>
        /// <returns>Documento</returns>
        public DTO_glDocumentoControl glDocumentoControl_GetByLibranzaSolicitud(Guid channel, int libranza, string actFlujoID, bool cerradoInd)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetByLibranzaSolicitud(libranza, actFlujoID, cerradoInd);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la info de los documento s para generarles el posteo de comprobantes
        /// </summary>
        /// <param name="mod">Modulo del cual se van a traer el listado de documentos</param>
        /// <param name="contabilizado">Indica si trae los documentos que ya fueron procesados</param>
        /// <returns>Retorna el listado de documentos</returns>
        public List<DTO_glDocumentoControl> glDocumentoControl_GetForPosteo(Guid channel, ModulesPrefix modulo, bool contabilizado)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetForPosteo(modulo, contabilizado);

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
        /// <param name="ctrl">Doc Control filtro</param>
        /// <returns>Lista de Doc Control </returns>
        public List<DTO_glDocumentoControl> glDocumentoControl_GetByParameter(Guid channel, DTO_glDocumentoControl ctrl)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoControl_GetByParameter(ctrl);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glLlamadasControl

        /// <summary>
        /// Trae la lista de glLlamadasControl
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        /// <returns>Retorna un listado de glLlamadasControl </returns>
        public List<DTO_glLlamadasControl> glLlamadasControl_GetByID(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal modulo = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.glLlamadasControl_GetByID(numeroDoc);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Agrega un registro en glLlamadasControl
        /// </summary>
        /// <param name="documentoID">Documento ID</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="llamadasCtrl">Lista de las llamdas realizadas</param>
        /// <param name="terceroRef">Lista de las referencias</param>
        /// <param name="sendToAprob">Indicador para establecer si se guarda el registro y se asigna la actividad de flujo o no</param>
        /// <returns>Retorna el resultado de la consulta</returns>
        public DTO_TxResult glLlamadasControl_Add(Guid channel, int documentoID, string actividadFlujoID, List<DTO_glLlamadasControl> llamadasCtrl, List<DTO_glTerceroReferencia> terceroRefs, bool sendToAprob)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal modulo = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = modulo.glLlamadasControl_Add(documentoID, actividadFlujoID, llamadasCtrl, terceroRefs, sendToAprob, false);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        
        #endregion

        #region glMovimientoDeta

        /// <summary>
        /// Obtiene la cantidad de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna la cantidad de registros de la consulta</returns>
        public long glMovimientoDeta_Count(Guid channel, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glMovimientoDeta_Count(consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene una lista de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de la pagina de consulta</param>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna los registros de la consulta</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetPaged(Guid channel, int pageSize, int pageNum, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glMovimientoDeta_GetPaged(pageSize, pageNum, consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Trae la lista de glMovimientoDeta para los activos
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        /// <returns></returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetBy_ActivoFind(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glMovimientoDeta_Get_ActivoFind(numeroDoc);

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
        /// <param name="filter"></param>
        /// <returns>Dto de Mvto detalle</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDeta_GetByParameter(Guid channel, DTO_glMovimientoDeta filter, bool isPre, bool onlyInventario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glMovimientoDeta_GetByParameter(filter, isPre, onlyInventario);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }


        /// <summary>
        /// Trae informacion de acuerdo al filtro a Excel
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Mvto detalle</returns>
        public DataTable glMovimientoDeta_GetByParameterExcel(Guid channel, DTO_glMovimientoDeta filter, bool isPre, bool onlyInventario)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glMovimientoDeta_GetByParameterExcel(filter, isPre, onlyInventario);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Consulta un movimientoDetaPRE relacionado con proyectos con saldos de Inventario
        /// </summary>
        /// <param name="periodo">Periodo de saldos de inventarios</param>
        /// <param name="bodega">Bodega a consultar</param>
        /// <param name="proyectoID">Proyecto a consultar</param>
        /// <returns>lista de movimientos</returns>
        public List<DTO_glMovimientoDeta> glMovimientoDetaPRE_GetSaldosInvByProyecto(Guid channel, DateTime periodo, string proyectoID, bool isPre)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glMovimientoDetaPRE_GetSaldosInvByProyecto(periodo, proyectoID,isPre);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glDocumentoChequeoLista
        /// <summary>
        /// Retorna el Listado los Documentos Anexos
        /// </summary>
        /// <returns></returns> 
        public List<DTO_glDocumentoChequeoLista> glDocumentoChequeoLista_GetByNumeroDoc(Guid channel, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glDocumentoChequeoLista_GetByNumeroDoc(numeroDoc);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }
        #endregion

        #region Consultas

        #region glConsultaFiltro

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsultaFiltro> glConsultaFiltro_GetAll(Guid channel, DTO_glConsultaFiltro filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glConsultaFiltro> response = mod.glConsultaFiltro_GetAll(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Get(Guid channel, DTO_glConsultaFiltro filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsultaFiltro response = mod.glConsultaFiltro_Get(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Add(Guid channel, DTO_glConsultaFiltro filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsultaFiltro response = mod.glConsultaFiltro_Add(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaFiltro_Delete(Guid channel, DTO_glConsultaFiltro filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.glConsultaFiltro_Delete(filtro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsultaFiltro glConsultaFiltro_Update(Guid channel, DTO_glConsultaFiltro filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsultaFiltro response = mod.glConsultaFiltro_Update(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glConsulta

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsulta> glConsulta_GetAll(Guid channel, DTO_glConsulta filtro, int? userID)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glConsulta> response = mod.glConsulta_GetAll(filtro, userID);
                foreach (DTO_glConsulta consulta in response)
                {
                    DTO_glConsultaFiltro f = new DTO_glConsultaFiltro();
                    f.glConsultaID = consulta.glConsultaID;
                    DTO_glConsultaSeleccion s = new DTO_glConsultaSeleccion();
                    s.glConsultaID = consulta.glConsultaID;
                    consulta.Selecciones = mod.glConsultaSeleccion_GetAll(s).ToList();
                    consulta.Filtros = mod.glConsultaFiltro_GetAll(f).ToList();
                }

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsulta glConsulta_Add(Guid channel, DTO_glConsulta filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.glConsulta_Add(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsulta_Delete(Guid channel, DTO_glConsulta filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.glConsulta_Delete(filtro, false);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsulta glConsulta_Update(Guid channel, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsulta response = mod.glConsulta_Update(consulta);
                mod.glConsultaFiltro_DeleteByQueryID(consulta.glConsultaID);
                mod.glConsultaSeleccion_DeleteByQueryID(consulta.glConsultaID);
                foreach (DTO_glConsultaSeleccion sel in consulta.Selecciones)
                {
                    mod.glConsultaSeleccion_Add(sel);
                }
                foreach (DTO_glConsultaFiltro fil in consulta.Filtros)
                {
                    mod.glConsultaFiltro_Add(fil);
                }

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region glConsultaSeleccion

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        public IEnumerable<DTO_glConsultaSeleccion> glConsultaSeleccion_GetAll(Guid channel, DTO_glConsultaSeleccion filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                IEnumerable<DTO_glConsultaSeleccion> response = mod.glConsultaSeleccion_GetAll(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Get(Guid channel, DTO_glConsultaSeleccion filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsultaSeleccion response = mod.glConsultaSeleccion_Get(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Add(Guid channel, DTO_glConsultaSeleccion filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsultaSeleccion response = mod.glConsultaSeleccion_Add(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        public void glConsultaSeleccion_Delete(Guid channel, DTO_glConsultaSeleccion filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.glConsultaSeleccion_Delete(filtro);
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        public DTO_glConsultaSeleccion glConsultaSeleccion_Update(Guid channel, DTO_glConsultaSeleccion filtro)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                DTO_glConsultaSeleccion response = mod.glConsultaSeleccion_Update(filtro);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #region Consultas Generales

        /// <summary>
        /// Consultas generales
        /// </summary>
        /// <param name="vista">Nombre de la vista</param>
        /// <param name="dtoType">Tipo de DTO</param>
        /// <param name="consulta">Consulta con filtros</param>
        /// <param name="fields">Capos a mostrar en los resultados</param>
        /// <returns></returns>
        public DataTable ConsultasGenerales(Guid channel, string vista, Type dtoType, DTO_glConsulta consulta)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                var response = mod.ConsultasGenerales(vista, dtoType, consulta);

                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        #endregion

        #endregion

        #region Global

        /// <summary>
        /// Crea un diccionario con la informacion de los combos de las maestras
        /// </summary>
        /// <param name="docId">identificador documemto</param>
        /// <param name="columnName">nombre columna</param>
        /// <param name="llave">llave de recurso</param>
        /// <returns></returns>
        public Dictionary<string, string> GetOptionsControl(Guid channel, int docId, string columnName, string llave)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                Dictionary<string, string> response = mod.GetOptionsControl(docId, columnName, llave);
                return response;
            }
            finally
            {
                this.ADO_CloseDBConnection(opIndex);
            }
        }

        /// <summary>
        /// Genera un reporte
        /// </summary>
        /// <param name="documentID">documento con el cual se salva el archivo</param>
        /// <param name="numeroDoc">Numero del documento con el cual se salva el archivo>
        public void GenerarReportOld(Guid channel, int documentID, int numeroDoc)
        {
            int opIndex = -1;
            try
            {
                opIndex = this.ADO_ConnectDB();
                ModuloGlobal mod = (ModuloGlobal)facade.GetModule(ModulesPrefix.gl, this._mySqlConnections[opIndex], null, _channels[channel].Item1, _channels[channel].Item2, this._connLoggerString);
                mod.GenerarReportOld(documentID, numeroDoc);
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
