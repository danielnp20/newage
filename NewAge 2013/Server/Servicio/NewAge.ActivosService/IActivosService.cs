using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using SentenceTransformer;
using NewAge.DTO.Reportes;

namespace NewAge.Server.ActivosService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface IActivosService
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

        #region Activos Fijos

        #region Procesos

        /// <summary>
        /// Genera la depreciacion de los activos
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="periodo">Periodo de cierre</param>
        [OperationContract]
        DTO_TxResult Proceso_GenerarDepreciacionActivos(Guid channel, int documentID, DateTime periodo);

        /// <summary>
        /// Genera un Reproceso de la Depreciación por Unidades de Producción
        /// </summary>
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="fechaIni">fechaIni</param>
        /// <param name="fechaFinal">fechaFinal</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_ReProcesoDepreciacion(Guid channel, int documentID, DateTime fechaIni, DateTime fechaFinal);
       
        #endregion

        #region acActivoControl


        /// <summary>
        /// Trae los activos fijos para la Empresa actual
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_acActivoControl> acActivoControl_Get(Guid channel);

        /// <summary>
        /// Obtiene los activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        [OperationContract]
        List<DTO_acActivoControl> acActivoControl_GetFilters(Guid channel, string serialID,
                                                                        string PlaquetaID,
                                                                        string locFisicaID,
                                                                        string referenciaID,
                                                                        string centroCosto,
                                                                        string proyecto,
                                                                        string clase,
                                                                        string tipo,
                                                                        string grupo,
                                                                        string responsable,
                                                                        bool isContenedor,
                                                                        int pageSize,
                                                                        int pageNum);


        /// <summary>
        /// Obtiene el numero de activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="centroCosto">identificador centro de costo</param>
        /// <param name="proyecto">identificador de proyecto</param>
        /// <param name="clase">identificador de clase</param>
        /// <param name="tipo">identificador de tipo</param>
        /// <param name="grupo">identificador de grupo</param>
        /// <param name="responsable">resposable</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        [OperationContract]
        int acActivoControl_GetFiltersCount(Guid channel, string serialID,
                                                        string PlaquetaID,
                                                        string locFisicaID,
                                                        string referenciaID,
                                                        string centroCosto,
                                                        string proyecto,
                                                        string clase,
                                                        string tipo,
                                                        string grupo,
                                                        string responsable,
                                                        bool isContenedor
                                                       );


        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        [OperationContract]
        DTO_acActivoControl acActivoControl_GetByID(Guid channel, int activoID);

        /// <summary>
        /// Trae un activo control
        /// </summary>
        /// <param name="plaqueta">Plaqueta</param>
        /// <returns>Retorna un activo</returns>
        [OperationContract]
        DTO_acActivoControl acActivoControl_GetByPlaqueta(Guid channel, string plaquetaID);

        /// <summary>
        /// Agrega un registro al control de documentos
        /// </summary>
        [OperationContract]
        DTO_TxResultDetail acActivoControl_Add(Guid channel, int documentID, DTO_acActivoControl acCtrl, string documento6ID, bool updateRecibido);

        /// <summary>
        /// Agrega varios registros al control de activos
        /// </summary>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_AddList(Guid channel, int documentoID, string actividadFlujoID, string prefijo, string tipoMvto, List<DTO_acActivoControl> docCtrls, decimal tasaCambio);

        /// <summary>
        /// Agrega varios registros al control de activos
        /// </summary>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_AddListActivos(Guid channel, int documentoID, int activoID, string actividadFlujoID, string prefijoID, string tipoMvto, List<DTO_acActivoControl> docCtrls, decimal tasaCambio);
        
        /// <summary>
        /// Actualiza varios registros al control de activos
        /// </summary>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_Update(Guid channel, List<DTO_acActivoControl> acCtrl, string tipoMvto, int activoID);

        /// <summary>
        /// Trae uno ó varios registros del acActivoControl de acuerdo al parametro.
        /// </summary> 
        [OperationContract]
        List<DTO_acActivoControl> acActivoControl_GetBy_Parameter(Guid channel, DTO_acActivoControl acCtrl);

        /// <summary>
        /// Trae uno ó varios registros del acActivoControl de acuerdo al parametro y tipo de movimiento.
        /// </summary> 
        [OperationContract]
        List<DTO_acActivoControl> acActivoControl_GetByParameterForTranfer(Guid channel, DTO_acActivoControl acCtrl);

        /// <summary>
        /// Actualiza la plaqueta
        /// </summary>
        [OperationContract]
        DTO_TxResult acActivoControl_UpdatePlaqueta(Guid channel, List<DTO_acActivoControl> acActivoControl, string tipoMvto, int activoID, string prefijo);

        #endregion

        #region Alta de Activos

        /// <summary>
        /// Obtiene la lista de activos recibidos por numero de Factura
        /// </summary>
        /// <param name="numeroDoc">Numero de factura</param>
        /// <returns>Lista de activos</returns>
        [OperationContract]
        List<DTO_acActivoControl> AltaActivos_GetActivosByNumDoc(Guid channel, int numeroDoc);
        #endregion

        #region Traslado de Activos
        /// <summary>
        /// Actualiza varios registros de acuerdo al movimiento
        /// </summary>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_TrasladoActivos(Guid channel, int documentoID, List<DTO_acActivoControl> acCtrl, int activoID, string prefijoID, string tipoMvto, DateTime fechaDocu);
        #endregion

        #region Saldos Activos

        /// <summary>
        /// Obtiene una lista de saldos de un activo
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <param name="periodo">Periodo en el q se encuetra</param>
        /// <returns>Lista de saldos de activos</returns>
        [OperationContract]
        List<DTO_acActivoSaldo> acActivoControl_CargarSaldos(Guid channel, int identificadorTR, DateTime periodo, string clase);

        /// <summary>
        /// Carga una lista de  dto_ActivoControl con los saldos por meses de acuerdo al año del periodo
        /// </summary>
        /// <param name="periodo">Periodo de busqueda</param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <returns>Lista de saldos de un activo</returns>
        [OperationContract]
        List<DTO_acActivoSaldo> acActivoControl_CargarSaldos_Meses(Guid channel, string año, int identificadorTR, string activoClaseID, bool mLocal);

        /// <summary>
        /// Trae los movimientos por componente de cada activo 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <param name="periodo">Periodo actual</param>
        /// <param name="clase">Clase de activo</param>
        /// <returns>Lista de DTO_activoSaldo</returns>
        [OperationContract]
        List<DTO_acActivoSaldo> acActivoControl_CargarMvtos(Guid channel, int identificadorTR, DateTime periodo, string clase);

        /// <summary>
        /// Funcion que trae el comprobante de acuerdo al numeroDoc y el idTR
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna una lista de comprobanteFooter</returns>
        [OperationContract]
        List<DTO_ComprobanteFooter> acActivoControl_GetByIdentificadorTR(Guid channel, int numeroDoc, int identTR);

        /// <summary>
        /// Funcion para actualiza saldos correspondiente a activos.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="documentoID">int id del documento</param>
        /// <param name="prefijoID">string prefijo del modlo</param>
        /// <param name="tipoMvto">tipo de mov q viene de la pantalla e identifica la transaccion</param>
        /// <param name="dto_activoSaldo">lista de saldos a actualizar</param>
        /// <param name="acActivoCtrl">lista de activos a los cuales se les actualiza los saldos</param>
        /// <returns>lista de resultados</returns>
        List<DTO_TxResult> acActivoControl_UpdateSaldos(Guid channel, int documentoID, string actividadFlujoID, string prefijoID, string tipoMvto, List<DTO_acActivoSaldo> dto_activoSaldo, List<DTO_acActivoControl> acActivoCtrl);

        /// <summary>
        /// Trae los compponentes de acContabiliza de acuerdo a la clase del activo
        /// </summary>
        /// <param name="clase">Activo clase </param>
        /// <returns>Lista de activosaldo</returns>
        List<DTO_acActivoSaldo> acActivoControl_GetComponentesPorClaseActivoID(Guid channel, string clase);

        /// <summary>
        /// Funcion que trae una lista de saldos por componente
        /// </summary>
        /// <param name="periodo">preiodo del modulo</param>
        /// <param name="componentes">Lista con info del componetne</param>
        /// <param name="concSaldo">Compnente</param>
        /// <param name="identificadorTR">Id del activo</param>
        /// <param name="activoClaseID">Clase del activoID</param>
        /// <returns>Lista de activosaldo</returns>
        List<DTO_acActivoSaldo> acActivoControl_GetSaldoXComponente(Guid channel, DateTime periodo, List<DTO_acActivoSaldo> componentes, int identificadorTR, string activoClaseID);

        #endregion

        #region Retiro Activos

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_acRetiroActivoComponente> RetiroActivos_GetComponenentes(Guid channel, int activoID, string tipoBalance);

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_acRetiroActivoComponente> acActivoFijos_GetComponenentes(Guid channel, int activoID);

        /// <summary>
        /// Da de baja un activo
        /// </summary>
        /// <param name="documentoID">Identificador Documento</param>
        /// <param name="actividadFlujoID">Identificador Actividad</param>
        /// <param name="prefijoID">Prefijo</param>
        /// <param name="acActivoControlList">Lista activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="batchProgress">Indicador de Progreso</param>
        /// <param name="insideAnotherTx">Indica si viene de una Transacción</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_RetiroActivos(Guid channel, int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto);


        #endregion

        #region Deterioro Activos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un listado de saldo</returns>
        [OperationContract]
        List<DTO_acActivoSaldo> acActivoControl_GetSaldoCompraActivo(Guid channel, string concSaldo, int identificadorTR, string tipoBalance);
        

        /// <summary>
        /// Crear el comprobante de Deterioro o Revalorización de un listado de Activos
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="documentoID">documento generador</param>
        /// <param name="actividadFlujoID">actividad Flujo</param>
        /// <param name="balanceID">identificador del balance</param>
        /// <param name="prefijoID">prefijo documento</param>
        /// <param name="acActivoControlList">listado de activos</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <param name="insideAnotherTx">determina si procede de una transacción</param>
        /// <returns>lista de resultados</returns>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_Deterioro(Guid channel, int documentoID, string actividadFlujoID, string balanceID, string prefijoID, bool deterioroInd, string tipoMov, DTO_coDocumentoRevelacion revelacion, List<DTO_acActivoControl> acActivoControlList);
        #endregion

        #region Contenedores

        /// <summary>
        /// Obtiene los activos hijos 
        /// </summary>
        /// <param name="activoID">activoID padre</param>
        /// <returns>listado de activos control</returns>
        [OperationContract]
        List<DTO_acActivoControl> acActivoControl_GetChildrenActivos(Guid channel, int activoID);        

        #endregion

        #region acActivoGarantia

        /// <summary>
        /// Trae lista activo garantia para plantilla de importacion
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        [OperationContract]
        List<DTO_acActivoGarantia> acActivoGarantia_GetForImport(Guid channel);

        /// <summary>
        /// Agrega una lista de activo garantia
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        [OperationContract]
        DTO_TxResult acActivoGarantia_Add(Guid channel, int documentoID, List<DTO_acActivoGarantia> acGar);

        #endregion

        #region Movimientos Activos

        /// <summary>
        /// Movimientos Activos 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="activoID">activoID</param>
        /// <param name="actividadFlujoID">actividadFlujoID</param>
        /// <param name="prefijoID">prefijoID</param>
        /// <param name="acActivoControlList">Listado de Activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="proyectoID">proyectoID</param>
        /// <param name="centroCostoID">centroCostoID</param>
        /// <param name="locFisicaID">locFisicaID</param>
        /// <param name="responsable">responsable</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <param name="insideAnotherTx">indica si viene de una transaccion</param>
        /// <returns>lista de resultados</returns>
        [OperationContract]
        List<DTO_TxResult> acActivoControl_MovActivos(Guid channel, int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto,
                                                             string proyectoID, string centroCostoID, string locFisicaID, string responsable);


        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que obtiene la información del activo de acuerdo al filtro
        /// </summary>
        /// <param name="plaqueta">Filtro de PLaquetaID</param>
        /// <param name="serial">Filtro de SerialID</param>
        /// <param name="referencia">Filtro de Referencia</param>
        /// <param name="clase">Filtro de ActivoClaseID</param>
        /// <param name="tipo">Filtro de ActivoTipoID</param>
        /// <param name="grupo">Filtro de ActivoGrupoID</param>
        /// <param name="locFisica">Filtro de LocfisicaIF</param>
        /// <param name="contenedor">Bool que dice si trae o no los activos contenidos</param>
        /// <param name="responsable">Filtro de Responsable - TerceroID</param>
        /// <returns>Lista de detalles para la consulta</returns>
        [OperationContract]
        List<DTO_acQueryActivoControl> ActivoGetByParameter(Guid channel, string plaqueta, string serial, string referencia, string clase, string tipo, string grupo, string locFisica, bool contenedor, string responsable);

        /// <summary>
        /// Funcionque obrtinee los saldos correspondientes al activo
        /// </summary>
        /// <param name="identificadorTr">Activo ID del activo</param>
        /// <param name="balanceTipoID">Tipo de Libro (Funcional o IFRS)</param>
        /// <param name="año">Año que se va a consultar</param>
        /// <param name="mes">Mes que se quiere consultar</param>
        /// <returns>Lista de saldos por activo</returns>
        [OperationContract]
        List<DTO_acActivoQuerySaldos> ActivoControl_GetSaldosByMesYLibro(Guid channel, int identificadorTr, string balanceTipoID, DateTime periodo);
        #endregion

        #endregion
    }
}
