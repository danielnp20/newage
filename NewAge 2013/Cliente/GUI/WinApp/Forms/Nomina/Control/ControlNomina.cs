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
    public partial class ControlNomina : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlNomina()
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
            this.DocumentID = AppControl.ControlNomina;
            this.FrmModule = ModulesPrefix.no;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_007, AppMasters.coRegimenFiscal, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_008, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_031, AppMasters.glBienServicioClase, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_032, AppMasters.glBienServicioClase, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_033, AppMasters.glBienServicioClase, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_034, AppMasters.glBienServicioClase, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_035, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_036, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_037, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_038, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_039, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_040, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_042, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_045, AppMasters.coPlanCuenta, false, true, true, false);

            _bc.InitMasterUC(this.ctrl_046, AppMasters.coPlanCuenta, false, true, true, false);

            _bc.InitMasterUC(this.ctrl_300, AppMasters.noComponenteNomina, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_301, AppMasters.noComponenteNomina, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_302, AppMasters.noComponenteNomina, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_303, AppMasters.noComponenteNomina, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_304, AppMasters.noComponenteNomina, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_400, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_401, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_402, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_403, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_404, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_405, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_406, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_407, AppMasters.noConceptoNOM, false, true, true, false); 
            _bc.InitMasterUC(this.ctrl_408, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_409, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_450, AppMasters.noContratoNov, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_451, AppMasters.noContratoNov, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_452, AppMasters.noContratoNov, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_500, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_501, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_502, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_503, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_504, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_505, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_506, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_507, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_508, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_509, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_510, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_511, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_600, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_601, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_602, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_603, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_604, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_605, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_606, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_607, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_608, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_609, AppMasters.noConceptoNOM, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_610, AppMasters.noConceptoNOM, false, true, true, false);
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);
            TablesResources.GetTableResources(this.ctrl_003, typeof(UltimaNomina));

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

        private void ctrl_003_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && e.KeyChar != 1)
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        #endregion

    }
}
