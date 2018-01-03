using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_prOrdenCompraFooter
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prOrdenCompraFooter 
    {
        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prOrdenCompraFooter()
        {
            this.DetalleDocu = new DTO_prDetalleDocu();
            this.SolicitudCargos = new List<DTO_prSolicitudCargos>();
        }        
        #endregion

        #region Propriedades
        [DataMember]
        public DTO_prDetalleDocu DetalleDocu { get; set; }

        [DataMember]
        public List<DTO_prSolicitudCargos> SolicitudCargos { get; set; }
        #endregion
    }
}
