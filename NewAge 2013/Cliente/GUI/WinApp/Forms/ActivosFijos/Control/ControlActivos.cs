using System;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para control
    /// </summary>
    public partial class ControlActivos : ControlForm 
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        #endregion

        public ControlActivos()
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
            this.DocumentID = AppControl.ControlActivos;
            this.FrmModule = ModulesPrefix.ac;

                #region Inicializar controles

            //Controles de FK para maestras
            _bc.InitMasterUC(this.ctrl_003, AppMasters.acMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_004, AppMasters.coDocumento, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_007, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_008, AppMasters.coPlanCuenta, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_011, AppMasters.acEstado, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_013, AppMasters.acMovimientoTipo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_014, AppMasters.faAsesor, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_015, AppMasters.glConceptoSaldo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_500, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_501, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_502, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_503, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_504, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_594, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_595, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_596, AppMasters.acComponenteActivo, false, true, true, false);
            _bc.InitMasterUC(this.ctrl_597, AppMasters.acComponenteActivo, false, true, true, false);

            TablesResources.GetTableResources(this.ctrl_400, typeof(TipoDepreciacion));
            TablesResources.GetTableResources(this.ctrl_401, typeof(BaseCalculo));
            TablesResources.GetTableResources(this.ctrl_402, typeof(TipoCalculoIFRS));
            TablesResources.GetTableResources(this.ctrl_403, typeof(TipoCalculoIFRS));
            
            //// Controles de periodos
            _bc.InitPeriodUC(this.ctrl_001, 2);
            
            #endregion
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            string indBloqModulo = this._bc.GetControlValueByCompany(ModulesPrefix.ac,AppControl.ac_IndBloqueoModuloTransAuxPrelim,true);
            if (indBloqModulo.Equals("0"))
            {
                this.ctrl_999.Checked = false;
                this.ctrl_999.Enabled = false;
            }
            else
                this.ctrl_999.Enabled = true;
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
