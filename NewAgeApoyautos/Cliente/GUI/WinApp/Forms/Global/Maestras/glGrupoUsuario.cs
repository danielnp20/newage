using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glGrupoUsuario
    /// </summary>
    public partial class glGrupoUsuario : MasterSimpleForm 
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glGrupoUsuario() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glGrupoUsuario;
            base.InitForm();
        }

    }//clase
}//namespace
       

     