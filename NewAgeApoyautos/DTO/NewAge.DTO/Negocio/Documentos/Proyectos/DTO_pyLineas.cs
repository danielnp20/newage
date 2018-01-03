using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_pyServicioDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyLineas
    {
        #region DTO_pyLineas

       
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyLineas()
        {
          
        }

        #endregion


        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public string LineaFlujoID { get; set; }

        [DataMember]
        public string LineaFlujoIDDesc { get; set; }

        [DataMember]
        public List<DTO_pyEtapas> LEtapas { get; set; }

    }
}
