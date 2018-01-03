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
    /// Maestra de acProduccionxPozoDUP
    /// </summary>
    public partial class acProduccionxPozoDUP : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public acProduccionxPozoDUP() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.acProduccionxPozoDUP;
            base.InitForm();
        }
    }//clase
}//namespace
       

     