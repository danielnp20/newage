using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_faFacturacionFooter
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faFacturacionFooter
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faFacturacionFooter()
        {
            this.Movimiento = new DTO_glMovimientoDeta();
            this.SelectInd = new UDT_SiNo();
        }
        #endregion

        #region Propiedades
        [DataMember]
        public DTO_glMovimientoDeta Movimiento { get; set; }

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        public decimal ValorBruto { get; set; }

        [DataMember]
        public decimal ValorIVA { get; set; }

        [DataMember]
        public decimal ValorTotal { get; set; }

        [DataMember]
        public decimal ValorNeto { get; set; }

        [DataMember]
        public decimal ValorRIVA { get; set; }

        [DataMember]
        public decimal ValorRFT { get; set; }

        [DataMember]
        public decimal ValorRICA { get; set; }

        [DataMember]
        public decimal ValorOtros { get; set; }

        [DataMember]
        public decimal ValorRetenciones { get; set; }

        [DataMember]
        public decimal PorcIVA { get; set; }

        [DataMember]
        public decimal PorcRIVA { get; set; }

        [DataMember]
        public decimal PorcRFT { get; set; }

        [DataMember]
        public decimal PorcOtros { get; set; }

        [DataMember]
        [NotImportable]
        public decimal ValorAdministracion { get; set; }

        [DataMember]
        [NotImportable]
        public decimal ValorImprevistos { get; set; }

        [DataMember]
        [NotImportable]
        public decimal ValorUtilidad { get; set; }

        [DataMember]
        public UDT_SiNo SelectInd { get; set; }

        #endregion
    }
}
