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
    class coComprobante : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coComprobante() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coComprobante;
            base.InitForm();
        }

        #region Validaciones Del formulario

        /// <summary>
        /// Carga los datos de la grilla de edicion
        /// </summary>
        /// <param name="isNew">Dice si el registro es nuevo</param>
        /// <param name="rowIndex">Numero de fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            base.LoadEditGridData(isNew, rowIndex);
            if (rowIndex >= 0)
            {
                string ComprSaldos = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteImportacionSaldos);
                DTO_coComprobante comprobante = this.gvModule.GetRow(rowIndex) as DTO_coComprobante;
                if (comprobante != null)
                {
                    if (comprobante.ID.Value.Equals(ComprSaldos) && !isNew)
                        this.grlControlRecordEdit.Enabled = false;
                    else
                        this.grlControlRecordEdit.Enabled = true;
                }
            }
        }

        #endregion
    }
}
