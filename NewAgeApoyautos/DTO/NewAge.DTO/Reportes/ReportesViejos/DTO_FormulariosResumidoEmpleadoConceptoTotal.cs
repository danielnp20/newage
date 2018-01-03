﻿using System;
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
    public class DTO_FormulariosResumidoEmpleadoConceptoTotal
    {
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_FormulariosResumidoEmpleadoConceptoTotal()
        {           
        }

        #region Propiades
         [DataMember]
        public List<DTO_ReportNominaInfoEmpleado> Detalles { get; set; }
        #endregion

    }
}