using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccSustitucionCreditos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSustitucionCreditos
    {
        #region DTO_ccSustitucionCreditos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ccSustitucionCreditos()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.Libranza = new UDT_LibranzaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.LibranzaSustituye = new UDT_LibranzaID();
            this.NumeroDocSustituye = new UDT_Consecutivo();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public DTO_ccCreditoDocu Credito { get; set; }

        [DataMember]
        public UDT_LibranzaID LibranzaSustituye { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDocSustituye { get; set; }

        [DataMember]
        [NotImportable]
        public DTO_ccCreditoDocu CreditoSustituye { get; set; }

        #endregion

    }
}
