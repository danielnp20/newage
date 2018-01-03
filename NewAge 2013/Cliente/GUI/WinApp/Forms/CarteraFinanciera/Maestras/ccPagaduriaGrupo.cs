using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ccPagaduriaGrupo : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public ccPagaduriaGrupo() : base() { }

        ///<summary>
        /// Constructor 
        /// </summary>
        public ccPagaduriaGrupo(string mod) : base(mod) { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.ccPagaduriaGrupo;
            base.InitForm();
        }
    }
}
