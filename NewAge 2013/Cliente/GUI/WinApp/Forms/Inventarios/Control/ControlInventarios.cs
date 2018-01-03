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
    public partial class ControlInventarios : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlInventarios()
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
            this.DocumentID = AppControl.ControlInventarios;
            this.FrmModule = ModulesPrefix.@in;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_003, AppMasters.inRefParametro1, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_004, AppMasters.inRefParametro2, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_010, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_011, AppMasters.inEmpaque, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_013, AppMasters.inReferencia, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_014, AppMasters.inBodega, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_015, AppMasters.inReferencia, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_024, AppMasters.inUnidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_025, AppMasters.inUnidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_027, AppMasters.inUnidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_028, AppMasters.inEmpaque, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_029, AppMasters.inRefTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_030, AppMasters.inRefGrupo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_031, AppMasters.inRefClase, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_032, AppMasters.inMaterial, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_033, AppMasters.inPosicionArancel, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_034, AppMasters.inMarca, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_035, AppMasters.inSerie, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_201, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_202, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_203, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_204, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_205, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_206, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_207, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_208, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_209, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_210, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_212, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_213, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_214, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_215, AppMasters.inMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_301, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_302, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_303, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_304, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_305, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_306, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_307, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_308, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_309, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_310, AppMasters.inImportacionModalidad, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_311, AppMasters.inImportacionModalidad, false, true, true, false);
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);
            TablesResources.GetTableResources(this.ctrl_019, typeof(EstadoInv));
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
