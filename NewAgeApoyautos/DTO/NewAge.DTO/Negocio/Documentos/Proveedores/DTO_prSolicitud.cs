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

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_prSolicitud
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolicitud : DTO_BasicReport
    {
        #region Propriedades
        [DataMember]
        public DTO_prSolicitudDocu Header { get; set; }

        [DataMember]
        public DTO_prSolicitudDirectaDocu HeaderSolDirecta { get; set; }

        [DataMember]
        public List<DTO_prSolicitudFooter> Footer { get; set; }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSolicitud()
        {
            this.Header = new DTO_prSolicitudDocu();
            this.HeaderSolDirecta = new DTO_prSolicitudDirectaDocu();
            this.Footer = new List<DTO_prSolicitudFooter>();
            this.DocCtrl = new DTO_glDocumentoControl();
        }
        #endregion
    }

}
