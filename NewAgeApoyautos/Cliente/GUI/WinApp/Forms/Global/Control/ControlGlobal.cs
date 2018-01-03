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
    public partial class ControlGlobal : ControlForm 
    {
        #region Variables

        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();

        #endregion

        public ControlGlobal()
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
            this.DocumentID = AppControl.ControlGlobal;
            this.FrmModule = ModulesPrefix.gl;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_52, AppMasters.glEmpresaGrupo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_90, AppMasters.glActividadFlujo, false, true, true, false);

            // Otras Opciones Controles
            this.ctrl_72.Visible = false;
            this.ctrl_73.Visible = false;
            this.ctrl_72.Enabled = false;
            this.ctrl_73.Enabled = false;
            this.lbl_72.Visible = false;
            this.lbl_73.Visible = false;
            this.ctrl_70.Enabled = false;
            this.ctrl_71.Enabled = false;
            this.ctrl_79.Enabled = false;
            #endregion
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void ctrl_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        #endregion
    }
}
