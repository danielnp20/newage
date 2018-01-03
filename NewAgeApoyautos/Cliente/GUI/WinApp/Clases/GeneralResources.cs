using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    public static class GeneralResources
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private static BaseController _bc = BaseController.GetInstance();
        private static string _periodTit = "PeriodTit";
        private static string _periodSelect = "PeriodSelect";
        private static string _periodAccept = "PeriodAccept";

        #endregion    
       
        #region Propiedades

        /// <summary>
        /// Mensaje: "Periodo Extra"
        /// </summary>
        internal static string PeriodTitFrm
        {
            get { return _bc.GetResource(Librerias.Project.LanguageTypes.Forms, _periodTit); }
        }

        /// <summary>
        /// MEnsaje: "Seleccione un periodo:" 
        /// </summary>
        internal static string PeriodSelect
        {
            get { return _bc.GetResource(Librerias.Project.LanguageTypes.Forms, _periodSelect); }
        }

        /// <summary>
        /// Mensaje: "Aceptar" 
        /// </summary>
        internal static string PeriodAccept
        {
            get { return _bc.GetResource(Librerias.Project.LanguageTypes.Forms, _periodAccept); }
        }

        #endregion
    }
}
