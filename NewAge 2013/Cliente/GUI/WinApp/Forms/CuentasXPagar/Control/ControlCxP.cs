﻿using System;
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
    public partial class ControlCxP : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlCxP()
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
            this.DocumentID = AppControl.ControlCxP;
            this.FrmModule = ModulesPrefix.cp;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_004, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_006, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_007, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_016, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_017, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_018, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_019, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_020, AppMasters.coImpuestoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_022, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_028, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_029, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_030, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_032, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_033, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_034, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_035, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_036, AppMasters.glDocumentoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_037, AppMasters.glDocumentoClase, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_031, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_039, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_040, AppMasters.glDocumentoMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_041, AppMasters.glDocumentoMovimientoTipo, false, true, true, false);
            
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
            if (ctrl_023.CheckState == CheckState.Unchecked)
                _bc.InitPeriodUC(this.ctrl_001, 1);
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