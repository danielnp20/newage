using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Collections.Generic;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glEmpresa
    /// </summary>
    public partial class glEmpresa : MasterSimpleForm 
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glEmpresa() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glEmpresa;
            base.InitForm();
        }

        #region Sobrecarga métodos de la maestar simple

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterBasic> insertList, int accion)
        {
            byte[] bItems = CompressedSerializer.Compress<IEnumerable<DTO_MasterBasic>>(insertList);
            return _bc.AdministrationModel.glEmpresa_Add(bItems, accion);
        }

        #endregion

        #region Eventos MDI
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemDelete.Enabled = false;
        }
        #endregion

    }//clase
}//namespace
       

     