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
    public partial class MessageForm : Form
    {
        /// <summary>
        /// Controlador base
        /// </summary>
        private BaseController _bc = BaseController.GetInstance();
        private string _tab = "\t";

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public MessageForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor para un objeto de tipo resultado
        /// </summary>
        /// <param name="result">Objeto con un resultado que se deba mostrar</param>
        /// <param name="type">Tipo de mensaje</param>
        public MessageForm(DTO_TxResult result)
        {
            InitializeComponent();
            try
            {
                if (result.Result == ResultValue.OK)
                {
                    this.SetTitle(MessageType.Confirmation);
                    if (string.IsNullOrWhiteSpace(result.ResultMessage))
                        this.lblMessage.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK);
                    else
                        this.lblMessage.Text = _bc.GetResourceFromResult(LanguageTypes.Messages, result.ResultMessage);
                }
                else
                {
                    this.SetTitle(MessageType.Error);

                    string msg = string.Empty;
                    if (!string.IsNullOrEmpty(result.ResultMessage))
                        msg = _bc.GetResourceError(result.ResultMessage) + Environment.NewLine;

                    if (result.Details != null)
                    {
                        #region Detalles
                        foreach (DTO_TxResultDetail detalle in result.Details)
                        {
                            string s1 = string.Empty;

                            if (!string.IsNullOrWhiteSpace(detalle.Message) && detalle.Message != "OK")
                            {
                                s1 = _bc.GetResourceError(detalle.Message);
                                if (s1 == detalle.Message)
                                    s1 = _bc.GetResource(LanguageTypes.Messages, detalle.Message);

                                msg += _tab + _bc.GetResourceFromResult(LanguageTypes.Messages, DictionaryMessages.LineMessage) + detalle.line.ToString() + ": " + s1 + Environment.NewLine;
                            }

                            #region Campos
                            if (detalle.DetailsFields != null)
                            {
                                string ext = "";
                                foreach (DTO_TxResultDetailFields campo in detalle.DetailsFields)
                                {
                                    if (campo.Message.ToUpper().StartsWith("ERR"))
                                    {
                                        string s2 = _bc.GetResourceError(campo.Message);
                                        msg += _tab + _tab + campo.Field + ": " + s2 + Environment.NewLine;
                                    }
                                    else
                                    {
                                        int place = campo.Message.Length;
                                        if (campo.Message.Contains("&&"))
                                        {
                                            place = campo.Message.IndexOf("&&");
                                            ext = campo.Message.Substring(place + 2);
                                        }

                                        string s2 = _bc.GetResource(LanguageTypes.Messages, campo.Message.Substring(0, place));
                                        var parametros = ext.Split(new string[] { "&&" }, StringSplitOptions.None);
                                        s2 = String.Format(s2, parametros);

                                        msg += _tab + _tab + campo.Field + ": " + _bc.GetResourceError(s2) + Environment.NewLine;
                                    }
                                }
                            }
                            #endregion

                        }
                        #endregion
                    }

                    this.lblMessage.Text = msg;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Constructor para un objeto de tipo resultado
        /// </summary>
        /// <param name="results">Objeto con un listado de resultados con error</param>
        public MessageForm(List<DTO_TxResult> results)
        {
            InitializeComponent();
            try
            {
                if (results == null)
                {
                    this.SetTitle(MessageType.Error);
                }
                else if (results.Count == 0)
                {
                    this.SetTitle(MessageType.Confirmation);
                    this.lblMessage.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK);
                }
                else
                {
                    bool OK = true;
                    string msg = string.Empty;
                    int i = -1;
                    foreach (DTO_TxResult result in results)
                    {
                        i++;
                        if (result.Result == ResultValue.NOK)
                        {
                            OK = false;

                            if (i > 0)
                                msg += Environment.NewLine;

                            msg += _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LineMessage) + i.ToString() + ": ";
                            if (!string.IsNullOrEmpty(result.ResultMessage))
                                msg += _bc.GetResourceError(result.ResultMessage);

                            if (result.Details != null)
                            {
                                msg += Environment.NewLine;
                                #region Detalles
                                foreach (DTO_TxResultDetail detalle in result.Details)
                                {
                                    string s1 = string.Empty;
                                    if (!string.IsNullOrWhiteSpace(detalle.Message) && detalle.Message != "OK")
                                    {
                                        s1 = _bc.GetResourceError(detalle.Message);
                                        if (s1 == detalle.Message)
                                            s1 = _bc.GetResource(LanguageTypes.Messages, detalle.Message);

                                        msg += _tab + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LineMessage) + detalle.line.ToString() + ": " + s1 + Environment.NewLine;
                                    }

                                    #region Campos
                                    if (detalle.DetailsFields != null)
                                    {
                                        string ext = "";
                                        foreach (DTO_TxResultDetailFields campo in detalle.DetailsFields)
                                        {
                                            if (campo.Message.ToUpper().StartsWith("ERR"))
                                            {
                                                string s2 = _bc.GetResourceError(campo.Message);
                                                msg += _tab + _tab + campo.Field + ": " + s2 + Environment.NewLine;
                                            }
                                            else
                                            {
                                                int place = campo.Message.Length;
                                                if (campo.Message.Contains("&&"))
                                                {
                                                    place = campo.Message.IndexOf("&&");
                                                    ext = campo.Message.Substring(place + 2);
                                                }

                                                string s2 = _bc.GetResource(LanguageTypes.Messages, campo.Message.Substring(0, place));
                                                var parametros = ext.Split(new string[] { "&&" }, StringSplitOptions.None);
                                                s2 = String.Format(s2, parametros);

                                                msg += _tab + _tab + campo.Field + ": " + _bc.GetResourceError(s2) + Environment.NewLine;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }

                    if (OK)
                    {
                        this.SetTitle(MessageType.Confirmation);
                        this.lblMessage.Text = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK);
                    }
                    else
                    {
                        this.SetTitle(MessageType.Error);
                        this.lblMessage.Text = msg;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Constructor para mostrar un mensaje en el formulario
        /// </summary>
        /// <param name="msg">Mensaje que se desea mostrar</param>
        /// <param name="type">Tipo de mensaje</param>
        public MessageForm(string msg, MessageType type)
        {
            InitializeComponent();
            this.SetTitle(type);
            this.lblMessage.Text = msg;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Asigna el titulo del formulario 
        /// </summary>
        /// <param name="type">Tipo de mensaje</param>
        private void SetTitle(MessageType type)
        {
            string s = type.ToString();
            string ret = string.Empty;
            switch (s)
            {
                case "Confirmation":
                    ret = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Type_confirm);
                    break;
                case "Error":
                    ret = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Type_error);
                    break;
                case "Warning":
                    ret = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Type_warning);
                    break;
                default:
                    ret = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Type_message);
                    break;
            }

            this.lblTitle.Text = ret;
            this.Text = ret;
        }

        #endregion

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

        /// <summary>
        /// Evento cuando se presina una tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lblMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
                this.Close();
        }
        #endregion
    }
}
