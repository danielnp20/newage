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
    /// Clase del reporte Comprobante
    /// </summary>
    public class DTO_ReportComprobanteTotal : DTO_BasicReport
    {
        #region Propiedades

        [DataMember]
        public List<DTO_ReportComprobante> Detalles { get; set; }
                
        #endregion
    }
}
