﻿using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using System.Collections.Generic;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de acContabiliza
    /// </summary>
    public partial class acContabiliza : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public acContabiliza() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.acContabiliza;
            base.InitForm();
        }
    }//clase
}//namespace
       

     