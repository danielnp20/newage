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
    /// Models DTO_DetalleFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_DetalleFactura
    {
        #region DTO_DetalleFactura

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_DetalleFactura(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.ValorPago.Value = Convert.ToDecimal(dr["ValorPago"]);
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Tercero.Value = dr["Tercero"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_DetalleFactura()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.Observacion = new UDT_DescripTExt();
            this.MonedaID = new UDT_MonedaID();
            this.ValorPago = new UDT_Valor();
            this.ValorPagoLocal = new UDT_Valor();
            this.ValorPagoExtra = new UDT_Valor();
            this.TerceroID = new UDT_TerceroID();
            this.Tercero = new UDT_DescripTBase();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_Valor ValorPago { get; set; }

        [DataMember]
        public UDT_Valor ValorPagoLocal { get; set; }

        [DataMember]
        public UDT_Valor ValorPagoExtra { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase Tercero { get; set; }
        #endregion
    }
}
