using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Resultados
{
    /// <summary>
    /// DTO para el transporte de los resultados de las transacciones
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_TxResult : DTO_SerializedObject
    {
        /// <summary>
        /// Result
        /// </summary>
        [DataMember]
        public ResultValue Result
        {
            get;
            set;
        }

        /// <summary>
        /// Result Message
        /// </summary>
        [DataMember]
        public string ResultMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Detalles para el resultado
        /// </summary>
        [DataMember]
        public List<DTO_TxResultDetail> Details
        {
            get;
            set;
        }

        /// <summary>
        /// Campo auxiliar
        /// </summary>
        [DataMember]
        public string ExtraField
        {
            get;
            set;
        }

    }
}
