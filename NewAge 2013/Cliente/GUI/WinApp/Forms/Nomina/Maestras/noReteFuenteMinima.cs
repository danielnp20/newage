using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using System.Collections.Generic;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de noReteFuenteMinima
    /// </summary>
    public partial class noReteFuenteMinima : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public noReteFuenteMinima() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.noReteFuenteMinima;
            base.InitForm();
        }

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterComplex> insertList, int accion)
        {
            try
            {
                DTO_noReteFuenteMinima reteFuenteMinima = insertList.Count > 0 ? (DTO_noReteFuenteMinima)insertList[0] : null;
                reteFuenteMinima.BaseUVTID.Value = Convert.ToDecimal(reteFuenteMinima.BaseUVT.Value.Value.ToString());
                reteFuenteMinima.PKValues["BaseUVTID"] = reteFuenteMinima.BaseUVTID.Value.ToString();
                return base.DataAdd(insertList, Convert.ToInt32(FormsActions.Add));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo que encapsula la funcion de actualizar
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="dto">dto</param>
        /// <param name="userId">usuario</param>
        /// <param name="documentId">documento</param>
        /// <returns></returns>
        protected override DTO_TxResult DataUpdate(DTO_MasterComplex dto)
        {
            DTO_noReteFuenteMinima reteFuenteMinima = (DTO_noReteFuenteMinima)dto;
            reteFuenteMinima.BaseUVTID.Value = reteFuenteMinima.BaseUVT.Value.Value;
            reteFuenteMinima.PKValues["BaseUVTID"] = reteFuenteMinima.BaseUVTID.Value.ToString();
            return base.DataUpdate(dto);
        }

    }//clase
}//namespace
       

     