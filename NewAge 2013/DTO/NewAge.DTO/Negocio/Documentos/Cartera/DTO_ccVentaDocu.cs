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
    /// Models DTO_ccVentaDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccVentaDocu
    {
        #region DTO_ccVentaDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccVentaDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["DocFactura"].ToString()))
                    this.DocFactura.Value = Convert.ToInt32(dr["DocFactura"]);
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoVenta"].ToString()))
                    this.TipoVenta.Value = Convert.ToByte(dr["TipoVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["Oferta"].ToString()))
                    this.Oferta.Value = dr["Oferta"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumCuotas"].ToString()))
                    this.NumCuotas.Value = Convert.ToInt16(dr["NumCuotas"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                this.FechaPago1.Value = Convert.ToDateTime(dr["FechaPago1"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToInt32(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToInt32(dr["Iva"]);
                this.VlrVenta.Value = Convert.ToInt32(dr["VlrVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSustRecompra"].ToString()))
                    this.VlrSustRecompra.Value = Convert.ToInt32(dr["VlrSustRecompra"]);
                if (!string.IsNullOrWhiteSpace(dr["NoComercialInd"].ToString()))
                    this.NoComercialInd.Value = Convert.ToBoolean(dr["NoComercialInd"]);
                if (!string.IsNullOrWhiteSpace(dr["RefCartaAceptacion"].ToString()))
                    this.RefCartaAceptacion.Value = dr["RefCartaAceptacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaLiquida"].ToString()))
                    this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAceptacion"].ToString()))
                    this.FechaAceptacion.Value = Convert.ToDateTime(dr["FechaAceptacion"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPagoUlt"].ToString()))
                    this.FechaPagoUlt.Value= Convert.ToDateTime(dr["FechaPagoUlt"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaDescuento"].ToString()))
                    this.TasaDescuento.Value = Convert.ToDecimal(dr["TasaDescuento"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorUtilidadInd"].ToString()))
                    this.FactorUtilidadInd.Value = Convert.ToBoolean(dr["FactorUtilidadInd"]);                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccVentaDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocFactura = new UDT_Consecutivo();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.TipoVenta = new UDTSQL_tinyint();
            this.Oferta = new UDT_DocTerceroID();
            this.NumCuotas = new UDTSQL_smallint();
            this.Observacion = new UDT_DescripTBase();
            this.FechaPago1 = new UDTSQL_smalldatetime();
            this.FactorCesion = new UDT_PorcentajeCarteraID();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.VlrVenta = new UDT_Valor();
            this.VlrSustRecompra = new UDT_Valor();
            this.NoComercialInd = new UDT_SiNo();
            this.RefCartaAceptacion = new UDTSQL_char(10);
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.FechaAceptacion = new UDTSQL_smalldatetime();
            this.FechaPagoUlt = new UDTSQL_smalldatetime();
            this.TasaDescuento = new UDT_PorcentajeCarteraID();
            this.FactorUtilidadInd = new UDT_SiNo();
            //Campo Adicional
            this.TerceroID = new UDT_TerceroID();
            this.FechaVenta = new UDTSQL_smalldatetime();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo DocFactura { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoVenta { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDTSQL_smallint NumCuotas { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPago1 { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID FactorCesion { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDT_Valor VlrSustRecompra { get; set; }

        [DataMember]
        public UDT_SiNo NoComercialInd { get; set; }

        [DataMember]
        public UDTSQL_char RefCartaAceptacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAceptacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPagoUlt { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID TasaDescuento { get; set; }

        [DataMember]
        public UDT_SiNo FactorUtilidadInd { get; set; }
        
        //Campo Adicional

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVenta { get; set; }

        #endregion
    }
}
