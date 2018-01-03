using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NewAge.Logger.Service;
using NewAge.Logger.Model;

namespace NewAge.Logger.Facade
{
    public static class LoggerFacade
    {
        #region Variables 

        /// <summary>
        /// The service client.
        /// </summary>
        private static ChannelFactory<ILoggerService> _loggerChannelFactory;

        /// <summary>
        /// Servicio del logger
        /// </summary>
        private static ILoggerService _loggerService;

        /// <summary>
        /// Logger model
        /// </summary>
        private static LoggerModel _loggerModel;

        #endregion

        private static string GetErrorMessage(Exception ex, string user)
        {
            string tab = "\t";

            string message =
            "(" + DateTime.Now.ToString("yyyy/MM/dd' - 'HH':'mm':'ss K") + ")" + Environment.NewLine +
            "User:" + tab + user + Environment.NewLine +
            "Stack Trace:" + tab + ex.ToString();

            return message;
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="error">Error to log</param>
        public static void LogError(string strConn, string user, Exception ex, string errorLocation)
        {
            try
            {
                string exMessage = GetErrorMessage(ex, user);
                _loggerModel = new LoggerModel(strConn);
                _loggerModel.WriteLogMessage(user, exMessage, errorLocation);
            }
            catch(Exception ex1)
            {
                throw ex1;
            }
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="error">Error to log</param>
        public static void LogServiceError(string user, Exception ex, string errorLocation)
        {
            try
            {
                string exMessage = GetErrorMessage(ex, user);

                _loggerChannelFactory = new ChannelFactory<ILoggerService>("WSHttpBinding_ILoggerService");
                _loggerService = new LoggerService();
                _loggerService = _loggerChannelFactory.CreateChannel();
                _loggerService.LogError(user, exMessage, errorLocation);
                _loggerChannelFactory.Close();
            }
            catch (Exception ex1)
            {
                _loggerChannelFactory.Abort();
            }
        }

    }
}
