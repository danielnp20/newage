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

    /// <summary>
    /// Clase del reporte Libro Mayor
    /// </summary>
    public class DTO_PlaneacionTotal 
    {
        #region Propiedades Genericas

        #endregion
    
        #region Propiedades de reportes

        /// <summary>
        /// Carga el DTO de Para verificar los presupuestos
        /// </summary>
        [DataMember]
        public List<DTO_ReportPresupuesto> DetallesPresupuesto { get; set; }

        /// <summary>
        /// Carga el listado de DTO para la realizar la ejecucion Presupuestal
        /// </summary>
        public List<DTO_ReportEjecucionPresupuestal> DetallesEjePresupuestal { get; set; }

        #endregion

    }
}
