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

namespace NewAge.Server.MasterService
{
    /// <summary>
    /// Interfaz con toda la lista de operaciones que se deben implementar para exponer o manejar la información de las maestras
    /// </summary>
    [ServiceContract]
    public interface IMasterService
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

        #region Maestras

        #region MasterComplex - Llaves multiples

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="pks">Identificador de la maestra</param>
        /// <param name="EmpresaGrupoID">Identificador por el cual se filtra</param>
        /// <returns>Devuelve la maestra basica</returns>
        [OperationContract]
        DTO_MasterComplex MasterComplex_GetByID(Guid channel, int documentID, Dictionary<string, string> pks, bool active);

        /// <summary>
        /// Cantidad de registros de una maestra
        /// </summary>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros de la consulta</returns>
        [OperationContract]
        long MasterComplex_Count(Guid channel, int documentID, DTO_glConsulta consulta, bool? active);

        /// <summary>
        /// Trae los registros de una maestra compleja
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtro</param>
        /// <returns>Devuelve los registros de una maestra compleja</returns>
        [OperationContract]
        IEnumerable<DTO_MasterComplex> MasterComplex_GetPaged(Guid channel, int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, bool? active);

        /// <summary>
        /// Adiciona una lista a la maestra
        /// </summary>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de insercion de los datos</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterComplex_Add(Guid channel, int documentID, byte[] bItems, int accion);

        /// <summary>
        /// Actualiza una maestra
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterComplex_Update(Guid channel, int documentID, DTO_MasterComplex item);

        /// <summary>
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        [OperationContract]
        DTO_TxResult MasterComplex_Delete(Guid channel, int documentID, Dictionary<string, string> pks);

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        [OperationContract]
        string MasterComplex_Export(Guid channel, int documentID, string colsRsx, DTO_glConsulta consulta);

        /// <summary>
        /// Trae el numero de la fila de una lista de Pks
        /// </summary>
        /// <param name="pks">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        [OperationContract]
        long MasterComplex_Rownumber(Guid channel, int documentID, Dictionary<string, string> pks, DTO_glConsulta consulta, bool? active);

        #endregion

        #region MasterHierarchy

        #region Funciones que llaman a MasterSimple

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        [OperationContract]
        IEnumerable<DTO_MasterHierarchyBasic> MasterHierarchy_GetPaged(Guid channel, int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active);

        #endregion

        #region Funciones Maestra jerarquica

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra jerarquica</returns>
        [OperationContract]
        DTO_MasterHierarchyBasic MasterHierarchy_GetByID(Guid channel, int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros);

        /// <summary>
        /// Trae la cantidad de hijos de un padre determinado
        /// </summary>
        /// <param name="parentId">id del padre</param>
        /// <param name="idFilter">filtro de id para los hijos</param>
        /// <param name="descFilter">filtro de descripcion</param>
        /// <param name="active">filtro de activo</param>
        /// <returns>Retorna la cantidad de hijos</returns>
        [OperationContract]
        long MasterHierarchy_CountChildren(Guid channel, int documentID, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros);

        /// <summary>
        /// Retorna una lista de hijos de una maestra de jerarquica
        /// </summary>
        /// <param name="pageSize">Número de registros por página</param>
        /// <param name="pageNum">Número de página</param>
        /// <param name="orderDirection">Ordenamiento</param>
        /// <param name="parentId">Identificador del padre</param>
        /// <param name="idFilter">Filtro para la columna del ID</param>
        /// <param name="descFilter">Filtro para la columna de la descripción</param>
        /// <param name="active">Indicador si se pueden traer solo datos activos</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Retorna la lista de resultados</returns>
        [OperationContract]
        IEnumerable<DTO_MasterHierarchyBasic> MasterHierarchy_GetChindrenPaged(Guid channel, int documentID, int pageSize, int pageNum, OrderDirection orderDirection, UDT_BasicID parentId, string idFilter, string descFilter, bool? active, List<DTO_glConsultaFiltro> filtros);

        /// <summary>
        /// Adiciona una lista de dtos
        /// </summary>
        /// <param name="bItems">Lista de datos</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="progress">Información con el progreso de la transacción</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterHierarchy_Add(Guid channel, int documentID, byte[] bItems, int accion);

        /// <summary>
        /// Actualiza un registro de una maestra jerarquica
        /// </summary>
        /// <param name="item">Registro para actualizar</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterHierarchy_Update(Guid channel, int documentID, DTO_MasterHierarchyBasic item);

