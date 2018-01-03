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
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class coCuentaGrupo : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coCuentaGrupo() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coCuentaGrupo;
            base.InitForm();
        }

        /// <summary>
        /// Sobrecargar para modificar alguna configuración de campo de la maestra
        /// </summary>
        public override void CustomizeFieldsConfig()
        {
            //DTO_glTabla cta = new DTO_glTabla();
            //Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, this.EmpresaGrupoID);
            //cta = _bc.AdministrationModel.Tables[tup];
            //int[] nivelUsed = cta.CompleteLevelLengths();
        
        }

    }
}
