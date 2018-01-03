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
    public partial class ControlOperacionesConjuntas : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlOperacionesConjuntas()
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
            this.DocumentID = AppControl.ControlOperacionesConjuntas;
            this.FrmModule = ModulesPrefix.oc;

            #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_002, AppMasters.ocSocio, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_004, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_006, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_008, AppMasters.ocOtrosConceptos, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_009, AppMasters.ocOtrosConceptos, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_010, AppMasters.ocOtrosConceptos, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_011, AppMasters.ocOtrosConceptos, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_012, AppMasters.ocOtrosConceptos, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_013, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_014, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_016, AppMasters.ocSocio, false, true, true, false);
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);

            TablesResources.GetTableResources(this.ctrl_007, typeof(TipoIVA));
            #endregion
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
        }

        #endregion

    }
}
