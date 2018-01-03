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
    class prProveedor : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public prProveedor() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.prProveedor;
            base.InitForm();
        }
        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        {
            bool res = true;
            err = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            if (this.Insertando)
            {
                if (fc.FieldName == "TerceroID")
                {
                    string value = Value.ToString();
                    DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, value, true);
                    FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("RepresentanteLegal");
                    FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("TelContacto");
                    FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("MailContacto");
                    FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("TelContactoComercial");
                    FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("MailContactoComercial");
                    this.SetEditGridValue(newFC1.ColumnIndex, tercero.Descriptivo.Value);
                    this.SetEditGridValue(newFC2.ColumnIndex, string.IsNullOrEmpty(tercero.Tel1.Value) || tercero.Tel1.Value == "0" ? tercero.Tel2.Value : tercero.Tel1.Value);
                    this.SetEditGridValue(newFC3.ColumnIndex, tercero.CECorporativo.Value);
                    this.SetEditGridValue(newFC4.ColumnIndex, string.IsNullOrEmpty(tercero.Tel1.Value) || tercero.Tel1.Value == "0" ? tercero.Tel2.Value : tercero.Tel1.Value);
                    this.SetEditGridValue(newFC5.ColumnIndex, tercero.CECorporativo.Value);
                }
            }
            return res;
        }
    }
}
