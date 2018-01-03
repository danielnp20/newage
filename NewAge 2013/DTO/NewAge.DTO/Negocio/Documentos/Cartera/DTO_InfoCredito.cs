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
    public class DTO_InfoCredito
    {
        public DTO_InfoCredito()
        {
            this.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.SaldosComponentes = new List<DTO_ccSaldosComponentes>();
            this.LibranzaCompraCartera = new UDTSQL_int();
            this.EstadoCompraCartera = new UDTSQL_tinyint();
            this.ActFlujoSolicitudCompraCartera = new UDTSQL_varchar(10);
            this.FechaUltPago = new UDTSQL_smalldatetime();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccCreditoPlanPagos> pp, List<DTO_ccSaldosComponentes> c)
        {
            this.PlanPagos = pp;
            this.SaldosComponentes = c;          
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccCreditoPlanPagos> PlanPagos
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSaldosComponentes> SaldosComponentes
        {
            get;
            set;
        }

        [DataMember]
        public UDTSQL_int LibranzaCompraCartera
        {
            get;
            set;
        }

        [DataMember]
        public UDTSQL_tinyint EstadoCompraCartera
        {
            get;
            set;
        }

        [DataMember]
        public UDTSQL_varchar ActFlujoSolicitudCompraCartera
        {
            get;
            set;
        }

        [DataMember]
        public UDTSQL_smalldatetime FechaUltPago
        {
            get;
            set;
        }

        #endregion
    }
}
