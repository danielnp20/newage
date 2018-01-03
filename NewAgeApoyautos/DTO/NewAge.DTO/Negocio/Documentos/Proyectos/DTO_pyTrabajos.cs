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
    public class DTO_pyTrabajos
    {
        #region DTO_pyTrabajos

       
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTrabajos()
        {
          
        }

           #endregion
               

        [DataMember]
        public string TrabajoID { get; set; }
        
        [DataMember]
        public string TrabajoIDDesc { get; set; }

 

    }
}
