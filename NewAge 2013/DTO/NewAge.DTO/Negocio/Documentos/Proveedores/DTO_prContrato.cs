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
    /// Models DTO_prContrato
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prContrato : DTO_BasicReport
    {
        #region Propriedades

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }

        [DataMember]
        public DTO_prContratoDocu Header { get; set; }

        [DataMember]
        public List<DTO_prOrdenCompraFooter> Footer { get; set; }

        [DataMember]
        public List<DTO_prOrdenCompraCotiza> Cotizacion { get; set; }

        [DataMember]
        public List<DTO_prConvenio> Convenio { get; set; }

        [DataMember]
        public List<DTO_prContratoPlanPago> ContratoPlanPagos { get; set; }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prContrato()
        {
            this.DocCtrl = new DTO_glDocumentoControl();
            this.Header = new DTO_prContratoDocu();
            this.Footer = new List<DTO_prOrdenCompraFooter>();
            this.Convenio = new List<DTO_prConvenio>();
            this.ContratoPlanPagos = new List<DTO_prContratoPlanPago>();
            this.Cotizacion = new List<DTO_prOrdenCompraCotiza>();
        }
        #endregion

    }

}
