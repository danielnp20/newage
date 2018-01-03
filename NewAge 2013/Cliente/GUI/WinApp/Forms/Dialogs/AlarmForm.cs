using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Clase para mostrar mensajes
    /// </summary>
    public partial class AlarmForm : Form
    {
        /// <summary>
        /// Controlador base
        /// </summary>
        private BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public AlarmForm()
        {
            InitializeComponent();
            string userName = _bc.AdministrationModel.User.ID.Value;
            List<DTO_Alarma> alarmas = _bc.AdministrationModel.Alarmas_GetAll(userName).ToList();
            FormProvider.LoadResources(this, AppForms.AlarmForm);

            int i = 1;
            string msg = string.Empty;

            string alarmInfo = string.Empty;
            string alarmStr = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Alarm);
            string points = ";";
            string tab = "\t";
            string data = tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Task) + ": {1}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentType) + ": ({2}) {3}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Nit) + ": {4}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Name) + ": {5}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Prefix) + ": {6}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Doc) + ": {7}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Responsible) + ": {8}{0}" +
                            tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentUrl) + ": {9}";

            if (alarmas.Count > 0)
            {
                foreach (DTO_Alarma alarma in alarmas)
                {
                    alarmInfo = string.Format(data, Environment.NewLine, alarma.Actividad, alarma.DocumentoID, alarma.DocumentoDesc, alarma.TerceroID, alarma.TerceroDesc,
                        alarma.PrefijoID, alarma.Consecutivo, alarma.UsuarioRESP, alarma.FileName);

                    msg += alarmStr + i.ToString() + points;
                    msg += Environment.NewLine;
                    msg += alarmInfo;
                    msg += Environment.NewLine;
                    i++;
                }
            }
            else
            {
                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoPendingAlarms);
            }

            this.lblMessage.Text = msg;
        }

        #region Eventos

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
