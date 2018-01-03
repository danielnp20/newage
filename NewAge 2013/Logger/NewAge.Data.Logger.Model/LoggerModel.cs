using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Logger;
using System.Configuration;
using NewAge.Logger.Data.Entities;
using NewAge.Logger.EntityService;
using System.Net.Mail;
using System.Net;

namespace NewAge.Data.Logger.Model
{
    /// <summary>
    /// Class LoggerModel:
    /// Logs error and performance messages into the database
    /// </summary>
    public class LoggerModel
    {
        #region Private Fields

        /// <summary>
        /// The application identifier
        /// </summary>
        private static byte ApplicationId = Convert.ToByte(ConfigurationManager.AppSettings["Application.Id"]);

        /// <summary>
        /// Variable that indicates if exceptions must be logged and emailed
        /// </summary>
        private static bool DoLogAndEmailExceptions = Convert.ToBoolean(ConfigurationManager.AppSettings["NewAge.Exceptions.LogAndEmailExceptions"]);

        /// <summary>
        /// Variable that indicates if log performance must be done
        /// </summary>
        private static bool DoLogPerformance = Convert.ToBoolean(ConfigurationManager.AppSettings["NewAge.Performance.LogPerformance"]);

        /// <summary>
        /// SMTP server address to send log emails
        /// </summary>
        private static string SmtpServer = ConfigurationManager.AppSettings["Mail.Smtp"];

        /// <summary>
        /// SMTP user name to send log emails
        /// </summary>
        private static string SmtpUser = ConfigurationManager.AppSettings["Mail.Sender"];

        /// <summary>
        /// SMTP user password to send log emails
        /// </summary>
        private static string SmtpUserPass = ConfigurationManager.AppSettings["Mail.PasswordSender"];

        /// <summary>
        /// From email address when an exception happens
        /// </summary>
        private static string ExceptionsFromEmailAddress = ConfigurationManager.AppSettings["Mail.Sender"];

        /// <summary>
        /// To email address when an exception happens
        /// </summary>
        private static string ExceptionsToEmailAddress = ConfigurationManager.AppSettings["Mail.Recipient"];

        /// <summary>
        /// From email address when a log performance happens
        /// </summary>
        private static string PerformanceFromEmailAddress = ConfigurationManager.AppSettings["Mail.Sender"];

        /// <summary>
        /// To email address when a log performance happens
        /// </summary>
        private static string PerformanceToEmailAddress = ConfigurationManager.AppSettings["Mail.Recipient"];

        /// <summary>
        /// Current machine name
        /// </summary>
        private string _machineName;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoggerModel()
        {
            this._machineName = Environment.MachineName;
        }

        #endregion

