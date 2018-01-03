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

namespace NewAge.Server.ContabilidadService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface IContabilidadService
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

        #region Contabilidad

        #region Cont - General y Procesos

        /// <summary>
        /// Genera los comprobantes y saldos para el ajuste en cambio
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="AreaFuncionalID">Area funcional desde ka cual se ejecuta el proceso (la del usuario)</param>
        /// <param name="ndML">Numero de documento de glDocumentoControl ML</param>
        /// <param name="ndME">Numero de documento de glDocumentoControl ME</param>
        /// <returns></returns>
        [OperationContract]
        Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> Proceso_AjusteEnCambio(Guid channel, int documentID, string actividadFlujoID, string areaFuncionalID,
            DateTime periodo, string libroID);

        /// <summary>
        /// Procesa el ajuste en cambio para un periodo seleccionado
        /// </summary>
        /// <param name="comps">Comprobantes para aprobar</param>
        /// <param name="periodo">Periodo del ajuste</param>
        /// <returns>Retorna el resultado de las operaciones</returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_ProcesarBalancePreliminar(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, 
            DateTime periodo, string libroID);

        /// <summary>
        /// Actualiza el valor de la cuenta alterna en las tablas de coBalance coCuentaSaldo y coAuxiliar
        /// </summary>
        [OperationContract]
        DTO_TxResult Proceso_CuentaAlterna(Guid channel, int documentID, string actividadFlujoID);

        /// <summary>
        /// Proceso de prorrateo IVA
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Proceso_ProrrateoIVA(Guid channel, int documentID, string actividadFlujoID);

        /// <summary>
        /// Proceso para consolidar balances entre empresas
        /// </summary>
        /// <param name="documentID">Identificador del documento que genera el proceso</param>
        /// <param name="list">Lista de empresas a consolidar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> Proceso_ConsolidacionBalances(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteConsolidacion> list);

        /// <summary>
        /// Reclasifica un libro fiscal 
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="libroID">Identificador del libro fiscal</param>
        /// <returns>Retorna el resultado de la operación</returns>
        [OperationContract]
        DTO_TxResult Proceso_ReclasificacionLibros(Guid channel, int documentID, DateTime periodoID, string libroID);

        #endregion

        #region Ajuste Comprobantes

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        DTO_Comprobante AjusteComprobante_Get(Guid channel, DateTime periodo, string comprobanteID, int compNro);

        /// <summary>
        /// Ajusta un comprobante existente
        /// </summary>
        /// <param name="documentID">documento Id</param>
        /// <param name="comp">Comprobante a ajustar</param>
        /// <param name="insideAnotherTx">determina si viene de una transaccion</param>
        [OperationContract]
        DTO_TxResult AjusteComprobante_Generar(Guid channel, int documentID, string actividadFlujoID, DTO_Comprobante comp);

        /// <summary>
        /// Elimina un ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult AjusteComprobante_Eliminar(Guid channel, int documentID, string actividadFlujoID, int numeroDoc);

        /// <summary>
        /// Trae un listado de los ajustes pendientes de aprobar
        /// </summary>
        /// <returns>Retorna una lista de comprobantes de ajuste</returns>
        [OperationContract]
        List<DTO_ComprobanteAprobacion> AjusteComprobante_GetPendientes(Guid channel, string actividadFlujoID);

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> AjusteComprobante_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps);

        #endregion

        #region Comprobantes

        #region AuxiliarPre

        /// <summary>
        /// Indica si hay un auxiliarPre
        /// </summary>
        /// <param name="empresaID">Codigo de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        bool ComprobantePre_Exists(Guid channel, int documentID, DateTime periodo, string comprobanteID, int compNro);

        /// <summary>
        /// Indica si hay un comprobante en auxiliarPre
        /// </summary>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <returns>Retorna un entero con la cantidad de comprobantes que hay en aux con un comprobanteID</returns>
        [OperationContract]
        bool ComprobanteExistsInAuxPre(Guid channel, string comprobanteID);

        /// <summary>
        /// Agrega un comprobante
        /// </summary>
        /// <param name="comprobante">Comprobante con los datos</param>
        /// <param name="areaFuncionalID">Identificador del area funcional</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="numeroDoc">Pk de glDocumentoControl (Null: El comprobante es el encargado de generar el registro)</param>
        /// <param name="numComp">Numero del comprobante generado (en caso de ser nuevo)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult ComprobantePre_Add(Guid channel, int documentoID, ModulesPrefix currMod, DTO_Comprobante comprobante, string areaFuncionalID, string prefijoID, int? numeroDoc, DTO_coDocumentoRevelacion revelacion);

        /// <summary>
        /// Elimina un auxiliar (pre) y crea el registro vacio
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        [OperationContract]
        void ComprobantePre_Delete(Guid channel, int documentID, string actividadFlujoID, DateTime periodo, string comprobanteID, int compNro);

        /// <summary>
        /// Envia para aprobacion un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        [OperationContract]
        DTO_SerializedObject ComprobantePre_SendToAprob(Guid channel, int documentID, string actividadFlujoID, ModulesPrefix currentMod, DateTime periodo, string comprobanteID, int compNro, bool createDoc);

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        List<DTO_ComprobanteAprobacion> ComprobantePre_GetPendientesByModulo(Guid channel, ModulesPrefix mod, string actividadFlujoID);

        #endregion

        #region Auxiliar

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual a correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        List<DTO_BitacoraSaldo> Comprobante_GetByIdentificadorTR(Guid channel, DateTime periodo, long identTR);

        /// <summary>
        /// Obtiene el numero de registros de un comprobante
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        int Comprobante_Count(Guid channel, bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, DTO_glConsulta consulta = null);

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves (si tiene el numero de documento busca las CxP)
        /// </summary>
        /// <param name="numDoc">Numero de documento de busqueda</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        DTO_Comprobante Comprobante_GetAll(Guid channel, int numDoc, bool isPre, DateTime periodo, string comprobanteID, int? compNro);

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        [OperationContract]
        DTO_Comprobante Comprobante_Get(Guid channel, bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, int? pageSize, int? pageNum, DTO_glConsulta consulta = null);

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_SerializedObject> Comprobante_AprobarRechazar(Guid channel, int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc);

        /// <summary>
        /// Genera una lista de comprobantes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="comps">Lista de comprobantes</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="borraInfoPeriodo">Inidca si se debe borrar la información del periodo</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        List<DTO_TxResult> Comprobante_Migracion(Guid channel, int documentID, DateTime periodo, List<DTO_Comprobante> comps, string areaFuncionalID, 
            string prefijoID, bool borraInfoPeriodo);

        /// <summary>
        /// Realiza la reclasificacion de saldos
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="numeroDoc">Numero de documento (PK)</param>
        /// <param name="proyectoID">Identificador del nuevo proyecto</param>
        /// <param name="ctoCostoID">Identificador del centro de costo</param>
        /// <param name="lgID">Identificador del lugar geografico</param>
        /// <param name="obs">Observacion del documento</param>
        /// <returns>Retorna el resultado del proceso</returns>
        [OperationContract]
        DTO_TxResult Comprobante_ReclasificacionSaldos(Guid channel, int documentID, string actividadFlujoID, int numeroDoc, string proyectoID, string ctoCostoID, string lgID, string obs);

        /// <summary>
        /// Trae el valor de las cuentas de costo
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        [OperationContract]
        decimal Comprobante_GetValorByCuentaCosto(Guid channel, int numeroDoc);

        /// <summary>
        /// Trae las transferencias bancarias por tercero
        /// </summary>
        /// <param name="terceroID">tercero a validar</param>
        /// <param name="docTercero">numero de la factura</param>
        /// <returns>lista de comp</returns>
        [OperationContract]
        DTO_Comprobante Comprobante_GetTransfBancariaByTercero(Guid channel, string terceroID, string docTercero);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo Inicial</param>
        /// <param name="periodoFinal">periodo final</param>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de auxiliares</returns>
        [OperationContract]
        List<DTO_QueryMvtoAuxiliar> Comprobante_GetAuxByParameter(Guid channel, DateTime? periodoInicial, DateTime? periodoFinal, DTO_QueryMvtoAuxiliar filter);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo</param>
        /// <param name="lugarxDef">lugarGeo</param>
        /// <returns>Lista de auxiliares</returns>
        [OperationContract]
        List<DTO_PagoImpuesto> Comprobante_GetAuxForImpuesto(Guid channel, DateTime periodoFilter);

        #endregion

        #endregion

        #region Cierres

        /// <summary>
        /// Obtiene ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <returns>Retorna periodo o null si no existe alguno</returns>
        [OperationContract]
        DateTime? GetUltimoMesCerrado(Guid channel, ModulesPrefix mod);

        /// <summary>
        /// Indica si el periodo enviado es el ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna True si el aperiodo se puede abrir de lo contrario false</returns>
        [OperationContract]
        bool UltimoMesCerrado(Guid channel, ModulesPrefix mod, DateTime periodo);

        /// <summary>
        /// Abre un nuevo mes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo para abrir</param>
        /// <param name="modulo">Modulo que se desea abrir</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult Proceso_AbrirMes(Guid channel, int documentID, DateTime periodo, ModulesPrefix modulo);

        /// <summary>
        /// Hace el cierre mensual
        /// </summary>
        /// <param name="empresa">Empresa</param>
        /// <param name="periodo">Periodo a cerrar</param>
        /// <param name="modulo">Módulo a cerrar</param>
        /// <param name="userId">Id del usuario</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult Proceso_CierrePeriodo(Guid channel, int documentID, DateTime periodo, ModulesPrefix modulo);

        /// <summary>
        /// Hace el cierre anual
        /// </summary>
        /// <param name="empresa">Empresa</param>
        /// <param name="year">Año a cerrar</param>
        /// <param name="userId">Usuario que hace el cierre</param>
        /// <returns></returns>
        [OperationContract]
        Tuple<DTO_TxResult, DTO_ComprobanteAprobacion> Proceso_CierreAnual(Guid channel, int documentID, string actividadFlujoID, string areaFuncionalID, int year, string libroID);

        /// <summary>
        /// Carga la Informacion del cierres mensual
        /// </summary>
        /// <param name="channel">Canal de transmicion de Datos</param>
        /// <param name="año">Periodo en que se cerro</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coCierreMes> coCierreMes_GetAll(Guid channel, Int16 año);

        /// <summary>
        ///  Carga la información para hacer un cierre Mensual
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coCierreMes> coCierreMes_GetByParameter(Guid channel, DTO_coCierreMes filter, RompimientoSaldos? romp1, RompimientoSaldos? romp2);

        #endregion

        #region CruceCuentas(Ajuste Saldos)

        /// <summary>
        /// Creal el documento Ajuste y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrl">referencia documento</param>
        /// <param name="ajuste">documento Ajuste</param>
        /// <param name="comp">Comprobante</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult CruceCuentas_Ajustar(Guid channel, int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_coDocumentoAjuste ajuste, 
            DTO_Comprobante comp);

        #endregion

        #region Distribucion Comprobante

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetDistribucion(Guid channel);

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coCompDistribuyeExcluye> ComprobanteDistribucion_GetExclusiones(Guid channel);

        /// <summary>
        /// Actualiza la informacion para la distribucion de comprobantes
        /// </summary>
        /// <param name="documentoId">Identificador del documento</param>
        /// <param name="tablas">Registros de distribucion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ComprobanteDistribucion_Update(Guid channel, int documentID, List<DTO_coCompDistribuyeTabla> tablas, List<DTO_coCompDistribuyeExcluye> excluyen);

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetForProcess(Guid channel);

        /// <summary>
        /// Genera los preliminares y revierto los comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="origenes">Lista de comprobantes que se deben distribuir</param>
        /// <param name="periodoIni">Periodo Inicial</param>
        /// <param name="periodoFin">Periodo Final</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> ComprobanteDistribucion_GenerarPreliminar(Guid channel, int documentID, string actividadFlujoID,
            List<DTO_coCompDistribuyeTabla> origenes, DateTime periodoIni, DateTime periodoFin, string libroID);

        #endregion

        #region Impuestos

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="modulo">Modulo que realiza la consulta</param>
        /// <param name="tercero">Tercero que esta ejecutando la consulta</param>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <returns>Retorna una lista de cuentas</returns>
        [OperationContract]
        List<DTO_SerializedObject> LiquidarImpuestos(Guid channel, ModulesPrefix modulo, DTO_coTercero tercero, string cuentaCosto, string conceptoCargoID, string operacionID, string lugarGeoID, string lineaPresID, decimal valor);

        /// <summary>
        /// Trae la lista de declaracion de impuestos para un periodo
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna la lista de declaraciones</returns>
        [OperationContract]
        List<DTO_DeclaracionImpuesto> DeclaracionesImpuestos_Get(Guid channel, DateTime periodo);

        /// <summary>
        /// Trae la lista de renglones de una declaracion
        /// </summary>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <returns>Retorna la lista de renglones</returns>
        [OperationContract]
        List<DTO_coImpDeclaracionDetaRenglon> DeclaracionesRenglones_Get(Guid channel, int numeroDoc, string impuestoID, short mesDeclaracion, short añoDeclaracion);

        /// <summary>
        /// Procesa una declaracion
        /// </summary>
        /// <param name="documentID">Identificador del documnto que genera el proceso</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult ProcesarDeclaracion(Guid channel, int documentID, string impuestoID, short periodoCalendario, short mesDeclaracion, short añoDeclaracion, int? numeroDoc);

        /// <summary>
        /// Trae los detalles de un renglon
        /// </summary>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="renglon">Renglon</param>
        /// <param name="mesDeclaracion">Mes de la declaracion</param>
        /// <param name="añoDeclaracion">Año de la declaracion</param>
        /// <returns>Retorna la lista de cuentas del detalle</returns>
        [OperationContract]
        List<DTO_DetalleRenglon> DetallesRenglon_Get(Guid channel, string impuestoID, string renglon, short mesDeclaracion, short añoDeclaracion);

        #endregion

        #region Mayorizacion

        /// <summary>
        /// Realiza la mayorización de balances de acuerdo a saldos
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="balance">Tipo de balance</param>
        /// <param name="userId">Usuario que realiza la mayorizacion</param>
        /// <param name="empresa">Empresa</param>
        [OperationContract]
        DTO_TxResult Proceso_Mayorizar(Guid channel, int documentID, DateTime periodo, string tipoBalance);

        #endregion

        #region Reclasificaciones Fiscales

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetDistribucion(Guid channel);

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coReclasificaBalExcluye> ReclasificacionFiscal_GetExclusiones(Guid channel);

        /// <summary>
        /// Actualiza la informacion para la reclasificacion de comprobantes
        /// </summary>
        /// <param name="documentoId">Identificador del documento</param>
        /// <param name="tablas">Registros de reclasificacion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult ReclasificacionFiscal_Update(Guid channel, int documentID, List<DTO_coReclasificaBalance> tablas, List<DTO_coReclasificaBalExcluye> excluyen);

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetForProcess(Guid channel, string tipoBalanceID);

        /// <summary>
        /// Procesa la reclasificacion
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        [OperationContract]
        DTO_TxResult ReclasificacionFiscal_Procesar(Guid channel, int documentID, string actividadFlujoID, string tipoBalanceID);

        #endregion

        #region Saldos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        [OperationContract]
        DTO_coCuentaSaldo Saldo_GetByDocumento(Guid channel, string cuentaID, string concSaldo, long identificadorTR, string balanceTipo);


        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        [OperationContract]
        decimal Saldo_GetByDocumentoCuenta(Guid channel, bool isML, DateTime PeriodoID, long identificadorTR, string cuentaID, string libroID);

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="ConceptoSaldoIDNew">Id de concepto saldo nuevo</param>
        /// <param name="conceptoSaldoIDOld">Id de concepto saldo anterior</param>
        /// <param name="cuentaID">Id de la cuenta anterior</param>
        /// <returns>true si existe</returns>
        [OperationContract]
        bool Saldo_ExistsByCtaConcSaldo(Guid channel, string ConceptoSaldoIDNew, string conceptoSaldoIDOld, string cuentaID);

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        [OperationContract]
        decimal Saldo_GetByPeriodoCuenta(Guid channel, bool isML, DateTime PeriodoID, string cuentaID, string libroID);

        /// <summary>
        /// Revisa si un documento ha tenido movimientos de saldos despues de su creación
        /// </summary>
        /// <param name="idTR">Identificador del documento</param>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta del documento</param>
        /// <param name="libroID">Libro de consulta</param>
        /// <returns>Retorna true si ha tenido nuevos movimientos, de lo contrario false</returns>
        [OperationContract]
        bool Saldo_HasMovimiento(Guid channel, int idTR, DateTime periodoID, DTO_coPlanCuenta cta, string libroID);

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        [OperationContract]
        List<DTO_coCuentaSaldo> Saldos_GetByParameter(Guid channel, DTO_coCuentaSaldo filter);

        #endregion

        #region Revelaciones

        /// <summary>
        /// Documento Revelacion
        /// </summary>
        /// <param name="revelacion">objeto Revelacion</param>
        /// <returns></returns>
        [OperationContract]
        DTO_TxResult DocumentoRevelacion_Add(Guid channel, DTO_coDocumentoRevelacion revelacion);
 

         /// <summary>
        /// Obtiene un documento revelación por numero de documento
        /// </summary>
        ///<param name="numeroDoc">número de documento</param>
        ///<returns>Revelación</returns>
        [OperationContract]
        DTO_coDocumentoRevelacion DocumentoRevelacion_Get(Guid channel, int numeroDoc);


        #endregion

        #endregion

    }
}
