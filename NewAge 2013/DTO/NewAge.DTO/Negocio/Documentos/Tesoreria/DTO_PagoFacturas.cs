using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pago Facturas:
    /// Models DTO_PagoFacturas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_PagoFacturas : DTO_SerializedObject
    {
        #region DTO_PagoFacturas
        
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_PagoFacturas(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();
                this.MonedaBancoCuenta.Value = Convert.ToByte(dr["MonedaBancoCuenta"]);
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.NumeroFacturas.Value = Convert.ToInt32(dr["NumeroFacturas"]);
                this.TotalFacturas.Value = Convert.ToDecimal(dr["TotalFacturas"]);
                this.BeneficiarioID.Value = dr["BeneficiarioID"].ToString();
                this.BeneficiarioDesc.Value = dr["Beneficiario"].ToString();
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();             
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PagoFacturas()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PagoFacturasInd = new UDT_SiNo();
            this.BancoCuentaID = new UDT_BancoCuentaID();
            this.MonedaBancoCuenta = new UDTSQL_tinyint();
            this.MonedaID = new UDT_MonedaID();
            this.TerceroID = new UDT_TerceroID();
            this.Descriptivo = new UDT_DescripTBase();
            this.BeneficiarioID = new UDT_TerceroID();
            this.BeneficiarioDesc = new UDT_DescripTBase();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ConsecutivoEgreso = new UDT_Consecutivo();
            this.NumeroFacturas = new UDTSQL_int();
            this.TotalFacturas = new UDT_Valor();
            this.TotalFacturasLocal = new UDT_Valor();
            this.TotalFacturasExtra = new UDT_Valor();
            this.DetallesFacturas = new List<DTO_DetalleFactura>();
        }


        #endregion

        #region Propiedades
        
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_SiNo PagoFacturasInd { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint MonedaBancoCuenta { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_TerceroID BeneficiarioID { get; set; }

        [DataMember]
        public UDT_DescripTBase BeneficiarioDesc { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoEgreso { get; set; }

        [DataMember]
        public UDTSQL_int NumeroFacturas { get; set; }

        [DataMember]
        public UDT_Valor TotalFacturas { get; set; }

        [DataMember]
        public UDT_Valor TotalFacturasLocal { get; set; }

        [DataMember]
        public UDT_Valor TotalFacturasExtra { get; set; }

        [DataMember]
        public List<DTO_DetalleFactura> DetallesFacturas { get; set; }

        #endregion
    }
}
