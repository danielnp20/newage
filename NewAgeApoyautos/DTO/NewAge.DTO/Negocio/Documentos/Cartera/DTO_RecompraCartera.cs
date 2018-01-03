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
    /// Models DTO_InfoCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_RecompraCartera
    {
        public DTO_RecompraCartera()
        {
            this.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.RecompraDeta = new List<DTO_ccRecompraDeta>();
            this.RecompraDocu = new DTO_ccRecompraDocu();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccCreditoPlanPagos> pp, List<DTO_ccRecompraDeta> rde, DTO_ccRecompraDocu rdo)
        {
            this.PlanPagos = pp;
            this.RecompraDeta = rde;
            this.RecompraDocu = rdo;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccCreditoPlanPagos> PlanPagos
        {
            get;
            set;
        }
        
        [DataMember]
        public List<DTO_ccRecompraDeta> RecompraDeta
        {
            get;
            set;
        }

        [DataMember]
        public DTO_ccRecompraDocu RecompraDocu
        {
            get;
            set;
        }

        #endregion
    }
}
