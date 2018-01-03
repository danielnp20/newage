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
    /// Models DTO_ccCompraDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompraDocu
    {
        #region DTO_ccCompraDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompraDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["DocCtaxPagar"].ToString()))
                    this.DocCtaxPagar.Value = Convert.ToInt32(dr["DocCtaxPagar"]);
                if (!string.IsNullOrWhiteSpace(dr["FactVendedor"].ToString()))
                    this.FactVendedor.Value = Convert.ToString(dr["FactVendedor"]);
                if (!string.IsNullOrWhiteSpace(dr["VendedorID"].ToString()))
                    this.VendedorID.Value = Convert.ToString(dr["DocFactura"]);
                if (!string.IsNullOrWhiteSpace(dr["NumCuotas"].ToString()))
                    this.NumCuotas.Value = Convert.ToByte(dr["NumCuotas"]);
                if (!string.IsNullOrWhiteSpace(dr["Oferta"].ToString()))
                    this.Oferta.Value = Convert.ToString(dr["Oferta"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPago1"].ToString()))
                    this.FechaPago1.Value = Convert.ToDateTime(dr["FechaPago1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPagoUlt"].ToString()))
                    this.FechaPagoUlt.Value = Convert.ToDateTime(dr["FechaPagoUlt"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaCompra"].ToString()))
                    this.TasaCompra.Value = Convert.ToInt32(dr["TasaCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToInt32(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToInt32(dr["Iva"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompraDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc=new UDT_Consecutivo();
            this.DocCtaxPagar=new UDT_Consecutivo();
            this.FactVendedor=new UDT_DocTerceroID();
            this.VendedorID=new UDT_CompradorCarteraID();
            this.NumCuotas=new UDTSQL_smallint();
            this.Oferta=new UDT_DocTerceroID();
            this.Observacion=new UDT_DescripTBase();
            this.FechaPago1=new UDTSQL_smalldatetime();
            this.FechaPagoUlt=new UDTSQL_smalldatetime();
            this.TasaCompra=new UDT_TasaID();
            this.Valor=new UDT_Valor();
            this.Iva = new UDT_Valor();
            //Campo Extra
            this.FechaFondeo = new UDTSQL_smalldatetime();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo DocCtaxPagar { get; set; }

        [DataMember]
        public UDT_DocTerceroID FactVendedor { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID VendedorID { get; set; }

        [DataMember]
        public UDTSQL_smallint NumCuotas { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPagoUlt { get; set; }

        [DataMember]
        public UDT_TasaID TasaCompra { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        //Campo Extra
        [DataMember]
        public UDTSQL_smalldatetime FechaFondeo { get; set; }

        #endregion
    }
}
