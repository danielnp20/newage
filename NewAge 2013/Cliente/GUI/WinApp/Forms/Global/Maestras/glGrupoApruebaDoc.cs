﻿using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using System.Collections.Generic;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glGrupoApruebaDoc
    /// </summary>
    public partial class glGrupoApruebaDoc : MasterSimpleForm 
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glGrupoApruebaDoc() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glGrupoApruebaDOC;
            base.InitForm();
        }

    }//clase
}//namespace
       

     