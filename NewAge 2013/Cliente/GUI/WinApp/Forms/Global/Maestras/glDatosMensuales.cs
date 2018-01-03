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
    class glDatosMensuales : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glDatosMensuales()
            : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glDatosMensuales;
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
                DTO_glDatosMensuales datosMensuales = insertList.Count > 0 ? (DTO_glDatosMensuales)insertList[0] : null;
                string p = datosMensuales.Periodo.Value.Value.ToString(FormatString.Date);
                datosMensuales.PeriodoID.Value = Convert.ToDateTime(p);
                datosMensuales.PKValues["PeriodoID"] = datosMensuales.PeriodoID.Value.ToString();
                return base.DataAdd(insertList, Convert.ToInt32(FormsActions.Add));
            }
            catch (Exception ex)
            {
                return null;
            }
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
        protected override DTO_TxResult DataUpdate(DTO_MasterComplex dto)
        {
            try
            {
                DTO_glDatosMensuales datosMensuales = (DTO_glDatosMensuales)dto;
                string p = datosMensuales.Periodo.Value.Value.ToString(FormatString.Date);
                datosMensuales.PeriodoID.Value = Convert.ToDateTime(p);
                datosMensuales.PKValues["PeriodoID"] = datosMensuales.PeriodoID.Value.ToString();
                return _bc.AdministrationModel.MasterComplex_Update(this.DocumentID, dto);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
