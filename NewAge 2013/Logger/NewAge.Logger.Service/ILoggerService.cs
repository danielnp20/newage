using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NewAge.Logger.Service
{
    /// <summary>
    /// Interface ILoggerService:
    /// Interface with a list of all operations that must be implemented to expose/manage the logger system
    /// </summary>
    [ServiceContract]
    public interface ILoggerService
    {
        #region Error

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="error">Error to log</param>
        [OperationContract]
        void LogError(string user, string exMessage, string errorLocation);

        #endregion

    }
}
