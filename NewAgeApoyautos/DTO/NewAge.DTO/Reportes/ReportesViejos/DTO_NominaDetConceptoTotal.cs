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
    public class DTO_NominaDetConceptoTotal
    {
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaDetConceptoTotal()
        {           
        }

        #region Propiades
         [DataMember]
        public List<DTO_ReportNominaInfoEmpleado_Totales> Detalles { get; set; }
        #endregion

    }
}