using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalIncorporaciones : Form
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private int documentID;
        private int userID;
        private string areaFuncionalID;
        private DTO_noEmpleado _empleado = null;

        #endregion

        public ModalIncorporaciones()
        {
            this.SetInitParameters();
            FormProvider.LoadResources(this, AppMasters.noEmpleado);
        }       

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.uc_MasterFindTercero, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindLugarGeografico, AppMasters.glLugarGeografico, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindCargo, AppMasters.rhCargos, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindConvencion, AppMasters.noConvencion, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindFondoSalud, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindFondoPension, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindFondoCesantias, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindCaja, AppMasters.noCaja, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindRiesgo, AppMasters.noRiesgo, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindTurno, AppMasters.noTurnoCompensatorio, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindRol, AppMasters.noRol, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindAreaFuncional, AppMasters.glAreaFuncional, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindFondoSalud, AppMasters.noFondo, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindOperacion, AppMasters.noOperacion, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindSeleccionRH, AppMasters.rhSeleccionForma, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindBrigada, AppMasters.noBrigada, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MasterFindBanco, AppMasters.tsBanco, true, true, true, false);            

            this.dtFechaNacimiento.DateTime = DateTime.Now;
            this.dtFechaIngreso.DateTime = DateTime.Now;
            this.dtFechaRetiro.Enabled = false;
           
            TablesResources.GetTableResources(this.cmbSexo, AppMasters.noEmpleado, "Sexo", DictionaryTables.Sexo);
            TablesResources.GetTableResources(this.cmbEstadoCivil, AppMasters.noEmpleado, "EstadoCivil", DictionaryTables.EstadoCivil);
            TablesResources.GetTableResources(this.cmbTipoContrato, AppMasters.noEmpleado, "TipoContrato", DictionaryTables.TipoContrato);
            TablesResources.GetTableResources(this.cmbEstado, AppMasters.noEmpleado, "Estado", DictionaryTables.Estado);
            TablesResources.GetTableResources(this.cmbProcedimientoRET, AppMasters.noEmpleado, "ProcedimientoRteFte", DictionaryTables.ProcedimientoRteFte);
            TablesResources.GetTableResources(this.cmbPeriodoPago, AppMasters.noEmpleado, "PeriodoPago", DictionaryTables.PeriodoPago);
            TablesResources.GetTableResources(this.cmbFormaPago, AppMasters.noEmpleado, "FormaPago", DictionaryTables.FormaPago);
            TablesResources.GetTableResources(this.cmbTipoCuenta, AppMasters.noEmpleado, "TipoCuenta", DictionaryTables.TipoCuenta);
            TablesResources.GetTableResources(this.txtFactorRH, AppMasters.noEmpleado, "FactorRH", DictionaryTables.FactorRH);
            TablesResources.GetTableResources(this.cmbPaseCategoria, AppMasters.noEmpleado, "PaseCategoria", DictionaryTables.PaseCategoria);
            TablesResources.GetTableResources(this.cmbGrupoSanguineo, AppMasters.noEmpleado, "GrupoSanguineo", DictionaryTables.GrupoSanguineo);
            TablesResources.GetTableResources(this.cmbTerminoContrato, AppMasters.noEmpleado, "TerminoContrato", DictionaryTables.TerminoContrato);

            this.cmbSexo.SelectedIndex = 0;
            this.cmbEstadoCivil.SelectedIndex = 0;
            this.cmbTipoContrato.SelectedIndex = 0;
            this.cmbEstado.SelectedIndex = 0;
            this.cmbProcedimientoRET.SelectedIndex = 0;
            this.cmbPeriodoPago.SelectedIndex = 0;
            this.cmbPeriodoPago.SelectedIndex = 0;
            this.cmbTipoCuenta.SelectedIndex = 0;
            this.cmbFormaPago.SelectedIndex = 0;
            this.txtFactorRH.SelectedIndex = 0;
            this.cmbPaseCategoria.SelectedIndex = 0;
            this.cmbTerminoContrato.SelectedIndex = 0;
            this.cmbGrupoSanguineo.SelectedIndex = 0;

        }

       
        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
           
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected void LoadData(bool firstTime)
        {

        }
       
        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppMasters.noEmpleado;
            this._empleado = new DTO_noEmpleado();

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;

            this.InitControls();
            this.LoadData(true);
            this.AfterInitialize();
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected void AfterInitialize()
        {
           
        }

        /// <summary>
        /// Valida los campos obligatorios
        /// </summary>
        /// <param name="emp">objeto empleado</param>
        /// <returns>mensaje de validación</returns>
        protected string ValidateDTO(DTO_noEmpleado emp)
        {
            try
            {

                string msg = string.Empty;
                string msgCampoVacion = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);

                if (this.dtFechaRetiro.Enabled == true)
                {
                    if (string.IsNullOrEmpty(this.dtFechaRetiro.Text))
                        return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("FechaRetiro").Name);
                }
                if (string.IsNullOrEmpty(this.uc_MasterFindTercero.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("TerceroID").Name);

                if (string.IsNullOrEmpty(this.txtEmpleadoID.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("ID").Name);

                if (string.IsNullOrEmpty(this.txtNombreEmpleado.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("Descriptivo").Name);

                if (string.IsNullOrEmpty(this.txtSeguro.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("SeguroID").Name);

                if (string.IsNullOrEmpty(this.txtLugarNacimiento.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("LugarNacimiento").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindLugarGeografico.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("LugarGeograficoID").Name);

                if (string.IsNullOrEmpty(this.txtDireccion.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("DireccionResidencia").Name);

                if (string.IsNullOrEmpty(this.txtTelefono.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("TelefonoResidencia").Name);

                if (string.IsNullOrEmpty(this.txtCelular.Text))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("TelefonoCelular").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindCargo.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("CargoEmpID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindConvencion.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("ConvencionNOID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindFondoSalud.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("FondoSalud").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindFondoPension.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("FondoPension").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindCaja.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("CajaNOID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindRiesgo.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("RiesgoNOID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindTurno.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("TurnoCompID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindRol.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("RolNOID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindAreaFuncional.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("AreaFuncionalID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindOperacion.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("OperacionNOID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindProyecto.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("ProyectoID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindCentroCosto.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("CentroCostoID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindCentroCosto.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("BrigadaID").Name);

                if (string.IsNullOrEmpty(this.uc_MasterFindSeleccionRH.Value))
                    return msg = string.Format(msgCampoVacion, emp.GetType().GetProperty("SeleccionRHID").Name);      

                return msg;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Guarda un Empleado en la Base de Datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this._empleado.ID.Value = this.txtEmpleadoID.Text;
                this._empleado.Descriptivo.Value = this.txtNombreEmpleado.Text;
                this._empleado.TerceroID.Value = this.uc_MasterFindTercero.Value;
                this._empleado.SeguroID.Value = this.txtSeguro.Text;
                this._empleado.Sexo.Value = Convert.ToByte((this.cmbSexo.SelectedItem as ComboBoxItem).Value);
                this._empleado.EstadoCivil.Value = Convert.ToByte((this.cmbEstadoCivil.SelectedItem as ComboBoxItem).Value);
                this._empleado.FechaNacimiento.Value = this.dtFechaNacimiento.DateTime;
                this._empleado.LugarNacimiento.Value = this.txtLugarNacimiento.Text;
                this._empleado.LugarGeograficoID.Value = this.uc_MasterFindLugarGeografico.Value;
                this._empleado.MonedaExtInd.Value = this.chkMdaExtInd.Checked;
                this._empleado.SalarioVariableInd.Value = this.chkSalarioVariableInd.Checked;
                this._empleado.DireccionResidencia.Value = this.txtDireccion.Text;
                this._empleado.TelefonoResidencia.Value = this.txtTelefono.Text;
                this._empleado.TelefonoCelular.Value = this.txtCelular.Text;
                this._empleado.CorreoElectronico.Value = this.txtEmail.Text;
                this._empleado.FactorRH.Value = Convert.ToByte((this.txtFactorRH.SelectedItem as ComboBoxItem).Value);
                this._empleado.PaseCategoria.Value = Convert.ToByte((this.cmbPaseCategoria.SelectedItem as ComboBoxItem).Value);
                this._empleado.PaseMotoInd.Value = this.chkPaseMoto.Checked;
                this._empleado.TipoContrato.Value = Convert.ToByte((this.cmbTipoContrato.SelectedItem as ComboBoxItem).Value);
                this._empleado.FechaIngreso.Value = this.dtFechaIngreso.DateTime;
                if(this.dtFechaRetiro.Enabled==true)
                    this._empleado.FechaRetiro.Value = this.dtFechaRetiro.DateTime;
                this._empleado.DiasContrato.Value = !string.IsNullOrEmpty(txtDiasContrato.Text) ? Convert.ToInt32(txtDiasContrato.Text) : 0;
                this._empleado.Sueldo.Value = !string.IsNullOrEmpty(txtSueldo.Text) ? Convert.ToDecimal(this.txtSueldo.EditValue, CultureInfo.InvariantCulture) : 0;
                this._empleado.Estado.Value = Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value);
                this._empleado.EmplConfianzaInd.Value = this.chkEmpConfianza.Checked;
                this._empleado.NoAuxilioTranspInd.Value = this.chkIndTransporte.Checked;
                this._empleado.NoAporteSaludInd.Value = this.chkIndSalud.Checked;
                this._empleado.QuincenalInd.Value = this.chkPagoQuincenalInd.Checked;
                this._empleado.NoAportePensionInd.Value = this.chkIndPension.Checked;
                this._empleado.ProcedimientoRteFte.Value = Convert.ToByte((this.cmbProcedimientoRET.SelectedItem as ComboBoxItem).Value);
                this._empleado.GrupoSanguineo.Value = Convert.ToByte((this.cmbGrupoSanguineo.SelectedItem as ComboBoxItem).Value);
                if (!string.IsNullOrEmpty(this.txtPorcentajeRET.Text))
                    this._empleado.PorcentajeRteFte.Value = Convert.ToDecimal(this.txtPorcentajeRET.EditValue, CultureInfo.InvariantCulture);
                this._empleado.DeclaraRentaInd.Value = this.chkDeclaraRenta.Checked;
                this._empleado.DependenciaInd.Value = chkDependencia.Checked;
                this._empleado.Pagos1.Value = 0;
                this._empleado.Pagos2.Value = 0;
                this._empleado.Pagos3.Value = 0;
                this._empleado.Pagos4.Value = 0;
                this._empleado.Pagos5.Value = 0;
                this._empleado.Pagos6.Value = 0;
                this._empleado.Pagos7.Value = 0;
                this._empleado.Pagos8.Value = 0;
                this._empleado.Pagos9.Value = 0;
                this._empleado.Pagos10.Value = 0;
                if (!string.IsNullOrEmpty(this.txtCupoCompFlexible.Text))
                    this._empleado.CupoCompFlexible.Value = Convert.ToDecimal(txtCupoCompFlexible.EditValue, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(this.txtCupoPlusAdicional.Text))
                    this._empleado.CupoPlusAdicional.Value = Convert.ToDecimal(txtCupoPlusAdicional.EditValue, CultureInfo.InvariantCulture);
                this._empleado.CargoEmpID.Value = this.uc_MasterFindCargo.Value;
                this._empleado.ConvencionNOID.Value = this.uc_MasterFindConvencion.Value;
                this._empleado.FondoSalud.Value = this.uc_MasterFindFondoSalud.Value;
                this._empleado.FondoPension.Value = this.uc_MasterFindFondoPension.Value;
                this._empleado.FondoCesantias.Value = this.uc_MasterFindFondoCesantias.Value;
                this._empleado.CajaNOID.Value = this.uc_MasterFindCaja.Value;
                this._empleado.RiesgoNOID.Value = this.uc_MasterFindRiesgo.Value;
                this._empleado.TurnoCompID.Value = this.uc_MasterFindTurno.Value;
                this._empleado.RolNOID.Value = this.uc_MasterFindRol.Value;
                this._empleado.AreaFuncionalID.Value = this.uc_MasterFindAreaFuncional.Value;
                this._empleado.OperacionNOID.Value = this.uc_MasterFindOperacion.Value;
                this._empleado.ProyectoID.Value = this.uc_MasterFindProyecto.Value;
                this._empleado.CentroCostoID.Value = this.uc_MasterFindCentroCosto.Value;
                this._empleado.BrigadaID.Value = this.uc_MasterFindBrigada.Value;
                this._empleado.SeleccionRHID.Value = this.uc_MasterFindSeleccionRH.Value;
                this._empleado.PeriodoPago.Value = Convert.ToByte((this.cmbPeriodoPago.SelectedItem as ComboBoxItem).Value);
                this._empleado.BancoID.Value = this.uc_MasterFindBanco.Value;
                this._empleado.FormaPago.Value = Convert.ToByte((this.cmbFormaPago.SelectedItem as ComboBoxItem).Value);
                this._empleado.TipoCuenta.Value = Convert.ToByte((this.cmbTipoCuenta.SelectedItem as ComboBoxItem).Value);
                this._empleado.TerminoContrato.Value = Convert.ToByte((this.cmbTerminoContrato.SelectedItem as ComboBoxItem).Value);
                if (!string.IsNullOrEmpty(this.txtNroCuenta.Text))
                    this._empleado.CuentaAbono.Value = txtNroCuenta.Text;
                this._empleado.CtrlVersion.Value = 1; //Seteo para inserción de registros

                string msgValid = this.ValidateDTO(this._empleado);

                if (string.IsNullOrEmpty(msgValid))
                {
                    var result = this._bc.AdministrationModel.Nomina_IncorporacionEmpleado(this._empleado);
                    if (result.Result == ResultValue.OK)
                    {
                        this.RefreshDocument();
                        //this.Close();
                    }

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
                else
                {
                    MessageForm frm = new MessageForm(msgValid, MessageType.Error);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Validación sobre la fecha de Nacimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaNacimiento_EditValueChanged(object sender, EventArgs e)
        {
            if (dtFechaNacimiento.DateTime > DateTime.Now)
            {
                MessageForm frm = new MessageForm(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.No_FechaNacimientoNotValid), MessageType.Error);
            }
        }        

        #endregion

        private void TerminoContrato_EditValueChanged(object sender, EventArgs e)
        {
            if (this.cmbTerminoContrato.SelectedIndex == 1)
                this.dtFechaRetiro.Enabled = true;
            else
            {
                if (!string.IsNullOrEmpty(this.dtFechaRetiro.Text))
                    this.dtFechaRetiro.Text = "";
                this.dtFechaRetiro.Enabled = false;
            }
        }

        private void Tercero_Leave(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.uc_MasterFindTercero.Value))
            {
                DTO_coTercero tercero = new DTO_coTercero();
                tercero=(DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.coTercero,false,this.uc_MasterFindTercero.Value,true);
                if (tercero != null)
                {
                    this.txtNombreEmpleado.Text = tercero.ApellidoPri.Value + " " + tercero.ApellidoSdo.Value + " " + tercero.NombrePri.Value + " " + tercero.NombreSdo.Value;
                    this.txtEmpleadoID.Text = tercero.ID.Value;
                    this.uc_MasterFindLugarGeografico.Value = tercero.LugarGeograficoID.Value;
                    this.txtDireccion.Text = tercero.Direccion.Value;
                    this.txtTelefono.Text = tercero.Tel1.Value;
                    this.txtCelular.Text = tercero.Tel2.Value;
                    this.txtEmail.Text = tercero.CECorporativo.Value;
                }
            }
        }

    }
}
