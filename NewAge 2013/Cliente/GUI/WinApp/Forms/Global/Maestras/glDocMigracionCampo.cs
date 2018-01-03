using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glDocMigracionCampo
    /// </summary>
    public partial class glDocMigracionCampo : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glDocMigracionCampo() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glDocMigracionCampo;
            base.InitForm();
        }

    }//clase
}//namespace
       

     