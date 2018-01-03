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
using NewAge.Librerias.Project;

namespace NewAge.Server.NominaService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface INominaService
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

        #region Nomina

        #region Aprobaciones - Pagos

        #region Aprobacion Nomina

        /// <summary>
        /// Aprueba ó Rechaza una solicitud de Pago de Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        [OperationContract]
        DTO_TxResult Nomina_AprobarLiquidacion(Guid channel, List<DTO_noNominaPreliminar> liquidaciones, string actividadFlujoID);

        #endregion

        #region Aprobacion Otros

        /// <summary>
        /// Trae las liquidaciones hacia Terceros
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="documentID">Identificador de Documento</param>
        /// <param name="Periodo">Periodo</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_noLiquidacionOtro> Nomina_GetLiquidacionOtros(Guid channel, List<int> documents, DateTime Periodo);


        /// <summary>
        /// Aprueba una solicitud de Pago a Otros
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="terceros">listado de terceros</param>
        /// <returns>objeto de resultado</returns>
        [OperationContract]
        DTO_TxResult Nomina_AprobarPagosTerceros(Guid channel, List<DTO_NominaPlanillaContabilizacion> terceros);

        #endregion

        #region Pagos Nomina

        /// <summary>
        /// Realiza el Pago de la Nomina
        /// </summary>
        /// <param name="liquidaciones">listado de documentos</param>
        /// <param name="insideAnotherTx">indica si viene o no de una transacción</param>
        /// <returns>objeto de resultado</returns>
        [OperationContract]
        DTO_TxResult Nomina_PagoNomina(Guid channel, int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones, string actividadFlujoID);

        /// <summary>
        /// Procesa el Pago de la Nomina detallando el comprobante de tesoreria por empleado
        /// </summary>
        /// <param name="channel">canal</param>
        /// <param name="documentoID">documento de Pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="fechaDoc">Fecha Documento</param>
        /// <param name="liquidaciones">liquidaciones</param>
        /// <returns>resultado</returns>
        [OperationContract]
        List<DTO_TxResult> Nomina_PagoNominaDetallada(Guid channel, int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_noPagoLiquidaciones> liquidaciones);

        #endregion

        #endregion

        #region Aumento Salario

        /// <summary>
        /// Actualiza el Ajuste de Salario en la tabla empleados y crea la novedad de contrato
        /// </summary>
        /// <param name="lSalarios">listado de ajustes en salarios</param>
        /// <param name="insideAnotherTx">verifica si viene de una transacción</param>
        /// <returns>listado de resultados</returns>
        [OperationContract]
        DTO_TxResult Nomina_UpdSalarioEmpleado(Guid channel, List<DTO_AumentoSalarial> lSalarios, bool insideAnotherTx);

        #endregion

        #region Cesantias e Intereses de Cesantias

        [OperationContract]
        void UpdateCesantias(Guid channel, int numeroDoc, decimal valorCesantias, decimal valorIntereses, bool indCesantias);

        #endregion

        #region Compensación Flexible

        /// <summary>
        /// Obtiene el listado de beneficios por Empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de beneficios</returns>
        [OperationContract]
        List<DTO_noBeneficiosxEmpleado> Nomina_GetBeneficioXEmpleado(Guid channel, string empleadoID);

        /// <summary>
        /// Adiciona un listado de beneficios por empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="lbeneficios">listado de beneficios</param>
        /// <param name="insideAnotherTx">verifica si viene ó no de una transacción</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_TxResult Nomina_AddBeneficioXEmpleado(Guid channel, List<DTO_noBeneficiosxEmpleado> lbeneficios, bool insideAnotherTx);

        #endregion

        #region Compensatorios

        /// <summary>
        /// Obtiene el historico de compensatorios
        /// </summary>
        /// <returns>listado de compensatorios</returns>
        [OperationContract]
        List<DTO_noCompensatorios> Nomina_GetCompensatorios(Guid channel);

        /// <summary>
        /// Actualiza la informacion del compensatorio
        /// </summary>
        /// <param name="compesatorio">objeto compensatorio</param>
        /// <returns>true si la operacion es exitosa</returns>
        [OperationContract]
        DTO_TxResult Nomina_UpdCompensatorio(Guid channel, List<DTO_noCompensatorios> compesatorio);

        #endregion

        #region Contabilizacion

        /// <summary>
        /// Obtiene el consolidado de las liquidacion de nomina X periodo
        /// </summary>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_NominaContabilizacion> noLiquidacionesDocu_GetTotal(Guid channel, DateTime periodo);


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
        [OperationContract]
        List<DTO_noEmpleado> Nomina_noEmpleadoGet(Guid channel, int documentoID, DateTime periodo, byte estadoLiquidacion, bool procesadaInd, byte estadoEmpleado);

        /// <summary>
        /// Lista los empleados segun el estado
        /// </summary>
        /// <param name="activoInd">estado</param>
        /// <param name="empleado">empleado</param>
        /// <returns>lista de empleados</returns>
        [OperationContract]
        List<DTO_noEmpleado> Nomina_SearchEmpleados(Guid channel, bool activoInd, string empleado);


        /// <summary>
        /// Reincorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        [OperationContract]
        DTO_TxResult Nomina_ReinCorporacionEmpleado(Guid channel, DTO_noEmpleado empleado);

        /// <summary>
        /// Incorpora un empleado a la empresa
        /// </summary>
        /// <param name="empleado">objeto empleado</param>
        /// <param name="insideAnotherTx">indica si viende de una transacción</param>
        /// <returns>restulado de la transacción</returns>
        [OperationContract]
        DTO_TxResult Nomina_IncorporacionEmpleado(Guid channel, DTO_noEmpleado empleado);

        /// <summary>
        /// Trae el estado de las liquidaciones del empleado del periodo de liquidación en curso
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="empleadoID">empleadoID</param>
        /// <returns>Estado Liquidaciones</returns>
        [OperationContract]
        DTO_noEstadoLiquidaciones Nomina_GetEstadoLiquidaciones(Guid channel, string empleadoID);

        /// <summary>
        /// dice si un tercero esta repetido o no 
        /// </summary>
        /// <param name="empleadoID">tercero</param>
        /// <returns>un contador con la cantidad de veces que el tercero se repitio</returns>
        [OperationContract]
        int noEmpleado_CountTerceroID(Guid channel, string tercero);
        #endregion

        #region Liquidacion Documento

        /// <summary>
        /// Obtiene la liquidaciones del empleado asociadas al tipo de documento a liquidar
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="documentoID">identificador del documento</param>
        /// <param name="empleado">empleado</param>
        /// <returns>listado de liquidaciones </returns>
        [OperationContract]
        List<DTO_noLiquidacionesDocu> Nomina_GetLiquidacionesDocu(Guid channel, int documentoID, DateTime periodo, DTO_noEmpleado empleado);

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <returns>listado de liquidaciones aprobadas para generar pago</returns>
        [OperationContract]
        List<DTO_noPagoLiquidaciones> Nomina_NominaPagosGet(Guid channel, string actividadFlujoID, DateTime periodo);


        /// <summary>
        /// Obtiene el periodo de vacaciones a liquidar
        /// </summary>
        /// <param name="empleadoId">identificador del empleado</param>
        /// <param name="estado">estado de la liquidacion</param>
        [OperationContract]
        List<DTO_noLiquidacionVacacionesDeta> Nomina_GetPeriodoVacaciones(Guid channel, string empleadoId, bool estado);


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
        [OperationContract]
        DTO_noNominaDefinitiva Nomina_NominaDefinitivaGet(Guid channel, int documentoId, DateTime periodo, DTO_noEmpleado empleado);

        /// <summary>
        ///  Obtiene el detalle para efectos de Pago de Nomina
        /// </summary>
        /// <param name="periodo">Periodo de Nomina</param>
        /// <param name="empleadoId">Identificador de Empleado</param>
        /// <returns>Listado Detalle</returns>
        [OperationContract]
        List<DTO_noLiquidacionesDetalle> Nomina_GetDetallePago(Guid channel, int documentoID, DateTime periodo, string empleadoId);

        #endregion

        #region Liquidacion Preliminar

        /// <summary>
        /// Obtiene listado de detalle liquidacion preliminar (Prenomina)
        /// </summary>
        /// <returns>Listado de detalles liquidacion</returns>
        [OperationContract]
        List<DTO_noLiquidacionPreliminar> Nomina_LiquidacionPreliminarGetAll(Guid channel, int documentoID, DateTime periodo, DTO_noEmpleado empleado);

        /// <summary>
        /// Obtiene el documento Completo de Liquidación
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="documentoId">identificador de documento</param>
        /// <param name="periodo">periodo de liquidacion de nomina</param>
        /// <param name="empleado">empleado</param>
        /// <returns>liquidacion completa</returns>
        [OperationContract]
        List<DTO_noNominaPreliminar> Nomina_NominaPreliminarGet(Guid channel, string actividadFlujoID, DateTime periodo);

        #endregion

        #region Novedades Contrato

        /// <summary>
        /// Lista las novedades de contrato por empleado
        /// </summary>
        /// <param name="empleadoID">identificador empleado</param>
        /// <returns>listado de novedades de contrato</returns>
        [OperationContract]
        List<DTO_noNovedadesContrato> Nomina_GetNovedadesContrato(Guid channel, string empleadoID);

        /// <summary>
        /// Adicona una novedad de contrato 
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la operacion es exitosa</returns>
        [OperationContract]
        DTO_TxResult Nomina_AddNovedadesContrato(Guid channel, List<DTO_noNovedadesContrato> novedades);


        /// <summary>
        /// Elimina una novedad de contrato
        /// </summary>
        /// <param name="novedad">novedad de contrato</param>
        /// <returns>true si la elimina</returns>
        [OperationContract]
        DTO_TxResult Nomina_noNovedadesContrato_Delete(Guid channel, DTO_noNovedadesContrato novedad);


        #endregion

        #region Novedades Nomina

        /// <summary>
        /// lista las novedades de nomina por empleado
        /// </summary>
        /// <param name="empleadoID">identificador de empleado</param>
        /// <returns>listado de novedades</returns>
        [OperationContract]
        List<DTO_noNovedadesNomina> Nomina_GetNovedades(Guid channel, string empleadoID);

        /// <summary>
        /// Agrega las novedades de nomina 
        /// </summary>
        /// <param name="novedades">listado de novedades</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult Nomina_AddNovedadNomina(Guid channel, List<DTO_noNovedadesNomina> novedades);


        /// <summary>
        /// Elimina una novedad de Nomina
        /// </summary>
        /// <param name="novedad">novedad de nomina</param>
        [OperationContract]
        void Nomina_DelNovedadesNomina(Guid channel, DTO_noNovedadesNomina novedad);


        #endregion

        #region Planilla de Aportes


        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        [OperationContract]
        DTO_noPlanillaAportesDeta Nomina_GetPlanillaAportes(Guid channel, string empleadoID, DateTime periodo);

        /// <summary>
        /// Obtiene la Liquidación de la Planilla de Aportes para el Empleado y periodo Actual
        /// </summary>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="periodo">periodo de liquidacion del empleado</param>
        /// <returns>Planilla de Aportes</returns>
        [OperationContract]
        List<DTO_noPlanillaAportesDeta> Nomina_GetAllPlanillaAportes(Guid channel, DateTime periodo);

        /// <summary>
        /// Actualiza listado de planillas de aportes modificadas
        /// </summary>
        /// <param name="lplanilla">listado de planillas</param>
        /// <param name="insideAnotherTx">si viene de alguna tx</param>
        /// <returns>resultado</returns>
        [OperationContract]
        DTO_TxResult Nomina_PlanillaAportesDeta_Upd(Guid channel, List<DTO_noPlanillaAportesDeta> lplanilla);

        /// <summary>
        /// Proceso trae valores planilla X tercero
        /// </summary>
        [OperationContract]
        List<DTO_NominaPlanillaContabilizacion> noPlanillaAportesDeta_GetValoreXTercero(Guid channel, bool isPlanilla);


        #endregion

        #region Planillas diarias trabajo

        /// <summary>
        /// Obtiene las planillas diarias de trabajos asociadas al empleado
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de planillas</returns>
        [OperationContract]
        List<DTO_noPlanillaDiariaTrabajo> Nomina_GetPlanillaDiaria(Guid channel, string empleadoID, DateTime periodo, Int16 contratoNo);

        /// <summary>
        /// Adiciona una planilla diaria de trabajo
        /// </summary>
        /// <param name="prestamo">planilla</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        [OperationContract]
        DTO_TxResult Nomina_AddPlanillaDiaria(Guid channel, List<DTO_noPlanillaDiariaTrabajo> planillas);

        #endregion

        #region Prestamos

        /// <summary>
        /// Obtiene prestamos asociados al empleados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de prestamos</returns>
        [OperationContract]
        List<DTO_noPrestamo> Nomina_GetPrestamos(Guid channel, string empleadoID);

        /// <summary>
        /// Adiciona un Prestamo
        /// </summary>
        /// <param name="prestamo">objeto prestamo</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        [OperationContract]
        DTO_TxResult Nomina_AddPrestamo(Guid channel, List<DTO_noPrestamo> prestamos);

        #endregion

        #region Procesos

        /// <summary>
        /// Liquidación Nomina
        /// </summary>
        /// <param name="periodo">perido del documento</param>
        /// <param name="lEmpleados">listado de empleados a liquidar</param>
        /// <param name="batchProgress">barra de progreso</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_TxResult> LiquidarNomina(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados);


        /// <summary>
        /// Liquidación de Contrato 
        /// </summary>
        /// <param name="periodo">Periodo de Documento</param>
        /// <param name="lEmpleados">Listado de Empleados</param>
        /// <param name="fechaRetiro">Fecha de Retiro</param>
        /// <param name="causa">Causa Liquidación</param>
        /// <param name="batchProgress">barra de Progreso</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_TxResult> LiquidarContrato(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaRetiro, int causa);

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
        [OperationContract]
        List<DTO_TxResult> LiquidarCesantias(Guid channel, DateTime periodo, DateTime fechaDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, DateTime fechaPago, string resolucion, TipoLiqCesantias tLiqCesantias, List<DTO_noEmpleado> lEmpleados);

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
        [OperationContract]
        List<DTO_TxResult> LiquidarPrima(Guid channel, DateTime periodo, DateTime fechaDoc, DateTime fechaIniLiq, DateTime fechaFinLiq, bool incNomina, List<DTO_noEmpleado> lEmpleados);

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
        [OperationContract]
        List<DTO_TxResult> LiquidarVacaciones(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados, DateTime fechaIniLiq, DateTime fechaFinLiq,
                                                     int diasVacTiempo, int diasVacDinero, bool indIncNomina, bool indPrima, string resolucion, DateTime fechaPagoLiq, DateTime fechaIniPagoLiq, DateTime fechaIniPendVacac, DateTime fechaFinPendVacac);

        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        [OperationContract]
        List<DTO_TxResult> LiquidarProvisiones(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados);


        /// <summary>
        ///  Liquida las Provisiones
        /// </summary>
        /// <param name="channel">canal de usuario</param>
        /// <param name="periodo">periodod de liquidación</param>
        /// <param name="lEmpleados">lista de empleados</param>
        /// <param name="batchProgress">progreso</param>
        /// <returns>listad de resultados</returns>
        [OperationContract]
        List<DTO_TxResult> LiquidarPlanilla(Guid channel, DateTime periodo, DateTime fechaDoc, List<DTO_noEmpleado> lEmpleados);

        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lEmpleados">liquidaciones Empleados</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_MigracionNomina(Guid channel, int documentoID, List<DTO_noMigracionNomina> lEmpleados);


        /// <summary>
        /// Migración Información Historica Nomina
        /// </summary>
        /// <param name="lProvisiones">Provisiones</param>
        /// <param name="batchProgress">barra de Progresp</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_MigracionProvisiones(Guid channel, int documentoID, List<DTO_noProvisionDeta> lProvisiones);


        /// <summary>
        /// Calculo los días reales de vacaciones del empleado
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="empleadoID">identificador del empleado</param>
        /// <param name="fechaIni">fecha inicial vacaciones</param>
        /// <param name="fechaFin">fecha final vacaciones</param>
        /// <returns>número de días de vacaciones</returns>
        [OperationContract]
        int CalcularDiasVacaciones(Guid channel, string empleadoID, DateTime fechaIni, DateTime fechaFin, out decimal diasCausados);

        /// <summary>
        /// Contabiliza la Nomina para el Periodo Actual
        /// </summary>
        /// <param name="documentoID">identifidor del documento</param>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="liquidaciones">liquidaciones</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_ContabilizarNomina(Guid channel, int documentoID, DateTime periodo, DateTime fechaDoc, List<DTO_NominaContabilizacionDetalle> liquidaciones,byte procesarSel);
        
        /// <summary>
        /// Proceso de contabilización de la Planilla de Aportes
        /// </summary>
        /// <param name="documentoID">identificador del documento de Pagos</param>
        /// <param name="periodo">Periodo actual de la Nomina</param>
        /// <param name="liquidaciones">liquidaciones de la Planilla</param>
        /// <returns>listado de resultados</returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_ContabilizarPlanilla(Guid channel, int documentoID, DateTime periodo, List<DTO_noPlanillaAportesDeta> liquidaciones);

        /// <summary>
        /// Contabiliza las Provisiones para el Periodo Actual
        /// </summary>
        /// <param name="periodo">Periodo de la Nomina</param>
        /// <param name="batchProgress">Barra de Progreso</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_ContabilizarProvisiones(Guid channel, int documentoID, DateTime periodo, List<DTO_noProvisionDeta> liquidaciones);

        /// <summary>
        /// Obtiene los documentos de liquidación aprobados por periodo
        /// </summary>
        /// <param name="documentoId">identificador del documento</param>
        /// <param name="periodo">periodo</param>
        /// <param name="tipoLiquida">Tipo de liquidacion</param>
        /// <returns>listado de liquidaciones aprobadas para generar boletas pago</returns>
        [OperationContract]
        List<DTO_NominaEnvioBoleta> Proceso_noLiquidacionesDocu_GetNominaLiquida(Guid channel, int documentoID, DateTime periodo, byte tipoLiquida);

        #endregion

        #region Provisiones

        /// <summary>
        /// Obtiene un listado de la liquidación de Provisiones del empleado en el periodo
        /// </summary>
        /// <param name="channel">canal del usuario</param>
        /// <param name="periodo">periodo</param>
        /// <param name="contratoNOID">número contrato empleado</param>
        /// <returns>lista de provisiones</returns>
        [OperationContract]
        List<DTO_noProvisionDeta> Nomina_ProvisionDeta_Get(Guid channel, DateTime periodo, int contratoNOID);

        #endregion

        #region Traslados

        /// <summary>
        /// Obtiene los traslados
        /// </summary>
        /// <param name="empleadoID">identicador de empleados</param>
        /// <returns>listado de traslados</returns>
        [OperationContract]
        List<DTO_noTraslado> Nomina_GetTraslados(Guid channel, string empleadoID);

        /// <summary>
        /// Adiciona un traslado
        /// </summary>
        /// <param name="prestamo">objeto traslado</param>
        /// <returns>indica si la operacion se realizo con exito</returns>
        [OperationContract]
        DTO_TxResult Nomina_AddTraslado(Guid channel, List<DTO_noTraslado> traslados);

        #endregion

        #region Reportes
        /// <summary>
        /// Envía la información correspondiente 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc"></param>
        [OperationContract]
        void Print(Guid channel, int numeroDoc, DateTime periodo, List<DTO_MasterBasic> lEmpleados, List<DTO_noLiquidacionPreliminar> _ldetalle);

        /// <summary>
        /// Funcion que llama la funcion para enviar a imprimir el reporte de Liquidacion de vacaciones
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="numeroDoc">Numero de documento</param>
        /// <param name="empleado">dto_noEmpleado</param>
        /// <param name="_ldetalle">Lista de detalles de DTO_noLiquidacionPreliminar</param>
        [OperationContract]
        void PrintVacaciones(Guid channel, int numeroDoc, DTO_noEmpleado empleado, DTO_noLiquidacionesDocu liquidacion, List<DTO_noLiquidacionPreliminar> _lDetalles, bool isApro);

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
        [OperationContract]
        DataTable Reportes_No_NominaToExcel(Guid channel, int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string empleadoID, string operacionNoID,
                                                         string conceptoNoID, string areaFuncID, string fondoID, string cajaID, string terceroID, object otroFilter, byte? agrup, byte? romp);


        /// <summary>
        /// Genera un archivo de las planillas de los empleados
        /// </summary>
        /// <param name="rutaArchivo">Ruta de archivo a guardar</param>
        /// <param name="planillas">Planillas a migrar</param>
        /// <returns>nombre del archivo</returns>
        [OperationContract]
        string Reportes_No_GenerarArchivoPlanilla(Guid channel, string rutaArchivo, List<DTO_noPlanillaAportesDeta> planillas);

        #endregion

        #region Reversiones

        /// <summary>
        /// Proceso para revertir la liquidación de la Nomina
        /// </summary>
        /// <param name="documentoID">Documento de Nomina</param>
        /// <param name="periodo">Periodo de Liquidacion</param>
        /// <param name="lEmpleados">lista de Empleados</param>
        /// <param name="batchProgress">Barra de progreso</param>
        [OperationContract]
        DTO_TxResult RevertirLiqNomina(Guid channel, int documentoID, DateTime periodo, List<DTO_noEmpleado> lEmpleados);


        #endregion

        #region Documento Vacaciones
        /// <summary>
        /// Consulta por empleado
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ReportVacacionesDocumento> Report_No_GetVacacionesByEmpleado(Guid channel, string _empleadoID);

        /// <summary>
        /// Consulta de por empleado para traer las fechas y utilizarlas como filtro.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="_empleadoID"></param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_ReportVacacionesDocumento> Report_No_GetLiquidaContratoByEmpleado(Guid channel, string _empleadoID);
        #endregion


        #endregion

    }

}