        #region Error methods

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, Exception ex, bool sendMail)
        {
            this.Error(loginName, LoggerModel.GetErrorLocation(), ErrorCategory.Error, ex, null, sendMail);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="message">Message to log</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, string message, bool sendMail)
        {
            this.Error(loginName, LoggerModel.GetErrorLocation(), ErrorCategory.Error, new LoggerException(message), null, sendMail);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="message">Message to log</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, string methodName, string message, bool sendMail)
        {
            this.Error(loginName, methodName, ErrorCategory.Error, new LoggerException(message), null, sendMail);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, string methodName, Exception ex, string context, bool sendMail)
        {
            this.Error(loginName, methodName, ErrorCategory.Error, ex, context, sendMail);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, Exception ex, string context, bool sendMail)
        {
            this.Error(loginName, GetErrorLocation(), ErrorCategory.Error, ex, context, sendMail);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, string methodName, Exception ex, bool sendMail)
        {
            this.Error(loginName, methodName, ErrorCategory.Error, ex, null, sendMail);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Errror category</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(string loginName, string methodName, ErrorCategory category, Exception ex, string context, bool sendMail)
        {
            if (LoggerModel.DoLogAndEmailExceptions)
            {
                string contextSmall = context + this.GetContextBasic();
                string contextDetailed = contextSmall + this.GetContextDetailed();
                this.WriteErrorToDatabase(loginName, methodName, category, ex, contextDetailed);
                if (sendMail)
                {
                    //Check for common errors
                    var commonErrors = ConfigurationManager.AppSettings["NewAge.Exceptions.CommonErrors"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    //Error
                    string error = ex.ToString();

                    //Indicator to know if current error is a common error
                    bool isCommonError = false;
                    foreach (var commonError in commonErrors)
                    {
                        if (error.IndexOf(commonError, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            isCommonError = true;
                            break;
                        }
                    }

                    if (!isCommonError)
                    {
                        this.EmailError(loginName, methodName, category, ex, contextDetailed);
                    }
                }
            }
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="application">Application identifier</param>
        /// <param name="loginName">Login name</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Errror category</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void Error(byte application, string loginName, string machineName, string methodName, ErrorCategory category, Exception ex, string context, bool sendMail)
        {
            if (LoggerModel.DoLogAndEmailExceptions)
            {
                string contextSmall = context + this.GetContextBasic();
                string contextDetailed = contextSmall + this.GetContextDetailed();
                this.WriteErrorToDatabase(application, loginName, machineName, methodName, category, ex, contextDetailed);
                if (sendMail)
                {
                    //Check for common errors
                    var commonErrors = ConfigurationManager.AppSettings["NewAge.Exceptions.CommonErrors"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    //Error
                    string error = ex.ToString();

                    //Indicator to know if current error is a common error
                    bool isCommonError = false;
                    foreach (var commonError in commonErrors)
                    {
                        if (error.IndexOf(commonError, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            isCommonError = true;
                            break;
                        }
                    }

                    if (!isCommonError)
                    {
                        this.EmailError(loginName, machineName, methodName, category, ex, contextDetailed);
                    }
                }
            }
        }

        /// <summary>
        /// Logs an error and send a friendly message by email
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        public void FriendlyError(Exception ex, string subject, string body)
        {
            if (LoggerModel.DoLogAndEmailExceptions)
            {
                this.WriteErrorToDatabase("", "", ErrorCategory.Error, ex, "");
                this.EmailFriendlyError(subject, body);
            }
        }

        /// <summary>
        /// Logs an error without context
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="message">Mesage to log</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public void ErrorNoContext(string methodName, string message, bool sendMail)
        {
            if (LoggerModel.DoLogAndEmailExceptions)
            {
                this.WriteErrorToDatabase("", methodName, ErrorCategory.Error, new LoggerException(message), null);
                if (sendMail)
                {
                    //Check for common errors
                    var commonErrors = ConfigurationManager.AppSettings["NewAge.Exceptions.CommonErrors"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    //Error
                    string error = message;

                    //Indicator to know if current error is a common error
                    bool isCommonError = false;
                    foreach (var commonError in commonErrors)
                    {
                        if (error.IndexOf(commonError, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            isCommonError = true;
                            break;
                        }
                    }

                    if (!isCommonError)
                    {
                        this.EmailErrorNoContext(methodName, ErrorCategory.Error, new LoggerException(message));
                    }
                }
            }
        }

        #endregion

        #region Performance methods

        /// <summary>
        /// Logs performance
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="miliseconds">Number of milliseconds</param>
        /// <param name="doDetailLogging">Slow and large logging of Session, QueryString, Form, Cache, etc.</param>
        public void Performance(string loginName, string methodName, int milliseconds, bool doDetailLogging)
        {
            this.Performance(loginName, methodName, "", milliseconds, doDetailLogging, false);
        }

        /// <summary>
        /// Logs performance
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="context">Context</param>
        /// <param name="milliseconds">Number of milliseconds</param>
        /// <param name="doDetailLogging">If true detailed logging must done</param>
        /// <param name="doEmail">If true, an email is sent</param>
        public void Performance(string loginName, string methodName, string context, int milliseconds, bool doDetailLogging, bool doEmail)
        {
            if (LoggerModel.DoLogPerformance)
            {
                if (doDetailLogging)
                {
                    context += this.GetContextBasic() + this.GetContextDetailed();
                }
                this.WritePerformanceToDatabase(loginName, methodName, context, milliseconds);

                if (doEmail)
                {
                    this.EmailPerformance(loginName, methodName, milliseconds, context);
                }
            }
        }

        /// <summary>
        /// Logs performance
        /// </summary>
        /// <param name="application">Application identifier</param>
        /// <param name="loginName">Login name</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="context">Context</param>
        /// <param name="milliseconds">Number of milliseconds</param>
        /// <param name="doDetailLogging">If true detailed logging must done</param>
        /// <param name="doEmail">If true, an email is sent</param>
        public void Performance(byte application, string loginName, string machineName, string methodName, string context, int milliseconds, bool doDetailLogging, bool doEmail)
        {
            if (LoggerModel.DoLogPerformance)
            {
                if (doDetailLogging)
                {
                    context += this.GetContextBasic() + this.GetContextDetailed();
                }
                this.WritePerformanceToDatabase(application, loginName, machineName, methodName, context, milliseconds);

                if (doEmail)
                {
                    this.EmailPerformance(loginName, machineName, methodName, milliseconds, context);
                }
            }
        }

        #endregion

        #region Saving data to database

        /// <summary>
        /// Writes a log error into the database
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Error category</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        private void WriteErrorToDatabase(string loginName, string methodName, ErrorCategory category, Exception ex, string context)
        {
            try
            {
                LogError logError = new LogError();
                logError.Application = LoggerModel.ApplicationId;
                logError.Category = (byte)category;
                logError.Context = context;
                logError.LoginName = loginName;
                logError.MachineName = this._machineName;
                logError.MessageText = ex.ToString();
                logError.MethodName = methodName;
                logError.Date = DateTime.Now;
                //AdministrationBLL.LogErrorInsert(logError);
                new LogErrorService().Create(logError);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.WriteLine(exc.ToString(), "Logger");
            }
        }

        /// <summary>
        /// Writes a log error into the database
        /// </summary>
        /// <param name="application">Application identifier</param>
        /// <param name="loginName">Login name</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Error category</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        private void WriteErrorToDatabase(byte application, string loginName, string machineName, string methodName, ErrorCategory category, Exception ex, string context)
        {
            try
            {
                LogError logError = new LogError();
                logError.Application = application;
                logError.Category = (byte)category;
                logError.Context = context;
                logError.LoginName = loginName;
                logError.MachineName = machineName;
                logError.MessageText = ex.ToString();
                logError.MethodName = methodName;
                logError.Date = DateTime.Now;
                new LogErrorService().Create(logError);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.WriteLine(exc.ToString(), "Logger");
            }
        }

        /// <summary>
        /// Writes the log performance into the database
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="context">Context</param>
        /// <param name="milliseconds">Number of milliseconds</param>
        private void WritePerformanceToDatabase(string loginName, string methodName, string context, int milliseconds)
        {
            if (!LoggerModel.DoLogPerformance)
            {
                return;
            }

            try
            {
                LogPerformance logPerformance = new LogPerformance();
                logPerformance.Application = LoggerModel.ApplicationId;
                logPerformance.LoginName = loginName;
                logPerformance.MachineName = this._machineName;
                logPerformance.MethodName = methodName;
                logPerformance.RunningTime = milliseconds;
                logPerformance.Date = DateTime.Now;
                logPerformance.Context = context;
                //AdministrationBLL.LogPerformanceInsert(logPerformance);
                new LogPerformanceService().Create(logPerformance);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString(), "Logger");
            }
        }

        /// <summary>
        /// Writes the log performance into the database
        /// </summary>
        /// <param name="application">Application identifier</param>
        /// <param name="loginName">Login name</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="context">Context</param>
        /// <param name="milliseconds">Number of milliseconds</param>
        private void WritePerformanceToDatabase(byte application, string loginName, string machineName, string methodName, string context, int milliseconds)
        {
            if (!LoggerModel.DoLogPerformance)
            {
                return;
            }

            try
            {
                LogPerformance logPerformance = new LogPerformance();
                logPerformance.Application = application;
                logPerformance.LoginName = loginName;
                logPerformance.MachineName = machineName;
                logPerformance.MethodName = methodName;
                logPerformance.RunningTime = milliseconds;
                logPerformance.Date = DateTime.Now;
                logPerformance.Context = context;
                //AdministrationBLL.LogPerformanceInsert(logPerformance);
                new LogPerformanceService().Create(logPerformance);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString(), "Logger");
            }
        }

        #endregion

        #region Email Error and Performance

        /// <summary>
        /// Emails the log error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Error category</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        private void EmailError(string loginName, string methodName, ErrorCategory category, Exception ex, string context)
        {
            if (LoggerModel.ExceptionsToEmailAddress == null || LoggerModel.ExceptionsToEmailAddress.Trim().Length == 0 ||
                LoggerModel.ExceptionsFromEmailAddress == null || LoggerModel.ExceptionsFromEmailAddress.Trim().Length == 0)
            {
                return;
            }

            try
            {
                string newContext = "";
                if (context != null)
                {
                    newContext = "Context: " + context;
                }
                this.SendMail(SmtpServer, SmtpUser, SmtpUserPass, ExceptionsFromEmailAddress, ExceptionsToEmailAddress, String.Format("Error - {0} - {1}", methodName, loginName), String.Format("User name: {0}{1}Method name: {2}{1}Machine: {3}{1}Message: {4}{1}{5}", loginName, Environment.NewLine, methodName, _machineName, ex.ToString(), newContext));
            }
            catch
            {
                //Do nothing if failed to mail exception
            }
        }

        /// <summary>
        /// Emails the log error
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Error category</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="context">Context</param>
        private void EmailError(string loginName, string machineName, string methodName, ErrorCategory category, Exception ex, string context)
        {
            if (LoggerModel.ExceptionsToEmailAddress == null || LoggerModel.ExceptionsToEmailAddress.Trim().Length == 0 ||
                LoggerModel.ExceptionsFromEmailAddress == null || LoggerModel.ExceptionsFromEmailAddress.Trim().Length == 0)
            {
                return;
            }

            try
            {
                string newContext = "";
                if (context != null)
                {
                    newContext = "Context: " + context;
                }
                this.SendMail(SmtpServer, SmtpUser, SmtpUserPass, ExceptionsFromEmailAddress, ExceptionsToEmailAddress, String.Format("Error - {0} - {1}", methodName, loginName), String.Format("User name: {0}{1}Method name: {2}{1}Machine: {3}{1}Message: {4}{1}{5}", loginName, Environment.NewLine, methodName, machineName, ex.ToString(), newContext));
            }
            catch
            {
                //Do nothing if failed to mail exception
            }
        }

        /// <summary>
        /// Emails a friendly error
        /// </summary>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        private void EmailFriendlyError(string subject, string body)
        {
            if (LoggerModel.ExceptionsToEmailAddress == null || LoggerModel.ExceptionsToEmailAddress.Trim().Length == 0 ||
                LoggerModel.ExceptionsFromEmailAddress == null || LoggerModel.ExceptionsFromEmailAddress.Trim().Length == 0)
            {
                return;
            }

            try
            {
                this.SendMail(SmtpServer, SmtpUser, SmtpUserPass, ExceptionsFromEmailAddress, ExceptionsToEmailAddress, subject, body);
            }
            catch
            {
                //Do nothing if failed to mail exception
            }
        }

        /// <summary>
        /// Emails the log error without context
        /// </summary>
        /// <param name="methodName">Method name</param>
        /// <param name="category">Error category</param>
        /// <param name="ex">Exception to log</param>
        private void EmailErrorNoContext(string methodName, ErrorCategory category, Exception ex)
        {
            if (LoggerModel.ExceptionsToEmailAddress == null || LoggerModel.ExceptionsToEmailAddress.Trim().Length == 0 ||
                LoggerModel.ExceptionsFromEmailAddress == null || LoggerModel.ExceptionsFromEmailAddress.Trim().Length == 0)
            {
                return;
            }

            try
            {
                this.SendMail(SmtpServer, SmtpUser, SmtpUserPass, ExceptionsFromEmailAddress, ExceptionsToEmailAddress, String.Format("Error - {0}", methodName), String.Format("Method name: {1}{0}Machine: {2}{0}Message: {3}", Environment.NewLine, methodName, _machineName, ex.ToString()));
            }
            catch
            {
                //Do nothing if failed to mail exception
            }
        }

        /// <summary>
        /// Emails the log performance
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="milliseconds">Number of milliseconds</param>
        /// <param name="context">Context</param>
        private void EmailPerformance(string loginName, string methodName, int milliseconds, string context)
        {
            if (LoggerModel.PerformanceToEmailAddress == null || LoggerModel.PerformanceToEmailAddress.Trim().Length == 0 ||
                LoggerModel.PerformanceFromEmailAddress == null || LoggerModel.PerformanceFromEmailAddress.Trim().Length == 0)
            {
                return;
            }

            try
            {
                this.SendMail (SmtpServer, SmtpUser, SmtpUserPass, PerformanceFromEmailAddress, PerformanceToEmailAddress, String.Format("Performance - {0} - {1}",
                    methodName, loginName), String.Format("User name: {0}{1}Method name: {2}{1}Milliseconds: {4}{1}Machine: {3}{1}Context: {5}", loginName, Environment.NewLine,
                    methodName, _machineName, milliseconds, context == null ? "" : context));
            }
            catch
            {
                //Do nothing if failed to mail exception
            }
        }

        /// <summary>
        /// Emails the log performance
        /// </summary>
        /// <param name="loginName">Login name</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="milliseconds">Number of milliseconds</param>
        /// <param name="context">Context</param>
        private void EmailPerformance(string loginName, string machineName, string methodName, int milliseconds, string context)
        {
            if (LoggerModel.PerformanceToEmailAddress == null || LoggerModel.PerformanceToEmailAddress.Trim().Length == 0 ||
                LoggerModel.PerformanceFromEmailAddress == null || LoggerModel.PerformanceFromEmailAddress.Trim().Length == 0)
            {
                return;
            }

            try
            {
                this.SendMail(SmtpServer, SmtpUser, SmtpUserPass, PerformanceFromEmailAddress, PerformanceToEmailAddress, String.Format("Performance - {0} - {1}",
                    methodName, loginName), String.Format("User name: {0}{1}Method name: {2}{1}Milliseconds: {4}{1}Machine: {3}{1}Context: {5}", loginName, Environment.NewLine,
                    methodName, machineName, milliseconds, context == null ? "" : context));
            }
            catch
            {
                //Do nothing if failed to mail exception
            }
        }

        #endregion

        #region Send mail

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
        private void SendMail(string smtpServer, string user, string password, string from, string to, string subject, string body)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = smtpServer;
            smtp.Credentials = new NetworkCredential(user, password);

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(from);

            //Mail to
            string[] mailsTo = to.Split(new char[] { ';' });
            for (int i = 0; i < mailsTo.Length; i++)
            {
                mail.To.Add(mailsTo[i]);
            }

            mail.Body = body;
            mail.Subject = subject;
            mail.IsBodyHtml = false;

            smtp.Send(mail);

        }

        #endregion

        #region Context information methods

        /// <summary>
        /// Gets the basic context to log
        /// </summary>
        /// <returns>Returns the basic context to log</returns>
        private string GetContextBasic()
        {
            //if called from app other than web, return empty
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }

            string remoteComputerName = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            string remoteIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return "<UserComputer>" + remoteComputerName + "</UserComputer>" + "<UserIP>" + remoteIP + "</UserIP>";
        }

        /// <summary>
        /// Gets the detailed context to log
        /// </summary>
        /// <returns>Returns the detailed context to log</returns>
        private string GetContextDetailed()
        {
            //If called from app other than web, return empty
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }

            string s = "";
            s += "<QueryString>" + System.Web.HttpContext.Current.Request.QueryString.ToString() + "</QueryString>";
            s += this.GetSessionValues();
            s += this.GetFormValues();
            s += this.GetCookieValues();
            s += this.GetCacheValues();

            if (s.Length > 256000)
            {
                //Get partial values so the exception mechanism stands attacks
                s = s.Substring(0, 255996) + "...";
            }

            return s;
        }

        /// <summary>
        /// Gets session values
        /// </summary>
        /// <returns>Returns session values</returns>
        private string GetSessionValues()
        {
            //If called from app other than web, return empty
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Session>");
                if (System.Web.HttpContext.Current.Session != null)
                {
                    string sv = "";
                    foreach (string sk in System.Web.HttpContext.Current.Session.Keys)
                    {
                        try
                        {
                            string listCount = "";
                            System.Reflection.PropertyInfo pi = System.Web.HttpContext.Current.Session[sk].GetType().GetProperty("Count");
                            if (pi != null)
                            {
                                listCount += "[" + pi.GetValue(System.Web.HttpContext.Current.Session[sk], null).ToString() + "]";
                            }
                            sv = System.Web.HttpContext.Current.Session[sk].ToString();
                            sv = (sv.Length < 41) ? sv : sv.Substring(0, 36) + "..."; //get partial values so log columns do not overflow
                            sv += listCount;
                            sb.Append(sk).Append(":").Append(sv).Append(";");
                        }
                        catch (Exception ex)
                        {
                            //If there's an exception logging the exception... bad luck, simply return the error.
                            sb.Append("Error building SessionValues: SessionKey=" + sk + " " + ex.Message);
                        }
                    }
                }
                sb.Append("</Session>");
            }
            catch (Exception ex)
            {
                //If there's an exception logging the exception... bad luck, simply return the error.
                sb.Append("Error building SessionValues: " + ex.Message);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets form values
        /// </summary>
        /// <returns>Returns form values</returns>
        private string GetFormValues()
        {
            //If called from app other than web, return empty
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Form>");
                foreach (string s in System.Web.HttpContext.Current.Request.Form.AllKeys)
                {
                    string v = System.Web.HttpContext.Current.Request.Form[s];
                    if (v.Length > 64000)
                    {
                        //Get partial values so log columns do not overflow
                        v = v.Substring(0, 63996) + "...";
                    }
                    sb.Append(string.Format("Key={0}, Value={1}; ", s, v));
                }
                sb.Append("</Form>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                //If there's an exception logging the exception... bad luck, simply return the error.
                return "Error getting forms in Logger " + ex.Message;
            }
        }

        /// <summary>
        /// Gets cache values
        /// </summary>
        /// <returns>Returns cache values</returns>
        private string GetCacheValues()
        {
            //If called from app other than web, return empty
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<Cache>");

                System.Collections.IDictionaryEnumerator cacheEnum = System.Web.HttpContext.Current.Cache.GetEnumerator();
                while (cacheEnum.MoveNext())
                {
                    string k = cacheEnum.Key.ToString();
                    string v = cacheEnum.Value.ToString();
                    if (v.Length > 64000)
                    {
                        //Get partial values so log columns do not overflow
                        v = v.Substring(0, 63996) + "...";
                    }
                    sb.Append(string.Format("Key={0}, Value={1}; ", k, v));
                }
                sb.Append("</Cache>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                //If there's an exception logging the exception... bad luck, simply return the error.
                return ex.Message;
            }
        }

        /// <summary>
        /// Gets form values
        /// </summary>
        /// <returns>Returns form values</returns>
        private string GetCookieValues()
        {
            //If called from app other than web, return empty
            if (System.Web.HttpContext.Current == null)
            {
                return "";
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Cookies>");
                foreach (string s in System.Web.HttpContext.Current.Request.Cookies.AllKeys)
                {
                    sb.Append(string.Format("CookieKey={0}, Value={1}, Expires={2}; ", System.Web.HttpContext.Current.Request.Cookies[s].Name, System.Web.HttpContext.Current.Request.Cookies[s].Value, System.Web.HttpContext.Current.Request.Cookies[s].Expires.ToString()));
                }
                sb.Append("</Cookies>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                //If there's an exception logging the exception... bad luck, simply return the error.
                return "Error getting Cookies in Logger " + ex.Message;
            }

        }

        #endregion

        #region Error location

        /// <summary>
        /// Gets the location where the error was produced
        /// </summary>
        /// <returns>Returns the location where the error was produced</returns>
        private static string GetErrorLocation()
        {
            System.Diagnostics.StackFrame sf = null;
            int frameCount = ((System.Diagnostics.StackTrace)new System.Diagnostics.StackTrace()).FrameCount;
            if (frameCount > 3)
            {
                //Usually an error is caught and then logged by calling one of the Error methods of this class, 
                //which in turn calls this GetErrorLocation() method. In that case, the error occurred 3 frames up on the stack.
                sf = new System.Diagnostics.StackFrame(3);
            }
            else
            {
                //Method called from this class.
                sf = new System.Diagnostics.StackFrame(1);
            }

            return sf.GetMethod().ReflectedType.Name + "." + sf.GetMethod().Name;
        }

        #endregion

        #region Static methods to write error easier

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="application">Application identifier</param>
        /// <param name="machineName">Machine name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="user">User</param>
        /// <param name="ex">Exception</param>
        /// <param name="context">Context</param>
        /// <param name="errorLocation">Error location</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public static void WriteLogMessage(byte application, string machineName, string methodName, string user, Exception ex, string context, string errorLocation, bool sendMail)
        {
            string tab = "\t";

            //Message
            string message =
                "(" + DateTime.Now.ToString("yyyy/MM/dd' - 'HH':'mm':'ss K") + ")" + Environment.NewLine +
                //"Error in:" + tab + errorLocation + Environment.NewLine +//
                //"Machine Name:" + tab + Environment.MachineName + Environment.NewLine +
                //"Method Name:" + tab + methodName + Environment.NewLine +
                "User:" + tab + user + Environment.NewLine +
                //"Error Message:" + tab + ex.Message.ToString() + Environment.NewLine //+
                "Stack Trace:" + tab + ex.ToString();

            //DataBase Logger
            var logger = new LoggerModel();
            logger.Error(application, user, machineName, methodName, ErrorCategory.Error, new Exception(message), context, sendMail);
        }

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="ex">Exception</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public static void WriteLogMessage(string user, Exception ex, bool sendMail)
        {
            string methodName = LoggerModel.GetErrorLocation();
            LoggerModel.WriteLogMessage(LoggerModel.ApplicationId, Environment.MachineName, methodName, user, ex, "", methodName, sendMail);
        }

        /// <summary>
        /// Writes a message to the log
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="sendMail">True to send a email, false to not send the email</param>
        public static void WriteLogMessage(Exception ex, bool sendMail)
        {
            string methodName = LoggerModel.GetErrorLocation();
            LoggerModel.WriteLogMessage(LoggerModel.ApplicationId, Environment.MachineName, methodName, String.Empty, ex, "", methodName, sendMail);
        }

        #endregion
    }
}
