using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class noEmpleado : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();


        ///<summary>
        /// Constructor 
        /// </summary>
        public noEmpleado() : base()
        {
            //this.InitializeComponent();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        ///
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.noEmpleado;
            base.InitForm();
        }

        /// <summary>
        /// Redirecciona al formulario Incorporaciones
        /// </summary>
        public override void TBNew()
        {
            ModalIncorporaciones addEmpleado = new ModalIncorporaciones();
            addEmpleado.Show();
        }
    }
}
