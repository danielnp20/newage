using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de ingreso
    /// </summary>
    public partial class LogIn : Form
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private ModulesPrefix _frmModule = ModulesPrefix.apl;
        private FormTypes _frmType = FormTypes.Other;
        //Identificador unico de la forma 
        private int _frmCode = AppForms.LognIn;

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public LogIn()
        {
            try
            {
                //Inicializa el formulario
                InitializeComponent();
                FormProvider.LoadResources(this, _frmCode);
                this.MdiParent = FormProvider.Master;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LogIn.cs", "LogIn"));
            }
        }

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta para validar las credenciales del usuario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                //Carga la información y seguridades del usuario
                this.lblMsg.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData);
                this.Enabled = false;

                #region validaciones de credenciales
                if (string.IsNullOrEmpty(this.txtUser.Text) || string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginNoData));
                    return;
                }

                var userVal = _bc.AdministrationModel.seUsuario_ValidateUserCredentials(this.txtUser.Text, this.txtPassword.Text);
                if (userVal == UserResult.NotExists)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginFailure));
                    this.txtUser.Focus();
                    this.txtPassword.Text = string.Empty;
                    return;
                }

                if (userVal == UserResult.BlockUser)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginUserBlocked));
                    this.txtUser.Focus();
                    this.txtPassword.Text = string.Empty;
                    return;
                }

                #endregion

                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this.txtUser.Text);

                #region Contraseña incorrecta
                if (userVal == UserResult.IncorrectPassword)
                {
                    string ctrl = _bc.GetControlValue(AppControl.RepeticionesContrasenaBloqueo);
                    int repPermitidas = Convert.ToInt16(ctrl);

                    if ((repPermitidas - user.ContrasenaRep.Value.Value) == 0)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginBlockUser));
                    }
                    else
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginIncorrectPwd);
                        msg = string.Format(msg, repPermitidas - user.ContrasenaRep.Value.Value);
                        MessageBox.Show(msg);
                    }

                    this.txtUser.Focus();
                    this.txtPassword.Text = string.Empty;
                    return;
                }
                #endregion
                #region Usuario Existente
                if (userVal == UserResult.AlreadyMember)
                {
                    //Carga la información y seguridades del usuario
                    this.lblMsg.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.CargandoInfo);

                    _bc.AdministrationModel.User = user;

                    #region Carga la info de la empresa y del usuario
                    //Trae la empresa y el grupo de seguridades para el usuario
                    Object empObj = _bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, false, user.EmpresaIDPref.Value, true);
                    if (empObj == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidUserCompany));
                        this.txtUser.Focus();
                        this.txtPassword.Text = string.Empty;
                        return;
                    }
                    DTO_glEmpresa emp = (DTO_glEmpresa)empObj;
                    var rsx = _bc.AdministrationModel.seGrupoDocumento_GetByUsuarioId(user.ReplicaID.Value.Value, user.EmpresaIDPref.Value);

                    //Asigna la empresa y trae el pais por defecto del usuario
                    _bc.AdministrationModel.Empresa = emp;
                    //Carga variables de control y de Grupos de empresa
                    _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, _bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();

                    DTO_MasterBasic mPais = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glPais, false, emp.PaisID.Value, true);
                    try
                    {
                        _bc.AdministrationModel.Pais = (DTO_glPais)mPais;
                    }
                    catch (Exception ex) { ; }

                    //Asigna el indicador de multimoneda para la empresa
                    string multimoneda = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
                    _bc.AdministrationModel.MultiMoneda = multimoneda == "1" ? true : false;

                    #endregion
                    #region Asignacion de seguridades del usuario
                    string outp = string.Empty;

                    foreach (var s in rsx)
                    {
                        _bc.AdministrationModel.FormsSecurity.TryGetValue(s.DocumentoID.ToString(), out outp);
                        if (string.IsNullOrEmpty(outp))
                        {
                            _bc.AdministrationModel.FormsSecurity.Add(s.DocumentoID.ToString(), s.AccionesPerm.ToString());
                        }
                        else
                        {
                            long oldV = Convert.ToInt64(outp);
                            long newV = (long)s.AccionesPerm.Value;
                            long ret = SecurityManager.SetFormSecurity(oldV, newV);

                            _bc.AdministrationModel.FormsSecurity[s.DocumentoID.ToString()] = ret.ToString();
                        }
                    }
                    #endregion
                    #region Carga de menus y configuracion

                    //Carga la lista de modulos
                    _bc.AdministrationModel.IniciarEmpresaUsuario(true);

                    FormProvider.Master.LoadModules();
                    DynamicMenu dm = new DynamicMenu(FormProvider.Master.MenuPath, FormProvider.Master.HelpPath);
                    _bc.AdministrationModel.Menus = dm.LoadMenu();

                    _bc.AdministrationModel.IniciarEmpresaUsuario(false);
                    #endregion
                    #region Carga de tablas para el grupo de empresas asignado

                    Dictionary<int, string> empGrupo = new Dictionary<int, string>();

                    empGrupo.Add((int)GrupoEmpresa.Automatico, emp.ID.Value);
                    empGrupo.Add((int)GrupoEmpresa.Individual, emp.EmpresaGrupoID_.Value);
                    empGrupo.Add((int)GrupoEmpresa.General, _bc.GetControlValue(AppControl.GrupoEmpresaGeneral));

                    _bc.AssignTablesByCompany(empGrupo);
                    #endregion
                    #region Actualizacion de la barra de estado
                    //Visibilidad de controles
                    FormProvider.Master.lblStatusUser.Visible = true;
                    FormProvider.Master.lblStatusCompany.Visible = true;
                    FormProvider.Master.ddlStatusCompany.Visible = true;
                    FormProvider.Master.lblStatusUserTit.Visible = true;
                    FormProvider.Master.lblStatusCompTit.Visible = true;
                    //Asignacion del usuario
                    FormProvider.Master.lblStatusUserTit.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ConnectionStatusUser) + " ";
                    FormProvider.Master.lblStatusUser.Text = user.ID.Value;
                    //Asignacion de la empresa
                    FormProvider.Master.lblStatusCompTit.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ConnectionStatusCompany) + " ";
                    FormProvider.Master.lblStatusCompany.Text = emp.Descriptivo.Value;
                    FormProvider.Master.ddlStatusCompany.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ConnectionStatusUpdateCompany);
                    //Asigna el listado de empresas disponibles para el usuario
                    List<DTO_glEmpresa> comps = _bc.AdministrationModel.seUsuario_GetUserCompanies(user.ID.Value).ToList();
                    foreach (DTO_glEmpresa comp in comps)
                        FormProvider.Master.ddlStatusCompany.DropDownItems.Add(comp.Descriptivo.Value);

                    #endregion
                    #region Revision de vigencia de password
                    //Verifica si el la clave esta vencida
                    TimeSpan span = DateTime.Now.Subtract(user.ContrasenaFecCambio.Value.Value);
                    int pwdDays = span.Days;
                    int pwdDuration = Convert.ToInt16(_bc.GetControlValue(AppControl.DuracionClave));
                    int pwdReminder = Convert.ToInt16(_bc.GetControlValue(AppControl.RecordatorioCambioClave));

                    if (pwdDays > pwdDuration)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdDefeated));
                        this.txtUser.Focus();
                        this.txtPassword.Text = string.Empty;
                        return;
                    }
                    else if ((pwdDuration - pwdDays) <= pwdReminder)
                    {
                        string msgReminder = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdReminder);
                        msgReminder = string.Format(msgReminder, (pwdDuration - pwdDays).ToString());
                        if (MessageBox.Show(msgReminder, string.Empty, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            RenewPassword rp = new RenewPassword(user.ContrasenaFecCambio.Value.Value);
                            rp.ShowDialog();
                        }
                    }
                    #endregion
                    #region Revisa si el usuario tiene tareas pendientes
                    bool hasAlarms = _bc.AdministrationModel.Alarmas_HasAlarms(this.txtUser.Text);
                    if (hasAlarms)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PendingAlarms));
                    #endregion

                    //Agrega eventos y cambia propiedades del padre
                    FormProvider.Master.pnlLeftContainer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    FormProvider.Master.tabForms.Visible = true;

                    this.Close();
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginError));
                    this.txtUser.Focus();
                    this.txtPassword.Text = string.Empty;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LogIn.cs", "btnLogIn_Click"));
            }
            finally
            {
                this.lblMsg.Text = string.Empty;
                this.Enabled = true;
                this.txtUser.Focus();
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._frmCode, this._frmType, this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LogIn.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Evento al presionar enter sobre caja de texto Password
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtBoxes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogIn_Click(sender, e);
        }

        #endregion

    }//clase
}//namespace


