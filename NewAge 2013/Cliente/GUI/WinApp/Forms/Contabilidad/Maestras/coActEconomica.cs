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
    class coActEconomica : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coActEconomica() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coActEconomica;
            base.InitForm();
            //SortedDictionary<string, string> DicIntern = new SortedDictionary<string, string>();
            //SortedDictionary<int, SortedDictionary<string, string>> Dic = new SortedDictionary<int, SortedDictionary<string, string>>();
            //var languages = _bc.AdministrationModel.aplIdioma_GetAll();
            //for (int i = 1; i < 7; i++)
            //{
            //    Dic.Add(i, DicIntern);
            //}
        }
    }
}
