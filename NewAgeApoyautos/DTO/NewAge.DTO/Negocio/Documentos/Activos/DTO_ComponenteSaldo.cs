using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Clase Componentes Saldos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ComponenteSaldo
    {
        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDT_ComponenteActivoID ComponenteActivoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_ConceptoSaldoID ConceptoSaldoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoComponente { get; set; }

        [DataMember]
        public UDT_Valor VlrFuncLoc { get; set; }

        [DataMember]
        public UDT_Valor VlrFunExt { get; set; }

        [DataMember]
        public UDT_Valor VlrIFRSLoc { get; set; }

        [DataMember]
        public UDT_Valor VlrIFRSExt { get; set; }
    }
}
