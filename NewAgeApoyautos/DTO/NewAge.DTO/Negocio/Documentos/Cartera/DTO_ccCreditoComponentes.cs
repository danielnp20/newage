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
    /// Models DTO_ccCreditoComponentes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCreditoComponentes : DTO_ccSolicitudComponentes
    {
        #region DTO_ccCreditoComponentes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCreditoComponentes(IDataReader dr)
            : base(dr)
        {
            this.InitCols();
            try
            {
                this.AbonoValor.Value = Convert.ToDecimal(dr["AbonoValor"]);
                this.DescuentoInd.Value = Convert.ToBoolean(dr["DescuentoInd"]);
                if(!String.IsNullOrWhiteSpace(dr["DocPago"].ToString()))
                    this.DocPago.Value = Convert.ToInt32(dr["DocPago"]);

                if (!String.IsNullOrWhiteSpace(dr["PorCapital"].ToString()))
                    this.PorCapital.Value = Convert.ToDecimal(dr["PorCapital"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCreditoComponentes()
            : base()
        {
            this.InitCols();
        }

        private void InitCols()
        {
            this.AbonoValor = new UDT_Valor();
            this.DescuentoInd = new UDT_SiNo();
            this.DocPago = new UDT_Valor();
            this.PorCapital = new UDT_Valor();
            //Props extras
            this.CuotaID = new UDT_CuotaID();
            this.AbonoCJValor = new UDT_Valor();
            this.VlrNoCausado = new UDT_Valor();
            this.VlrCausado = new UDT_Valor();
            //Propiedades de saldos
            this.ComponenteFijo = new UDTSQL_tinyint();
            this.VlrPagar = new UDT_Valor();
        }

        #endregion
       
        [DataMember]
        public UDT_Valor AbonoValor { get; set; }

        [DataMember]
        public UDT_SiNo DescuentoInd { get; set; }

        [DataMember]
        public UDT_Valor DocPago { get; set; }

        //Campos Adicionales
        
        [DataMember]
        public UDT_Valor PorCapital { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDT_Valor AbonoCJValor { get; set; }

        [DataMember]
        public UDT_Valor VlrNoCausado { get; set; }

        [DataMember]
        public UDT_Valor VlrCausado { get; set; }

        //Propieades de saldos
        [DataMember]
        public UDTSQL_tinyint ComponenteFijo { get; set; }

        [DataMember]
        public UDT_Valor VlrPagar { get; set; }

    }
}
