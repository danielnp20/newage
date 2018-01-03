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
    public class DTO_ccCartas
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCartas()
        {
        }

        #region Propiades

        /// <summary>
        /// Carga el DTO que Genera la carta de Incoporaciones 
        /// </summary>
        [DataMember]
        public List<DTO_ccIncorporaciones> DetallesIncorporaciones { get; set; }

        /// <summary>
        /// Carga el DTO que genera la los Reportes de Cesion de Cartera
        /// </summary>
        [DataMember]
        public List<DTO_ccCesionCartera> DetallesCesionCartera { get; set; }
        
        #endregion
    }
}
