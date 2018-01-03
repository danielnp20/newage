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
    public partial class ControlCartera : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlCartera()
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
            this.DocumentID = AppControl.ControlCartera;
            this.FrmModule = ModulesPrefix.cc;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_002, AppMasters.ccLineaCredito, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_003, AppMasters.glZona, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_004, AppMasters.seUsuario, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_005, AppMasters.glAreaFuncional, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_006, AppMasters.glAreaFuncional, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_007, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_008, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_009, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_016, AppMasters.ccCompradorCartera, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_017, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_019, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_020, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_021, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_022, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_023, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_024, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_025, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_026, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_027, AppMasters.cpAnticipoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_029, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_030, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_031, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_032, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_033, AppMasters.tsCaja, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_034, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_035, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_036, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_037, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_038, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_043, AppMasters.ccClasificacionCredito, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_046, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_049, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_052, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_053, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_055, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_056, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_057, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_059, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_060, AppMasters.glActividadFlujo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_067, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_070, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_074, AppMasters.ccAbogado, false, true, true, false);
            //_bc.InitMasterUC(this.ctrl_039, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_500, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_501, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_502, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_503, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_504, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_505, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_506, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_507, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_509, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_511, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_512, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_513, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_514, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_515, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_526, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_534, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_535, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_536, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_539, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_541, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_542, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_543, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_544, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_545, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_549, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_600, AppMasters.ccPagaduria, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_601, AppMasters.ccPagaduria, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_602, AppMasters.ccPagaduria, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_603, AppMasters.ccPagaduria, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_604, AppMasters.ccPagaduria, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_605, AppMasters.ccPagaduria, false, true, true, false);
            this._bc.InitMasterUC(this.ctrl_045, AppMasters.cpConceptoCXP, false, true, true, false);
            this._bc.InitMasterUC(this.ctrl_047, AppMasters.coDocumento, false, true, true, false);
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);
            // Combos
            TablesResources.GetTableResources(this.ctrl_058, typeof(TipoInteres));
            TablesResources.GetTableResources(this.ctrl_066, typeof(TasaVenta));
            TablesResources.GetTableResources(this.ctrl_015, typeof(SectorCartera));

            // Otras propiedades
            this.ctrl_508.Enabled = false;
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

        #region Funciones Privadas

        /// <summary>
        /// Valida que solo Numeros se puedan escribir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void ctrl_018_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
            if (!char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == '\b')
                    e.Handled = false;
                else
                    e.Handled = true;
            }
        }

        #endregion

        private void tpProvisiones_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tpGeneral_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ctrl_070_Load(object sender, EventArgs e)
        {

        }

        private void lbl_070_Click(object sender, EventArgs e)
        {

        }

        private void ctrl_026_Load(object sender, EventArgs e)
        {

        }

        private void lbl_026_Click(object sender, EventArgs e)
        {

        }

        private void ctrl_025_Load(object sender, EventArgs e)
        {

        }

        private void lbl_025_Click(object sender, EventArgs e)
        {

        }

    }
}
