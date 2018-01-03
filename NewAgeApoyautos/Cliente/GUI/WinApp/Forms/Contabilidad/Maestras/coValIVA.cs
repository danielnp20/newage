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
    class coValIVA : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coValIVA() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coValIVA;
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
            string res = string.Empty;

            if (fieldName == "CuentaCostoReteIVA")
            {
                FieldConfiguration newFC = this.GetFieldConfigByFieldName(fieldName);

                if (newFC != null)
                {
                    newFC.Editable = false;
                    newFC.AllowNull = true;
                }
            }

            return res;
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

            //Habilita o deshabilita el campo de cuenta costo reteiva segun el campo de reteiva
            if (fc.FieldName == "CuentaReteIVA")
            {
                string v = Value.ToString();

                int valIVADoc = AppMasters.coPlanCuenta;
                DTO_coPlanCuenta tipo = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, valIVADoc, false, v, true);

                FieldConfiguration newFC = this.GetFieldConfigByFieldName("CuentaCostoReteIVA");
                if (tipo != null && newFC != null)
                {
                    newFC.Editable = true;
                    newFC.AllowNull = false;
                }
                else
                {
                    newFC.Editable = false;
                    newFC.AllowNull = true;
                    this.SetEditGridValue(newFC.ColumnIndex - 1,string.Empty);
                    this.SetEditGridValue(newFC.ColumnIndex, string.Empty);
                    this.SetEditGridValue(newFC.ColumnIndex + 1,string.Empty);
                }
            }
            return res;
        }
        #endregion

    }
}
