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
    public class DTO_RecaudoPagos
    {
        public DTO_RecaudoPagos()
        {
            this.FechaCuota = new UDTSQL_smalldatetime();
            this.CuotaID = new UDTSQL_int();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumDocEstadoCta = new UDT_Consecutivo();
            this.CanceladoInd = new UDT_SiNo();
            this.Libranza = new UDTSQL_int();
            this.VlrPago = new UDT_Valor();
            this.SaldosComponentes = new List<DTO_ccSaldosComponentes>();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccCreditoPlanPagos> pp, List<DTO_ccSaldosComponentes> c)
        {
            this.SaldosComponentes = c;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccSaldosComponentes> SaldosComponentes
        {
            get;
            set;
        }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota {  get;  set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocEstadoCta { get; set; }

        [DataMember]
        public UDT_SiNo CanceladoInd { get; set; }

        [DataMember]
        public UDTSQL_int Libranza { get; set; }

        [DataMember]
        public UDTSQL_int CuotaID { get; set; }

        [DataMember]
        public UDT_Valor VlrPago { get; set; }

        [DataMember]
        public int indexCuota { get; set; }

        #endregion
    }
}
