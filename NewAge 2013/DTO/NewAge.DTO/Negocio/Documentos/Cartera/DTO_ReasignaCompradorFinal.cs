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
    /// Models DTO_ReasignaCompradorFinal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ReasignaCompradorFinal
    {
        public DTO_ReasignaCompradorFinal()
        {
            this.CompradorFinalDeta = new List<DTO_ccCompradorFinalDeta>();
            this.CompradorFinalDocu = new DTO_ccCompradorFinalDocu();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccCompradorFinalDeta> cfDeta, DTO_ccCompradorFinalDocu cfDocu)
        {
            this.CompradorFinalDeta = cfDeta;
            this.CompradorFinalDocu = cfDocu;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccCompradorFinalDeta> CompradorFinalDeta
        {
            get;
            set;
        }

        [DataMember]
        public DTO_ccCompradorFinalDocu CompradorFinalDocu
        {
            get;
            set;
        }

        #endregion
    }
}
