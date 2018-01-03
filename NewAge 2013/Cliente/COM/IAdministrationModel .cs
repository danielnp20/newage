using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewAgeCOM
{
    /// <summary>
    /// Extiendo la clase AdministrationModel
    /// </summary>
    [ComVisible(true), GuidAttribute("ea4f5711-60e4-433e-93df-702dd9b7560a")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAdministrationModel
    {
        #region Constructor

        /// <summary>
        /// Revisa si la conexión abre exitosamente
        /// </summary>
        /// <returns></returns>
        string InitVars(string connStr, string userID, string pass, string empresaID);

        #endregion

        #region Maestras

        #region MasterSimple

        /// <summary>
        ///  Adiciona una lista a la maestra básica
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de la operacion "usuario,progreso"</param>
        /// <returns>Resultado</returns>
        string MasterSimple_Add(int documentID, string props, string values);

        /// <summary>
        /// Actualiza una maestra básica
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        string MasterSimple_Update(int documentID, string id, string props, string values);

        /// <summary>
        /// Borra una maestra básica a partir de su id
        /// </summary>
        /// <param name="id">Identificador de la maestra</param>
        /// <returns>Resultado</returns>  
        string MasterSimple_Delete(int documentID, string id);

        #endregion

        #region MasterHierarchy

        /// <summary>
        /// Adiciona una lista de dtos
        /// </summary>
        /// <param name="bItems">Lista de datos</param>
        /// <param name="seUsuario">Usuario que ejecuta la acción</param>
        /// <param name="progress">Información con el progreso de la transacción</param>
        /// <returns>Resultado</returns>
        string MasterHierarchy_Add(int documentID, string props, string values);

        /// <summary>
        /// Actualiza un registro de una maestra jerarquica
        /// </summary>
        /// <param name="item">Registro para actualizar</param>
        /// <returns>Resultado</returns>
        string MasterHierarchy_Update(int documentID, string id, string props, string values);

        /// <summary>
        /// Borra un registro
        /// </summary>
        /// <param name="id">Identificador del registro</param>
        /// <returns>Resultado</returns>
        string MasterHierarchy_Delete(int documentID, string id);

        #endregion

        #region MasterComplex - Llaves multiples

        /// <summary>
        /// Adiciona una lista a la maestra
        /// </summary>
        /// <param name="bItems">Lista de registros</param>
        /// <param name="accion">Identifica la acción </param>
        /// <param name="progress">Progreso de insercion de los datos</param>
        /// <returns>Resultado</returns>
        string MasterComplex_Add(int documentID, string props, string values);

        /// <summary>
        /// Actualiza una maestra
        /// </summary>
        /// <param name="item">registro donde se realiza la acción</param>
        /// <returns>Resultado</returns>
        string MasterComplex_Update(int documentID, string pkKeys, string pkValues, string props, string values);

        /// <summary>
        /// Borra una maestra a partir de su id
        /// </summary>
        /// <param name="pks">Llaves primarias de la maestra</param>
        /// <returns>Devuelve el resultado de la operacion</returns>  
        string MasterComplex_Delete(int documentID, string pkKeys, string pkValues);

        #endregion

        #endregion

        #region Global

        /// <summary>
        /// Agrega un registro a glDocumentoControl y guarda en las bitacoras
        /// </summary>
        /// <param name="documentID">Identificador del documento que inicia el documento</param>
        /// <param name="docCtrl">Documento que se va a insertar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        string glDocumentoControl_Add(int documentID, string props, string values);

        /// <summary>
        /// Edita un registro al control de documentos
        /// </summary>
        /// <param name="docCtrl">Documento que se va a editar</param>
        /// <param name="updBitacora">Indica si se debe actualizar la bitacora</param>
        /// <returns></returns>
        string glDocumentoControl_Update(int documentID, int numeroDoc, string props, string values);

        #endregion

        #region Planeacion

        #region plCierreLegalizacion

        /// <summary>
        /// Actualiza un registro de plCierreLegalizacion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        string plCierreLegalizacion_Add(string props, string values);

        #endregion

        #region plSobreEjecucion

        /// <summary>
        /// Actualiza un registro de plSobreEjecucion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        string plSobreEjecucion_Add(string props, string values);

        #endregion

        #region plPresupuestoSoporte

        /// <summary>
        /// Actualiza un registro de plPresupuestoSoporte
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        string plPresupuestoSoporte_Add(string props, string values);

        #endregion

        #region plPresupuestoPxQ

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQ
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        string plPresupuestoPxQ_Add(string props, string values);

        #endregion

        #region plPresupuestoPxQDeta

        /// <summary>
        /// Actualiza un registro de plPresupuestoPxQDeta
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        string plPresupuestoPxQDeta_Add(string props, string values);

        #endregion

        #endregion

        #region Proveedores

        #region prCierreMesCostos

        /// <summary>
        /// Actualiza un registro de plCierreLegalizacion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        string prCierreMesCostos_Add(string props, string values);

        #endregion

        #endregion

    }
}
