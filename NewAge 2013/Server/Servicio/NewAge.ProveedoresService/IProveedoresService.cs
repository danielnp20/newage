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

namespace NewAge.Server.ProveedoresService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface IProveedoresService
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

        #region Proveedores

        #region DetalleDocu

        /// <summary>
        /// Trae un detalle de proveedores 
        /// </summary>
        /// <param name="detalleID">Identificador del detalle</param>
        /// <returns>Retorna el detalle</returns>
        [OperationContract]
        DTO_prDetalleDocu prDetalleDocu_GetByID(Guid channel, int detalleID);

        /// <summary>
        /// Trae la lista de prDetalleDocu segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <param name="isFactura">Valida si se filtra por identificador de FacturaDocuID</param>
        /// <returns>Lista de detalle</returns>
        [OperationContract]
        List<DTO_prDetalleDocu> prDetalleDocu_GetByNumeroDoc(Guid channel, int NumeroDoc, bool isFactura);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="document">Documento a filtrar</param>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="consecutivoDeta">identificador del detalle si se requiere</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_prDetalleDocu> prDetalleDocu_GetByDocument(Guid channel,int document, int numeroDoc, int consecutivoDeta);
        
        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">Filtro de la tabla</param>
        /// <returns>Lista Dto de Detalle Docu</returns>
        [OperationContract]
        List<DTO_prDetalleDocu> prDetalleDocu_GetParameter(Guid channel, DTO_prDetalleDocu filter);

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
        [OperationContract]
        List<DTO_prDetalleDocu> prDetalleDocu_GetPendienteForCierre(Guid channel, int documentFilter, string prefijoID, int? docNro, string proveedorID, string referenciaID, string codigoBS);

        #endregion

        #region Cierre Detalle
        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="ctrl">referencia a glDocumentoControl</param>
        /// <param name="footer">la lista de detalle</param>
        /// <returns>si la operacion es exitosa</returns>
        [OperationContract]
        DTO_SerializedObject CierreDetalle_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, List<DTO_prDetalleDocu> footer);
        #endregion

        #region Solicitud

        /// <summary>
        /// Carga la informacion completa dela Solicitud
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="facturaNro">Numero del Documento interno</param>
        /// <returns>Retorna la Solicitud</returns>
        [OperationContract]
        DTO_prSolicitud Solicitud_Load(Guid channel, int documentID, string prefijoID, int solNro);

        /// <summary>
        /// Guardar nueva Solicitud y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="header">SolicitudDocu</param>
        /// <param name="footer">SolicitudFooter</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject Solicitud_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_prSolicitudDocu header, DTO_prSolicitudDirectaDocu headerDirecta,List<DTO_prSolicitudFooter> footer, bool update, out int numDoc);
                
        /// <summary>
        /// Manda el documento a aprobacion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject Solicitud_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc);

        /// <summary>
        /// Trae un listado de solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_prSolicitudAprobacion> Solicitud_GetPendientesByModulo(Guid channel, ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario);

        /// <summary>
        /// Trae un listado de solicitudes pendientes para asignar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_prSolicitudAsignacion> Solicitud_GetPendientesForAssign(Guid channel, ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario);
        

        /// <summary>
        /// Recibe una lista de solicitudes para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Solicitud_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_prSolicitudAprobacion> sol, bool createDoc);

        /// <summary>
        /// Recibe una lista de solicitudes para asignar
        /// </summary>
        /// <param name="documentID">documento que relaciona la asignacion</param>
        /// <param name="sol">solicitud que se deben asignar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Solicitud_Asignar(Guid channel, int documentID, string actividadFlujoID, List<DTO_prSolicitudAsignacion> sol);

        /// <summary>
        /// Trae un listado de solicitudes para orden de compra
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_prSolicitudResumen> Solicitud_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, TipoMoneda origenMonet);

        /// <summary>
        /// Trae un listado de las solicitudes direcctas pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes</returns>
        [OperationContract]
        List<DTO_prSolicitudDirectaAprob> SolicitudDirecta_GetPendientesByModulo(Guid channel, int documentoID, string actFlujoID, DTO_seUsuario usuario);

        /// <summary>
        /// Trae la lista de prSolicitudCargos segun el identificador del detalle
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_prSolicitudCargos> prSolicitudCargos_GetByConsecutivoDetaID(Guid channel, int documentID, int ConsecutivoDetaID);

        /// <summary>
        /// Recibe una lista de solicitudes directas para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="actividadFlujoID">actividad del documento</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> SolicitudDirecta_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_prSolicitudDirectaAprob> sol, bool createDoc);
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
        [OperationContract]
        DTO_prOrdenCompra OrdenCompra_Load(Guid channel, int documentID, string prefijoID, int ordenNro, int numeroDoc);

        /// <summary>
        /// Guardar nuevo Orden de Compra y asociar en glDocumentoControl
        /// </summary>
        /// <param name="orden">Data completa</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject OrdenCompra_Guardar(Guid channel, int documentID, DTO_prOrdenCompra orden, bool update, out int numDoc);
                
        /// <summary>
        /// Trae un listado de ordenes de compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>la lista de los ordenes de compra pendientes </returns>
        [OperationContract]
        List<DTO_prOrdenCompraAprob> OrdenCompra_GetPendientesByModulo(Guid channel, int documentID, string actFlujoID, DTO_seUsuario usuario);

        /// <summary>
        /// Recibe una lista de Ordenes de Compra para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="usuario">usuario que aprueba el orden de compra</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> OrdenCompra_AprobarRechazar(Guid channel, int documentID,string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prOrdenCompraAprob> ord, bool createDoc);

        /// <summary>
        /// Trae un listado de ordenes de compra para solicitud
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_prOrdenCompraResumen> OrdenCompra_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, List<Tuple<string, string>> filtros);
        #endregion

        #region prContratoPlanPagos

        /// <summary>
        /// Actualiza la lista de prContratoPlanPago en base de datos
        /// </summary>
        /// <param name="listContPlanPago">la lista de DTO_prContratoPlanPago</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult prContratoPlanPago_Upd(Guid channel, List<DTO_prContratoPlanPago> listContPlanPago, int numeroDoc);
        #endregion

        #region Contrato

        /// <summary>
        /// Trae un listado de los contratos pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes para aprobar</returns>
        [OperationContract]
        List<DTO_prContratoAprob> Contrato_GetPendientesByModulo(Guid channel, int documentID, string actFlujoID, DTO_seUsuario usuario);

        /// <summary>
        /// Recibe una lista de contratos para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="contrato">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Contrato_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prContratoAprob> contrato, bool createDoc);

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
        [OperationContract]
        DTO_SerializedObject Recibido_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_prRecibidoDocu header, List<DTO_prOrdenCompraResumen> footer, out int numDoc, string docTransporte, string manifCarga);

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        [OperationContract]
        DTO_prRecibido Recibido_Load(Guid channel, int documentID, string prefijoID, int recNro, int numeroDoc);

        /// <summary>
        /// Trae un listado de Recibidos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>la lista de los Recibidos pendientes </returns>
        [OperationContract]
        List<DTO_prRecibidoAprob> Recibido_GetPendientesByModulo(Guid channel, int documentID, string actFlujoID,DTO_seUsuario usuario);

        /// <summary>
        /// Trae un listado de los Recibidos no facturados ya aprobados
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="actFlujoID">Actividad Flujo</param>
        /// <param name="usuario">usuario actual</param>
        /// <param name="NroDocfacturaExist">Numero Doc del documento de la factura si existe</param>
        /// <param name="loadIVA">Indica si carga el porc del iva o no</param>, bool 
        /// <returns>Lista de Recibidos Aprobados</returns>
        [OperationContract]
        List<DTO_prRecibidoAprob> Recibido_GetRecibidoNoFacturado(Guid channel, int documentID, string actFlujoID, string proveedor, bool loadIVA, int NroDocfacturaExist);

        /// <summary>
        /// Recibe una lista de Ordenes de Compra para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="usuario">usuario que aprueba el orden de compra</param>
        /// <param name="rec">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Recibido_AprobarRechazar(Guid channel, int documentID,string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prRecibidoAprob> rec, bool createDoc);

        /// <summary>
        /// Envia para aprobacion un recibido
        /// </summary>
        /// <param name="numeroDoc">Codigo del comprobante</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_SerializedObject Recibido_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc);

        /// <summary>
        /// Radicar o Devuelve Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="_dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject Recibido_RadicarDevolver(Guid channel, int documentID, List<DTO_prRecibidoAprob> recibidosNoFactSelect, DTO_glDocumentoControl _dtoCtrl, DTO_cpCuentaXPagar cta, bool update, out int numeroDoc, bool devolverInd);
        
        /// <summary>
        /// Genera el detalle de cargos de los Recibidos aprobados pendientes
        /// </summary>
        /// <returns>lista de resultados</returns>
        [OperationContract]
        List<DTO_SerializedObject> Recibido_GenerarDetalleCargosRecib(Guid channel, int documentID);

        #endregion

        #region prComprasModificaFechas
        /// <summary>
        /// Consulta las tareas existentes con filtro
        /// </summary>
        /// <param name="numeroDoc">Identificador documento</param>
        /// <param name="aprobadoDocInd">Indica si trae un documento aprobado o sin aprobar</param>
        /// <returns>Lista de dtos</returns>
        [OperationContract]
        List<DTO_prComprasModificaFechas> prComprasModificaFechas_GetByNumeroDoc(Guid channel, int numeroDoc, bool aprobadoDocInd);

        [OperationContract]
        DTO_prComprasModificaFechas prComprasModificaFechas_Load(Guid channel, int numeroDoc);

        [OperationContract]
        DTO_TxResult prComprasModificaFechas_Add(Guid channel, DTO_prComprasModificaFechas datos);

        [OperationContract]
        DTO_TxResult prComprasModificaFechas_Upd(Guid channel, DTO_prComprasModificaFechas datos);


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
        [OperationContract]
        DTO_SerializedObject Convenio_Add(Guid channel, int documentID, DTO_Convenios data, out int numeroDoc, bool update);

        /// <summary>
        /// Trae un listado de los Convenios pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ConvenioAprob> Convenio_GetPendientesByModulo(Guid channel, int documentID, string actFlujoID, DTO_seUsuario usuario, bool SolicitudInd);

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="Nro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        [OperationContract]
        DTO_Convenios Convenio_GetByNroContrato(Guid channel, int documentID, string prefijoID, int Nro);

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="NroConsecutivo">Numero de Documento consecutivo</param>
        /// <returns>Retorna el Convenio</returns>
        [OperationContract]
        DTO_Convenios Convenio_Get(Guid channel, int documentID, string prefijoID, int NroConsecutivo, bool ConsumoProyectoInd);

        /// <summary>
        /// Recibe una lista de Convenios para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Convenio_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_ConvenioAprob> convAp, bool createDoc);

        /// <summary>
        ///   Envia para aprobacion un convenio
        /// </summary>
        /// <param name="documentID">Documento de aprobacion</param>
        /// <param name="actividadFlujoID">actividad actual</param>
        /// <param name="numeroDoc">identificador del documento a aprobar</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_SerializedObject Convenio_SendToAprob(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, bool createDoc);
        
        /// <summary>
        /// Trae un listado de Solicitudes de Despacho Convenios para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de Solicitudes</returns>
        [OperationContract]
        List<DTO_ConveniosResumen> SolicitudDespachoConvenio_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, string proveedor);

        #endregion
        #region Consumo Proyecto
        /// <summary>
        /// Trae un listado de los Consumos de Proyecto para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ConveniosResumen> ConsumoProyecto_GetResumen(Guid channel, int documentID, DTO_seUsuario usuario, ModulesPrefix mod, DateTime fechaCorte, string proveedorID);

        /// <summary>
        /// Agrega un recibido de consumo y Recibe los consumos de proyecto en los saldos 
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="recibCons">Lista de items a recibir</param>
        /// <returns>Respuesta</returns>
        [OperationContract]
        DTO_SerializedObject RecibidoConvenios_Add(Guid channel, int documentID, List<DTO_ConveniosResumen> recibCons, string proveedorID, DateTime fechaDoc);
        
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
        [OperationContract]
        DTO_TxResult Proveedores_Reversion(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Anula Proveedores
        /// </summary>
        /// <param name="numDoc">nums de las Proveedores a anular</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult Proveedoress_Anular(Guid channel, int documentID, List<int> numDocs);

        #endregion

        #region Provision
        /// <summary>
        /// Guarda documento de Provision con un comprobante de los Recibidos Pendientes por facturar
        /// </summary>
        /// <param name="documentID">documento de provision</param>
        /// <returns>Lista de Resultados</returns>
        [OperationContract]
        DTO_TxResult Provision_RecibidoNotFacturadoAdd(Guid channel, int documentID);
        #endregion

        #region Consultas

        /// <summary>
        /// Trae un listado de las documentos con detalle como consulta
        /// </summary>
        /// <param name="documentID">documento relacionado</param>
        /// <param name="docs">Lista de documentos a consultar</param>
        /// <returns>Detalle de la consulta</returns>
        [OperationContract]
        List<DTO_ConsultaCompras> ConsultaCompras_Get(Guid channel, int documentID, List<DTO_glDocumentoControl> ctrls);
     
        /// <summary>
        /// Obtiene una lista de cierres
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <param name="codigoBSID">codigo Bien servicio</param>
        /// <param name="referenciaID">referencia</param>
        /// <param name="numeroDocOC">numero Doc Orden Compra</param>
        /// <returns>lista de cierres</returns>
        [OperationContract]
        List<DTO_prCierreMesCostos> prCierreMesCostos_GetByParameter(Guid channel, DateTime periodo, string codigoBSID, string referenciaID, int? numeroDocOC);

        #endregion

        #endregion

    }

}
