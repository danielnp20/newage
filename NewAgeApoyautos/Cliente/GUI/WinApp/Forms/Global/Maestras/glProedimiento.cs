using System;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de glProcedimiento
    /// </summary>
    public partial class glProcedimiento : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glProcedimiento() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glProcedimiento;
            base.InitForm();
        }

        #region Eventos MDI
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemImport.Enabled = false;
            FormProvider.Master.itemNew.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            FormProvider.Master.itemExport.Enabled = false;
            FormProvider.Master.itemDelete.Enabled = false;
        }
        #endregion

        #region Validaciones Del formulario
        #endregion
    }//clase
}//namespace
       

     