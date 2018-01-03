using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using NewAge.DTO.UDT;
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
    public class DTO_ActivosTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ActivosTotales()
        {
        }

        #region Datos por Defecto
        
        #endregion

        #region Propiades para Reportes
        
        /// <summary>
        /// Carga la lista con lo componente y sus saldos
        /// </summary>
        [DataMember]
        public List<DTO_acCostos> DetallesSaldosComponentes { get; set; }

        [DataMember]
        public List<DTO_acComparacionLibros> DetallesComparacion { get; set; }

        [DataMember]
        public List<DTO_ReportEquiposArrendados> DetallesEquipos { get; set; }

        [DataMember]
        public List<DTO_ReportImportacionesTemporales> DetallesImportacionesT { get; set; }  
        
        #endregion
    }
}
