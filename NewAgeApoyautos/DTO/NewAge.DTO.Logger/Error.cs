using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NewAge.DTO.Logger
{
    /// <summary>
    /// Class Error:
    /// Models all information that a log to log must have
    /// </summary>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Gets or sets the error id
        /// </summary>
        [DataMember]
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error category
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the machine name
        /// </summary>
        [DataMember]
        public string MachineName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the login name
        /// </summary>
        [DataMember]
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the method name
        /// </summary>
        [DataMember]
        public string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message text
        /// </summary>
        [DataMember]
        public string MessageText
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the error context
        /// </summary>
        [DataMember]
        public string Context
        {
            get;
            set;
        }
    }
}

