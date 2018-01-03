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
    public class DTO_pyTareas
    {
        #region DTO_pyTareas

       
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareas()
        {
          
        }
        
        #endregion
               

        [DataMember]
        public string TareaID { get; set; }

        [DataMember]
        public string TareaIDDesc { get; set; }

        [DataMember]
        public List<DTO_pyTrabajos> LTrabajos { get; set; }

    }
}
