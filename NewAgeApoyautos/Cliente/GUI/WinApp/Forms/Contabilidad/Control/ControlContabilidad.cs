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
    public partial class ControlContabilidad : ControlForm // FormWithToolbar //
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlContabilidad()
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
            this.DocumentID = AppControl.ControlContabilidad;
            this.FrmModule = ModulesPrefix.co;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_003, AppMasters.glEmpresa, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_004, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_005, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_009, AppMasters.glMoneda, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_010, AppMasters.glMoneda, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_011, AppMasters.coCentroCosto, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_012, AppMasters.coProyecto, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_013, AppMasters.coRegimenFiscal, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_014, AppMasters.plLineaPresupuesto, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_017, AppMasters.glPrefijo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_018, AppMasters.glLugarGeografico, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_019, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_020, AppMasters.coConceptoCargo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_021, AppMasters.glConceptoSaldo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_100, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_101, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_102, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_103, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_104, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_105, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_106, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_107, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_108, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_109, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_110, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_111, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_112, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_114, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_115, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_117, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_118, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_120, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_121, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_122, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_123, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_125, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_126, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_127, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_132, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_133, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_134, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_135, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_139, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_141, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_142, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_143, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_144, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_146, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_147, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_148, AppMasters.coBalanceTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_152, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_156, AppMasters.glZona, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_157, AppMasters.coDocumento, false, true, true, false);

            TablesResources.GetTableResources(this.ctrl_026, typeof(ContabilizaIVA));
            TablesResources.GetTableResources(this.ctrl_027, typeof(ImpuestoLoc));
            // Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);
            #endregion
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //Deshabilita el mes 14 si no esta activo el indicador de Mes 14
            if (ctrl_008.CheckState == CheckState.Unchecked)
                _bc.InitPeriodUC(this.ctrl_001, 1);

            //Deshabilita la ultima pestaña
            if (!_bc.AdministrationModel.MultiMoneda)
            {
                this.ctrl_010.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_010.Value))
                    this.ctrl_010.Enabled = true;

                this.ctrl_101.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_101.Value))
                    this.ctrl_101.Enabled = true;

                this.ctrl_106.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_106.Value))
                    this.ctrl_106.Enabled = true;

                this.ctrl_107.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_107.Value))
                    this.ctrl_107.Enabled = true;

                this.ctrl_110.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_110.Value))
                    this.ctrl_110.Enabled = true;

                this.ctrl_010.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_010.Value))
                    this.ctrl_010.Enabled = true;

                this.ctrl_111.Enabled = false;
                if (string.IsNullOrWhiteSpace(this.ctrl_111.Value))
                    this.ctrl_111.Enabled = true;
            }

            string indMultim = _bc.GetControlValue(AppControl.IndicadorMultiMoneda);
            if (indMultim.Equals("0"))
            {
                this.ctrl_022.Checked = false;
                this.ctrl_022.Enabled = false;
            }
            else
                this.ctrl_022.Enabled = true;
        }

        /// <summary>
        /// Valida el formulario del control
        /// </summary>
        protected override bool ValidateControl()
        {
            #region IFRS
            if (this.ctrl_134.ValidID)
            {
                DTO_coComprobante comp134 = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, this.ctrl_133.Value, false);
                if (comp134.BalanceTipoID.Value != this.ctrl_125.Value)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CompAndBalanceIFRSDiferents));
                    return false;
                }
            } 
            #endregion
            #region FISCAL
            if (this.ctrl_134.ValidID)
            {
                DTO_coComprobante comp134 = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, this.ctrl_134.Value, false);
                if (comp134.BalanceTipoID.Value != this.ctrl_135.Value)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CompAndBalanceFiscalDiferents));
                    return false;
                }
            } 
            #endregion

            return true;
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

        /// <summary>
        /// Habilita o no el mes 14 para el control de fecha 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void ctrl_008_CheckedChanged(object sender, EventArgs e)
        {
            if (ctrl_008.CheckState == CheckState.Unchecked)
            {
                this.ctrl_001.DateTime = new DateTime(this.ctrl_001.DateTime.Year, this.ctrl_001.DateTime.Month, 2);
                ctrl_001.ExtraPeriods = 1;
            }
            else if (ctrl_008.CheckState == CheckState.Checked)
            {
                this.ctrl_001.DateTime = new DateTime(this.ctrl_001.DateTime.Year, this.ctrl_001.DateTime.Month, 3);
                ctrl_001.ExtraPeriods = 2;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento para validar el campo de moneda extranjera cuando no es editable.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">args</param>
        private void ctrl_009_Leave(object sender, EventArgs e)
        {
            if (ctrl_110.Enabled == false)
                ctrl_110.Value = ctrl_109.Value;
        }

        /// <summary>
        /// Evento para validar el campo de moneda extranjera cuando no es editable.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">args</param>
        private void ctrl_104_Leave(object sender, EventArgs e)
        {
            if (ctrl_106.Enabled == false)
                ctrl_106.Value = ctrl_104.Value;
        }

        /// <summary>
        /// Evento para validar el campo de moneda extranjera cuando no es editable.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">args</param>
        private void ctrl_105_Leave(object sender, EventArgs e)
        {
            if (ctrl_107.Enabled == false)
                ctrl_107.Value = ctrl_105.Value;
        }

        /// <summary>
        /// Evento para validar el campo de moneda extranjera cuando no es editable.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">args</param>
        private void ctrl_108_Leave(object sender, EventArgs e)
        {
            if (ctrl_110.Enabled == false)
                ctrl_110.Value = ctrl_108.Value;
        }

        /// <summary>
        /// Evento para validar el campo de moneda extranjera cuando no es editable.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">args</param>
        private void ctrl_109_Leave(object sender, EventArgs e)
        {
            if (ctrl_111.Enabled == false)
                ctrl_111.Value = ctrl_109.Value;
        }

        /// <summary>
        /// Evento que se ejecuta al digitar valores en el centro de costo corporativo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrl_002_KeyPress(object sender, KeyPressEventArgs e)
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
            if (e.KeyChar == 10)
                e.Handled = true;
        }

        #endregion

        
        
    }
}