        /// <summary>
        /// Borra un registro
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterHierarchy_Delete(Guid channel, int documentID, UDT_BasicID id);

        /// <summary>
        /// Valida si los padres de un id existen
        /// </summary>
        /// <param name="documentID">Identificador de la maestra</param>
        /// <param name="id">id del hijo</param>
        /// <returns>True si existen los padres</returns>
        [OperationContract]
        bool MasterHierarchy_CheckParents(Guid channel, int documentID, UDT_BasicID id);

        #endregion

        #endregion

        #region MasterSimple

        /// <summary>
        /// Retorna una maestra básica a partir del identificador
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <param name="active">indica si el registro debe estar activo</param>
        /// <param name="filtros">Filtros</param>
        /// <returns>Devuelve la maestra basica</returns>
        [OperationContract]
        DTO_MasterBasic MasterSimple_GetByID(Guid channel, int documentID, UDT_BasicID id, bool active, List<DTO_glConsultaFiltro> filtros);

        /// <summary>
        /// Retorna una maestra básica a partir de su descripcion
        /// En caso de encontrar mas de un resultado devolvera el primero
        /// </summary>
        /// <param name="desc">Descriptivo de la maestra</param>
        /// <returns>Devuelve la maestra basica</returns>
        [OperationContract]
        DTO_MasterBasic MasterSimple_GetByDesc(Guid channel, int documentID, string desc);

        /// <summary>
        /// Cuenta la cantidad de resultados dado un filtro
        /// </summary>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Retorna el numero de registros encontrados</returns>
        [OperationContract]
        long MasterSimple_Count(Guid channel, int documentID, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active);

        /// <summary>
        /// Trae los registros de una maestra 
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Numero de página</param>
        /// <param name="consulta">Filtros</param>
        /// <param name="FiltrosExtra">Filtros de codigo y descripción</param>
        /// <param name="active">Si solo cuentan resultados activos</param>
        /// <returns>Devuelve los registros de una maestra</returns>
        [OperationContract]
        IEnumerable<DTO_MasterBasic> MasterSimple_GetPaged(Guid channel, int documentID, long pageSize, int pageNum, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> FiltrosExtra, bool? active);

        /// <summary>
        ///  Adiciona una lista a la maestra básica
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de la operacion "usuario,progreso"</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterSimple_Add(Guid channel, int documentID, byte[] bItems, int accion);

        /// <summary>
        /// Actualiza una maestra básica
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        [OperationContract]
        DTO_TxResult MasterSimple_Update(Guid channel, int documentID, DTO_MasterBasic item);

        /// <summary>
        /// Borra una maestra básica a partir de su id
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <returns>Resultado</returns>  
        [OperationContract]
        DTO_TxResult MasterSimple_Delete(Guid channel, int documentID, UDT_BasicID id);

        /// <summary>
        /// Exporta los registros de una maestra
        /// </summary>
        /// <param name="colsRsx">Nombres de las columnas con los recursos</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="filtrosExtra">Filtro especial</param>
        /// <returns>Retorna el nombre del archivo</returns>
        [OperationContract]
        string MasterSimple_Export(Guid channel, int documentID, string colsRsx, DTO_glConsulta consulta, List<DTO_glConsultaFiltro> filtrosExtra);

        /// <summary>
        /// Trae el numero de la fila de un ID
        /// </summary>
        /// <param name="id">Idenfiticador de la maestra</param>
        /// <param name="consulta">Filtro</param>
        /// <param name="active">Indicador de activo</param>
        /// <returns>Devuelve el número de fila ordenando por id</returns>
        [OperationContract]
        long MasterSimple_Rownumber(Guid channel, int documentID, UDT_BasicID id, DTO_glConsulta consulta, bool? active);

        #endregion

        #region Masters Common

        /// <summary>
        /// Metodo de TODAS las maestras para traer un campo de imagen
        /// </summary>
        /// <param name="replicaId">Replica Id de la fila a buscar</param>
        /// <param name="fieldName">Nombre del campo que contiene la imagen</param>
        /// <returns>arreglo de bytes con la imagen del logo</returns>
        [OperationContract]
        byte[] Master_GetImage(Guid channel, int docId, int replicaId, string fieldName);

        #endregion

        #endregion

    }
}
