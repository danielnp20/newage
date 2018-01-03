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
    class seUsuarioBodega : MasterComplexForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public seUsuarioBodega() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.seUsuarioBodega;
            base.InitForm();
        }

        #region Validaciones Del formulario

        ///// <summary>
        ///// Carga los datos iniciales de una maestra
        ///// </summary>
        ///// <param name="fieldName">Nombre del campo</param>
        ///// <returns>Retorna el valor que debe ir en el campo</returns>
        //protected override string NewRecordLoadData(string fieldName)
        //{
        //    string res = string.Empty;
        //    FieldConfiguration newFC = this.GetFieldConfigByFieldName(fieldName);
        //    if (fieldName == "ConsultaCostosInd")
        //    {
        //        if(base.gvRecordEdit.DataSource != null)
        //            this.SetEditGridValue(newFC.ColumnIndex, bool.TrueString);
        //    }
        //    return res;
        //}
        #endregion
    }
}
