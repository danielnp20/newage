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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para reiniciar la contraseña de un usuario
    /// </summary>
    public partial class RenewPassword : Form
    {
        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Identificador unico de la forma 
        private int _frmCode = AppForms.RenewPassword;
        private string _oldDate = string.Empty;

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// <param name="d">Fecha de contrasea actual</param>
        /// </summary>
        public RenewPassword(DateTime d)
        {
            try
            {
                this._oldDate = d.ToString(FormatString.Date);

                //Inicializa el formulario
                InitializeComponent();
                FormProvider.LoadResources(this, _frmCode);

                this.txtOldPassword.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenewPassword.cs", "RenewPassword"));
            }
        }

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta para validar las credenciales del usuario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida que se hayan digitado todos los campos
                if (string.IsNullOrEmpty(this.txtOldPassword.Text) || string.IsNullOrEmpty(this.txtNewPassword.Text) || string.IsNullOrEmpty(this.txtConfirmPassword.Text))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdEmptyFields));
                    this.txtOldPassword.Text = string.Empty;
                    this.txtNewPassword.Text = string.Empty;
                    this.txtConfirmPassword.Text = string.Empty;
                    return;
                }

                //Valida que la nueva contraseña este bien escrita
                if (!this.txtNewPassword.Text.Equals(this.txtConfirmPassword.Text))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdInvalidConfirm));
                    this.txtOldPassword.Text = string.Empty;
                    this.txtNewPassword.Text = string.Empty;
                    this.txtConfirmPassword.Text = string.Empty;
                    return;
                }

                //Valida la longitud de la nueva contraseña
                if (this.txtNewPassword.Text.Length < 6)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdInvalidLenght));
                    this.txtOldPassword.Text = string.Empty;
                    this.txtNewPassword.Text = string.Empty;
                    this.txtConfirmPassword.Text = string.Empty;
                    return;
                }

                //Valida el formato de la nueva contraseña
                string cleanPass = Utility.RemoveDigits(this.txtNewPassword.Text);
                if (cleanPass == this.txtNewPassword.Text || cleanPass == string.Empty)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdInvalidFormat));
                    this.txtOldPassword.Text = string.Empty;
                    this.txtNewPassword.Text = string.Empty;
                    this.txtConfirmPassword.Text = string.Empty;
                    return;
                }

                //Valida que la contraseña vieja escrita por el usuario sea correcta
                string usr = _bc.AdministrationModel.User.ID.Value;
                var userVal = _bc.AdministrationModel.seUsuario_ValidateUserCredentials(usr, this.txtOldPassword.Text);
                if (userVal == UserResult.NotExists)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdInvalid));
                    this.txtOldPassword.Text = string.Empty;
                    this.txtNewPassword.Text = string.Empty;
                    this.txtConfirmPassword.Text = string.Empty;
                    this.txtOldPassword.Focus();
                    return;
                }
                else if (userVal == UserResult.AlreadyMember)
                {
                    int usrID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
                    bool res = _bc.AdministrationModel.seUsuario_UpdatePassword(usrID, this.txtNewPassword.Text, this.txtOldPassword.Text, this._oldDate);
                    
                    if(res)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdUpdated));
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PwdUpdatedErr));

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Opcion inexistente (quemado)");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenewPassword.cs", "btnUpdatePassword_Click"));
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
                btnUpdatePassword_Click(sender, e);
        }

        #endregion

    }
}
