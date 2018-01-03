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
    public class DTO_pyCostos
    {
        #region DTO_pyEtapas

       
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyCostos()
        {
            this.CentroCostoID = new UDT_CentroCostoID();
            this.CentroCostoIDDesc = new UDT_DescripTBase();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LineaPresupuestoIDDesc = new UDT_DescripTBase();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
        }

           #endregion
               

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_DescripTBase CentroCostoIDDesc { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTBase LineaPresupuestoIDDesc { get; set; }

        [DataMember]
        public UDT_Valor CostoLocal { get; set; }

        [DataMember]
        public UDT_Valor CostoExtra { get; set; }

 

    }
}
