using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.ExceptionHandler
{
    public class MentorDataException : Exception
    {
        #region Propiedades

        /// <summary>
        /// Data
        /// </summary>
        public string Detail
        {
            get;
            set;
        }

        /// <summary>
        /// Error Code
        /// </summary>
        public ExceptionHandler.Enums.MentorDataExceptionType ErrorCode
        {
            get;
            set;
        }

        /// <summary>
        /// Origen
        /// </summary>
        public string Origen
        {
            get;
            set;
        }

        #endregion

        #region Constructoras

        public MentorDataException(string detail, ExceptionHandler.Enums.MentorDataExceptionType? errorCode, string origen)
        {
            this.Detail = detail;
            if (errorCode != null)
            {
                this.ErrorCode = (ExceptionHandler.Enums.MentorDataExceptionType)errorCode;
            }
            this.Origen = origen;
        }
        
        public MentorDataException(string message, string detail, ExceptionHandler.Enums.MentorDataExceptionType? errorCode, string origen)
            : base(message)
        {
            this.Detail = detail;
            if (errorCode != null)
            {
                this.ErrorCode = (ExceptionHandler.Enums.MentorDataExceptionType)errorCode;
            }
            this.Origen = origen;
        }

        public MentorDataException(string message, Exception inner, string detail, ExceptionHandler.Enums.MentorDataExceptionType? errorCode, string origen)
            : base(message, inner)
        {
            this.Detail = detail;
            if (errorCode != null)
            {
                this.ErrorCode = (ExceptionHandler.Enums.MentorDataExceptionType)errorCode;
            }
            this.Origen = origen;
        }

        #endregion
    }
}
