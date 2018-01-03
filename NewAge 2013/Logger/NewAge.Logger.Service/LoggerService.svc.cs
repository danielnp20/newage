using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NewAge.Logger.Model;

namespace NewAge.Logger.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoggerService" in code, svc and config file together.
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class LoggerService : ILoggerService
    {
        #region Models

        /// <summary>
        /// Logger model
        /// </summary>
        private LoggerModel _loggerModel;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoggerService()
        {

        }

        #region Error

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="error">Error to log</param>
        public void LogError(string user, string exMessage, string errorLocation)
        {
            string connStr = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString();
            this._loggerModel = new LoggerModel(connStr);
            this._loggerModel.WriteLogMessage(user, exMessage, errorLocation);
        }

        #endregion

    }
}
