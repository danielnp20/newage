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
using System.Reflection;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class glDiasFestivos : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glDiasFestivos()
            : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glDiasFestivos;
            base.InitForm();
        }

        /// <summary>
        /// Metodo encargado de encapsular la insercion de registros de maestras simples
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="insertList">Lista de dtos</param>
        /// <param name="userId">usuario logueado</param>
        /// <param name="documentId">docuemnto</param>
        /// <param name="accion">accion (insercion/importar)</param>
        /// <returns></returns>
        protected override DTO_TxResult DataAdd(List<DTO_MasterComplex> insertList, int accion)
        {
            try
            {
                DTO_glDiasFestivos planCta = insertList.Count > 0 ? (DTO_glDiasFestivos)insertList[0] : null;
                planCta.DiasFestivoID.Value = Convert.ToDateTime(planCta.Fecha.Value.Value.ToString(FormatString.Date));
                planCta.PKValues["DiasFestivoID"] = planCta.DiasFestivoID.Value.ToString();
                return base.DataAdd(insertList, Convert.ToInt32(FormsActions.Add));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
