using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Auxiliar
    /// </summary>
    public class DTO_ReportLibroDiarioTotales
    {
        #region Propiedades Genericaa

        /// <summary>
        /// Carga la Fecha Inicial
        /// </summary>
        [DataMember]
        public DateTime FechaFinal { get; set; }

        /// <summary>
        /// Carga la Fecha Final
        /// </summary>
        [DataMember]
        public DateTime FechaInicial { get; set; }

        #endregion
   
        #region Propiedades

        [DataMember]
        public List<DTO_ReportLibroDiario> Detalles { get; set; }
                
        #endregion

    }
}
