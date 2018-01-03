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
    public class DTO_TxResultDetail
    {
        /// <summary>
        /// Llave
        /// </summary>
        [DataMember]
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Linea dada
        /// </summary>
        [DataMember]
        public int line
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
        /// Detalles por fields
        /// </summary>
        [DataMember]
        public List<DTO_TxResultDetailFields> DetailsFields = new List<DTO_TxResultDetailFields>();
        
    }
}
