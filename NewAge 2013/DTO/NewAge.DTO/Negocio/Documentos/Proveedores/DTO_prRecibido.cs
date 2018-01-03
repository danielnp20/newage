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
    /// Models DTO_prRecibido
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prRecibido : DTO_BasicReport
    {
        #region Propriedades
        [DataMember]
        public DTO_prRecibidoDocu Header { get; set; }

        [DataMember]
        public List<DTO_prOrdenCompraFooter> Footer { get; set; }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prRecibido()
        {
            this.Header = new DTO_prRecibidoDocu();
            this.Footer = new List<DTO_prOrdenCompraFooter>();
            this.DocCtrl = new DTO_glDocumentoControl();
        }
        #endregion
    }

}
