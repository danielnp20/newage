using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glTerceroReferencia
    /// </summary>
    public partial class glTerceroReferencia : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glTerceroReferencia() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glTerceroReferencia;
            base.InitForm();
        }
        protected override bool FieldValidate(string Field, object Value, int RowIndex, out string err)
        {
            bool res = true;
            err = string.Empty;
            FieldConfiguration fc = this.GetFieldConfigByCaption(Field);
            if (fc.FieldName == "TipoReferencia")
            {
                int valor = int.Parse(Value.ToString());
                FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("TipoReferencia");
                if (valor == 4) {
                    FieldConfiguration newFC2 = this.GetFieldConfigByFieldName("TerceroID");
                    string terceroID = this.GetEditRow(newFC2.RowIndex).Valor.ToString();
                    DTO_coTercero tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, terceroID, true);
                    FieldConfiguration newFC3 = this.GetFieldConfigByFieldName("Nombre");
                    FieldConfiguration newFC4 = this.GetFieldConfigByFieldName("Relacion");
                    FieldConfiguration newFC5 = this.GetFieldConfigByFieldName("Direccion");
                    FieldConfiguration newFC6 = this.GetFieldConfigByFieldName("Telefono");
                    FieldConfiguration newFC7 = this.GetFieldConfigByFieldName("Ciudad");
                    FieldConfiguration newFC8 = this.GetFieldConfigByFieldName("Correo");
                    this.SetEditGridValue(newFC3.RowIndex, tercero.Descriptivo);
                    this.SetEditGridValue(newFC4.RowIndex, "EL MISMO");
                    this.SetEditGridValue(newFC5.RowIndex, tercero.Direccion);
                    this.SetEditGridValue(newFC6.RowIndex, tercero.Tel1.Value + " - " + tercero.Tel2.Value);
                    this.SetEditGridValue(newFC7.RowIndex, tercero.LugarGeoDesc);
                    this.SetEditGridValue(newFC8.RowIndex, tercero.CECorporativo);
                    //newFC2.
                    //this.SetEditGridValue(newFC2.RowIndex, tercero.Descriptivo);
                    
                }
            }
            return res;
        }

    }//clase
}//namespace
       

     