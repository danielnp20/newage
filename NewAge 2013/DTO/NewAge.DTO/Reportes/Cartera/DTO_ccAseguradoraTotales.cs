using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ccAseguradoraTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccAseguradoraTotales()
        {
        }

        #region Propiades
        [DataMember]
        public List<DTO_ccAseguradoraReport> Detalles { get; set; }

        #endregion
    }
}
