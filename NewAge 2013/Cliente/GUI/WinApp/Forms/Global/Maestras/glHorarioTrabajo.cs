using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glHorarioTrabajo
    /// </summary>
    public partial class glHorarioTrabajo : MasterSimpleForm 
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glHorarioTrabajo() : base()   { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glHorarioTrabajo;
            base.InitForm();
        }
        //#region Validaciones Del formulario

        ///// <summary>
        ///// Carga los datos iniciales de una maestra
        ///// </summary>
        ///// <param name="fieldName">Nombre del campo</param>
        ///// <returns>Retorna el valor que debe ir en el campo</returns>
        //protected override string NewRecordLoadData(string fieldName)
        //{
        //    string res = string.Empty;
        //    FieldConfiguration newFC = this.GetFieldConfigByFieldName(fieldName);

        //    if (fieldName == "AjCambioLinPresupuestal")
        //    {
               
        //    }
        //    return res;
        //}

        ///// <summary>
        ///// Sobrecargar para procesar el cambio en un campo especifico de la grilla de edición
        ///// </summary>
        ///// <param name="Field">Nombre del campo en la grilla de edición (caption)</param>
        ///// <param name="Value">Valor ingresado</param>
        ///// <param name="RowIndex">Numero de la fila en la grilla de edición</param>
        //protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        //{
        //    bool res = true;
        //    err = string.Empty;
        //    FieldConfiguration fc = this.GetFieldConfigByCaption(Field);

        //    if (fc.FieldName == "EntradaHora")
        //    {
        //        FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("EntradaHora");
        //        Regex.IsMatch(newFC1.FieldName, "0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23");
        //    }

        //    return res;
        //}

        //#endregion

    }//clase
}//namespace
       

     