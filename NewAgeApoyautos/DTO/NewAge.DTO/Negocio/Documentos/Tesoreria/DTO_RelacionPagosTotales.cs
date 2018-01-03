using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{

    [DataContract]
    [Serializable]

    // <summary>
    // Clase del reporte detallado del Formulario
    // </summary>
    public class DTO_RelacionPagosTotales
    {
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_RelacionPagosTotales()
        {           
        }

        #region Propiades
        [DataMember]
        public List<DTO_RelacionPagosDeta2> Detalles { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
        #endregion
    }
}
