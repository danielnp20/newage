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
    /// Models DTO_InfoPagos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_InfoPagos
    {
        public DTO_InfoPagos()
        {
            this.CreditoPagos = new List<DTO_ccCreditoPagos>();
            this.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccCreditoPagos> cp, List<DTO_ccCreditoPlanPagos> pp)
        {
            this.CreditoPagos = cp;
            this.PlanPagos = pp;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccCreditoPagos> CreditoPagos
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccCreditoPlanPagos> PlanPagos
        {
            get;
            set;
        }

        #endregion
    }
}
