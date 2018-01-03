using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_ResumenImpustos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ResumenImpustos
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public string Descriptivo { get; set; }
        
        [DataMember]
        public string CuentaID { get; set; }

        [DataMember]
        public decimal BaseML { get; set; }

        [DataMember]
        public decimal BaseME { get; set; }

        [DataMember]
        public decimal Tarifa { get; set; }

        [DataMember]
        public decimal ValorML { get; set; }

        [DataMember]
        public decimal ValorME { get; set; }
    }
}
