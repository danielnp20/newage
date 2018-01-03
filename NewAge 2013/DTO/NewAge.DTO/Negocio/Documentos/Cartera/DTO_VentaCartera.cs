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
    /// Models DTO_VentaCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_VentaCartera
    {
        public DTO_VentaCartera()
        {
            this.Creditos = new List<DTO_ccCreditoDocu>();
            this.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.VentaDeta = new List<DTO_ccVentaDeta>();
            this.VentaDocu = new DTO_ccVentaDocu();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccCreditoDocu> cdo, List<DTO_ccCreditoPlanPagos> pp, List<DTO_ccVentaDeta> vde, DTO_ccVentaDocu vdo)
        {
            this.Creditos = cdo;
            this.PlanPagos = pp;
            this.VentaDeta = vde;
            this.VentaDocu = vdo;
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccVentaDeta> vde, DTO_ccVentaDocu vdo)
        {
            this.VentaDeta = vde;
            this.VentaDocu = vdo;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccCreditoDocu> Creditos
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

        [DataMember]
        public List<DTO_ccVentaDeta> VentaDeta
        {
            get;
            set;
        }

        [DataMember]
        public DTO_ccVentaDocu VentaDocu
        {
            get;
            set;
        }

        #endregion
    }
}
