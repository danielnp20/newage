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
using NewAge.DTO.Reportes;
using SentenceTransformer;

namespace NewAge.Server.CFTService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface ICFTService
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

        #region Cuentas X Pagar

        #region cpAnticipos

        /// <summary>
        /// obtiene un documento de anticipo por identificador unico
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        [OperationContract]
        DTO_cpAnticipo cpAnticipos_GetByEstado(Guid channel, int numeroDoc, EstadoDocControl estado);

        /// <summary>
        /// radica o actualiza documento de anticipo
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_anticipo">anticipo</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject cpAnticipos_Guardar(Guid channel, int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpAnticipo _anticipo, bool update);

        /// <summary>
        /// Retorna el valor total para una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo los anticipos</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de los anticipos</returns>
        [OperationContract]
        decimal cpAnticipos_GetResumenVal(Guid channel, DateTime periodo, TipoMoneda tm, decimal tc, string terceroID);

        /// <summary>
        /// Retorna una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer los anticipos</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna una lista de anticipos</returns>
        [OperationContract]
        List<DTO_AnticiposResumen> cpAnticipos_GetResumen(Guid channel, DateTime periodo, TipoMoneda tipoMoneda, string terceroID, bool anticipoTarjeta);

        /// <summary>
        /// Trae un listado de anticipos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        List<DTO_AnticipoAprobacion> cpAnticipos_GetPendientesByModulo(Guid channel, ModulesPrefix modulo, string actividadFlujoID);

        /// <summary>
        /// Proceso que aprueba o rechaza un listado de anticipos
        /// </summary>
        /// <param name="comps">listado de anticipos</param>
        /// <param name="userId">usuario responsable</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> cpAnticipos_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> comps, bool createDoc);

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
        [OperationContract]
        DTO_TxResult CuentasXPagar_Migracion(Guid channel, int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrls, List<DTO_cpCuentaXPagar> cxpList);

        /// <summary>
        /// Obtener Cuenta X Pagar segun el documento 
        /// </summary>
        /// <param name="NumeroDoc">Numero Documento Asociado</param>
        /// <returns>Cuenta X Pagar</returns>
        [OperationContract]
        DTO_cpCuentaXPagar CuentasXPagar_Get(Guid channel, int NumeroDoc);

        /// <summary>
        /// Adiciona en tabla cpCuentaXPagar 
        /// </summary>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject CuentasXPagar_Radicar(Guid channel, int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cxp, bool mainWindow, bool update, out int numDoc);

        /// <summary>
        /// Radicar Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult CuentasXPagar_Add(Guid channel, int documentID, DTO_cpCuentaXPagar cta);

        /// <summary>
        /// Funcion para devolver una factura radicada
        /// </summary>
        /// <param name="dtoCtrl">documento control</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult CuentasXPagar_Devolver(Guid channel, int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cxp, bool mainWindow);

        /// <summary>
        /// Carga la informacion completa de una facturA
        /// </summary>
        /// <param name="terceroID">Identificador del tercero</param>
        /// <param name="documentoNro">Numero del documento(tercero)</param>
        /// <param name="documentoTercero">Documento del tercero</param>
        /// <returns>Retorna la factura</returns>
        [OperationContract]
        DTO_CuentaXPagar CuentasXPagar_GetForCausacion(Guid channel, int documentID, string terceroID, string documentoTercero, bool chekEstado = true);

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="cxp">Cuenta por pagar</param>
        /// <param name="comp">Comprobante que se debe agregar</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult CuentasXPagar_Causar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_cpCuentaXPagar cxp, DTO_Comprobante comp);

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="listCxP">Lista de Cuenta por pagar</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult CuentasXPagar_CausarMasivo(Guid channel, int documentID, List<DTO_CuentaXPagar> listCxP);

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        List<DTO_CausacionAprobacion> CuentasxPagar_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID, bool checkUser);

        /// <summary>
        /// Aprueba o rechazas causacion de facturas
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="comps">lista comprobantes</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        [OperationContract]
        List<DTO_SerializedObject> CuentasXPagar_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc);

        /// <summary>
        /// Revierte una cuenta por pagar
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult CuentasXPagar_Revertir(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Generar los consecutivos de las Facturas Equivalentes
        /// </summary>
        /// <param name="periodo">periodo</param>
        [OperationContract]
        DTO_TxResult CuentasXPagar_ConsecutivoFactEquivalente(Guid channel, DateTime periodo);

        /// <summary>
        /// Reclasificacion SaldosCxP
        /// </summary>
        /// <param name="facturas">Pagos a programar o desprogramar</param>
        /// <param name="cuenta">cuenta contrapartida</param>
        /// <param name="fecha">fecha Doc</param>
        /// <param name="tc">tasa de cambio</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult ReclasificacionSaldosCxP(Guid channel, int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> facturas, string cuenta,
            DateTime fecha, decimal tc);

        #endregion

        #region Legalización

        /// <summary>
        /// Adiciona en una legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalizacion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject Legalizacion_Add(Guid channel, int documentID, DTO_Legalizacion leg, bool ComprobantePRE);

        /// <summary>
        /// Implementacion Legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <returns>DTO_LegalizaHeader</returns>
        [OperationContract]
        DTO_Legalizacion Legalizacion_Get(Guid channel, int NumeroDoc);

        /// <summary>
        /// Actualiza una legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalizacion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult Legalizacion_Update(Guid channel, int documentID, List<DTO_cpLegalizaFooter> leg, DTO_cpLegalizaDocu header);

        /// <summary>
        /// Envia para aprobacion una legalizacion
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la legalizacion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_SerializedObject Legalizacion_SendToAprob(Guid channel, int documentID, int numeroDoc, bool createDoc);

        /// <summary>
        /// Trae un listado de cajas menores pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        List<DTO_LegalizacionAprobacion> Legalizacion_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID);

        /// <summary>
        /// Recibe una lista de cajas menores para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="leg">Cajas menores que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Legalizacion_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_LegalizacionAprobacion> leg, bool createDoc);

        /// <summary>
        /// Guardar el Auxiliar Pre
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="leg">Legalizacion a agregar con la informacion necesaria</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_TxResult Legalizacion_ComprobantePreAdd(Guid channel, int documentID, DTO_Legalizacion leg);

        #endregion

        #region Tarjeta Credito

        /// <summary>
        /// Obtiene un objeto DTO_cpTarjetaDocu por numero de documento
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns></returns>
        [OperationContract]
        DTO_cpTarjetaDocu cpTarjetaDocu_GetByEstado(Guid channel, int numeroDoc, EstadoDocControl estado, out List<DTO_cpTarjetaPagos> lisTarjetaPago);

        /// <summary>
        /// Guarda o actualiza documento de Tarjeta Credito
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_tarjetaPago">Tarjeta Docu</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject cpTarjetaDocu_Guardar(Guid channel, int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpTarjetaDocu _tarjetaPago, List<DTO_cpTarjetaPagos> _listTarjetaPago, bool update);

        /// <summary>
        /// Retorna el valor total para una lista de Tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo las tarjetas</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de las tarjetas</returns>
        [OperationContract]
        decimal cpTarjetaDocu_GetResumenVal(Guid channel, DateTime periodo, TipoMoneda tm, decimal tc, string terceroID);

        /// <summary>
        /// Retorna una lista de tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las tarjetas</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna una lista de Tarjetas Docu</returns>
        [OperationContract]
        List<DTO_AnticiposResumen> cpTarjetaDocu_GetResumen(Guid channel, DateTime periodo, TipoMoneda tipoMoneda, string terceroID);

        /// <summary>
        /// Trae un listado de Tarjetas pago pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        List<DTO_AnticipoAprobacion> cpTarjetaDocu_GetPendientesByModulo(Guid channel, ModulesPrefix modulo, string actividadFlujoID);

        /// <summary>
        /// Recibe una lista de Tarjetas Pago para aprobar o rechazar
        /// </summary>
        /// <param name="tarjetaPago">Tarjetas Docu que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> cpTarjetaDocu_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> comps, bool createDoc);

        #endregion

        #region Consultas

        /// <summary>
        /// Función que carga una lista de facrutas
        /// </summary>
        /// <param name="año">Año para filtrar</param>
        /// <param name="terceroId">Filtro del tercero</param>
        /// <param name="factNro">Numero de factura</param>
        /// <returns>Lista de Facturas</returns>
        ///</summary>
        [OperationContract]
        List<DTO_QueryFacturas> ConsultarFacturas(Guid channel, DateTime periodo, string terceroId, string conceptoCxP, string factNro, int tipoConsul, int? tipoFact);

        #endregion

        #endregion

        #region Facturacion

        #region FacturaVenta

        /// <summary>
        /// Consulta una tabla faFacturaDocu segun el numero de documento
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        [OperationContract]
        DTO_faFacturaDocu faFacturaDocu_Get(Guid channel, int numeroDoc);

        /// <summary>
        /// Carga la informacion completa dela FacturaVenta
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="facturaNro">Numero del Documento interno</param>
        /// <returns>Retorna FacturaVenta</returns>
        [OperationContract]
        DTO_faFacturacion FacturaVenta_Load(Guid channel, int documentID, string prefijoID, int facturaNro);

        /// <summary>
        /// Guardar nueva Factura de Venta y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="header">FacturaDocu</param>
        /// <param name="footer">FacturaFooter</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject FacturaVenta_Guardar(Guid channel, int documentID, DTO_glDocumentoControl ctrl, DTO_faFacturaDocu header, List<DTO_faFacturacionFooter> footer, bool update, out int numDoc);

        /// <summary>
        /// Trae un listado de facturas pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_faFacturacionAprobacion> FacturaVenta_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID);

        /// <summary>
        /// Aprueba o rechazas factura
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="factList">lista facturas</param>
        /// <param name="updateDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        [OperationContract]
        List<DTO_SerializedObject> FacturaVenta_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_faFacturacionAprobacion> factList);

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las facturas</param>
        /// <param name="terceroID">Tercero del Cliente</param>
        /// <returns>Retorna una lista de facturas</returns>
        [OperationContract]
        List<DTO_faFacturacionResumen> Facturacion_GetResumen(Guid channel, DateTime periodo, TipoMoneda tipoMoneda, string terceroID);

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="clienteID">Periodo de consulta</param>
        /// <param name="NotaEnvioEmptyInd">filtrar por nota de envio</param>
        /// <returns>Retorna una lista de facturas</returns>
        [OperationContract]
        List<DTO_faFacturaDocu> FacturaVenta_GetByCliente(Guid channel, int documentID, string clienteID, bool NotaEnvioEmptyInd);

        /// <summary>
        /// Actualiza la tabla faFacturaDocu 
        /// </summary>
        /// <param name="fact">DTO_faFacturaDocu</param>
        /// <param name="OnlyFacturaFija">Indica si solo guarda el indicador de facturaFija</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult FacturaDocu_Upd(Guid channel, int documentoID, List<DTO_faFacturaDocu> fact, bool OnlyFacturaFija);

        /// <summary>
        /// Agrega una lista de facturas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="migrarList">Datos para migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult FacturaVenta_MigracionGeneral(Guid channel, int documentID, string actividadFlujoID, List<DTO_MigrarFacturaVenta> migrarList, ref List<int> numDocs);

        /// <summary>
        /// Anula una factura de venta
        /// </summary>
        /// <param name="numDocFacts">nums de las facturas a anular</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult FacturaVenta_Anular(Guid channel, int documentID, List<int> numDocFacts);

        /// <summary>
        /// Revierte una factura venta
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Facturacion_Revertir(Guid channel, int documentID, int numeroDoc);

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
        [OperationContract]
        DTO_TxResult CuentasXCobrar_Migracion(Guid channel, int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrlList, List<DTO_faFacturaDocu> cxcFactList);
        
        #endregion

        #region Consultas

        List<DTO_QueryHeadFactura> ConsultarFacturasXNota(Guid channel, DateTime año, string terceroId, int tipoConsulta, string Asesor, string Zona, string Proyecto, int TipoFact, string NumFact, string Prefijo, bool facturaFijaInd);

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
        [OperationContract]
        List<DTO_QueryFlujoFondos> tsFlujoFondos(Guid channel, DateTime fechaCorte);

        /// <summary>
        ///  Obtiene el detalle de los proyectos
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
        /// <param name="proyecto">proyecto</param>
        /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_QueryFlujoFondosTareas> tsFlujoFondos_Tareas(Guid channel, DateTime fechaCorte, string proyecto, bool? recaudosInd);


        /// <summary>
        ///  Obtiene el  Flujo de Caja
        /// </summary>
        /// <param name="fechaCorte">Fecha corte</param>
                /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_QueryFlujoCaja> tsFlujoCaja(Guid channel, DateTime fechaCorte);

        /// <summary>
        ///  Obtiene el  Flujo de Caja Detallado
        /// </summary>
        /// <param name="Documento">Documento</param>
        /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_QueryFlujoCajaDetalle> tsFlujoCajaDetalle(Guid channel, String Documento);


        /// <summary>
        ///  Obtiene la semana
        /// </summary>
        /// <param name="Documento">Semana</param>
        /// <returns>resultado</returns>
        [OperationContract]
        string Global_DiaSemana(Guid channel, int Semana);


        /// <summary>
        ///  Obtiene el mes
        /// </summary>
        /// <param name="Documento">Mes</param>
        /// <returns>resultado</returns>
        [OperationContract]
        string Global_Mes(Guid channel, int Mes);



        #region PagosElectronicos

        /// <summary>
        /// Consulta las facturas para transmitir al banco
        /// </summary>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        [OperationContract]
        List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosSinTransmitir(Guid channel);

        /// <summary>
        /// Guarda el estado actual de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult PagosElectronicos_Guardar(Guid channel, int documentID, List<DTO_PagosElectronicos> pagosElectronicos);

        /// <summary>
        /// Transmite los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos actuales</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult PagosElectronicos_Transmitir(Guid channel, int documentID, List<DTO_PagosElectronicos> pagosElectronicos);

        /// <summary>
        /// Consulta los pagos transmitidos al banco, buscando por tercero y fecha de transmicion
        /// </summary>
        /// <param name="terceroID">Tercero al que se le realizó el pago</param>
        /// <param name="fechaTransmicion">Fecha en la que se realizó la transmición</param>
        /// <returns>Lista de facturas para transmitir al banco</returns>
        [OperationContract]
        List<DTO_PagosElectronicos> PagosElectronicos_GetPagosElectronicosTransmitidos(Guid channel, string terceroID, DateTime fechaTransmicion);

        /// <summary>
        /// Revierte la transmicion de los pagos electronicos
        /// </summary>
        /// <param name="pagosElectronicos">Listado de los pagos a revertir</param>
        /// <returns>Resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult PagosElectronicos_RevertirTransmicion(Guid channel, int documentID, List<DTO_PagosElectronicos> pagosElectronicos);


        #endregion

        #region Pagos

        /// <summary>
        /// Obtiene la lista de pagos para su programación
        /// </summary>
        /// <returns>Lista de pagos para su programación</returns>
        [OperationContract]
        List<DTO_ProgramacionPagos> ProgramacionPagos_GetProgramacionPagos(Guid channel);

        /// <summary>
        /// Programa pagos
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult ProgramacionPagos_ProgramarPagos(Guid channel, int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos, bool pagoAprobacionInd);

        /// <summary>
        /// Consulta las facturas programadas previamente para pagar
        /// </summary>
        /// <returns>Lista de facturas por tercero para pagar</returns>
        [OperationContract]
        List<DTO_SerializedObject> PagoFacturas_GetPagoFacturas(Guid channel);

        /// <summary>
        /// Registra el pago de una factura en un documento
        /// </summary>
        /// <param name="registroPagoFacturas">Pagos a registrar</param>
        /// <param name="areaFuncionalID">Código del área funcional</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        List<DTO_TxResult> PagoFacturas_RegistrarPagoFacturas(Guid channel, int documentID, string actividadFlujoID, List<DTO_PagoFacturas> pagoFacturas, DateTime fechaPago, 
            string areaFuncionalID);

        /// <summary>
        /// Transferencias bancarias
        /// </summary>
        /// <param name="programacionesPagos">Pagos a programar o desprogramar</param>
        /// <param name="pagoAprobacionInd">Indica si la empresa aprueba o deja pendiente de aprobación</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        List<DTO_TxResult> TransferenciasBancarias_Transferencias(Guid channel, int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> programacionesPagos, 
            DateTime fecha, decimal tc);

        /// <summary>
        /// Revierte una transferencia bancaria
        /// </summary>
        /// <param name="documentID">documento</param>
        /// <param name="compTransf">comprobante a revertir</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult TransferenciasBancarias_Revertir(Guid channel, int documentID, DTO_Comprobante compTransf);

        /// <summary>
        /// Carga la informacion para la anulacion de un cheque
        /// </summary>
        /// <param name="documentID">Documento que realiza la transaccion</param>
        /// <param name="fechaPago">fecha de la operacion</param>
        /// <param name="pagoFactura">Pago factura</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult AnularCheques(Guid channel, int documentID, DateTime fechaPago, DTO_PagoFacturas pagoFactura);

        /// <summary>
        /// Obtiene el docu de los pagos
        /// </summary>
        /// /// <returns>DTO de pagos</returns>
        [OperationContract]
        DTO_tsBancosDocu tsBancosDocu_Get(Guid channel, int numeroDoc);

        #endregion

        #region Recibos de Caja

        /// <summary>
        /// Consulta una tabla tsReciboCajaDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Recibo</param>
        /// <returns></returns>
        [OperationContract]
        DTO_tsReciboCajaDocu ReciboCaja_Get(Guid channel, int NumeroDoc);

        /// <summary>
        /// Carga la informacion completa del Recibo de Caja
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="cajaID">PrefijoID (corresponde a cajaID)</param>
        /// <param name="reciboNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibo de Caja</returns>
        [OperationContract]
        DTO_ReciboCaja ReciboCaja_GetForLoad(Guid channel, int documentID, string cajaID, int reciboNro);

        /// <summary>
        /// Guardar nuevo Recibo de Caja y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="recibo">Recibo de Caja</param>
        /// <param name="comp">Comprobante</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReciboCaja_Guardar(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsReciboCajaDocu recibo, DTO_Comprobante comp, out int numDoc);

        /// <summary>
        /// Migra una lista de recibos de caja
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="recibos">Lista de recibos</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> ReciboCaja_Migracion(Guid channel, int documentID, List<DTO_ReciboCaja> recibos);

        #endregion

        #region Traslado de Fondos

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <param name="generaOrdenPago">Valor que indica si el documento genra orden de pago o no</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult TrasladoFondos_TrasladarFondos(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux, 
            bool generaOrdenPago);

        #endregion

        #region Consignaciones

        /// <summary>
        /// Genera el documento de traslado de fondos
        /// </summary>
        /// <param name="ctrl">Documento a guardar</param>
        /// <param name="tblAux">Tabla auxiliar con datos adicionales</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult Consignaciones_Consignar(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_tsBancosDocu tblAux);

        #endregion

        #region Notas Bancarias

        /// <summary>
        /// Crea una nota bancaria
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult NotasBancarias_Radicar(Guid channel, int documentID, DTO_glDocumentoControl dtoCtrl, DTO_Comprobante comp, DTO_coDocumentoRevelacion revelacion);

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
        [OperationContract]
        List<DTO_ChequesGirados> GetCheques(Guid channel, string bancoID, string nit, DateTime fechaIni, DateTime fechaFin, string numCheque, out string reportUrl, int? numeroDoc);

        /// <summary>
        /// Funcion que carga la información de los recibos de caja
        /// </summary>
        /// <param name="CajaID">Filtro de la caja</param>
        /// <param name="tercero">TerceroID</param>
        /// <param name="fechaIni">Fecha inicial de la consulta</param>
        /// <param name="fechaFin">Fecha final de la consulta</param>
        /// <param name="numReciboCaja">Numero del recibo</param>
        /// <returns>Lista de recibos</returns>
        [OperationContract]
        List<DTO_QueryReciboCaja> ReciboCaja_GetByParameter(Guid channel, string CajaID, string tercero, DateTime fechaIni, DateTime fechaFin, string numReciboCaja);

        #endregion

        #endregion
    }
}
