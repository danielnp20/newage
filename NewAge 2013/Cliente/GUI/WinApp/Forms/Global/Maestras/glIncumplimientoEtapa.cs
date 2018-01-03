using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.API.Native;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glIncumplimientoEtapa
    /// </summary>
    public partial class glIncumplimientoEtapa : MasterSimpleForm
    {
        private int entradas = 0;
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glIncumplimientoEtapa;
            base.InitForm();

        }

    }//clase
}//namespace


