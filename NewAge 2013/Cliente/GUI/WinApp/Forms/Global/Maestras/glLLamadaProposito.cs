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
    /// Maestra de glLLamadaProposito
    /// </summary>
    public partial class glLLamadaProposito : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public glLLamadaProposito() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.glLLamadaProposito;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-glLLamadaProposito", "LoadEditGridData"));
            }
        }
        #endregion

    }//clase
}//namespace
       

     