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
    public class DTO_noAportesVoluntariosPensionTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noAportesVoluntariosPensionTotales()
        {
        }

        #region Propiades
        [DataMember]
        public List<DTO_noAportesVoluntariosPension> Detalles { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
        #endregion

    }
}
