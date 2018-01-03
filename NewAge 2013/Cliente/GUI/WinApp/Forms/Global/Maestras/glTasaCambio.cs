using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Collections.Generic;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de TasaCambio
    /// </summary>
    public partial class glTasaCambio : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public glTasaCambio()
            : base()
        {
            try
            {
                bool IsCurrent = false;
                for (int i = 0; i < gvModule.DataRowCount; i++)
                {
                    string date = gvModule.GetRowCellValue(i, "Fecha").ToString();
                    if (date.Equals(DateTime.Now.ToShortDateString()))
                    {
                        this.gvModule.Focus();
                        this.gvModule.FocusedRowHandle = i;
                        IsCurrent = true;
                        break;
                    }
                }
                if (!IsCurrent)
                    this.TBNew();
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glTasaCambio;
            base.InitForm();
        }

        protected override string NewRecordLoadData(string fieldName)
        {
            string res = string.Empty;
            FieldConfiguration newFC1 = this.GetFieldConfigByFieldName("MonedaID");

            if (fieldName == "MonedaID")
                res = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            if (fieldName == "MonedaDesc")
            {
                string monedaExt = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                DTO_MasterBasic moneda = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glMoneda, false, monedaExt, true);

                res = moneda.Descriptivo.Value;
            }
            if (fieldName == "Fecha")
                res = DateTime.Now.ToShortDateString();
            return res;
        }
    }//clase
}//namespace


