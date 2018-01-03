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

namespace NewAge.Server.AppService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información general del sistema 
    /// </summary>
    [ServiceContract]
    public interface IAppService
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

        #region Aplicacion

        #region Alarmas

        /// <summary>
        /// Dice si un usuario tiene alarmas pendientes
        /// </summary>
        /// <returns>Devuelve verdadero si el usuario tiene alarmas</returns>
        [OperationContract]
        bool Alarmas_HasAlarms(Guid channel, string userName);

        /// <summary>
        /// Trae el listado de tareas pendientes para envio de correos
        /// </summary>
        /// <returns>Retorna el listado de tareas pendientes</returns>
        [OperationContract]
        IEnumerable<DTO_Alarma> Alarmas_GetAll(Guid channel, string userName = null);

        #endregion

        #region aplBitacora

        /// <summary>
        /// Consulta la bitácora dado un filtro
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_aplBitacora> aplBitacoraGetFiltered(Guid channel, DTO_glConsulta consulta);

        /// <summary>
        /// Consulta la bitácora dado un filtro y devuelve la cantidad de resultados
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        [OperationContract]
        long aplBitacoraCountFiltered(Guid channel, DTO_glConsulta consulta);

        /// <summary>
        /// Consulta la bitácora dado un filtro por páginas
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_aplBitacora> aplBitacoraGetFilteredPaged(Guid channel, int pageSize, int pageNum, DTO_glConsulta consulta);
        #endregion

        #region aplIdioma

        /// <summary>
        /// Trae todos los Idiomas
        /// </summary>
        /// <returns>Devuelve los Idiomas</returns>
        [OperationContract]
        IEnumerable<DTO_aplIdioma> aplIdioma_GetAll(Guid channel);

        #endregion

        #region aplIdiomaTraduccion

        /// <summary>
        /// Trae la versión de un idioma
        /// </summary>
        /// <returns>Devuelve la versión de un Idioma</returns>
        [OperationContract]
        IEnumerable<DTO_aplIdiomaTraduccion> aplIdiomaTraduccion_GetResources(Guid channel, string idIdioma);

        #endregion

        #region aplMaestraPropiedad

        /// <summary>
        /// Trae toda la informacion de las maestras
        /// </summary>
        /// <returns>Devuelve la lista de maestras</returns>
        [OperationContract]
        IEnumerable<DTO_aplMaestraPropiedades> aplMaestraPropiedad_GetAll(Guid channel);

        #endregion

        #region aplModulo

        /// <summary>
        /// Trae todos los modulos segun si están activos
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DTO_aplModulo> aplModulo_GetByVisible(Guid channel, short visible, bool onlyOperative);

        #endregion

        #region AplReporte

        /// <summary>
        /// Obtiene un resporte predefinido para una empresa
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        [OperationContract]
        byte[] aplReporte_GetByID(Guid channel, int documentoID);

        /// <summary>
        /// Ingresa o actualiza un reporte predefinido para un usuario
        /// </summary>
        /// <param name="documentoID">Identificador del reporte</param>
        /// <returns>Retorna el reporte del documento</returns>
        [OperationContract]
        void aplReporte_Update(Guid channel, DTO_aplReporte report);

        #endregion

        #region Operaciones Documentos

        /// <summary>
        /// Verifica si se ha realizado algun proceso de cierre de un modulo para un oerdioo determinado
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo ha tenido cierres</returns>
        [OperationContract]
        bool PeriodoHasCierre(Guid channel, DateTime periodo, ModulesPrefix mod);

        /// <summary>
        /// Verifica si un periodo existe y el mes esta abierto
        /// </summary>
        /// <param name="empresaID">Identificador de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo sobre el cual se esta trabajando</param>
        /// <returns>Verdadero si el periodo se puede usar</returns>
        [OperationContract]
        EstadoPeriodo CheckPeriod(Guid channel, DateTime periodo, ModulesPrefix mod);

        /// <summary>
        /// Obtiene el prefijo de un documento dado
        /// </summary>
        /// <param name="areaFuncionalID">Codigo del area funcional</param>
        /// <param name="documentoID">Codigo del documento</param>
        /// <param name="empresaGrupoID">Codigo de grupo de empresas</param>
        /// <returns>Retorna el codigo del prefijo</returns>
        [OperationContract]
        string PrefijoDocumento_Get(Guid channel, string areaFuncionalID, int documentoID);

        /// <summary>
        /// Trae el consecutivo para un numero de documento
        /// Si no existe crea uno y lo inicia en 1
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="onlyGet">Indica si solo puede traer la info o tambien crear un nuevo numero</param>
        /// <returns>Retorna el consecutivo</returns>
        [OperationContract]
        int DocumentoNro_Get(Guid channel, int documentID, string prefijoID);

        #endregion

        #region Reportes

        /// <summary>
        /// Trae la data para un reporte
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="consulta"></param>
        /// <returns></returns>
        [OperationContract]
        List<DTO_BasicReport> GetReportData(Guid channel, int reportId, DTO_glConsulta consulta, byte[] bItems);

        #endregion

        #region Temporales

        /// <summary>
        /// Revisa si existe un temporal segun el origen y el usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        [OperationContract]
        bool aplTemporales_HasTemp(Guid channel, string origen, DTO_seUsuario usuario);

        /// <summary>
        /// Trae el temporal de un origen determinado y luego lo borra de los temporales
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario relacionado</param>
        /// <returns>objeto temporal</returns>
        [OperationContract]
        byte[] aplTemporales_GetByOrigen(Guid channel, string origen, DTO_seUsuario usuario);

        /// <summary>
        /// Guarda un objeto en temporales. También borra un objeto que anteriormente estuviese bajo el mismo origen para ese usuario
        /// </summary>
        /// <param name="origen">origen relacionado</param>
        /// <param name="usuario">usuario</param>
        /// <param name="objeto">objeto a guardar</param>
        [OperationContract]
        void aplTemporales_Save(Guid channel, string origen, DTO_seUsuario usuario, object objeto);

        /// <summary>
        /// Elimina los temporales de un usuario
        /// </summary>
        /// <param name="origen">Origen de los datos</param>
        /// <param name="usuario">Usuario que esta buscando temporales</param>
        [OperationContract]
        void aplTemporales_Clean(Guid channel, string origen, DTO_seUsuario usuario);

        #endregion

        #endregion

    }

}
