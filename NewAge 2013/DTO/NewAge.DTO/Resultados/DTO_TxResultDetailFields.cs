using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NewAge.DTO.Resultados
{
    /// <summary>
    /// DTO para el transporte del detalle de una tx
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_TxResultDetailFields
    {
        /// <summary>
        /// Field 
        /// </summary>
        [DataMember]
        public string Field
        {
            get;
            set;
        }

        /// <summary>
        /// Mensaje 
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// OldValue
        /// </summary>
        [DataMember]
        public string OldValue
        {
            get;
            set;
        }

        /// <summary>
        /// NewValue
        /// </summary>
        [DataMember]
        public string NewValue
        {
            get;
            set;
        }
    }
}
