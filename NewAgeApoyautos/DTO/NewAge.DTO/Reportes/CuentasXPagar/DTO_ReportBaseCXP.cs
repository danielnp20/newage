using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    public class DTO_ReportBaseCXP
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportBaseCXP() { }

        #region Propiedade de Reportes

        /// <summary>
        /// Carga la lista de las tarjetas de pago
        /// </summary>
        [DataMember]
        public List<DTO_ReportTarjetaPago> DetalleTarjetaPago { get; set; }

        [DataMember]
        public List<DTO_ReportLegalizacionTarjetas> DetalleLegalizaTarjeta { get; set; } 

        #endregion

    }
}
