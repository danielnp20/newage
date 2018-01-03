using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_tsChequesGiradosTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsChequesGiradosTotales()
        {
        }

        [DataMember]
        public List<DTO_ChequesGiradosDetaReport> Detalles { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoDesc { get; set; }

        [DataMember]
        public UDTSQL_int NumCheque { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
    }
}
