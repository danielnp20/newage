﻿using System;
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
    public class DTO_pyLineaFlujoXEtapa
    {
        #region DTO_pyLineaFlujoXEtapa

       
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyLineaFlujoXEtapa()
        {
          
        }

           #endregion
               

        [DataMember]
        public string ActividadEtapaID { get; set; }

        [DataMember]
        public string ActividadEtapaIDDesc { get; set; }

 

    }
}
