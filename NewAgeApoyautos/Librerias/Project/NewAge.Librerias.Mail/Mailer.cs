using System;
using System.Net;

namespace NewAge.Librerias.Mail
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Net.Mail;
    using System.Text;
    using Xipton.Razor;

    /// <summary>
    /// The mailer.
    /// It allow to send predefined email messages, using the preconfigured settings.
    /// </summary>
    public class Mailer
    {
        /// <summary>
        /// The message views.
        /// Holds the path to the view physical file.
        /// </summary>
        private static readonly Dictionary<MailMessageType, string> MessageViews;

        /// <summary>
        /// The view content.
        /// </summary>
        private static readonly Dictionary<MailMessageType, string> ViewContent;

        /// <summary>
        /// The mail subjects.
        /// </summary>
        private static readonly Dictionary<MailMessageType, string> MailSubjects;

        /// <summary>
        /// The base view path.
        /// </summary>
        private static readonly string BaseViewPath;

        /// <summary>
        /// The from address.
        /// </summary>
        private static readonly MailAddress FromAddress;

        /// <summary>
        /// Initializes static members of the <see cref="Mailer"/> class.
        /// </summary>
        static Mailer()
        {
            // Set up mail templates files.
            MessageViews = new Dictionary<MailMessageType, string>
            {
                { MailMessageType.NewClientPassword, "Online.Client.NewPassword.Mail.cshtml" },
                { MailMessageType.ResetClientPassword, "Online.Client.ResetPassword.Mail.cshtml" },
                { MailMessageType.UpdateClientPassword, "Online.Client.UpdatePassword.Mail.cshtml" },
            };

            // Set up mail subjects.
            MailSubjects = new Dictionary<MailMessageType, string>
            {
                { MailMessageType.NewClientPassword, "Apoyos Financieros - Bienvenido al sitio web de consulta de cartera" },
                { MailMessageType.ResetClientPassword, "Apoyos Financieros - Reinicio de contraseña" },
                { MailMessageType.UpdateClientPassword, "Apoyos Financieros - Actualización de contraseña" },
            };

            // Initilice the message content holder.
            ViewContent = new Dictionary<MailMessageType, string>();

            // Basic mail data.
            BaseViewPath = ConfigurationManager.AppSettings["Mail.Template.Directory"];
            FromAddress = new MailAddress(ConfigurationManager.AppSettings["Mail.From.Address"], ConfigurationManager.AppSettings["Mail.From.Name"], Encoding.UTF8);
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendMessage(MailMessageType type, object model, List<MailAddress> to)
        {
            this.LoadContent(type);
            var rm = new RazorMachine();
            var template = rm.ExecuteContent(ViewContent[type], model);
            
            var mailBody = template.Result;
            var message = new MailMessage() { Body = mailBody, IsBodyHtml = true, Subject = MailSubjects[type], From = FromAddress };
            to.ForEach(m => message.To.Add(m));
            var client = new SmtpClient()
            {
                Host = ConfigurationManager.AppSettings["Mail.Smtp.Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Mail.Smtp.Port"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail.Smtp.EnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Mail.Smtp.UserName"], ConfigurationManager.AppSettings["Mail.Smtp.Password"])
            };
            
            client.Send(message);
            return true;
        }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        private void LoadContent(MailMessageType type)
        {
            if (ViewContent.ContainsKey(type))
            {
                return;
            }

            var path = Path.Combine(BaseViewPath, MessageViews[type]);
            var content = File.ReadAllText(path);
            ViewContent.Add(type, content);
        }
    }
}