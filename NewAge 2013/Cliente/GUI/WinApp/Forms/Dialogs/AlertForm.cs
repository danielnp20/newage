using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Resultados;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Clase para mostrar mensajes
    /// </summary>
    public partial class AlertForm : Form
    {
        /// <summary>
        /// Constructor para mostrar un mensaje en el formulario
        /// </summary>
        /// <param name="title">Titulo del dialogo</param>
        /// <param name="warn">Advertencia</param>
        /// <param name="quest">Pregunta</param>
        /// <param name="formCode">codigo del formulario q aloja los mensajes</param>
        public AlertForm(string title, string warn, string quest)
        {
            BaseController bc = BaseController.GetInstance();
            InitializeComponent();
            this.Text = bc.GetResource(LanguageTypes.Forms,title);
            this.lblQuest.Text = bc.GetResource(LanguageTypes.Forms,quest);
            this.lblWarn.Text = bc.GetResource(LanguageTypes.Forms,warn);
            this.btnYes.Text = bc.GetResource(LanguageTypes.Tables, "tbl_res_yes");
            this.btnNo.Text = bc.GetResource(LanguageTypes.Tables, "tbl_res_no");
        }

        #region  Eventos

        /// <summary>
        /// Evento que se ejecuta al cerrar el formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void AlertForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult != DialogResult.Yes)
                this.DialogResult = DialogResult.No;
        }

        /// <summary>
        /// Evento que se ejecuta al presionar una tecla
        /// </summary>
        /// <param name="msg">Mensaje del evento</param>
        /// <param name="keyData">tecla presionada</param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

    }
}
