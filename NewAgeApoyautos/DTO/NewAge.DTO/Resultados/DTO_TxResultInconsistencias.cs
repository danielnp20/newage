using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NewAge.DTO.Resultados
{
    /// <summary>
    /// DTO Resultado Inconsistencias
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_TxResultInconsistencias
    {
        [DataMember]
        public List<DTO_TxResultDetailFields> Fields{ get; set; }

        [DataMember]
        public string Mensaje{ get; set; }

        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public string ResultMessage { get; set; }

        [DataMember]
        public int Linea { get; set; }

        [DataMember]
        public string posicion { get; set; }

    }
}
