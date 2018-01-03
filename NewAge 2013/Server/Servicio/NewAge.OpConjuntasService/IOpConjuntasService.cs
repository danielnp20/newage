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
using NewAge.DTO.Negocio.Reportes;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.Server.OpConjuntasService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface IOpConjuntasService
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

        #region OpConjuntas

        #region Billing

        /// <summary>
        /// Proceso para hacer la particion del Billing
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Billing_Particion(Guid channel, int documentID, string actividadFlujoID);

        /// <summary>
        /// Procesa el billing y generas los comprobantes Gross
        /// </summary>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="periodo">Periodo del billing</param>
        /// <returns>Retirna al resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult ProcesarBilling(Guid channel, int documentID, string actividadFlujoID, DateTime periodo);

        #endregion

        #region CashCall

        /// <summary>
        /// Genera el proceso de cash call
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult CashCall_Procesar(Guid channel, int documentID, string actividadFlujoID, DateTime periodoID);

        #endregion

        #region Distribucion

        /// <summary>
        /// Distribuye entre los socios de acuerdo a los porcentajes dados
        /// </summary>
        /// <param name="periodo">PeriodoID</param>
        [OperationContract]
        DTO_TxResult Proceso_ocDetalleLegalizacion_Distribucion(Guid channel, DateTime periodo);

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
        [OperationContract]
        List<DTO_QueryInformeMensualCierre> ocDetalleLegalizacion_GetInfoMensual(Guid channel, int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen);

        #endregion

        #endregion

        #region Planeacion

        #region Presupuesto

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="loadTareas">Carga las tareas del proyecto</param>
        /// <param name="loadDetalleTarea">carga el detalle por tarea</param>
        /// <param name="loadDetalle">carga el detalle por proyecto</param>
        /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_QueryEjecucionPresupuestal> plEjecucionPresupuestal(Guid channel, DateTime fechaCorte);

        /// <summary>
        ///  Obtiene Indicadores
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_QueryIndicadores> plIndicadores(Guid channel, DateTime fechaCorte);

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Presupuesto consolidado</returns>
        [OperationContract]
        DTO_Presupuesto Presupuesto_GetConsolidadoTotal(Guid channel, int documentID, string proyectoID, DateTime periodoID, byte proyectoTipo, string contratoID, string actividadID, string campo);

        /// <summary>
        /// Trae la informacion de los presupuestos de un usuario por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DTO_Presupuesto Presupuesto_GetNuevo(Guid channel, int documentoID, string proyectoID, DateTime periodoID, bool orderByAsc);

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
        [OperationContract]
        DTO_SerializedObject Presupuesto_Add(Guid channel, int documentoID, DateTime periodoPresupuesto,string proyectoID, decimal tc, DTO_Presupuesto presupuesto,bool onlySave);

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_PresupuestoAprob> Presupuesto_GetNuevosForAprob(Guid channel,int documentoID, string actividadFlujo);

        /// <summary>
        /// Trae la informacion de los presupuestos para aprobar por proyecto y periodo
        /// </summary>
        /// <param name="areaPresupuestalID">Area presupuestal</param>
        /// <param name="centroCostoID">Centro de costo</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_PresupuestoDetalle> Presupuesto_GetNuevosForAprobDetails(Guid channel, string proyectoID, DateTime periodoID, string areaPresupuestalID, string centroCostoID);

        /// <summary>
        /// Aprueba una lista de presupuestos
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="proyectoID">Identificador del proyecto</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <param name="areasIDs">Lista de areas para aprobar</param>
        /// <returns>Retorna una lista de resultados o  alarmas</returns>
        [OperationContract]
        List<DTO_SerializedObject> Presupuesto_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_PresupuestoAprob> docs);

        /// <summary>
        /// Trae la informacion de los presupuestos consolidados con un filtro
        /// </summary>
        /// <returns>Lista de presupuestos</returns>
        [OperationContract]
        List<DTO_plPresupuestoTotal> plPresupuestoTotal_GetByParameter(Guid channel, DTO_plPresupuestoTotal filter);

        /// <summary>
        ///  Trae la informacion de planeacion proveedores
        /// </summary>
        /// <param name="consActLinea"> Consecutivo filtro</param>
        /// <returns>Lista</returns>
        [OperationContract]
        List<DTO_plPlaneacion_Proveedores> plPlaneacion_Proveedores_GetByConsActLinea(Guid channel, int consActLinea);

        /// <summary>
        /// Aprueba Un Presupuesto contable
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="numeroDoc">Identificador del doc</param>
        /// <param name="periodoID">Periodo del presupuesto</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult PresupuestoContable_Aprobar(Guid channel, int documentID, int numeroDoc, DateTime periodoDoc);

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
        [OperationContract]
        DTO_SerializedObject PresupuestoPxQ_Add(Guid channel, int documentoID, DateTime periodoPresupuesto, string proyectoID, decimal tc, DTO_Presupuesto presupuesto, bool onlySave);

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
        [OperationContract]
        DTO_Presupuesto PresupuestoPxQ_GetPresupuestoPxQConsolidado(Guid channel, int documentID, string proyecto, DateTime periodo, byte proyectoTipo, string contratoID, string actividadID, string areaFisica, byte estadoDocCtrl);

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
        [OperationContract]
        DTO_Presupuesto PresupuestoPxQ_GetDataPxQ(Guid channel, int documentID, byte tipoProyecto, string proyectoID, DateTime periodo, string contratoID, string actividadID, string areaFisicaID, string lineaPresupID, string recursoID);

        /// <summary>
        /// Envia para aprobacion 
        /// </summary>
        /// <param name="currentMod">Modulo que esta ejecutando la operacion</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="userId">Usuario que ejecuta la transaccion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_SerializedObject PresupuestoPxQ_SendToAprob(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de Planeacion
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Planeacion_Reversion(Guid channel, int documentID, int numeroDoc);
            
        #endregion

        #region Procesos

        /// <summary>
        /// Ejecutar proceso de cierre Legalizacion en Planeacion 
        /// </summary>
        /// <param name="periodo">Periodo de Cierre</param>
        [OperationContract]
        DTO_TxResult Proceso_plCierreLegalizacion_Cierre(Guid channel, DateTime periodo);

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
        [OperationContract]
        List<DTO_QueryInformeMensualCierre> plCierreLegalizacion_GetInfoMensual(Guid channel, int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen);

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de Estado de presupuesto por periodo
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        [OperationContract]
        List<DTO_QueryEstadoEjecucion> plCierreLegalizacion_GetEstadoEjecByPeriodo(Guid channel, int documentID, DTO_QueryEstadoEjecucion filter, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen);
        
        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de plSobreEjecucion
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="estado">Estado de los documentos</param>
        /// <param name="areaAprob">Area de aprobacion</param>
        /// <returns>Lista de informe </returns>
        [OperationContract]
        List<DTO_plSobreEjecucion> plSobreEjecucion_GetOrdenCompraSobreEjec(Guid channel,int documentID, int estado, string areaAprob);
        #endregion

        #endregion

        #region Proyectos

        #region Solicitud Proyecto

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        [OperationContract]
        DTO_pyPreProyectoDocu pyPreProyectoDocu_Get(Guid channel, int numeroDoc);

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
        [OperationContract]
        DTO_TxResult SolicitudProyecto_Add(Guid channel, int documentoID, ref int numeroDoc, string claseServicioID, string areaFuncional, string prefijo, string proyectoID, string centroCto, string observaciones, DTO_SolicitudTrabajo transaccion);

        /// <summary>
        /// Enviar aprobación la solicitud de trabajo
        /// </summary>
        /// <param name="documentID">documentoID</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject SolicitudProyecto_AprobarProy(Guid channel, int documentID, DTO_SolicitudTrabajo transaccion, DateTime fechaInicio);

        /// <summary>
        /// Proceso que guarda un detalle del Cliente
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="numeroDoc">numero doc</param>
        /// <param name="transaccion">info del proyecto</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult SolicitudProyecto_AddAPUCliente(Guid channel, int documentoID, int numeroDoc, DTO_SolicitudTrabajo transaccion, bool saveProyectoInd);

        /// <summary>
        /// Trae un registro de glDocumentoControl
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="idPrefijo">Identificador del prefijo</param>
        /// <param name="documentoNro">Numero de documento</param>
        /// <param name="actividadFlujoID">Identificador del Documento</param>
        /// <returns></returns>
        [OperationContract]
        DTO_glDocumentoControl pyServicioDocu_GetInternalDoc(Guid channel, int documentID, string idPrefijo, int documentoNro, string actividadFlujoID);

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="loadDetaInd">Indica si carga detalle de un documento</param>
        /// <returns>Lista de componentes</returns>
        [OperationContract]
        List<DTO_pyPreProyectoDeta> pyPreProyectoDeta_GetByParameter(Guid channel, int documentID, string tareaID, string claseServidioID, int? numeroDoc, bool loadDetaInd, decimal tasaCambio);

        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="tarea">tarea a analizar</param>
        /// <returns>Lista de resultados</returns>
        [OperationContract]
        List<DTO_pyPreProyectoTarea> pyPreProyectoTarea_Get(Guid channel, int? numeroDoc, string tareaID, string claseServicioID);

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
        [OperationContract]
        DTO_SolicitudTrabajo SolicitudProyecto_Load(Guid channel, int documentoID, string prefijoID, int? docNro, int? numeroDoc, string claseServicioID, 
                                                    string proyectoID, bool isPreProyecto, bool loadMvtos, bool loadActas,bool loadAPUCliente, bool loadTrazabilidad);

        #endregion

        #region pyProyectoDocu

        /// <summary>
        /// Obtiene el detalle del documento
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        [OperationContract]
        DTO_pyProyectoDocu pyProyectoDocu_Get(Guid channel, int numeroDoc);

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
        [OperationContract]
        List<DTO_pyProyectoTarea> pyProyectoTarea_Get(Guid channel, int? numeroDoc, string tareaID, string claseServicioID, bool recursoTimeInd);

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
        [OperationContract]
        List<DTO_pyProyectoDeta> pyProyectoDeta_GetByParameter(Guid channel, int documentID, string tareaID, string claseServicioID, int? numeroDoc, bool loadDetaExist, bool recursoTimeInd, decimal tasaCambio);

        #endregion

        #region pyProyectoMvto
        /// <summary>
        /// Actualiza la tabla de mvtos
        /// </summary>
        /// <param name="numeroDoc">numero Documento</param>
        /// <returns>Detalle Documento</returns>
        [OperationContract]
        DTO_SerializedObject pyProyectoMvto_Upd(Guid channel, int documentID, List<DTO_pyProyectoMvto> listMvtos, bool SendToSolicitudInd);

        /// <summary>
        /// Obtiene el consolidado de documentos relacionados al proyecto
        /// </summary>
        /// <param name="consProyMvto">identificador del mvto de proyecto</param>
        /// <returns>Documentos</returns>
        [OperationContract]
        List<DTO_glDocumentoControl> pyProyectoMvto_GetDocsAnexo(Guid channel, int? consProyMvto);

        ///<summary>
        /// Trae los mvtos con el filtro
        /// </summary>
        /// <param name="filter">filtro adecuado</param>
        /// <returns>Detalle mvtos</returns>
        [OperationContract]
        List<DTO_pyProyectoMvto> pyProyectoMvto_GetParameter(Guid channel, DTO_pyProyectoMvto filter);

        ///<summary>
        /// Trae un mvto de proyecto por consecutivo
        /// </summary>
        /// <param name="consecMvto">filtro consec</param>
        /// <returns>mvto</returns>
        [OperationContract]
        DTO_pyProyectoMvto pyProyectoMvto_GetByConsecutivo(Guid channel, int? consecMvto);
        #endregion

        #region Planeacion Compras

        /// <summary>
        /// Aprueba los recursos para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="listMvtos">Lista de recursos a aprobar</param>
        /// <returns>Result</returns>
        [OperationContract]
        DTO_SerializedObject CompraRecursos_ApproveSolicitudOC(Guid channel, int documentID, List<DTO_pyProyectoMvto> listMvtos);

        /// <summary>
        /// Obtiene los recursos pendientes para  crear una Solicitud de Orden de Compra
        /// </summary>
        /// <returns>Lista de</returns>
        [OperationContract]
        List<DTO_pyProyectoMvto> CompraRecursos_GetPendientesForApprove(Guid channel, DateTime fechaTope);

        #endregion

        #region Acta de Trabajo

        /// <summary>
        /// Aprueba el Acta de Trabajo y genera un recibido de Proveedores
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="proyecto">Lista de actas a aprobar</param>
        /// <returns>Result</returns>
        [OperationContract]
        DTO_SerializedObject ActaTrabajo_ApproveRecibidoBS(Guid channel, int documentID, DTO_SolicitudTrabajo proyecto, DTO_glDocumentoControl ctrlActa);

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="actasList">lista de actas</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject ActaTrabajo_Add(Guid channel, int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyActaTrabajoDeta> actasList, DTO_coProyecto proy, bool update);

        /// <summary>
        /// Obtiene las actas de trabajo
        /// </summary>
        /// <param name="numeroDoc">identificador doc</param>
        /// <returns>Objeto con todos los datos</returns>
        [OperationContract]
        List<DTO_pyActaTrabajoDeta> ActasTrabajo_Load(Guid channel, int? numeroDoc);

        #endregion

        #region Otras
        /// <summary>
        /// Trae las tareas(pyTarea) con un filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista</returns>
        [OperationContract]
        List<DTO_TareasFilter> TareasFilter_Get(Guid channel, DTO_TareasFilter filter);

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">FEcha corte</param>
        /// <param name="loadTareas">Carga las tareas y recursos del proyecto</param>
        /// <param name="loadCompras">carga las compras del proyecto</param>
        /// <param name="loadFinanciero">carga las transacciones financieras</param>
        /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_QueryComiteTecnico> pyProyectoDocu_GetAllProyectos(Guid channel, DateTime fechaCorte, bool loadTareas, bool loadCompras, bool loadFinanciero);

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
        [OperationContract]
        DTO_TxResult EntregasCliente_Add(Guid channel, int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> tareasCliente, List<DTO_pyProyectoTarea> tareas, List<DTO_pyProyectoTarea> tareasAdic,List<int> entregasDelete);

        /// <summary>
        /// Trae las tareas Cliente asignadas a un proyecto
        /// </summary>
        /// <param name="numDocProy">identificador del proyecto</param>
        /// <param name="tareaCliente">tarea Cliente</param>
        /// <param name="desc">descripcion de la tarea</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_pyProyectoTareaCliente> pyProyectoTareaCliente_GetByNumeroDoc(Guid channel, int numDocProy, string tareaCliente, string desc);

        /// <summary>
        /// Agrega un docum de Acta de Trabajo
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="listEntregables">lista de actas</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_SerializedObject ActaEntrega_Add(Guid channel, int documentoID, DTO_glDocumentoControl docCtrl, List<DTO_pyProyectoTareaCliente> listEntregables, DTO_coProyecto proy, bool update);

        /// <summary>
        /// Aprueba un docum de Acta de Entrega
        /// </summary>
        /// <param name="documentoID">documento ID</param>
        /// <param name="docCtrl">doc control</param>
        /// <param name="proy">proyecto actual</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_SerializedObject ActaEntrega_Aprobar(Guid channel, int documentoID, DTO_glDocumentoControl docCtrl, DTO_coProyecto proy);

        /// <summary>
        /// Obtiene las actas de un proyecto con un filtro
        /// </summary>
        /// <param name="filter">filtros</param>
        /// <returns>lista</returns>
        [OperationContract]
        List<DTO_pyActaEntregaDeta> pyActaEntregaDeta_GetByParameter(Guid channel, DTO_pyActaEntregaDeta filter);

        /// <summary>
        /// Acta de entrega
        /// </summary>
        /// <param name="documentID">Doc</param>
        /// <param name="ctrlProy">ctrl</param>
        /// <param name="docuProy">header</param>
        /// <param name="listActaDeta">lista</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_SerializedObject ActaEntrega_ApprovePreFactura(Guid channel, int documentID, DTO_glDocumentoControl ctrlProy, DTO_pyProyectoDocu docuProy, List<DTO_pyProyectoTareaCliente> listActaDeta, List<DTO_faFacturacionFooter> listDetalleFact);

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
        [OperationContract]
        DTO_TxResult EntregasCliente_Delete(Guid channel, int documentoID, int numDocProy, List<DTO_pyProyectoTareaCliente> entregables, bool deleteEntregable, bool deleteProgramacion, bool deleteActas, bool anulaPreFacturas);

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
        [OperationContract]
        DTO_TxResult MigracionInsumos(Guid channel, int documentID, List<DTO_MigracionInsumos> listInsumos, List<DTO_MigracionProveedor> listProveedores, List<DTO_MigracionGrupos> listAnalisis, List<DTO_MigracionAPU> lisAPU);

        /// <summary>
        /// Agrega o actualiza una lista de locaciones y entregas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="listLocaciones">Lista de Locaciones</param>
        /// <param name="listLocTareas">Lista de tareas</param>
        /// <param name="listLocRecursos">Lista de recursos</param>
        /// <param name="listEntregas">Lista de entregas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult MigracionLocacionEntregas(Guid channel, int documentID, int numDocProy, List<DTO_pyProyectoLocacion> listLocaciones, List<DTO_pyProyectoIngDetalleTarea> listLocTareas, List<DTO_pyProyectoIngDetalleDeta> listLocRecursos, List<DTO_pyProyectoEntregasxMes> listEntregas);

        /// <summary>
        /// Trea la lista de locaciones y entregas de un proyecto
        /// </summary>
        /// <param name="numDocProy">Documnto que inicia la tx</param>
        /// <param name="listLocaciones">Lista de Locaciones</param>
        /// <param name="listLocTareas">Lista de tareas</param>
        /// <param name="listLocRecursos">Lista de recursos</param>
        /// <param name="listEntregas">Lista de entregas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult LocacionEntregasGetByProyecto(Guid channel, int documentID, int numDocProy, ref List<DTO_pyProyectoLocacion> listLocaciones, ref List<DTO_pyProyectoIngDetalleTarea> listLocTareas, ref List<DTO_pyProyectoIngDetalleDeta> listLocRecursos, ref List<DTO_pyProyectoEntregasxMes> listEntregas);

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
        [OperationContract]
        DTO_TxResult Proyectos_Revertir(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region pyProyectoModificaFechas
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="aprobadoDocInd">Indica si trae un documento aprobado o sin aprobar</param>
        /// <returns>Lista de dtos</returns>
        [OperationContract]
        List<DTO_pyProyectoModificaFechas> pyProyectoModificaFechas_GetByNumeroDoc(Guid channel, int numeroDoc, string tarea);

        [OperationContract]
        DTO_pyProyectoModificaFechas pyProyectoModificaFechas_Load(Guid channel, int numeroDoc);

        [OperationContract]
        DTO_TxResult pyProyectoModificaFechas_Add(Guid channel, DTO_pyProyectoModificaFechas datos);

        [OperationContract]
        DTO_TxResult pyProyectoModificaFechas_Upd(Guid channel, DTO_pyProyectoModificaFechas datos);


        #endregion

        #endregion
    }

}
