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
    public partial class ControlDecisor : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlDecisor()
        {
            ////se descomentarea para ver diseño
            //InitializeComponent();
            ////se descomentarea para ver diseño
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            //se comentarea para ver diseño
            base.SetInitParameters();
            //se comentarea para ver diseño
            this.DocumentID = AppControl.ControlDecisor;
            this.FrmModule = ModulesPrefix.dr;

            #region Inicializar controles
            _bc.InitMasterUC(this.ctrl_125, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_126, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_130, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_131, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_132, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_133, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_134, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_135, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_136, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_137, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_138, AppMasters.glActividadFlujo, false, true, true, false);

            _bc.InitMasterUC(this.ctrl_127, AppMasters.seUsuario, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_128, AppMasters.seUsuario, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_129, AppMasters.seUsuario, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_143, AppMasters.ccDevolucionCausal, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_144, AppMasters.seUsuario, false, true, true, false);

            //Controles de FK para maestras
            #endregion
        }
        
        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //Deshabilita el mes 14 si no esta activo el indicador de Mes 14
            //if (ctrl_023.CheckState == CheckState.Unchecked)
            //    _bc.InitPeriodUC(this.ctrl_001, 1);
        }

        #endregion

    }
}
