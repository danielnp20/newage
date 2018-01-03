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
    class coImpuestoDeclaracion : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coImpuestoDeclaracion() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coImpuestoDeclaracion;
            base.InitForm();
        }

        #region Validaciones Del formulario

        /// <summary>
        /// Carga los datos iniciales de una maestra
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Retorna el valor que debe ir en el campo</returns>
        protected override string NewRecordLoadData(string fieldName)
        {
            try
            {
                string res = string.Empty;
                FieldConfiguration newFC = this.GetFieldConfigByFieldName(fieldName);
                DTO_glLugarGeografico dtoLugar;

                //Trae el dato de Lugar Geografico por defecto 
                string lugarGeoDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                if (fieldName == "LugarGeograficoID")
                    res = lugarGeoDef;
                if (fieldName == "LugarGeoDesc")
                {
                    dtoLugar = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, lugarGeoDef, true);
                    res = dtoLugar != null? dtoLugar.Descriptivo.Value : string.Empty;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sobrecargar para procesar el cambio en un campo especifico de la grilla de edición
        /// </summary>
        /// <param name="Field">Nombre del campo en la grilla de edición (caption)</param>
        /// <param name="Value">Valor ingresado</param>
        /// <param name="RowIndex">Numero de la fila en la grilla de edición</param>
        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        {
            bool res = true;
            err = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            DTO_glLugarGeografico dtoLugar;
            //Valida la linea presupuestal coincida con la tabla de control para permitir editar
            if (fc.FieldName == "MunicipalInd")
            {
                string LugarGeoDefault = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                dtoLugar = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, LugarGeoDefault, true);
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("LugarGeograficoID");
                FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("LugarGeoDesc");
                if (newFC1 != null && newFC2 != null && dtoLugar != null && !Convert.ToBoolean(Value))
                {
                    this.SetEditGridValue(newFC1.ColumnIndex, LugarGeoDefault);
                    this.SetEditGridValue(newFC2.ColumnIndex, dtoLugar.Descriptivo.Value);
                    newFC1.Editable = false;
                }
                else if (newFC1 != null &&  newFC2 != null)
                {
                    this.SetEditGridValue(newFC1.ColumnIndex, string.Empty);
                    this.SetEditGridValue(newFC2.ColumnIndex, string.Empty);
                    newFC1.Editable = true;
                }
            }
            return res;
        }

        #endregion
    }
}
