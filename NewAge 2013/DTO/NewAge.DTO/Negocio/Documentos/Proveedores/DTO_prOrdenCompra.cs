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
    /// Models DTO_prOrdenCompra
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prOrdenCompra : DTO_BasicReport
    {
        #region Propiedades

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }

        [AllowNull]
        [DataMember]
        public DTO_prOrdenCompraDocu HeaderOrdenCompra { get; set; }

        [AllowNull]
        [DataMember]
        public DTO_prContratoDocu HeaderContrato { get; set; }

        [DataMember]
        public List<DTO_prOrdenCompraFooter> Footer { get; set; }

        [DataMember]
        public List<DTO_prOrdenCompraCotiza> Cotizacion { get; set; }

        [AllowNull]
        [DataMember]
        public List<DTO_prConvenio> Convenio { get; set; }

        [AllowNull]
        [DataMember]
        public List<DTO_prContratoPolizas> Polizas { get; set; }

        [AllowNull]
        [DataMember]
        public List<DTO_prContratoPlanPago> ContratoPlanPagos { get; set; }

        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prOrdenCompra()
        {
            this.DocCtrl = new DTO_glDocumentoControl();
            this.HeaderOrdenCompra = new DTO_prOrdenCompraDocu();
            this.HeaderContrato = new DTO_prContratoDocu();
            this.Footer = new List<DTO_prOrdenCompraFooter>();
            this.Cotizacion = new List<DTO_prOrdenCompraCotiza>();
            this.Convenio = new List<DTO_prConvenio>();
            this.ContratoPlanPagos = new List<DTO_prContratoPlanPago>();
            this.Polizas = new List<DTO_prContratoPolizas>();
        }
        #endregion
    }

}
