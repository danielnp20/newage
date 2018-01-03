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
    /// Clase Global Total
    /// </summary>
    public class DTO_GlobalTotal 
    {
        #region Propiedades Genericas

        #endregion
    
        #region Propiedades de reportes

        /// <summary>
        /// Carga el DTO de Para verificar los Documentos Pendientes
        /// </summary>
        [DataMember]
        public List<DTO_ReportDocumentoPendientes> DetallesDocPendientes { get; set; }

        #endregion

    }
}
