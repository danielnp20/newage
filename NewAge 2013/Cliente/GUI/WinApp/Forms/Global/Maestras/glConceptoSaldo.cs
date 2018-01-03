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
    /// Maestra de glConceptoSaldo
    /// </summary>
    public partial class glConceptoSaldo : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glConceptoSaldo() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glConceptoSaldo;
            base.InitForm();
        }

        #region Eventos del MDI
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

        /// <summary>
        /// Carga los datos de la grilla de edicion
        /// </summary>
        /// <param name="isNew">Dice si el registro es nuevo</param>
        /// <param name="rowIndex">Numero de fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            try
            {
                base.LoadEditGridData(isNew, rowIndex);
                if (isNew)
                    this.grlControlRecordEdit.Enabled = true;
                else
                    this.grlControlRecordEdit.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-glConceptoSaldo", "LoadEditGridData"));
            }
        }
        #endregion

    }//clase
}//namespace
       

     