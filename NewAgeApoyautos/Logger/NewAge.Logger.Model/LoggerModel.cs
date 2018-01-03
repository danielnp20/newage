using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Logger;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using NewAge.Logger.DAL;

namespace NewAge.Logger.Model
{
    /// <summary>
    /// Class LoggerModel:
    /// Logs error and performance messages into the database
    /// </summary>
    public class LoggerModel
    {
        #region Variables

        /// <summary>
        /// Acceso a datos
        /// </summary>
        private DAL_Logger dal_Logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoggerModel(string connStr)
        {
            this.dal_Logger = DAL_Logger.GetInstance(connStr);
        }

        #endregion

        #region Funciones privadas

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

        #region Funciones públicas

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
        public void WriteLogMessage(string user, string exMessage, string errorLocation)
        {
            Error logError = new Error()
            {
                Date = DateTime.Now,
                MachineName = Environment.MachineName,
                LoginName = user,
                MethodName = errorLocation,
                MessageText = exMessage,
                Context = this.GetContextDetailed()
            };

            //DataBase Logger
            this.dal_Logger.LogErrors_Add(logError);
        }

        #endregion
    }
}
