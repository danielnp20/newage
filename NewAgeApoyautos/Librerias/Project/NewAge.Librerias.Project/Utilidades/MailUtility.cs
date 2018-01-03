using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Reflection;

namespace NewAge.Librerias.Project
{
    public class MailUtility
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="smtpServer">Smtp server</param>
        /// <param name="user">User</param>
        /// <param name="password">User password</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        public static void SendMail(string smtpServer, int port, bool ssl, string user, string password, string from, string to, string subject, string body, string userToken, List<string> attachedFiles = null)
        {
            #region AMP - GoDaddy

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = smtpServer;
                smtp.Port = port;
                smtp.EnableSsl = ssl;
                smtp.Credentials = new NetworkCredential(user, password);
                smtp.Timeout = 30000;
                //Mensaje
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);

                //Enviar A:
                string[] mailsTo = to.Split(new char[] { ';' });
                for (int i = 0; i < mailsTo.Length; i++)
                    mail.To.Add(mailsTo[i]);

                //Adjunta archivos al mensaje
                if(attachedFiles != null && attachedFiles.Count > 0)
                    foreach (var pathFile in attachedFiles)
                        mail.Attachments.Add(new Attachment(pathFile));                

                //Vista del Contenido
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

                mail.AlternateViews.Add(htmlView);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                //if (string.IsNullOrWhiteSpace(userToken))
                smtp.Send(mail);
                //else
                //    smtp.SendAsync(mail, userToken);
            }
            catch (Exception ex)
            {
                throw;
            }
            #endregion
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="smtpServer">Smtp server</param>
        /// <param name="user">User</param>
        /// <param name="password">User password</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="linkedResources">
        /// Linked resources. Object multi-dimensional array with a stream resource, a content type and an object identifier for every resource:
        /// [i, 0]: Stream resource
        /// [i, 1]: Content type
        /// [i, 2]: Object identifier
        /// </param>
        public static void SendMail(string smtpServer, int port, bool ssl, string user, string password, string from, string to, string subject, string body, object[,] linkedResources, string userToken, List<string> attachedFiles = null)
        {
            var isEnabled = smtpServer.IndexOf("ssl://", StringComparison.InvariantCultureIgnoreCase) != -1 ? true : false;
            smtpServer = smtpServer.ToLower().Replace("ssl://", "");
            var host = smtpServer;
            //var port = 25;
            var index = smtpServer.IndexOf(":");
            if (index != -1)
            {
                host = smtpServer.Substring(0, index);
                port = Convert.ToInt32(smtpServer.Substring(index + 1));
            }
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Host = host;
            smtp.Port = port;
            smtp.EnableSsl = isEnabled;
            smtp.Credentials = new NetworkCredential(user, password);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);

            //Mail to
            string[] mailsTo = to.Split(new char[] { ';' });
            for (int i = 0; i < mailsTo.Length; i++)
            {
                mail.To.Add(mailsTo[i]);
            }

            //Body view
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            //Linked resources
            for (int i = 0; i < linkedResources.GetLength(0); i++)
            {
                //LinkedResource linkedResource = new LinkedResource((Stream)linkedResources[i, 0], linkedResources[i, 1].ToString());
                //linkedResource.ContentId = linkedResources[i, 2].ToString();
                //htmlView.LinkedResources.Add(linkedResource);

                Attachment a = new Attachment((Stream)linkedResources[i, 0], linkedResources[i, 2].ToString(), MediaTypeNames.Application.Octet);
                mail.Attachments.Add(a);
            }

            mail.AlternateViews.Add(htmlView);
            mail.Subject = subject;
            mail.IsBodyHtml = true;

            //smtp.SendCompleted += new SendCompletedEventHandler(MailDeliveryComplete);
            //smtp.SendAsync(mail, userToken);
            smtp.Send(mail);
        }

        /// <summary>
        /// sends a message with the outlook user account
        /// </summary>
        /// <param name="subject">Mail subject</param>
        /// <param name="body">Mail Body</param>
        /// <param name="recipients">Recipients</param>
        public static void SendOutlookMail(string subject, string body, string recipients, List<string> attachedFiles = null)
        {
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();

                // Get the NameSpace and Logon information.
                Outlook.NameSpace oNS = oApp.GetNamespace("mapi");

                // Log on by using a dialog box to choose the profile.
                oNS.Logon(Missing.Value, Missing.Value, true, true);

                // Alternate logon method that uses a specific profile.
                // TODO: If you use this logon method, 
                //  change the profile name to an appropriate value.
                //oNS.Logon("YourValidProfile", Missing.Value, false, true); 

                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                // Set the subject.
                oMsg.Subject = subject;

                // Set HTMLBody.
                String sHtml;
                sHtml = body;
                oMsg.HTMLBody = sHtml;

                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // TODO: Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(recipients);
                oRecip.Resolve();

                // Send.
                oMsg.Send();

                // Log off.
                oNS.Logoff();

                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oNS = null;
                oApp = null;
            }

            // Simple error handling.
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
