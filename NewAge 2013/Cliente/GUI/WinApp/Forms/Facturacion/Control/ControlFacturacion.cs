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
    public partial class ControlFacturacion : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlFacturacion()
        {
           // InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            this.DocumentID = AppControl.ControlFacturacion;
            this.FrmModule = ModulesPrefix.fa;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_002, AppMasters.faServicios, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_003, AppMasters.faFacturaTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_004, AppMasters.faListaPrecio, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_005, AppMasters.glZona, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_118, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_100, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_070, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_071, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_072, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_073, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_074, AppMasters.faAsesor, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_075, AppMasters.faServicios, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_006, AppMasters.faServicios, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_007, AppMasters.faFacturaTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_076, AppMasters.glPrefijo, false, true, true, false);
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);
            #endregion
        }

         #endregion

    }
}
