using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Data.Logger.Model
{
    /// <summary>
    /// Class LoggerException:
    /// Class that defines a new exception for logging
    /// </summary>
    public class LoggerException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public LoggerException(string message)
            : base(message)
        {
        }
    }
}
