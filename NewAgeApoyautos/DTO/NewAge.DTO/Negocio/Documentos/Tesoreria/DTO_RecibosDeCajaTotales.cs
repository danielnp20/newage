using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    // <summary>
    // Clase del reporte detallado del Formulario
    // </summary>
    public class DTO_RecibosDeCajaTotales
    {
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_RecibosDeCajaTotales()
        {           
        }

        #region Propiades
         [DataMember]
        public List<DTO_RecibosDeCaja> Detalles { get; set; }

        #endregion

    }
}