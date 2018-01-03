using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using System.Data;
using SentenceTransformer;

namespace NewAge.Server.GlobalService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información del sistema
    /// </summary>
    [ServiceContract]
    public interface IGlobalService
    {
        #region Conection Management

        /// <summary>
        /// Cierra la conexion
        /// </summary>
        [OperationContract]
        void ADO_CloseDBConnection(int connIndex);

        #endregion

        #region Funciones Para Manejo de Progreso

        /// <summary>
        /// Consulta el progreso de una transacción
        /// </summary>
        /// <param name="DocumentID">Identificador del proceso (documento)</param>
        /// <returns>Retorna el porcentaje del progreso</returns>
        [OperationContract]
        int ConsultarProgreso(Guid channel, int documentID);

        #endregion

        #region Funciones Propias del Servicio

        /// <summary>
        /// Crea una nueva sesion pra el usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        [OperationContract]
        void CrearCanal(Guid channel);

        /// <summary>
        /// Cierra la sesion de un usuario
        /// </summary>
        /// <param name="g">Identificador unico del usuario, para identificar la sesion actual</param>
        [OperationContract]
        void CerrarCanal(Guid channel);

        /// <summary>
        /// Carga las variable de empresa y usario para el servicio
        /// </summary>
        /// <param name="emp">empresa</param>
        /// <param name="userID">userId</param>
        [OperationContract]
        void IniciarEmpresaUsuario(Guid channel, DTO_glEmpresa emp, int userID);

        #endregion

        #region Operaciones Globales

        #region Maestras

        #region ccCarteraComponete

        /// <summary>
        /// Trae los componentes de  cartera dependiendo de la linea de credito
        /// </summary>
        /// <param name="pagaduriaID">Linea de credito</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccCarteraComponente> ccCarteraComponente_GetByLineaCredito(Guid channel, string lineaCreditoID);
        #endregion

        #region ccAnexosLista

        /// <summary>
        /// Trae los documentos anexos dependiendo de la pagaduria
        /// </summary>
        /// <param name="pagaduriaID">Id de la pagaduria</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_MasterBasic> ccAnexosLista_GetByPagaduria(Guid channel, string pagaduriaID);
        #endregion

        #region ccCliente

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="bItems">Lista de empresas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult ccCliente_Add(Guid channel, int documentoID, byte[] bItems);

        /// <summary>
        /// Actualiza un cliente
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult ccCliente_Update(Guid channel, DTO_ccCliente cliente);

        /// <summary>
        ///  Adiciona una lista de clientes
        /// </summary>
        /// <param name="documentoID">Identifica el documento</param>
        /// <param name="data">data a guardar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResultDetail ccCliente_AddFromSource(Guid channel, int documentoID, object data);

        #endregion

        #region ccFasecolda
        /// <summary>
        ///  Adiciona una lista de fasecoldas y modelos
        /// </summary>
        /// <param name="fasecoldas">fasecoldas</param>
        /// <param name="modelos">modelos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult ccFasecolda_Migracion(Guid channel, byte[] fasecoldas, byte[] modelos);
        #endregion

        #region ccChequeoLista

        /// <summary>
        /// Trae la lista de tareas asociados a un documento
        /// </summary>
        /// <param name="documentoID">Id de la pagaduria</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        [OperationContract]
        List<DTO_MasterBasic> ccChequeoLista_GetByDocumento(Guid channel, int documentoID);

        #endregion

        #region coCargoCosto

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="ConceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o null si no existe</returns>
        [OperationContract]
        string coCargoCosto_GetCuentaIDByCargoOper(Guid channel, string ConceptoCargoID, string proyID, string ctoCostoID, string lineaPresID);

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una cuenta o string.Empty si no existe</returns>
        [OperationContract]
        DTO_CuentaValor coCargoCosto_GetCuentaByCargoOper(Guid channel, string conceptoCargoID, string operacionID, string lineaPresID, decimal valor);

        #endregion

        #region coComprobantePrefijo

        /// <summary>
        /// Trae el identificador de un comprobante segun el documento y el prefijo
        /// </summary>
        /// <param name="documentoIDExt">Identificador del documento de la PK</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="compAnulacion">Indica si debe traer el comprobante de anulacion</param>
        /// <returns>Retorna el identificador de un comprobante o null si no existe</returns>
        [OperationContract]
        string coComprobantePrefijo_GetComprobanteByDocPref(Guid channel, int coDocumentID, int documentoIDExt, string prefijoID, bool compAnulacion);

        #endregion

        #region coDocumento

        /// <summary>
        /// Dice si un prefijo ya esta asignado en la tabla coDocumento
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna Verdadero si el prefijo ya existe, de lo contrario retorna falso</returns>
        [OperationContract]
        bool coDocumento_PrefijoExists(Guid channel, string prefijoID);

        #endregion

        #region coPlanCuenta

        /// <summary>
        /// Trae una lista de tarifas para una cuenta
        /// </summary>
        /// <param name="impTipoID">Tipo de impuesto</param>
        [OperationContract]
        List<decimal> coPlanCuenta_TarifasImpuestos(Guid channel);

        /// <summary>
        /// Trae la cuenta de la cuenta alterna
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna un registro de la cuenta alterna</returns>
        [OperationContract]
        DTO_coPlanCuenta coPlanCuenta_GetCuentaAlterna(Guid channel, int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros);

        /// <summary>
        /// Cuenta la cantidad de resultados dado un filtro
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        [OperationContract]
        long coPlanCuenta_CountChildren(Guid channel, int documentID, UDT_BasicID parentId, string idFilter, string descFilter, bool? active,
            List<DTO_glConsultaFiltro> filtros);

        /// <summary>
        /// Trae los registros de un plan de cuentas corporativo 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de un plan de cuentas corporativo</returns>
        [OperationContract]
        IEnumerable<DTO_MasterHierarchyBasic> coPlanCuenta_GetPagedChildren(Guid channel, int documentID, int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId,
            string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros);

        #endregion

        #region coPlanillaConsolidacion

        /// <summary>
        /// Trae la lista de empresas a consilidar
        /// </summary>
        /// <returns>Retorna la lista de empresas a consilidar</returns>
        [OperationContract]
        List<DTO_ComprobanteConsolidacion> coPlanillaConsolidacion_GetEmpresas(Guid channel);

        #endregion

        #region glActividadFlujo

        /// <summary>
        /// Obtiene la lista de tareas de un documento
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <returns>Retorna la lista de tareas</returns>
        [OperationContract]
        List<string> glActividadFlujo_GetActividadesByDocumentID(Guid channel, int documentoID);

        /// <summary>
        /// Obtiene la lista de padres de una actividad de flujo
        /// </summary>
        /// <param name="actFlujoID">Actividad hija</param>
        /// <returns>Retorna la lista de tareas</returns>
        [OperationContract]
        List<DTO_glActividadFlujo> glActividadFlujo_GetParents(Guid channel, string actFlujoID);

        #endregion

        #region glActividadPermiso

        /// <summary>
        /// Consulta un listado de tareas de glTareaPermiso para usuarios
        /// </summary>
        /// <returns>Listado de TareaID(int)</returns>
        [OperationContract]
        List<string> glActividadPermiso_GetActividadesByUser(Guid channel);

        #endregion

        #region glEmpresa

        /// <summary>
        /// Trae la imagen del logo de la empresa
        /// </summary>
        /// <param name="empresaId">Id de la empresa</param>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        [OperationContract]
        byte[] glEmpresaLogo(Guid channel, DTO_glEmpresa empresa);

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="empresa">Empresa sobre la que se esta trabajando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult glEmpresa_Add(Guid channel, int documentoID, byte[] bItems, int accion);

        /// <summary>
        ///  Elimina una empresa
        /// </summary>
        /// <param name="empresaDel">Empresa que de desea eliminar</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="empresa">Empresa sobre la que se esta trabajando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult glEmpresa_Delete(Guid channel, int documentoID, DTO_glEmpresa empresaDel);

        #endregion

        #region glEmpresaGrupo

        /// <summary>
        /// Agrega un nuevo grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        [OperationContract]
        bool glEmpresaGrupo_Add(Guid channel, byte[] bItems);

        /// <summary>
        /// Elimina un grupo de empresas
        /// </summary>
        /// <param name="grupo">Grupo de embresas</param>
        /// <param name="egCopia">Grupo de empresas del cual se saca la copia</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transacción</param>
        /// <returns></returns>
        [OperationContract]
        bool glEmpresaGrupo_Delete(Guid channel, string egID);

        #endregion

        #region glActividadChequeoLista
        /// <summary>
        /// Trae la lista de chequeos de un flujo
        /// </summary>
        /// <param name="actividadFlujo">Flujo</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        [OperationContract]
        List<DTO_MasterBasic> glActividadChequeoLista_GetByActividad(Guid channel, string actividadFlujo);

        #endregion

        #region glTabla

        /// <summary>
        /// Retorna todas las gl tablas de un grupo de empresas
        /// </summary>
        /// <param name="empGrupo">Nombre del grupo de empresas</param>
        /// <returns>Lista de glTabla</returns>
        [OperationContract]
        IEnumerable<DTO_glTabla> glTabla_GetAllByEmpresaGrupo(Guid channel, Dictionary<int, string> empGrupo, bool jerarquicaInd);

        /// <summary>
        /// Indica si una tabla tiene datos para un grupo empresa
        /// </summary>
        /// <param name="tablaNombre">Nombre de la tabla</param>
        /// <param name="empGrupo">Empresa Grupo</param>
        /// <returns>True si tiene datos, False si no tiene datos</returns>
        [OperationContract]
        bool glTabla_HasData(Guid channel, string tablaNombre, string empresaGrupo);

        /// <summary>
        /// Retorna la info de una tabla segun el nombre y el grupo de empresas
        /// </summary>
        /// <param name="tablaNombre">Tabla Nombre</param>
        /// <param name="empGrupo">Grupo de empresas</param>
        /// <returns>Retorna la informacion de una tabla</returns>
        [OperationContract]
        DTO_glTabla glTabla_GetByTablaNombre(Guid channel, string tablaNombre, string empGrupo);

        #endregion

        #region glTasaDeCambio

        /// <summary>
        /// Obtiene el la tasa de cambio
        /// </summary>
        /// <param name="monedaID">Identificador de la moneda</param>
        /// <param name="fecha">Fecha</param>
        /// <returns>Retorna la tasa de canbio</returns>
        [OperationContract]
        decimal TasaDeCambio_Get(Guid channel, string monedaID, DateTime fecha);

        #endregion

        #region seDelegacionTarea

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        [OperationContract]
        List<DTO_seDelegacionHistoria> seDelegacionHistoria_Get(Guid channel, string userId);

        /// <summary>
        /// Agrega una delegacion
        /// </summary>
        /// <param name="del">Delegacion</param>
        [OperationContract]
        bool seDelegacionHistoria_Add(Guid channel, int documentID, DTO_seDelegacionHistoria del);

        /// <summary>
        /// Actualiza el estado de un delegado
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="enabled">Nuevo estado</param>
        [OperationContract]
        bool seDelegacionHistoria_UpdateStatus(Guid channel, int documentID, string userID, DateTime fechaIni, bool enabled);

        #endregion

        #region seGrupoDocumento

        /// <summary>
        /// Trae la lista de seguridades de documentos de un usuario dada la empresa
        /// </summary>
        /// <param name="userId">Codigo de seguridad del usuario</param>
        /// <param name="userEmpDef">Empresa</param>
        /// <param name="isGroupActive">Si el grupo de seguridad esta activo</param>
        /// <returns>Retorna las seguridades de un usuario en una empresa</returns>
        [OperationContract]
        IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByUsuarioId(Guid channel, string userEmpDef, int userId, bool isGroupActive);

        /// <summary>
        /// Obtiene las seguridades del sistema para un grupo dado el modulo y el tipo de documento
        /// </summary>
        /// <param name="grupo">Grupo de seguridades</param>
        /// <param name="tipo">Tipo de documento</param>
        [OperationContract]
        IEnumerable<DTO_seGrupoDocumento> seGrupoDocumento_GetByType(Guid channel, string grupo, string tipo);

        /// <summary>
        /// Actualiza una lista de seguridades
        /// </summary>
        /// <param name="bItems">Lista de seguridades comprimidas</param>
        /// <param name="seUsuario">Identificador del usuario</param>
        /// <returns>Retorna la lista de seguridades comprimidas</returns>
        [OperationContract]
        DTO_TxResult seGrupoDocumento_UpdateSecurity(Guid channel, byte[] bItems, int seUsuario);

        #endregion

        #region seLAN

        /// <summary>
        /// Trae la configuración de una LAN
        /// </summary>
        /// <param name="lan">Nombre de la LAN</param>
        /// <returns>Retorna la configuracion de una LAN</returns>
        [OperationContract]
        DTO_seLAN seLAN_GetLanByID(Guid channel, string lan, int documentID);

        /// <summary>
        /// Trae todas las configuraciones de LAN
        /// </summary>
        /// <returns>Retorna la lista de LANs y sus configuraciones</returns>
        [OperationContract]
        List<DTO_seLAN> seLAN_GetLanAll(Guid channel, int documentID);

        #endregion

        #region seMaquina

        /// <summary>
        /// valida que la maquina pueda ingresar al sistema
        /// </summary>
        /// <param name="pcMAC">MACs posibles</param>
        /// <returns>Retorna verdadero si la maquina tiene permiso</returns>
        /// <returns>Devuelve si el usuarios es valido Usuarios</returns>
        [OperationContract]
        bool seMaquina_ValidatePC(Guid channel, List<string> macs);

        #endregion

        #region seUsuario

        /// <summary>
        /// Trae un usuario valido
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <param name="password">Contraseña de usuario</param>
        /// <returns>Retorna un usuario valido</returns>
        [OperationContract]
        UserResult seUsuario_ValidateUserCredentials(Guid channel, string userId, string password);

        /// <summary>
        /// Trae un usuario
        /// </summary>
        /// <param name="userId">Identificador de usuario</param>
        /// <returns>Returna un usuario</returns>
        [OperationContract]
        DTO_seUsuario seUsuario_GetUserbyID(Guid channel, string userId);

        /// <summary>
        /// Trae un usuario de acuerdo con el id de la replica (pk)
        /// </summary>
        /// <param name="userID">Identificador del usuario (ReplicaID)</param>
        /// <returns>Retorna el Usuario</returns>
        [OperationContract]
        DTO_seUsuario seUsuario_GetUserByReplicaID(Guid channel, int replicaID);

        /// <summary>
        /// Trae la lista de usuarios
        /// </summary>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve la lista de usuarios </returns>
        [OperationContract]
        IEnumerable<DTO_MasterBasic> seUsuario_GetAll(Guid channel, int documentoID, bool? active);

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <param name="oldPwd">Contraseña vieja</param>
        /// <param name="oldPwdDate">Fecha en que fue modificada por ultima vez la contraseña</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        [OperationContract]
        bool seUsuario_UpdatePassword(Guid channel, int userID, string pwd, string oldPwd, string oldPwdDate);

        /// <summary>
        /// Actualiza la contraseña de un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <param name="pwd">Contraseña nueva</param>
        /// <returns>Retorna verdadero si la operación se pudo realizar, de lo contrario falso</returns>
        [OperationContract]
        bool seUsuario_ResetPassword(Guid channel, int userID, string pwd);

        /// <summary>
        /// Devuelve las empresas a las que tiene permiso un usuario
        /// </summary>
        /// <param name="userID">Identificador del usuario</param>
        /// <returns>Retorna una lista de empresas</returns>
        [OperationContract]
        IEnumerable<DTO_glEmpresa> seUsuario_GetUserCompanies(Guid channel, string userID);

        /// <summary>
        ///  Adiciona una lista de empresas
        /// </summary>
        /// <param name="bItems">Lista de empresas</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="empresa">Empresa sobre la que se esta trabajando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult seUsuario_Add(Guid channel, int documentoID, byte[] bItems, int seUsuario, int accion);

        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="usr">registro donde se realiza la acción</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <returns>Resultado TxResult</returns>
        [OperationContract]
        DTO_TxResult seUsuario_Update(Guid channel, int documentoID, DTO_seUsuario usr, int seUsuario);

        #endregion

        #endregion

        #region glActividadControl

        /// <summary>
        /// Función para consultar trazabilidad tabla gl_TareasControl
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_glActividadControl> glActividadControl_Get(Guid channel, int numeroDoc);

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
        [OperationContract]
        List<DTO_InfoTarea> glActividadEstado_GetPendientesByParameter(Guid channel, int? numeroDoc, int? documentoID, string actFlujoID, DateTime? fechaIni,
            DateTime? fechaFin, string terceroID, string prefijoID, int? docNro, EstadoTareaIncumplimiento tipo, bool llamadaInd, bool? vencidas);

        /// <summary>
        /// Agrega registros a actividadEstado
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="notas">lista de notas</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult glActividadEstado_AddNotas(Guid channel,int documentID, List<DTO_InfoTarea> notas);

        /// <summary>
        /// DEvuelve el flujo de un documento a un estado o flujo anterior
        /// </summary>
        /// <param name="documentActividad">Documento de la transaccion</param>
        /// <param name="actFlujoNueva">Actividad destino de devolucion</param>
        /// <param name="numeroDoc">id del documento</param>
        /// <param name="observacion">Observacion del proceso</param>
        [OperationContract]
        DTO_TxResult DevolverFlujoDocumento(Guid channel, int documentActividad, string actFlujoNueva, int? numeroDoc, string observacion);
        #endregion

        #region glGarantiaControl

        /// <summary>
        /// Agrega un registro al control de garantias
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="garantias">data a guardar o actualizar</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult glGarantiaControl_Add(Guid channel, int documentID, List<DTO_glGarantiaControl> garantias);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> Filtro</param>
        /// <param name="prefijoID"> Prefijo filtro</param>
        /// <param name="docNro"> Doc nro filtro</param>
        /// <param name="estado"> Estado de la garantia</param>
        /// <returns>Lista </returns>
        [OperationContract]
        List<DTO_glGarantiaControl> glGarantiaControl_GetByParameter(Guid channel, DTO_glGarantiaControl filter, string prefijoID, int? docNro, byte? estado);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"> Filtro</param>
        /// <param name="prefijoID"> Prefijo filtro</param>
        /// <param name="docNro"> Doc nro filtro</param>
        /// <param name="estado"> Estado de la garantia</param>
        /// <returns>Lista </returns>
        [OperationContract]
        List<DTO_QueryGarantiaControl> glGarantiaControl_Decisor(Guid channel, int numerodoc);
        #endregion

        #region glIncumpleCambioEstado

        /// <summary>
        /// Agrega info a glIncumpleCambioEstado
        /// </summary>
        /// <param name="documentID">documento Actual</param>
        /// <param name="gestionList">gestionList</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult glIncumpleCambioEstado_Update(Guid channel, int documentID, List<DTO_GestionCobranza> gestionList);

        #endregion

        #region glControl

        /// <summary>
        /// Actualiza glControl
        /// </summary>
        /// <param name="control">control</param> 
        /// <returns>Retorna una respuesta TxResult</returns>
        [OperationContract]
        DTO_TxResult glControl_Update(Guid channel, DTO_glControl control);

        /// <summary>
        /// Trae todos los glControl de una empresa
        /// </summary>
        /// <param name="isBasic">Indica si solo trae la informacion basica</param>
        /// <param name="numEmpresa">Numero de control de una empresa</param>
        /// <returns>enumeracion de glControl</returns>
        [OperationContract]
        IEnumerable<DTO_glControl> glControl_GetByNumeroEmpresa(Guid channel, bool isBasic, string numEmpresa);

        /// <summary>
        ///  Trae una fila de la tabla de control de acuerdo a un id
        /// </summary>
        /// <param name="controlId">ID de control</param>
        /// <returns>dto control encontrado</returns>
        [OperationContract]
        DTO_glControl glControl_GetById(Guid channel, int controlId);

        /// <summary>
        /// Actualiza una lista de registros 
        /// </summary>
        /// <param name="data">Diccionario con la lista de datos "Llave,Valor"</param>
        [OperationContract]
        void glControl_UpdateModuleData(Guid channel, Dictionary<string, string> data);

        #endregion

        #region glDocAnexoControl

        /// <summary>
        /// Retorna la lista de anexos de un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <returns>Retorna la lista de anexos</returns>
        [OperationContract]
        List<DTO_glDocAnexoControl> glDocAnexoControl_GetAnexosByNumeroDoc(Guid channel, int numeroDoc);

        /// <summary>
        /// Retorna un anexo de un documento
        /// </summary>
        /// <param name="">replica</param>
        /// <returns>Retorna un anexo</returns>
        [OperationContract]
        DTO_glDocAnexoControl glDocAnexoControl_GetAnexosByReplica(Guid channel, int replica);

        /// <summary>
        /// Actualiza los anexos de un documento
        /// </summary>
        /// <param name="mod">Modulo que guarda los anexos</param>
        /// <param name="list">lista de anexos</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult glDocAnexoControl_Update(Guid channel, ModulesPrefix mod, List<DTO_glDocAnexoControl> list);


        #endregion

        #region glDocumentoControl

        /// <summary>
        /// Anula un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        [OperationContract]
        DTO_TxResult glDocumentoControl_Anular(Guid channel, int documentID, List<int> numeroDoc);

        /// <summary>
        /// Revierte un documento
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la operacion</param>
        /// <param name="numeroDoc">Pk del documento a anular</param>
        /// <returns>Retorna el resultado</returns>
        [OperationContract]
        DTO_TxResult glDocumentoControl_Revertir(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Le cambia el estado a un documentoControl y guarda en la bitacora
        /// </summary>
        /// <param name="documentoID">Documento que esta ejecutando la transaccion (TAREA ACTUAL) y del cual se busca la siguiente tarea</param>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) de glDocumentoControl</param>
        /// <param name="estado">Nuevo estado</param>
        /// <param name="obs">Observaciones</param>
        /// <returns>Retorna el identificador de la bitacora con que se guardo la info</returns>
        [OperationContract]
        int glDocumentoControl_ChangeDocumentStatus(Guid channel, int documentoID, int numeroDoc, EstadoDocControl estado, string obs);

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetByID(Guid channel, int numeroDoc);

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetInternalDoc(Guid channel, int documentId, string idPrefijo, int numeroDoc);

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetExternalDoc(Guid channel, int documentId, string idTercero, string DocumentoTercero);

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Nunero de documento</param>
        /// <returns></returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetInternalDocByCta(Guid channel, string cuentaID, string idPrefijo, int numeroDoc);

        /// <summary>
        /// Trae un registro de glDocuemntoControl
        /// </summary>
        /// <param name="idTercero">Identificador del tercero</param>
        /// <param name="DocumentoTercero">Documento del tercero</param>
        /// <returns>Retorna el documento</returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetExternalDocByCta(Guid channel, string cuentaID, string idTercero, string DocumentoTercero);

        /// <summary>
        /// Trae un documento relacionado a un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Identificador del comprobante</param>
        /// <param name="compNro">Numeor de comprobante</param>
        /// <param name="estado">Estado del comprobante</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Retorna </returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetByComprobante(Guid channel, int documentId, DateTime periodo, string comprobanteID, int compNro);

        /// <summary>
        /// Obtiene el documento relacionado con una libranza
        /// </summary>
        /// <param name="libranza">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="cerradoInd">Indica si trae la actividad con estado cerrado o abierto</param>
        /// <returns>Documento</returns>
        [OperationContract]
        DTO_glDocumentoControl glDocumentoControl_GetByLibranzaSolicitud(Guid channel, int libranza, string actFlujoID, bool cerradoInd);

        /// <summary>
        /// Trae la info de los documento s para generarles el posteo de comprobantes
        /// </summary>
        /// <param name="mod">Modulo del cual se van a traer el listado de documentos</param>
        /// <param name="contabilizado">Indica si trae los documentos que ya fueron procesados</param>
        /// <returns>Retorna el listado de documentos</returns>
        [OperationContract]
        List<DTO_glDocumentoControl> glDocumentoControl_GetForPosteo(Guid channel, ModulesPrefix mod, bool contabilizado);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrl">Doc Control filtro</param>
        /// <returns>Lista de Doc Control </returns>
        [OperationContract]
        List<DTO_glDocumentoControl> glDocumentoControl_GetByParameter(Guid channel, DTO_glDocumentoControl ctrl);

        #endregion

        #region glLlamadasControl

        /// <summary>
        /// Trae la lista de glLlamadasControl
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        /// <returns>Retorna un listado de glLlamadasControl </returns>
        [OperationContract]
        List<DTO_glLlamadasControl> glLlamadasControl_GetByID(Guid channel, int numeroDoc);

        /// <summary>
        /// Agrega un registro en glLlamadasControl
        /// </summary>
        /// <param name="documentoID">Documento ID</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="llamadasCtrl">Lista de las llamdas realizadas</param>
        /// <param name="terceroRefs">Lista de las referencias</param>
        /// <param name="sendToAprob">Indicador para establecer si se guarda el registro y se asigna la actividad de flujo o no</param>
        /// <returns>Retorna el resultado de la consulta</returns>
        [OperationContract]
        DTO_TxResult glLlamadasControl_Add(Guid channel, int documentoID, string actividadFlujoID, List<DTO_glLlamadasControl> llamadasCtrl, List<DTO_glTerceroReferencia> terceroRefs, bool sendToAprob);

        #endregion

        #region glMovimientoDeta

        /// <summary>
        /// Obtiene la cantidad de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna la cantidad de registros de la consulta</returns>
        [OperationContract]
        long glMovimientoDeta_Count(Guid channel, DTO_glConsulta consulta);

        /// <summary>
        /// Obtiene una lista de registros de la tabla glMovimientoDeta
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de la pagina de consulta</param>
        /// <param name="consulta">Filtros</param>
        /// <returns>Retorna los registros de la consulta</returns>
        [OperationContract]
        List<DTO_glMovimientoDeta> glMovimientoDeta_GetPaged(Guid channel, int pageSize, int pageNum, DTO_glConsulta consulta);

        /// <summary>
        /// Trae la lista de glMovimientoDeta Con campos especificos para los activos
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_glMovimientoDeta> glMovimientoDeta_GetBy_ActivoFind(Guid channel, int numeroDoc);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Mvto detalle</returns>
        [OperationContract]
        List<DTO_glMovimientoDeta> glMovimientoDeta_GetByParameter(Guid channel, DTO_glMovimientoDeta filter, bool isPre);

        /// <summary>
        /// Consulta un movimientoDetaPRE relacionado con proyectos con saldos de Inventario
        /// </summary>
        /// <param name="periodo">Periodo de saldos de inventarios</param>
        /// <param name="bodega">Bodega a consultar</param>
        /// <param name="proyectoID">Proyecto a consultar</param>
        /// <returns>lista de movimientos</returns>
        [OperationContract]
        List<DTO_glMovimientoDeta> glMovimientoDetaPRE_GetSaldosInvByProyecto(Guid channel, DateTime periodo, string proyectoID,bool isPre);

        #endregion

        #region glDocumentoChequeoLista
        /// <summary>
        /// Retorna el Listado 
        /// </summary>
        /// <returns></returns> 
        [OperationContract]
        List<DTO_glDocumentoChequeoLista> glDocumentoChequeoLista_GetByNumeroDoc(Guid channel, int numeroDoc);
        #endregion

        #region Consultas

        #region glConsultaFiltro

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        [OperationContract]
        IEnumerable<DTO_glConsultaFiltro> glConsultaFiltro_GetAll(Guid channel, DTO_glConsultaFiltro filtro);

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        [OperationContract]
        DTO_glConsultaFiltro glConsultaFiltro_Get(Guid channel, DTO_glConsultaFiltro filtro);

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        [OperationContract]
        DTO_glConsultaFiltro glConsultaFiltro_Add(Guid channel, DTO_glConsultaFiltro filtro);

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        [OperationContract]
        void glConsultaFiltro_Delete(Guid channel, DTO_glConsultaFiltro filtro);

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        [OperationContract]
        DTO_glConsultaFiltro glConsultaFiltro_Update(Guid channel, DTO_glConsultaFiltro filtro);

        #endregion

        #region glConsulta

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        [OperationContract]
        IEnumerable<DTO_glConsulta> glConsulta_GetAll(Guid channel, DTO_glConsulta filtro, int? userID);

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        [OperationContract]
        DTO_glConsulta glConsulta_Add(Guid channel, DTO_glConsulta filtro);

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        void glConsulta_Delete(Guid channel, DTO_glConsulta filtro);

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        [OperationContract]
        DTO_glConsulta glConsulta_Update(Guid channel, DTO_glConsulta filtro);

        #endregion

        #region glConsultaSeleccion

        /// <summary>
        /// Trae todos los glConsulta
        /// </summary>
        /// <param name="filtro">Informacion del filtro de la consulta</param>
        /// <returns>Devuelve los glConsulta a partir de un filtro aplicado</returns>
        [OperationContract]
        IEnumerable<DTO_glConsultaSeleccion> glConsultaSeleccion_GetAll(Guid channel, DTO_glConsultaSeleccion filtro);

        /// <summary>
        /// Obtiene una consulta especifica
        /// </summary>
        /// <param name="filtro">Informacion para fitrar la consulta</param>
        /// <returns>consulta</returns>
        [OperationContract]
        DTO_glConsultaSeleccion glConsultaSeleccion_Get(Guid channel, DTO_glConsultaSeleccion filtro);

        /// <summary>
        /// Adicionar una nueva consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta adicionada</returns>
        [OperationContract]
        DTO_glConsultaSeleccion glConsultaSeleccion_Add(Guid channel, DTO_glConsultaSeleccion filtro);

        /// <summary>
        /// Eliminar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a adicionar</param>
        /// <returns>consulta eliminada</returns>
        [OperationContract]
        void glConsultaSeleccion_Delete(Guid channel, DTO_glConsultaSeleccion filtro);

        /// <summary>
        /// Actualizar una  consulta
        /// </summary>
        /// <param name="filtro">consulta a actualizar</param>
        /// <returns>consulta actualizada</returns>
        [OperationContract]
        DTO_glConsultaSeleccion glConsultaSeleccion_Update(Guid channel, DTO_glConsultaSeleccion filtro);

        #endregion

        #region Consultas Generales

        /// <summary>
        /// Consultas generales
        /// </summary>
        /// <param name="vista">Nombre de la vista</param>
        /// <param name="dtoType">Tipo de DTO</param>
        /// <param name="consulta">Consulta con filtros</param>
        /// <returns></returns>
        [OperationContract]
        DataTable ConsultasGenerales(Guid channel, string vista, Type dtoType, DTO_glConsulta consulta);

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
        [OperationContract]
        Dictionary<string, string> GetOptionsControl(Guid channel, int docId, string columnName, string llave);


        /// <summary>
        /// Genera un reporte
        /// </summary>
        /// <param name="documentID">documento con el cual se salva el archivo</param>
        /// <param name="numeroDoc">Numero del documento con el cual se salva el archivo>
        [OperationContract]
        void GenerarReportOld(Guid channel, int documentID, int numeroDoc);
        #endregion

        #endregion
    }
}
