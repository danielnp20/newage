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
using System.Data;

namespace NewAge.Server.CarteraService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface ICarteraService
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

        #region Cartera Corporativa

        #region Cierres

        /// <summary>
        /// Realiza el proceso de cierre diario
        /// </summary>
        [OperationContract]
        DTO_TxResult Cartera_CerrarDia(Guid channel, DateTime fechaCierre);

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccCierreDia> ccCierreDia_GetAll(Guid channel, DateTime periodo);

        /// <summary>
        /// Carga la información para hacer un cierre diario
        /// </summary>
        /// <param name="fecha">Fecha de cierre</param>
        /// <param name="balanceFunc">Balance funcional</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccCierreMes> ccCierreMes_GetAll(Guid channel, Int16 año);

        /// <summary>
        /// Carga todos los cierres mes con uno o varios filtros
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de CierresCartera</returns>
        [OperationContract]
        List<DTO_ccCierreMesCartera> ccCierreMesCartera_GetByParameter(Guid channel, DTO_ccCierreMesCartera filter);

        /// <summary>
        /// Realiza el proceso de cierre mensual
        /// </summary>
        [OperationContract]
        DTO_TxResult Proceso_CierreMesCartera(Guid channel, int documentID, DateTime periodo);

        /// <summary>
        /// Carga la información para realizar listado de Centrales de Riesgo
        /// </summary>
        /// <param name="periodo">Periodo a consultar</param>
        /// <returns>Listado de Cierres</returns>
        [OperationContract]
        List<DTO_CentralRiesgoMes> ccCierreMesCartera_GetCierreCentralRiesgoMes(Guid channel, DateTime periodo);

        #endregion

        #region Migraciones

        /// <summary>
        /// Valida que la información basica de la migracion nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="numDoc">Identificador del documento control</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="docTercero">Documento Tercero</param>
        /// <param name="isMensual">Indica si la consulta es quincenal o mensual</param>
        /// <param name="fechaIni">Fecha inicial de la migracion</param>
        /// <param name="fechaFin">Fecha final de la migracion</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult RecaudosMasivos_Validar(Guid channel, int documentID, DateTime periodo, string pagaduriaID, DateTime fechaAplica,
            ref List<DTO_ccIncorporacionDeta> data);

        /// <summary>
        /// Procesa la migracion de nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="numDoc">Identificador unico del documento</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="docTercero">Identificador del documento tercero</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="isMensual">Indica si la consulta es quincenal o mensual</param>
        /// <param name="fecha">Fecha de la migracion (15 o ultimo dia del mes)</param>
        /// <param name="data">Información a migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult RecaudosMasivos_Procesar(Guid channel, int documentID, string centroPagoID, string pagaduriaID, DateTime periodo, DateTime fecha, 
            List<DTO_ccIncorporacionDeta> data);
        
        /// <summary>
        /// Paga los registros de la migracion de nomina
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna la lista de resultados</returns>
        [OperationContract]
        DTO_TxResult RecaudosMasivos_Pagar(Guid channel, int documentID, string actFlujoID, DateTime periodo, DateTime fecha, DateTime fechaAplica,
            decimal valorPagaduria, string centroPagoID, DTO_tsBancosCuenta banco, List<DTO_ccIncorporacionDeta> data, List<DTO_ccIncorporacionDeta> exclusiones);

        /// <summary>
        /// Proceso de migracion de cartera
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="periodo">Periodo de ingreso de datos</param>
        /// <param name="compComodinID">Identificador del componente comodin</param>
        /// <param name="data">Lista de creditos a migrar</param>
        /// <returns>Retorna al resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> MigracionCartera(Guid channel, int documentID, DateTime periodo, string compComodinID, List<DTO_MigracionCartera> data);

        /// <summary>
        /// Proceso de migracion de cartera
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="data">Lista de creditos a migrar</param>
        /// <returns>Retorna al resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> MigracionEstadoCartera(Guid channel, int documentID, List<DTO_MigracionEstadoCartera> data);

        /// <summary>
        /// Proceso de migracion de cartera
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el proceso</param>
        /// <param name="periodo">Periodo de ingreso de datos</param>
        /// <param name="pagaduria">pagaduria</param>
        /// <param name="data">Lista de creditos a migrar</param>
        /// <returns>Retorna al resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> MigracionVerificacion(Guid channel, int documentID, DateTime periodo, string pagaduria, List<DTO_MigracionVerificacion> data);


        #endregion

        #region Consultas Generales

        #region Información de los Creditos

        /// <summary>
        /// Funcion que trae la informacion de un credito y su plan de pagos
        /// </summary>
        /// <param name="libranza">Identificador del credito</param>
        /// <returns>Retorna el DTO_Credito con la informacion del credito y su plan de pagos</returns>
        [OperationContract]
        DTO_Credito GetCredito_All(Guid channel, int libranza);

        /// <summary>
        /// Trae la info de un credito por su numero doc
        /// </summary>
        /// <param name="numDoc">Num Doc del credito a buscar</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        DTO_ccCreditoDocu GetCreditoByNumeroDoc(Guid channel, int numDoc);

        /// <summary>
        /// Trae la info de un credito por su libranza
        /// </summary>
        /// <param name="libranzaID">Identificador de la libranza</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        DTO_ccCreditoDocu GetCreditoByLibranza(Guid channel, int libranzaID);

        /// <summary>
        /// Muestra el credito de acuerdo a la libranza 
        /// </summary>
        /// <param name="libranza">Numero de libranza a consultar</param>
        /// <param name="numeroDoc">Numero doc de la libranza</param>
        /// <param name="fechaCorte">Fecha en se que realizo la libranza</param>
        /// <param name="isCooperativa"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_ccCreditoDocu GetCreditosByLibranzaAndFechaCorte(Guid channel, int libranza, int numeroDoc, DateTime fechaCorte);

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditoByCliente(Guid channel, string cliente);

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditoByClienteAndFecha(Guid channel, string cliente, DateTime fechaCorte, bool onlyWithSaldo, bool useFechaCorte);

        /// <summary>
        /// Trae la info de un credito segun el comprador de cartera
        /// </summary>
        /// <param name="compradorCartera">Identificador del comprador de cartera</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditoByCompradorCartera(Guid channel, string compradorCartera);

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditosPendientesByCliente(Guid channel, string clienteID);

        /// <summary>
        /// Trae todos los creditos donde la cedula sea codeudor
        /// </summary>
        /// <param name="codeudor">Identificador del codeudor</param>       
        /// <returns>retorna una lista de DTO_ccCreditoDocu</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditosByCodeudor(Guid channel, string codeudor);

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="isFijado">Indica si el credito esta fijado</param>        
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditosPendientesByClienteAndEstado(Guid channel, string clienteID, List<byte> estados);

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente y el proposito
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="proposito">Proposito en el que esta el credito</param>
        /// <param name="isFijado">Indicador EC_FijadoInd</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditosPendientesByProposito(Guid channel, string clienteID, int proposito);

        /// <summary>
        /// Trae el plan de pagos de un crédito
        /// </summary>
        /// <param name="numeroDoc">Identificador único del crédito</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccCreditoPlanPagos> GetPlanPagos(Guid channel, int numeroDoc);

        /// <summary>
        /// Obtiene los correos de los clientes
        /// </summary>
        /// <param name="clienteID">cliente filtrado</param>
        /// <param name="cleinteInd">solo clientes</param>
        /// <param name="conyugeInd">solo conyuges</param>
        /// <param name="codeudorInd">solo codeudores</param>
        /// <returns>Correos</returns>
        [OperationContract]
        List<DTO_CorreoCliente> GetCorreosCliente(Guid channel, string clienteID, bool clienteInd, bool conyugeInd, bool codeudorInd);

        #endregion

        #region Estado de pagos y saldos

        /// <summary>
        /// Funcion que retorna la informacion del credito
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <returns>Retorna DTO con el plan de pagos y sus componentes</returns>
        [OperationContract]
        DTO_InfoCredito GetInfoCredito(Guid channel, int numDoc, DateTime fechaCorte);

        /// <summary>
        /// Funcion que retorna la informacion del recuado manual
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <returns>Retorna DTO con el plan de pagos y sus componentes</returns>
        [OperationContract]
        DTO_InfoCredito GetSaldoCredito(Guid channel, int numDoc, DateTime fechaCorte, bool asignaMora, bool asignaUsura, bool asignaPJ, bool useWhere);

        /// <summary>
        /// Funcion que retorna la informacion de los pagos del credito
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <returns>Retorna DTO con el plan de pagos y sus componentes</returns>
        [OperationContract]
        DTO_InfoCredito GetPagosCredito(Guid channel, int numDoc, DateTime fechaCorte);

        /// <summary>
        /// Funcion que trae la informacion de los pagos de un credito
        /// </summary>
        /// <param name="numDoc">Numero de referencia del credito</param>
        /// <returns></returns>
        [OperationContract]
        DTO_InfoPagos GetInfoPagos(Guid channel, int numDoc);

        /// <summary>
        /// Funcion que trae los saldos de las cuentas asociadas a un credito
        /// </summary>
        /// <param name="numDoc">Numero de referencia del credito</param>
        /// <param name="tercero">Tercero al cual pertenece el credito</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccSaldosComponentes> GetSaldoCuentasForCredito(Guid channel, int numDoc, string tercero);

        /// <summary>
        /// Trae un registro ccCreditoPagos por consecutivo
        /// </summary>
        /// <returns>retorna un ccCreditoPagos</returns>
        [OperationContract]
        DTO_ccCreditoPagos ccCreditoPagos_GetByCons(Guid channel,int consecutivo);

        #endregion

        #region Ventas de cartera

        /// <summary>
        /// Trae la info de la venta de un credito por su libranza
        /// </summary>
        /// <param name="numDocLibranza">Identificador de la libranza</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        DTO_VentaCartera GetInfoVentaByLibranza(Guid channel, int numDocLibranza, int libranza);

        /// <summary>
        /// Trae un registro de DTO_ccVentaDeta con base a libranza
        /// </summary>
        /// <returns>retorna un registro de DTO_ccVentaDeta</returns>
        [OperationContract]
        DTO_ccVentaDeta ccVentaDeta_GetByNumDocLibranza(Guid channel, int numDocCredito);

        #endregion

        #region Gestion Cobranza

        /// <summary>
        /// Trae la info del cierre del ultima dia de cartera
        /// </summary>
        /// <param name="cliente">Cliente a consultar</param>
        /// <param name="numDoc Credito">orden de la lista</param>
        /// <returns>Dia de Cierre</returns>
        [OperationContract]
        DTO_ccCierreDiaCartera ccCierreDiaCartera_GetUltimoDiaCierre(Guid channel, string cliente, int? numDocCredito);

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Cliente a consultar</param>
        /// <param name="orden">orden de la lista</param>
        /// <returns>Lista de creditos por cliente</returns>
        [OperationContract]
        List<DTO_GestionCobranza> ccCierreDiaCartera_GetGestionCobranza(Guid channel, string cliente, byte orden);

        /// <summary>
        /// Trae la info de las actividades de Gestion Cobranza
        /// </summary>
        /// <param name="fechaINi">Fecha inicial a consultar</param>
        /// <param name="fechaFin">Fecha inicial a consultar</param>
        /// <param name="ActividadFlujoID">actividad a consultar</param>
        /// <param name="cliente">client a consultar</param>
        /// <returns>Lista de actividades por cobranza</returns>
        [OperationContract]
        List<DTO_QueryGestionCobranza> GestionCobranza_GetActividades(Guid channel, DateTime fechaIni, DateTime fechaFin, string etapaFilter, string cliente);

        /// <summary>
        /// Agrega datos de actividad y actualiza historia del credito
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="cobranzas">Lista de los cobranzas</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult GestionCobranza_Add(Guid channel, int documentoID, List<DTO_GestionCobranza> cobranzas);

        /// <summary>
        /// Obtiene datatable para Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaCorte">Fecha Corte</param>
        /// <param name="gestionCobrID">Gestion CobranzaID</param>
        /// <param name="estado">Estado de la cobranza</param>
        /// <param name="tipoGestion">Tipo gestion</param>
        /// <returns>Datatable</returns>
        [OperationContract]
        DataTable HistoricoGestionCobranza_GetExcel(Guid channel, int documentoID, DateTime fechaCorte, string gestionCobrID, string clienteID, string libranza, byte? estado, byte? tipoGestion);

        /// <summary>
        /// Obtiene lista para gestion
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaCorte">Fecha Corte</param>
        /// <param name="gestionCobrID">Gestion CobranzaID</param>
        /// <param name="estado">Estado de la cobranza</param>
        /// <param name="tipoGestion">Tipo gestion</param>
        /// <returns>Lista</returns>
        [OperationContract]
        List<DTO_ccHistoricoGestionCobranza> HistoricoGestionCobranza_GetGestion(Guid channel, int documentoID, DateTime fechaCorte, string gestionCobrID, byte? estado, byte? tipoGestion);

       /// <summary>
        /// Actualiza la tabla de gestiones historico
        /// </summary>
        /// <param name="data">datos a actualizar</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult HistoricoGestionCobranza_Update(Guid channel, int documentoID, List<DTO_ccHistoricoGestionCobranza> data);

        #endregion

        /// <summary>
        /// OBtiene los documentos de movimiento de cartera
        /// </summary>
        /// <param name="clienteMov">Filtro del cliente</param>
        /// <param name="libranza">Filtro del libranza</param>
        /// <param name="fechaInt">Filtro del fechaIni</param>
        /// <param name="fechaFin">Filtro del fechaFin</param>
        /// <param name="pagaduria">Filtro de la pagaduria</param>
        /// <param name="tipoMovimiento">Filtro del tipoMov</param>
        /// <returns>Lista de documentos</returns> 
        [OperationContract]
        List<DTO_QueryCarteraMvto> Cartera_GetMvto(Guid channel, string clienteMov, string NroCredito, DateTime fechaInt, DateTime fechaFin, int tipoMovimiento, int tipoAnulado);

        /// <summary>
        /// Funcion que trae los movimientos de cartera de una libranza
        /// </summary>
        /// <param name="libranza">libranza</param>
        /// <returns>Lista</returns>
        [OperationContract]
        List<DTO_QueryCarteraMvto> CarteraMvto_QueryByLibranza(Guid channel, int libranza);
        #endregion

        #region Operaciones Generales

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <returns></returns> 
        [OperationContract]
        List<DTO_SolicitudAprobacionCartera> SolicitudLibranza_GetForVerificacion(Guid channel, int DocumentoID, string actividadFlujoID);

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="solicitudes">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> SolicitudLibranza_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_SolicitudAprobacionCartera> solicitudes, List<DTO_ccSolicitudAnexo> anexos, List<DTO_glDocumentoChequeoLista> tareas);

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="creditos">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> PantallaCredito_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_ccCreditoDocu> creditos);

        /// <summary>
        /// Realiza el proceso de liquidacion de cartera
        /// </summary>
        /// <param name="edad">Edad del cliente. No se usa en el simulador</param>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="pagaduriaID">Identificador de la pagaduria</param>
        /// <param name="plazo">Plazo de pago</param>
        /// <param name="valorPrestamo">Valor del prestamo</param>
        /// <param name="fechaLiquida">Fecha de liquidacion</param>
        /// <param name="traerCuotas">Indica si se debe incluir el plan de pagos</param>
        /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        [OperationContract]
        DTO_SerializedObject GenerarLiquidacionCartera(Guid channel, string lineaCredID, string pagaduriaID, int valorSolicitado, int vlrGiro, int plazo,
            int edad, DateTime fechaLiquida, decimal? interes, DateTime fechaCuota1);

        /// <summary>
        /// Funcion que actualiza la fecha del plan de pagos y la informacion del credito
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="credito">DTO con la informacion del credito y su plan de pagos</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> CambiaFechaPlanPagos(Guid channel, int documentID, DTO_Credito credito);

        #endregion

        #region Creacion Creditos

        #region Solicitud Libranza

        /// <summary>
        /// Trae la info de las solicitudes de un cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        List<DTO_ccSolicitudDocu> GetSolicitudesByCliente(Guid channel, string cliente);

        /// <summary>
        /// Retorna el Listado de las solicitudes de viabilidad
        /// </summary>
        /// <param name="documentoID">Id del Documento</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna un listado de las solicitudes que se encuentrar para aprobar o rechazar</returns> 
        [OperationContract]
        List<DTO_ccSolicitudDocu> GetSolicitudesByActividad(Guid channel, string actFlujoID);

        /// <summary>
        /// Trae la informacion de una solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        [OperationContract]
        DTO_SolicitudLibranza SolicitudLibranza_GetByLibranza(Guid channel, int libranzaID, string actFlujoID);

        /// <summary>
        /// Trae el NumeroDoc de la tarea
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="NumeroDoc"></param>
        /// <returns>Trae el NumeroDoc de la tarea</returns>
        [OperationContract]
        string EstadoSolicitud_GetByNumeroDoc(Guid channel, string _numeroLibranza);

        /// <summary>
        /// Actualizar el campo de seUsuario del glDocumentoControl
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="ctrSolicitud"></param>
        /// <returns>Actualizar el campo de seUsuario del glDocumentoControl</returns>
        [OperationContract]
        void Solicitud_UpdateUSer(Guid channel, DTO_glDocumentoControl ctrSolicitud);

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="solicitud">Solicitud que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult SolicitudLibranza_Add(Guid channel, int documentoID, DTO_SolicitudLibranza solicitud);

        /// <summary>
        /// Retorma los Documentos Anexos
        /// </summary>
        /// <param name="NumeroDoc">Recive un Numero De Documento</param>
        /// <returns>Lista de DTO_ccSolicitudAnexo</returns>
        [OperationContract]
        List<DTO_ccSolicitudAnexo> SolicitudLibranza_GetAnexosByID(Guid channel, int NumeroDoc);

        /// <summary>
        /// Retorma Las Tareas
        /// </summary>
        /// <param name="NumeroDoc">Recive un Numero De Documento</param>
        /// <returns>Lista de DTO_ccSolicitudAnexo</returns>
        [OperationContract]
        List<DTO_ccTareaChequeoLista> SolicitudLibranza_GetTareasByNumeroDoc(Guid channel, int NumeroDoc);


        /// <summary>
        /// Trae todos los registros de las solicitudes con su estado actual
        /// </summary>
        /// <returns>retorna una lista de DTO_SolicitudGestion</returns>
        [OperationContract]
        List<DTO_SolicitudGestion> SolicitudLibranza_GetGestionSolicitud(Guid channel);
        #endregion

        #region Registro Solicitud

        /// <summary>
        /// Trae la informacion de un registro de solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">id del credito</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        [OperationContract]
        DTO_DigitaSolicitudCredito RegistroSolicitud_GetBySolicitud(Guid channel, int libranzaID);

        /// <summary>
        /// Agrega informacion a las tablas del registro de solicitud
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="data">Datos que se debe agregar</param>
        ///  <param name="isNewVersion">Indica si es nueva version</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult RegistroSolicitud_Add(Guid channel, int documentoID, string actFlujoId, DTO_DigitaSolicitudCredito data, bool isNewVersion);

        /// <summary>
        /// Trae la informacion de un registro de solicitud con cedula
        /// </summary>
        /// <param name="deudor">Deudor del credito</param>
        /// <param name="cedula">cedula a obtener</param>
        /// <param name="tipoPersona">Tipo de persona a obtener</param>
        /// <param name="versionNro">Version del registro</param>
        /// <returns>Datos del cliente</returns>
        [OperationContract]
        DTO_ccSolicitudDatosPersonales RegistroSolicitud_GetByCedula(Guid channel, string deudor, string cedula, byte tipoPersona, int versionNro);

        #endregion

        #region Aprobacion Solicitud Libranza

        /// <summary>
        /// Retorna el Listado de las solicitudes de credito
        /// </summary>
        /// <param name="documentoID">Id del Documento</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna un listado de las solicitudes que se encuentrar para aprobar o rechazar</returns> 
        [OperationContract]
        List<DTO_SolicitudAprobacionCartera> SolicitudDocu_GetForAprobacion(Guid channel, int documentoID, string actFlujoID, int _libranza);

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="solicitudes">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> AprobacionSolicitud_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_SolicitudAprobacionCartera> solicitudes);

        #endregion

        #region Digitacion Credito

        /// <summary>
        /// Trae la informacion de una solicitud de credito segun a libranza
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <param name="actFlujoID">Identificador de la actividad de flujo</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        [OperationContract]
        DTO_DigitacionCredito DigitacionCredito_GetByLibranza(Guid channel, int libranzaID, string actFlujoID);

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="digCredito">Solicitud que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult DigitacionCredito_Add(Guid channel, int documentoID, string actFlujoId, DTO_DigitacionCredito digCredito, List<DTO_Cuota> cuotasExtras);

        /// <summary>
        /// Añade los un registro a ccCreditoComponentes
        /// </summary>
        /// <param name="mvto">Movimiento de la caretra</param>
        [OperationContract]
        List<DTO_ccCreditoComponentes> ccCreditoComponentes_GetByNumDocCred(Guid channel, int numeroDoc);

        #endregion

        #region Referenciacion Cartera


        #endregion

        #region Solicitud Anticipos

        /// <summary>
        /// Funcion que retorna el listado de las compra de cartera para solicitud de anticipos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="libranzaID">Numero de la libranza</param>
        /// <returns>Retorna las compra de cartera para realizar la solicitud de anticipo</returns>     
        [OperationContract]
        List<DTO_ccSolicitudCompraCartera> SolicitudAnticipo_GetByLibranza(Guid channel, int documentID, int libranzaID, int numeroDoc);

        /// <summary>
        /// Envia la solicitud de nuevos anticipos
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="compras">Listado de carteras externas a comprar</param>
        /// <returns>Retorna la lista de resultados</returns>
        [OperationContract]
        List<DTO_SerializedObject> SolicitudAnticipos_SolicitarAnticipos(Guid channel, int documentID, List<DTO_ccSolicitudCompraCartera> compras);

        /// <summary>
        /// Genera los paz y salvos de la compra de cartea
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="compras">Listado de carteras externas a comprar</param>
        /// <returns>Retorna la lista de resultados</returns>
        [OperationContract]
        List<DTO_SerializedObject> SolicitudAnticipos_GenerarPazYSalvo(Guid channel, int documentID, List<DTO_ccSolicitudCompraCartera> compras);

        /// <summary>
        /// Genera los paz y salvos de la compra de cartea
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="compras">Listado de carteras externas a comprar</param>
        /// <returns>Retorna la lista de resultados</returns>
        [OperationContract]
        List<DTO_SerializedObject> SolicitudAnticipos_RevertirAnticipos(Guid channel, int documentID, List<DTO_ccSolicitudCompraCartera> compras);

        #endregion

        #region Devolución Solicitud

        /// <summary>
        /// Trae la informacion de una solicitud de credito para devolición
        /// </summary>
        /// <param name="libranzaID">Libranza</param>
        /// <returns>Retorna la informacion de una solicitud de credito</returns>
        [OperationContract]
        DTO_ccSolicitudDocu DevolucionSolicitud_GetByLibranza(Guid channel, int libranzaID, ref List<DTO_ccSolicitudDevolucionDeta> devoluciones);

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="solicitud">Solicitud que se debe agregar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult DevolucionSolicitud_Add(Guid channel, int documentoID, DTO_ccSolicitudDevolucion devolucion);

        /// <summary>
        /// Consulta informacion de la tabla ccSolicitudDevolucion
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="numDocSol">Solicitud que se debe consultar</param>
        /// <returns>Retorna lista</returns>
        [OperationContract]
        List<DTO_ccSolicitudDevolucion> DevolucionSolicitud_GetByNumeroDoc(Guid channel, int documentoID, int numDocSol);

        #endregion

        #region Liquidación Crédito

        /// <summary>
        /// Retorna el Listado de todos los creditos aprobados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="detalleBeneficiarios">Indicador para establecer si debe buscar los beneficiarios del credito</param>
        /// <param name="allEmpresas">Indica si trae la infomacion de todas las empresas</param>
        /// <returns></returns>     
        [OperationContract]
        List<DTO_ccCreditoDocu> LiquidacionCredito_GetAll(Guid channel, string actFlujoID, bool detalleBeneficiarios, bool allEmpresas);

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="solicitudes">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> LiquidacionCredito_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_SolicitudAprobacionCartera> ccCreditos);

        /// <summary>
        /// Revierte un crédito
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult LiquidacionCredito_Revertir(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Restructura sin Cambio de credito
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <ctrlCredito>Documento control relacionado con el crédito</ctrlCredito>
        /// <digCredito>Datos nuevos del credito</creditoPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult RestructuracionSinCambio(Guid channel, int documentID, string actFlujoID, DTO_ccCreditoDocu credito, DTO_DigitacionCredito digCredito);

        #endregion

        #endregion

        #region Aprobación Giro

        /// <summary>
        /// Funcion que aprueba o rechaza un giro
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="actFlujoID">Actividad Flujo ID</param>
        /// <param name="ccCreditos">Lista con los giros para aprobar o rechazar</param>
        /// <param name="batchProgress"></param>
        /// <param name="insideAnotherTx"></param>
        /// <returns>Retorna una lista de objetos</returns>
        [OperationContract]
        List<DTO_TxResult> AprobarGiro_Credito_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_ccCreditoDocu> ccCreditos, bool pagoMaviso);

        /// <summary>
        /// Funcion que aprueba o rechaza un giro
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="actFlujoID">Actividad Flujo ID</param>
        /// <param name="ccCreditos">Lista con los giros para aprobar o rechazar</param>
        /// <param name="batchProgress"></param>
        /// <param name="insideAnotherTx"></param>
        /// <returns>Retorna una lista de objetos</returns>
        [OperationContract]
        List<DTO_TxResult> AprobarGiro_CreditoRechazo(Guid channel, int documentID, string actFlujoID, List<DTO_ccCreditoDocu> ccCreditos);

        /// <summary>
        /// Retorna el Listado de todos los creditos rechazados
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <returns></returns>     
        [OperationContract]
        List<DTO_ccCreditoDocu> AprobarGiroRechazo_GetAll(Guid channel, string actFlujoID);

        #endregion

        #region Estado Cuenta

        /// <summary>
        /// Agrega un estado de cuenta
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoId">Actividad de flujo</param>
        /// <param name="infoCredito">Informacion Credito</param>
        /// <param name="estadoCuenta">Informacion del estado de cuenta</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult EstadoCuenta_Add(Guid channel, int documentoID, string actFlujoId, DTO_InfoCredito infoCredito, DTO_EstadoCuenta estadoCuenta);

        /// <summary>
        /// Funcion que retorna los componentes del estado de cuenta
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <param name="isPagoParcial">Indicador para verificar si hay pagos parciales (Pagos Totales)</param>
        /// <returns>Retorna el listado de los componentes del estado de cuenta segun el numDoc</returns>
        [OperationContract]
        List<DTO_ccEstadoCuentaComponentes> EstadoCuenta_GetComponentesByNumeroDoc(Guid channel, int numDoc, bool isPagoTotal);

        /// <summary>
        /// Funcion que retorna los componentes del estado de cuenta para revocatoria
        /// </summary>
        /// <param name="aseguradora">Aseguradora</param>
        /// <returns>Retorna el listado de creditos para revocar </returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> EstadoCuenta_GetForRevocatoria(Guid channel, string aseguradora);

        /// <summary>
        /// Funcion que retorna los componentes del estado de cuenta
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <returns>Retorna el listado de los componentes del estado de cuenta segun el numDoc</returns>
        [OperationContract]
        DTO_ccEstadoCuentaHistoria EstadoCuenta_GetHistoria(Guid channel, int numDocEC);

        /// <summary>
        /// Funcion que retorna la información de un estado de cuenta
        /// </summary>
        /// <param name="numDoc">Numero Doc</param>
        /// <returns>Retorna la información asociada a un estado de cuenta</returns>
        [OperationContract]
        DTO_EstadoCuenta EstadoCuenta_GetAll(Guid channel, int numDoc);

        /// <summary>
        /// Agrega un documento de revocacion
        /// </summary>
        /// <param name="documentoID">ID</param>
        /// <param name="actFlujoId">flujo</param>
        /// <param name="aseguradoraID">ASeguradora</param>
        /// <param name="ctrl">doc control</param>
        /// <param name="creditosRevocar">lista de creditos a revocar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult PolizaRevocatoria_Add(Guid channel, int documentoID, string aseguradoraID, DTO_glDocumentoControl ctrl, List<DTO_ccCreditoDocu> creditosRevocar, bool update);

        #endregion

        #region Pagos Creditos

        /// <summary>
        /// Funcion que agrega un racaudo manual a la tabla ccCreditoPagos
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoId">Actividad de flujo</param>
        /// <param name="reciboCaja">Recibo de caja</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="planPagos">Lista del plan de pagos</param>
        /// <param name="componentes">Lista de los componentes de cada cuota</param> List<DTO_ccCreditoComponentes> componentes
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult PagosCreditos_Parcial(Guid channel, TipoRecaudo tipoRecaudo, int documentID, string actFlujoID, DateTime fechaDoc, DateTime fechaPago,
            DTO_tsReciboCajaDocu reciboCaja, DTO_ccCreditoDocu credito, List<DTO_ccCreditoPlanPagos> planPagos, List<DTO_ccSaldosComponentes> componentesPago, List<DTO_ccSaldosComponentes> componentes);

        /// <summary>
        /// Funcion que realiza el pago parcial o total de un credito para pago total
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="reciboCaja">Recibo de caja</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="componentesPago">Lista de los componentes de cada cuota</param> List<DTO_ccCreditoComponentes> componentes
        /// <param name="isPagoParcial">Indica si se esta realizando un pago parcial</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult PagosCreditos_Total(Guid channel, int documentID, string actFlujoID, DateTime fechaDoc, DateTime fechaPago, DTO_tsReciboCajaDocu reciboCaja, 
            DTO_ccCreditoDocu credito, List<DTO_ccEstadoCuentaComponentes> ec_componentes, List<DTO_ccSaldosComponentes> componentesPago, bool isPagoParcial);

        /// <summary>
        /// Revierte un pago de cartera
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult CarteraPagos_Revertir(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region Otras Operaciones Créditos

        /// <summary>
        /// Funcion que realiza el rechazo de un crédito
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Credito_Rechazo(Guid channel, int documentID, string actFlujoID, DTO_ccCreditoDocu credito, DateTime fecha, string bancoID);

        /// <summary>
        /// Funcion que realiza el desistimiento de un crédito
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="reciboCaja">Recibo de caja</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="componentesPago">Lista de los componentes de cada cuota</param> List<DTO_ccCreditoComponentes> componentes
        /// <param name="isPagoParcial">Indica si se esta realizando un pago parcial</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Credito_Desistimiento(Guid channel, int documentID, string actFlujoID, DTO_ccCreditoDocu credito, DateTime fecha);

        /// <summary>
        /// Funcion que realiza el desistimiento de un crédito
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="credito">Informacion basica del credito</param>
        /// <param name="centroPagoID">Identificador del nuevo centro de pago</param>
        /// <param name="ciudadID">Identificador de la ciudad</param>
        /// <param name="pagaduriaID">Identificador de la pagaduría</param>
        /// <param name="zonaID">Identificador de la zona</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Credito_CambioDatos(Guid channel, int documentID, DTO_ccCreditoDocu credito, string centroPagoID, string pagaduriaID, string zonaID, string ciudadID,
            string cooperativaID, string novedad, string estadoCobranza, string gestionCobranza, string estadoSinisestro, string obs);

        /// <summary>
        /// Funcion que realiza el cambio de datos cliente
        /// </summary>
        /// <param name="Cambio">Tipo Cambio</param>
        /// <param name="cliente">Informacion basica del cliente</param>
        /// <param name="tercero">Informacion basica del tercero</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Cliente_CambioDatos(Guid channel,int cambio, DTO_ccCliente cliente, DTO_coTercero tercero);

        /// <summary>
        /// Revierte el desistimiento de un crédito
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Credito_Desistimiento_Revertir(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Trae el saldo de un crédito
        /// </summary>
        /// <returns>retorna una el saldo de un crédito</returns>
        [OperationContract]
        decimal Credito_GetSaldoFlujos(Guid channel, int numeroDoc, out int flujosPagados);

        #endregion

        #region Incorporaciones

        #region Incorporacion

        /// <summary>
        /// Rertorna los pagaduria inválidas en una fechade incorporación
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<string> IncorporacionCredito_GetInvalidPagadurias(Guid channel, DateTime fechaIncorpora);

        /// <summary>
        /// Funcion que consulta las solicitudes que estan en estado de incorporacion previa activado
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <param name="tipoVerificacion">Indica el tipo de verificacion</param>
        /// <returns>Retorna una lista con las solicitudes de credito para incorporacion segun la pagaduria</returns>
        [OperationContract]
        List<DTO_ccSolicitudDocu> IncorporacionSolicitud_GetByCentroPago(Guid channel, string centroPago, DateTime fechaIncorpora, string actFlujoID);

        /// <summary>
        /// Funcion que consulta las solicitudes que estan en estado de incorporacion previa activado
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <param name="tipoVerificacion">Indica el tipo de verificacion</param>
        /// <returns>Retorna una lista con las solicitudes de credito para incorporacion segun la pagaduria</returns>
        [OperationContract]
        List<DTO_ccSolicitudDocu> IncorporacionSolicitudVerificacion_GetByCentroPago(Guid channel, string centroPago, string actFlujoID, int tipoVerificacion);

        /// <summary>
        /// Funcion que consulta los creditos para incorporacion segun una pagaduria
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <returns>Retorna una lista con los creditos para incorporacion segun la pagaduria</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> IncorporacionCredito_GetByCentroPago(Guid channel, string centroPago, DateTime fechaIncorpora, string actFlujoID, bool getPendientes);

        /// <summary>
        /// Funcion que consulta los creditos para incorporacion segun una pagaduria
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <param name="isVerificacion">Indica si la operacion es la de verificacion o no</param>
        /// <returns>Retorna una lista con los creditos para incorporacion segun la pagaduria</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> IncorporacionCreditoVerificacion_GetByCentroPago(Guid channel, string centroPago, string actFlujoID);

        /// <summary>
        /// Retorna las incorporaciones por credito
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccIncorporacionDeta> IncorporacionCredito_GetByNumDocCred(Guid channel, int numDocredito);

        /// <summary>
        /// Realiza la incorporacion de los creditos o las solicitudes
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="CentroPago">Id del centro de pago</param>
        /// <param name="fechaIncorpora">Fecha en la que se incorporan los creditos</param>
        /// <param name="vlrIncorporacion"> Valor total de la incorporacion</param>
        /// <param name="creditos">Lista de creditos que se estan incorporando</param>
        /// <param name="solicitudes">Lista de solicitudes que se estan incorporando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        Tuple<int, List<DTO_SerializedObject>, string> IncorporacionCredito_Aprobar(Guid channel, int documentID, string actFlujoID, string centroPago, DateTime fechaIncorpora, Decimal vlrIncorporacion, List<DTO_ccCreditoDocu> creditos, List<DTO_ccSolicitudDocu> solicitudes);

        /// <summary>
        /// Revierte una incorporacion
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult IncorporacionCredito_Revertir(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Guarda las incorporaciones 
        /// </summary>
        /// <param name="documentID">documento actual</param>
        /// <param name="periodoID">periodo actual</param>
        /// <param name="fecha">fecha actual</param>
        /// <param name="migraciones">lista de miegraciones a incorporar</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_TxResult Incorporacion_Guardar(Guid channel, int documentID, DateTime periodoID, DateTime fecha, List<DTO_MigrarIncorporacionDeta> migraciones);

        #endregion

        #region Verificación Incorporación

        /// <summary>
        /// Realiza la incorporacion de los creditos o las solicitudes
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="CentroPago">Id del centro de pago</param>
        /// <param name="fechaVerificacion">Fecha en la que se verifica la incorporacion</param>
        /// <param name="vlrVerificacion"> Valor total de la verificacion</param>
        /// <param name="creditos">Lista de creditos que se estan incorporando</param>
        /// <param name="solicitudes">Lista de solicitudes que se estan incorporando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> VerificacionIncorporacion_AprobarRechazar(Guid channel, int documentID, string actFlujoID, string centroPago, DateTime fechaVerificacion, Decimal vlrVerificacion, List<DTO_ccCreditoDocu> creditos, List<DTO_ccSolicitudDocu> solicitudes);

        #endregion

        #region DesIncorporacion

        /// <summary>
        /// Funcion que consulta los creditos para desincorporar que pertenecen a un centro de pago
        /// </summary>
        /// <param name="centroPago">Centro de pago que se usa para consultar</param>
        /// <returns>Retorna una lista con los creditos para incorporacion segun la pagaduria</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> DesIncorporacionCredito_Get(Guid channel);

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="creditos">Listado de documentos</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> DesIncorporacion_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_ccCreditoDocu> creditos, DateTime fechaNovedad);

        /// <summary>
        /// Revierte una desincorporacion
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult DesIncorporacion_Revertir(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region Reincorporacion

        /// <summary>
        /// Trae la información para hacer reincorporaciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ccReincorporacionDeta> Reincorporacion_GetForReincorporacion(Guid channel, DateTime periodo, string centroPagoID);

        /// <summary>
        /// Agrega reincorporaciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult Reincorporacion_Aprobar(Guid channel, int documentID, string actFlujoID, DateTime periodo, DateTime fecha, List<DTO_ccReincorporacionDeta> data);

        /// <summary>
        /// Revierte una reincorporacion
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Reincorporacion_Revertir(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region Archivos Incorporacion

        /// <summary>
        /// Carga el Listado de los creditos Incorporados
        /// </summary>
        /// <param name="channel">Canal de comunicacion</param>
        /// <param name="periodo">Fecha de incorporacion de Creditos</param>
        /// <param name="pagaduria">pagaduria para incorporacion</param>
        /// <returns>Retorna las incorporaciones por fecha</returns>
        [OperationContract]
        List<DTO_ccArchivoIncorporaciones> GetArchivosIncorporacion(Guid channel, DateTime periodo, string pagaduria);

        /// <summary>
        /// Actualiza los campos de la tabla ccIncorporacionDeta
        /// </summary>
        [OperationContract]
        void UpdateIncorporacionFechaTransmite(Guid channel, DateTime fechaTransmite, List<int> consecutivos);

        #endregion

        #endregion

        #region Venta Cartera

        #region Endodo Libranzas

        /// <summary>
        /// Verifica en la lista que documentos estan para aprobar y rechazar, Actualiza la tarea si es aprobado.
        /// </summary>
        /// <param name="documentID">Documento que ejecula la transaccion</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="creditosVendidos">Lista de creditos para aprobar/rechazar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> EndosoLibranzasVendidas_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_ccVentaDeta> creditosVendidos);

        #endregion

        #region Preventa Cartera

        /// <summary>
        /// Trae todos las ofertas de preventa y venta
        /// </summary>
        /// <param name="compradorCarteraID">Comprador a consultar</param>
        /// <param name="isVenta">Indicador para establecer si trae las ofertas de preventas ode ventas</param>
        /// <returns>retorna una lista de ccCreditoDocu</returns>
        [OperationContract]
        List<string> PreventaCartera_GetOfertas(Guid channel, string compradorCarteraID, bool isVenta);

        /// <summary>
        /// Funcion que obtiene los creditos que fueron preseleccionados para la venta
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compradorCarteraID">Comprador de cartera</param>
        /// <param name="oferta">Oferta de la compra</param>
        /// <param name="factorCesion">Factor que se aplica a la venta</param>
        /// <returns></returns> 
        [OperationContract]
        DTO_VentaCartera PreventaCartera_GetCreditos(Guid channel, string actFlujoID, string compradorCarteraID, string oferta, DateTime fechaIni, DateTime fechaFin);

        /// <summary>
        /// Funcion que genera un nuevo documento de venta de cartera
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="ventaDocu">DTO que contiene la informacion de la venta docu</param>
        /// <param name="creditos">Lista de creditos pre vendidos</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        Tuple<int, List<DTO_SerializedObject>> PreventaCartera_Add(Guid channel, int documentID, string actFlujoID, bool sendToApprove, DateTime fechaVenta, 
            DTO_VentaCartera ventaCartera);

        #endregion

        #region Venta Cartera

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        [OperationContract]
        DTO_ccVentaDocu ccVentaDocu_GetByID(Guid channel, int NumeroDoc);

        /// <summary>
        /// Funcion que trae los creditos para la venta de cartera
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compradorCarteraID">Comprador de la venta de cartera</param>
        /// <param name="oferta">Oferta de la compra de cartera</param>
        /// <returns></returns>
        [OperationContract]
        DTO_VentaCartera VentaCartera_GetForVenta(Guid channel, string actFlujoID, string compradorCarteraID, string oferta);

        /// <summary>
        /// Funcion que trae los creditos para la venta de cartera
        /// </summary>
        /// <param name="compradorCarteraID">Comprador de la venta de cartera</param>
        /// <param name="oferta">oferta del credito</param>
        /// <param name="mesINI">Mes Inicial de la consulta</param>
        /// <param name="mesFIN">Mes Final de la consulta</param>
        /// <param name="Tipo">Tipo de Consulta</param>
        /// <returns>Lista de Ventas</returns>        
        [OperationContract]
        List<DTO_QueryVentaCartera> VentaCartera_GetForCompradorCart(Guid channel, string compradorCarteraID, string oferta, DateTime mesINI, DateTime mesFIN, TipoVentaCartera tipo);

        /// <summary>
        /// Funcion que trae los creditos vendidos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <returns></returns>
        [OperationContract]
        DTO_VentaCartera VentaCartera_GetByActividadFlujo(Guid channel, string actFlujoID);

        /// <summary>
        /// Funcion que genera un nuevo documento de recompra de cartera con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="ventaCartera">Dto que contiene el documento y el detalle de la venta de cartera</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        Tuple<int, List<DTO_SerializedObject>, string> VentaCartera_Add(Guid channel, int documentID, string actFlujoID, DTO_VentaCartera ventaCartera);

        /// <summary>
        /// Revierte una venta de cartera
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult VentaCartera_Revertir(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region Fondeo Cartera

        /// <summary>
        /// Funcion que guarda los creditos comprados por el fondeador
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compraDocu">DTO del documento a generar</param>
        /// <param name="migracionVenta">Lista con los detalles a guardad</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> FondeoCartera_Add(Guid channel, int documentID, string actFlujoID, DTO_ccCompraDocu compraDocu, List<DTO_MigrarVentaCartera> migracionVenta);

        #endregion

        #region Pago Flujos

        /// <summary>
        /// Funcion que trae los creditos para el pago de flujos
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="fechaPeriodo">Fecha actual del periodo</param>
        /// <returns></returns>
        [OperationContract]
        DTO_PagoFlujos PagoFlujos_GetForPago(Guid channel, string actFlujoID, DateTime fechaPeriodo, string oferta, int? libranza, string comprador);

        /// <summary>
        /// Funcion que genera un nuevo documento de pago de flujos con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="recompraCartera">Dto que contiene el documento y el detalle del pago de flujo</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> PagoFlujos_Add(Guid channel, int documentID, string actFlujoID, string compradorID, DateTime fechaPago, DTO_PagoFlujos pagoFlujos);

        #endregion

        #region Reasigna Compra Cartera

        /// <summary>
        /// Funcion que trae los creditos para reasignar el comprador
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="compradorCarteraID">Comprador de la venta de cartera</param>
        /// <returns></returns>
        [OperationContract]
        DTO_ReasignaCompradorFinal ReasignaCompradorCartera_Get(Guid channel, string actFlujoID, string compradorCarteraID);

        /// <summary>
        /// Funcion que genera un nuevo documento de reasignacion de compradores de cartera con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reasignaCompFinal">Dto que contiene el documento y el detalle del pago de flujo</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> ReasignaCompradorCartera_Add(Guid channel, int documentID, string actFlujoID, DTO_ReasignaCompradorFinal reasignaCompFinal);

        #endregion

        #region Recompra Cartera

        /// <summary>
        /// Funcion que retorna la lista de recompras asignadas a ese comprado
        /// </summary>
        /// <param name="compradorCarteraID">Comprador de cartera</param>
        /// <returns>Lista con las recompras del comprador de cartera</returns>
        [OperationContract]
        List<DTO_ccRecompraDeta> RecompraCartera_GetByCompradorCartera(Guid channel, string compradorCarteraID);

        /// <summary>
        /// Funcion que trae la lista de creditos disponibles para la compra o sustitucion
        /// </summary>
        /// <param name="actFlujoID">Actividad de Flujo</param>
        /// <param name="compradorCarteraID">Id del comprador de la cartera</param>
        /// <returns></returns>
        [OperationContract]
        DTO_RecompraCartera RecompraCartera_GetForCompraAndSustitucion(Guid channel, string compradorCarteraID, List<int> libranzasFilter, ref string msgError);

        /// <summary>
        /// Funcion que genera un nuevo documento de recompra de cartera con sus detalles
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="recompraCartera">Dto que contiene el documento y el detalle de la recompra de cartera</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> RecompraCartera_Add(Guid channel, int documentID, string actFlujoID, bool isIndividual, bool isMaduracionAnt, DTO_RecompraCartera recompraCartera);

        #endregion

        #endregion

        #region Nota Credito

        /// <summary>
        /// Funcion que genera una nota credito de un credito
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="resumenNC">Lista de componentes</param>
        /// <param name="infoCartera">lista de cuotas</param>
        /// <param name="ctrl">documento nota credito</param>
        /// <param name="resintegroSaldo">cuenta para reintegro saldo</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_SerializedObject NotaCredito_Add(Guid channel, int documentID, List<DTO_NotaCreditoResumen> resumenNC, DTO_InfoCredito infoCartera, DTO_glDocumentoControl ctrl, string reintegroSaldo);

        #endregion

        #region Cobro Jurídico y Acuerdos de Pago

        /// <summary>
        /// Trae la info de un historico
        /// </summary>
        /// <param name="numDocCredito">Numero doc del crédito</param>
        /// <param name="withFilter">PErmite filtrar o no la consulta</param>
        /// <returns>retorna los 2 ultimos registros del histórico de CJ</returns>
        [OperationContract]
        Tuple<DTO_ccCJHistorico, DTO_ccCJHistorico> ccCJHistorico_GetForAbono(Guid channel, int numDocCredito, bool withFilter);

        /// <summary>
        /// Trae todos los registros de DTO_ccCarteraDocu segun al cliente, y que tenga un estado de cuenta en cobro juridico
        /// </summary>
        /// <param name="clienteID">Identificador del cliente</param>
        /// <param name="proposito">Indicador del estado de cuenta en cobro juridico</param>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCobroJuridicoByCliente(Guid channel, string clienteID, DateTime fecha);

        /// <summary>
        /// Funcion que genera un nuevo documento de cobro juridico
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="cobroJuridicos">Lista de los creditos a realizar cobro juridico</param>
        /// <param name="NUevoEstadoCart">El nuevo estado en que queda el cliente</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> EnvioCobroJuridico(Guid channel, int documentID, string actFlujoID, string clienteID, DTO_ccCreditoDocu cobroJuridico,
            List<DTO_ccSaldosComponentes> saldosComponentes, DateTime fechaDoc, DateTime fechaMvto, TipoEstadoCartera nuevoEstadoCart);

        /// <summary>
        /// Trae todos los registros del coAuxiliar que tengan cobro juridico y no existan en ccCJHistorico
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCarteraDocu</returns>
        [OperationContract]
        List<DTO_CobroJuridicoAuxiliar> GetCobroJuridicoFromAuxiliar(Guid channel);

        /// <summary>
        /// Funcion que agrega info a ccCJHistorico
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="cobroJuridicos">resumen de los cobrosJuridicos a guardar</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_TxResult> ccCJHistorico_Add(Guid channel, int documentID, List<DTO_CobroJuridicoAuxiliar> cobroJuridicos);

        /// <summary>
        /// Revierte un envío a CJ
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult EnvioCJ_Revertir(Guid channel, int documentID, int numeroDoc);

        /// <summary>
        /// Funcion que genera un nuevo documento de sentencia judicial
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="clienteID">Cliente de la sentencia</param>
        /// <param name="creditoSentencia">credito</param>
        /// <param name="credComponentes">componentes</param>
        /// <param name="fechaDoc">fecha del documento</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> SentenciaJudicial_Add(Guid channel, int documentID, string actFlujoID, DTO_ccCreditoDocu creditoSentencia, TipoEstadoCartera nuevoEst, DateTime fechaDoc, DateTime fechaSentencia);
        #endregion

        #region Reintegros

        #region Pagos Especiales

        /// <summary>
        /// Funcion que trae los creditos para el reintegro a los clientes
        /// </summary>
        /// <param name="componenteCarteraID">Componente que se esta consultando</param>
        /// <returns>Retorna una lista con los creditos que poseen saldo en el componente especificado</returns>
        [OperationContract]
        DTO_SerializedObject PagosEspeciales_GetByComponente(Guid channel, string actFlujoID, string componenteCarteraID);

        /// <summary>
        /// Funcion que trae los creditos para la aprobacion del reintegro a los clientes
        /// </summary>
        /// <param name="componenteCarteraID">Componente que se esta consultando</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna una lista con los creditos que poseen saldo en el componente especificado</returns>
        [OperationContract]
        DTO_SerializedObject PagosEspeciales_GetAprobByComponente(Guid channel, string actFlujoID, string componenteCarteraID);

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> PagosEspeciales_Aprobar(Guid channel, int documentID, string actFlujoID, string headerRsx,
            List<DTO_ccReintegroClienteDeta> reintegrosClientes);

        #endregion

        #region Reintegros

        /// <summary>
        /// Funcion que trae los terceros para el reintegro a los clientes
        /// </summary>
        /// <param name="cuentaID">Cuenta que se esta consultando</param>
        /// <returns>Retorna una lista con los terceros que poseen saldo en la especificada</returns>
        [OperationContract]
        List<DTO_ccReintegroClienteDeta> ReintegroClientes_GetByCuenta(Guid channel, string cuentaID);

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="fechaReintegro">Fecha en la que se realiza el reintegro</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <param name="vlrTotalReintegro">Valor total del reintegro</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> ReintegroClientes_Add(Guid channel, int documentID, string actFlujoID, List<DTO_ccReintegroClienteDeta> reintegrosClientes,
            DateTime fechaReintegro, decimal vlrTotalReintegro, bool isGiroAsociado, string reintegroSaldo);

        /// <summary>
        /// Funcion que trae los creditos para la aprobacion del reintegro a los clientes
        /// </summary>
        /// <param name="componenteCarteraID">Componente que se esta consultando</param>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna una lista con los creditos que poseen saldo en el componente especificado</returns>
        [OperationContract]
        List<DTO_ccReintegroClienteDeta> ReintegroClientes_GetAprobByCuenta(Guid channel, string actFlujoID, string cuentaID);

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> ReintegroClientes_AprobarGiro(Guid channel, int documentID, string actFlujoID, string headerRsx,
            List<DTO_ccReintegroClienteDeta> reintegrosClientes);

        /// <summary>
        /// Funcion que genera un nuevo documento de reintegro a clientes con su detalle
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="reintegrosClientes">Lista de los reintegros a realizar</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> ReintegroClientes_AprobarAjuste(Guid channel, int documentID, string actFlujoID, string headerRsx,
            List<DTO_ccReintegroClienteDeta> reintegrosClientes);


        #endregion

        #endregion

        #region Sustituciones

        /// <summary>
        /// Valida que la información basica de la sustitución de créditos
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult SustitucionCreditos_Validar(Guid channel, int documentID, DateTime fecha, ref List<DTO_ccSustitucionCreditos> data);

        /// <summary>
        /// Procesa la sustitución de créditos
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult SustitucionCreditos_Procesar(Guid channel, int documentID, DateTime fecha, List<DTO_ccSustitucionCreditos> data);

        #endregion

        #region Liquidacion Comisiones

        /// <summary>
        /// Funcion que trae la lista con las liquidaciones de los asesores
        /// </summary>
        /// <returns>Retorna una lista con los asesores para liquidar las comisiones </returns>
        [OperationContract]
        List<DTO_ccComisionDeta> LiquidacionComisionesCartera_GetForLiquidacion(Guid channel, DateTime fecha);

        /// <summary>
        /// Funcion que trae la lista con las liquidaciones de los asesores
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo del documento</param>
        /// <returns>Retorna una lista con las liquidaciones de comision para aprobacion </returns>
        [OperationContract]
        List<DTO_ccComisionDeta> LiquidacionComisionesCartera_GetForAprobacion(Guid channel, string actFlujoID, DateTime Periodo);

        /// <summary>
        /// Funcion que genera un nuevo documento de venta de cartera
        /// </summary>
        /// <param name="documentID">Id del documento</param>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="liquidaComisiones">DTO con la informacion de las liquidaciones a procesar</param>
        /// <param name="insideAnotherTx">Indicador para establecer si se encuentra dentor de otra transaccion</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_SerializedObject> LiquidacionComisionesCartera_Add(Guid channel, int documentID, string actFlujoID, DTO_LiquidacionComisiones liquidaComisiones);

        /// <summary>
        /// Realiza la aprobacion de la liquidacion de las comisiones
        /// </summary>
        /// <param name="actFlujoID">Actividad de flujo actual</param>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="comisionesDeta">Lista de las comisiones</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> LiquidacionComisionesCartera_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_ccComisionDeta> comisionesDeta);

        #endregion
              
        #endregion

        #region Cartera Financiera

        #region Cierres

        /// <summary>
        /// Realiza el proceso de cierre mensual
        /// </summary>
        [OperationContract]
        DTO_TxResult Proceso_CierreMesCarteraFin(Guid channel, int documentID, DateTime periodo);

        /// <summary>
        /// Realiza el proceso de amortizacion mensual
        /// </summary>
        [OperationContract]
        DTO_TxResult Proceso_AmortizacionMensual(Guid channel, int documentID, DateTime periodo);

        /// <summary>
        /// Realiza el proceso de Cartera en Administracion
        /// </summary>
        [OperationContract]
        DTO_TxResult Proceso_CarteraAdministracion(Guid channel, int documentID, DateTime periodo);


        #endregion

        #region Polizas

        /// <summary>
        /// Obtiene las polizas
        /// </summary>
        /// <param name="poliza">filtro</param>
        /// <returns><Retorna la info filtrada/returns>
        [OperationContract]
        void PolizaEstado_Upd(Guid channel, DTO_ccPolizaEstado poliza);

        /// <summary>
        /// Elimina la poliza pedida
        /// </summary>
        /// <param name="terceroID">tercero de la poliza</param>
        /// <param name="poliza">Nro de la poliza</param>
        /// <returns></returns>
        [OperationContract]
        void PolizaEstado_Delete(Guid channel, string terceroID, string poliza);

        /// <summary>
        /// Obtiene las polizas
        /// </summary>
        /// <param name="poliza">filtro</param>
        /// <returns><Retorna la info filtrada/returns>
        [OperationContract]
        List<DTO_ccPolizaEstado> PolizaEstado_GetByParameter(Guid channel, DTO_ccPolizaEstado poliza);

        /// <summary>
        /// Renueva una póliza
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <ctrlCredito>Documento control relacionado con el crédito</ctrlCredito>
        /// <creditoPoliza>Credito con las actualizaciones de la poliza</creditoPoliza>
        /// <cuota1Pol>Primera cuota del crédito asignado a la poliza</cuota1Pol>
        /// <cuotasPoliza>Plan de pagos por cada cuota para la poliza</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult RenovacionPoliza(Guid channel, int documentID, int? numDocSolicitud, DTO_ccCreditoDocu credito, DTO_PlanDePagos planPagos, DTO_ccPolizaEstado poliza);

        /// <summary>
        /// Registra o actualiza las polizas de cartera
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="polizaEstado">Encabezado</param>
        /// <param name="detallePoliza">Detalle</param>
        /// <param name="insideAnotherTx">Indica es otra transaccion</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult RegistroPoliza(Guid channel, int documentID, byte tipoMvto, DTO_ccPolizaEstado polizaEstado);

        /// <summary>
        /// Obtiene las polizas de cartera para pagos
        /// </summary>
        /// <returns>Lista</returns>
        [OperationContract]
        List<DTO_ccPolizaEstado> Poliza_GetForPagos(Guid channel);

        /// <summary>
        /// Realiza los pagos de poliza con autorizacion de Giro(CxP)
        /// </summary>
        /// <param name="documentID">doc actual</param>
        /// <param name="pagos">lista de pagos</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult PagoPolizasCartera(Guid channel, int documentID, DateTime fechaDoc, List<DTO_ccPolizaEstado> pagos, string aseguradora);

        /// <summary>
        /// Trae la ultima poliza de un crédito en cobro jurídico
        /// </summary>
        /// <param name="numDocCredito">Identificador único del crédito</param>
        /// <returns>Información de la póliza</returns>
        [OperationContract]
        DTO_ccPolizaEstado PolizaEstado_GetLastPoliza(Guid channel, int numDocCredito, int libranza);

        /// <summary>
        /// Trae la info de un credito segun el cliente
        /// </summary>
        /// <param name="cliente">Identificador del cliente</param>
        /// <returns><Retorna la info de un credito/returns>
        [OperationContract]
        List<DTO_ccCreditoDocu> GetCreditosForRenovacionPoliza(Guid channel, string cliente);

        /// <summary>
        /// Renueva una póliza de cobro Juridico
        /// </summary>
        /// <documentID>Identificador del socumento</documentID>
        /// <poliza>Poliza a renovar</cuotasPoliza>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult CobroJuridicoRenovacionPoliza_Add(Guid channel, int documentID, DTO_ccPolizaEstado poliza);

        /// <summary>
        /// Revierte la renovacion de poliza
        /// </summary>
        /// <param name="documentID">Documento que genera la reversión</param>
        /// <param name="numeroDoc">Numero doc del pago a revertir</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult RenovacionPoliza_Revertir(Guid channel, int documentID, int numeroDoc);

        #endregion

        #region Creación Créditos

        /// <summary>
        /// Realiza el proceso de liquidacion de cartera financiera
        /// </summary>
        /// <param name="lineaCredID">Identificador de la linea de credito</param>
        /// <param name="valorCredito">Valor solicitado para el credito</param>
        /// <param name="valorPoliza">Valor solicitado para la poliza</param>
        /// <param name="valorFuturo">Valor futuro de la solicitud solo para cooperativas</param>
        /// <param name="vlrGiro">Valor a girar</param>
        /// <param name="plazoCredito">Plazo del credito solicitado</param>
        /// <param name="plazoPoliza">PLazo de la poliza</param>
        /// <param name="edad"> Edad de la persona que solicita el credito</param>
        /// <param name="fechaLiquida">Fecha en la cual se solicita el credito</param>
        /// <param name="fechaCuota1">Fecha de la primera cuota</param>
        /// <param name="interesCredito">Tasa de interes que se aplica para el credito</param>
        /// <param name="interesPoliza">Tasa de interes que se aplica a la poliza</param>
        /// <param name="cta1Poliza">Cuota en la que debe empezar la poliza</param>
        /// <param name="liquidaAll">Indica si debe liquidar Credito y Poliza, o solo Poliza</param>
        /// <returns>Retorna un objeto TxResult si se presenta un error, de lo contrario devuelve un objeto de tipo DTO_PlanDePagos</returns>
        [OperationContract]
        DTO_SerializedObject GenerarPlanPagosFinanciera(Guid channel, string lineaCredID, int valorCredito, int valorPoliza, int valorFuturo,int vlrGiro, int plazoCredito, int plazoPoliza,
            int edad, DateTime fechaLiquida, DateTime fechaCuota1, decimal interesCredito, decimal interesPoliza, int cta1Poliza, int vlrCuotaPol, bool liquidaAll,
            List<DTO_Cuota> cuotasExtras, Dictionary<string, decimal> compsNuevoValor, int numDocCredito, string tipoCredito, bool excluyeCompInvisibleInd);

        /// <summary>
        /// Actualiza la informacion de la vaibilidad
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccione</param>
        /// <param name="actFlujoID">Actividad de flujo</param>
        /// <param name="solicitudes">Solicitudes que se estan modificando</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> SolicitudFin_AprobarRechazar(Guid channel, int documentID, string actFlujoID, List<DTO_ccSolicitudDocu> solicitudes, List<DTO_ccSolicitudAnexo> anexos);

        #endregion

        #region Pagos

        /// <summary>
        /// Valida que la información basica de la migracion nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="fechaNomina">Fecha de los recaudos</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult RecaudosMasivosFin_Validar(Guid channel, int documentID, DateTime fechaNomina, string pagaduriaID, ref List<DTO_ccIncorporacionDeta> data);

        /// <summary>
        /// Procesa la migracion de nomina
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="data">Información a migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult RecaudosMasivosFin_Procesar(Guid channel, int documentID, DateTime periodo, List<DTO_ccIncorporacionDeta> data,string pagaduria);

        /// <summary>
        /// Trae la relación de pagos de un recauso masivo
        /// </summary>
        /// <param name="data">Datos a migrar</param>
        /// <returns>Retorna la lista de resultados</returns>
        [OperationContract]
        DataTable RecaudosMasivos_GetRelacionPagos(Guid channel, int documentID, List<DTO_ccIncorporacionDeta> data);

        #endregion

        #region Cobro Juridico
        /// <summary>
        /// Actualiza la info del cobro juridico
        /// </summary>
        /// <param name="fechaCorte">Fecha de corte</param>
        /// <param name="libranza">Identificador de la libranza</param>
        [OperationContract]
        void ccCJHistorico_RecalcularInteresCJ(Guid channel, DateTime fechaCorte, int libranza);
        #endregion

        #endregion

    }
}
