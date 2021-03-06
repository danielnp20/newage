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
    public partial class ControlCarteraFin : ControlForm
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlCarteraFin()
        {
           // InitializeComponent();
        }

        public ControlCarteraFin(string mod = null) 
            : base()
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
            this.DocumentID = AppControl.ControlCarteraFin;
            this.FrmModule = ModulesPrefix.cc;
            this.isFinanciera = true;

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
            _bc.InitMasterUC(this.ctrl_045, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_046, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_047, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_049, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_052, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_053, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_055, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_056, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_057, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_059, AppMasters.coComprobante, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_060, AppMasters.ccCliente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_062, AppMasters.ccAsesor, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_063, AppMasters.ccPagaduria, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_067, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_070, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_074, AppMasters.ccAbogado, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_077, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_078, AppMasters.ccCarteraComponente, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_081, AppMasters.ccConcesionario, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_082, AppMasters.ccCentroPagoPAG, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_083, AppMasters.ccProfesion, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_084, AppMasters.cpConceptoCXP, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_101, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_102, AppMasters.coDocumento, false, true, true, false);
            //_bc.InitMasterUC(this.ctrl_039, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_500, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_501, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_502, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_504, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_505, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_506, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_507, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_509, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_510, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_511, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_512, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_513, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_514, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_515, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_526, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_527, AppMasters.coPlanCuenta, false, true, true, false);
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
            _bc.InitMasterUC(this.ctrl_118, AppMasters.seUsuario, false, true, true, false);
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 0);
            // Combos
            TablesResources.GetTableResources(this.ctrl_058, typeof(TipoInteres));
            TablesResources.GetTableResources(this.ctrl_066, typeof(TasaVenta));
            TablesResources.GetTableResources(this.ctrl_015, typeof(SectorCartera));

            // Otras propiedades
            this.ctrl_508.Enabled = false;
            this.ctrl_063.EnableControl(false);            
            #endregion
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //this.FrmModule = ModulesPrefix.cc;
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

        /// <summary>
        /// Centro de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrl_082_Leave(object sender, EventArgs e)
        {
            if (this.ctrl_082.ValidID)
            {
                DTO_ccCentroPagoPAG cp = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.ctrl_082.Value, true);
                this.ctrl_063.Value = cp.PagaduriaID.Value;
            }
            else
                this.ctrl_063.Value = string.Empty;
        }

        #endregion

    }
}
