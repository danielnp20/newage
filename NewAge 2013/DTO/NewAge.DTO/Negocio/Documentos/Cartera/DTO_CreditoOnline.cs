using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class 
    /// Models DTO_CreditoOnline
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CreditoOnline
    {
        public DTO_CreditoOnline()
        {
        }

        #region Propiedades

        [DataMember]
        public int Libranza
        {
            get;
            set;
        }

        [DataMember]
        public int Cuota
        {
            get;
            set;
        }

        [DataMember]
        public DateTime FechaPago
        {
            get;
            set;
        }

        [DataMember]
        public long Valor
        {
            get;
            set;
        }

        [DataMember]
        public string Tipo
        {
            get;
            set;
        }


        #endregion
    }
}
