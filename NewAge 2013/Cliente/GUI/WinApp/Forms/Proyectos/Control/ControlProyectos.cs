using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para control
    /// </summary>
    public partial class ControlProyectos : ControlForm
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlProyectos()
        {
            //InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            this.DocumentID = AppControl.ControlProyectos;
            this.FrmModule = ModulesPrefix.py;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_100, AppMasters.pyClaseProyecto, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_101, AppMasters.pyTrabajo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_102, AppMasters.pyEtapa, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_103, AppMasters.pyLineaFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_104, AppMasters.pyListaPrecio, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_105, AppMasters.pyRecurso, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_106, AppMasters.coActividad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_107, AppMasters.pyTarea, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_200, AppMasters.pyClaseProyecto, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_900, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_005, AppMasters.inUnidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_016, AppMasters.pyRecurso, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_017, AppMasters.seUsuario, false, true, true, false);

            // Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 0);
            #endregion
            #region Validaciones
            this.ctrl_002.Enabled = true;
            #endregion



        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void ctrl_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        #endregion
    }
}
