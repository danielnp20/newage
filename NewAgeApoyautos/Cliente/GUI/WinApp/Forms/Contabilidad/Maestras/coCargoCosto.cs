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
    class coCargoCosto : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coCargoCosto() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coCargoCosto;
            base.InitForm();
        }

        #region Validaciones

        /// <summary>
        /// Carga los datos iniciales de una maestra
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Retorna el valor que debe ir en el campo</returns>
        protected override string NewRecordLoadData(string fieldName)
        {
            string res = string.Empty;

            if (fieldName == "LineaPresupuestoID")
                res = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

            if (fieldName == "LineaPresupuestoDesc")
            {
                string linea = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                DTO_plLineaPresupuesto descLinea = (DTO_plLineaPresupuesto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, linea, true);
                if (descLinea != null)
                    res = descLinea.Descriptivo.Value;
            }

            return res;
        }
        #endregion
    }
}
