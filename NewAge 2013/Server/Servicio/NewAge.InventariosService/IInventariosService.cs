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

namespace NewAge.Server.InventariosService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface IInventariosService
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

        #region Inventarios

        #region Comprobantes

        /// <summary>
        /// Proceso de posteo de comprobantes para el modulo de inventarios
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna la lista de resultados (uno por cada comprobante)</returns>
        [OperationContract]
        List<DTO_TxResult> PosteoComprobantes(Guid channel, int documentID);

        /// <summary>
        /// Aprueba una lista de posteo decomprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento qu eejecuta la transaccion</param>
        /// <param name="mod">Modulo del cual se esta aprobando el posteo</param>
        /// <param name="docs">Lista de documentos a aprobar</param>
        /// <returns>Retorna el resultado dela operacion</returns>
        [OperationContract]
        List<DTO_TxResult> AprobarPosteo(Guid channel, int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_glDocumentoControl> docs);

        #endregion

        #region Movimientos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        [OperationContract]
        DTO_SerializedObject Transaccion_Add(Guid channel, int documentID, DTO_MvtoInventarios transaccion, bool update, out int numeroDoc);

        /// <summary>
        ///Obtiene una transaccion manual
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <param name="trasladoNotaEnvio">Indica si es traslado de Nota Envio</param>
        /// <returns>DTO_inControlSaldosCostos</returns>
        [OperationContract]
        DTO_MvtoInventarios Transaccion_Get(Guid channel, int documentID, int numeroDoc, bool trasladoNotaEnvio);

        /// <summary>
        /// Envia para aprobacion una transaccion manual
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la transacción</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Transaccion_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc);

        /// <summary>
        ///Obtiene la relacion de saldos y costos para las salidas por referencia
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="saldo">dto para filtrar</param>
        /// <param name="costoTotal">costoTotal</param>
        [OperationContract]
        decimal Transaccion_SaldoExistByReferencia(Guid channel, int documentID, DTO_inControlSaldosCostos saldo, ref DTO_inCostosExistencias costo);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrlSaldoCosto"></param>
        /// <returns>Dto de Control de saldos y costos</returns>
        [OperationContract]
        List<DTO_inControlSaldosCostos> inControlSaldosCostos_GetByParameter(Guid channel, int documentID, DTO_inControlSaldosCostos ctrlSaldoCosto);

        /// <summary>
        /// Retorna una lista de notas envio 
        /// </summary>        
        /// <returns>Retorna una lista de notas envio</returns>
        [OperationContract]
        List<DTO_NotasEnvioResumen> NotasEnvio_GetResumen(Guid channel, int documentID);

        /// <summary>
        /// Recibe o devuelve entradas de una Nota de Envio
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="notaEnvio">resumen de mvtos</param>
        /// <param name="actFlujoID">Actividad actual</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject NotaEnvio_RecibirDevolver(Guid channel, int documentID, DTO_MvtoInventarios notaEnvio, string actFlujoID, bool createDoc);

        /// <summary>
        /// Obtiene una lista de movimientos Docu
        /// </summary>
        /// <param name="documentoID">Documento Asociado</param>
        /// <param name="header">Filtro para consulta</param>
        /// <returns>List DTO_inMovimientoDocu </returns>
        [OperationContract]
        List<DTO_inMovimientoDocu> inMovimientoDocu_GetByParameter(Guid channel,int documentoID, DTO_inMovimientoDocu header);

        /// <summary>
        /// Actualiza la tabla inMovimientoDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult inMovimientoDocu_Upd(Guid channel, int documentoID, DTO_inMovimientoDocu header);

        #endregion

        #region Inventario Fisico
        /// <summary>
        /// Guardar el inventario fisico
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="bodega">Bodega relacionada</param>
        /// <param name="periodo">Periodo del inventario fisico</param>
        /// <param name="numeroDoc">Numero Doc relacionado</param>
        /// <param name="fisicoInventario"></param>
        /// <returns>resultado de la transaccion</returns>
        [OperationContract]
        DTO_SerializedObject InventarioFisico_Add(Guid channel, int documentID, string bodega, DateTime periodo, out int numeroDoc, List<DTO_inFisicoInventario> fisicoInventario);

        /// <summary>
        ///Obtiene un inventario fisico
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="fisicoInv">Dto filtro</param>
        /// <returns> lista de DTO_inFisicoInventario</returns>
        [OperationContract]
        List<DTO_inFisicoInventario> InventarioFisico_Get(Guid channel, int documentID, DTO_inFisicoInventario fisicoInv);

        /// <summary>
        /// Envia para aprobacion un inventario fisico
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_SerializedObject InventarioFisico_SendToAprob(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, bool createDoc);

        /// <summary>
        /// Trae un listado de inventario fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un listado</returns>
        [OperationContract]
        List<DTO_InvFisicoAprobacion> InventarioFisico_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID);

        /// <summary>
        /// Aprueba o rechaza Inventario Fisico
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="invFisico">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        [OperationContract]
        List<DTO_SerializedObject> InventarioFisico_AprobarRechazar(Guid channel, int documentID, List<DTO_InvFisicoAprobacion> invFisico, string actFlujoID, bool updDocCtrl, bool createDoc);

        /// <summary>
        /// Elimina los saldos guardados de la bodega actual
        /// </summary>        
        [OperationContract]
        void InventarioFisico_Delete(Guid channel, int numeroDoc);
        #endregion

        #region Distribucion Costos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="costosDist">Dto de Distribucion Costos</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        [OperationContract]
        DTO_SerializedObject inDistribucionCostos_Add(Guid channel, int documentID, DTO_MvtoInventarios transaccion, List<DTO_inDistribucionCosto> costosDist, bool update, out int numeroDoc);
        
        /// <summary>
        ///Obtiene una lista de Distribucion Costo
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="numeroDoc">numero Doc inv</param>
        /// <param name="byNroDocFact">Indica si filtra por numero doc de la factura</param>
        /// <returns> lista de DTO_inDistribucionCosto</returns>
        [OperationContract]
        List<DTO_inDistribucionCosto> inDistribucionCosto_GetByNumeroDoc(Guid channel, int documentID, int numeroDoc, bool byNroDocFact);
        #endregion

        #region Liquidacion de Importacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="importacion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        [OperationContract]
        DTO_SerializedObject LiquidacionImportacion_Add(Guid channel, int documentID, DTO_LiquidacionImportacion importacion, bool update, out int numeroDoc);

        #endregion

        #region Deterioro / Revalorizacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="deterioro">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        [OperationContract]
        DTO_SerializedObject Deterioro_Add(Guid channel, int documentID, DTO_MvtoInventarios deterioro, bool update, out int numeroDoc);

        /// <summary>
        /// Envia para aprobacion un deterioro
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_SerializedObject Deterioro_SendToAprob(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, bool createDoc);

        /// <summary>
        /// Aprueba o rechaza un Deterioro
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="deterioro">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DTO</returns>
        [OperationContract]
        List<DTO_SerializedObject> Deterioro_AprobarRechazar(Guid channel, int documentID, List<DTO_inDeterioroAprob> deterioro, string actFlujoID, bool createDoc);

        /// <summary>
        /// Trae un listado de deterioro fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        ///  <param name="actFlujoID">Actividad Actual</param>
        /// <returns>Retorna un listado</returns>
        [OperationContract]
        List<DTO_inDeterioroAprob> inMovimientoDocu_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actFlujoID);

        /// <summary>
        /// Traslado Saldos de inventario a un nuevo periodo
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="periodoID">periodo actual del modulo</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult Proceso_TrasladoSaldosInventario(Guid channel, int documentID, DateTime periodoOld);

        #endregion

        #region  Orden Salida

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="ordenSalida">Dto de la Orden Salida</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        [OperationContract]
        DTO_SerializedObject OrdenSalida_Add(Guid channel, int documentID, DTO_OrdenSalida ordenSalida, bool update, out int numeroDoc);

        /// <summary>
        /// Obtiene una Orden de Salida
        /// </summary>
        /// <param name="bodegaID">Bodega a Filtrar</param>
        /// <returns>Una orden</returns>
        [OperationContract]
        DTO_OrdenSalida OrdenSalida_GeyByBodega(Guid channel, string bodegaID, int? numeroDoc);

        /// <summary>
        /// Aprueba la Orden de Salida y cambia el estado
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        [OperationContract]
        DTO_SerializedObject OrdenSalida_ApproveOrden(Guid channel, int documentID, DTO_OrdenSalida orden);

        /// <summary>
        /// Genera un  mvto de salida de Inventarios a partir de la ORden de Salida
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        [OperationContract]
        DTO_SerializedObject OrdenSalida_ApproveMvtoInv(Guid channel, int documentID, DTO_OrdenSalida orden);
       
        #endregion        

        #region Consultas

        /// <summary>
        /// Funcion que obriene la informacion del serial
        /// </summary>
        /// <param name="serial">Filtro SerialID</param>
        /// <param name="bodegaID">Filtro de BodegaID</param>
        /// <param name="inReferenciaID">Filtro de Referencia</param>
        /// <param name="inCliente">Filtro de TerceroID</param>
        /// <returns>Lista de detalles de las consultas</returns>
        [OperationContract]
        List<DTO_inQuerySeriales> inSaldosExistencias_GetBySerial(Guid channel, string serial, string bodegaID, string inReferenciaID, string inCliente);

        #endregion

        #endregion

    }

}