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
    /// Models DTO_ccFlujoCesionDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccFlujoCesionDocu
    {
        #region DTO_ccFlujoCesionDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccFlujoCesionDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TerceroPago"].ToString()))
                    this.TerceroPago.Value = Convert.ToString(dr["TerceroPago"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToInt32(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToInt32(dr["Iva"]);
                this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccFlujoCesionDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.TerceroPago = new UDT_TerceroID();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.NumeroDocCXP = new UDT_Consecutivo();

            //Campo Adicional
            this.Detalle = new List<DTO_ccCreditoDocu>();
            this.FechaPagoFlujo = new UDTSQL_smalldatetime();
            this.Inversionista = new UDT_CompradorCarteraID();
            this.NombreCompradorCartera = new UDT_DescripTBase();
            this.NombreInversionista = new UDT_DescripTBase();
            this.NumCreditos = new UDTSQL_int();
            this.Oferta = new UDT_DocTerceroID();
            this.PagoFlujoInd = new UDT_SiNo();
            this.PagoInversionista = new UDT_SiNo();
            this.Portafolio = new UDT_PortafolioID();
            this.TerceroPago = new UDT_TerceroID();
            this.ValorPago = new UDT_Valor();
        }

        #endregion

        #region Propiedades
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set;}
        
        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set;}

        [DataMember]
        public UDT_TerceroID TerceroPago { get; set; }
        
        [DataMember]
        public UDT_Valor Valor { get; set;}
        
        [DataMember]
        public UDT_Valor Iva { get; set;}
        
        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

        #endregion

        #region Campos Adicionales

        public int index { get; set; }

        [DataMember]
        public List<DTO_ccCreditoDocu> Detalle { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPagoFlujo { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID Inversionista { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreCompradorCartera { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreInversionista { get; set; }

        [DataMember]
        public UDTSQL_int NumCreditos { get; set; }

        [DataMember]
        public UDT_DocTerceroID Oferta { get; set; }

        [DataMember]
        public UDT_SiNo PagoFlujoInd { get; set; }

        [DataMember]
        public UDT_SiNo PagoInversionista { get; set; }

        [DataMember]
        public UDT_PortafolioID Portafolio { get; set; }

        [DataMember]
        public UDT_Valor ValorPago { get; set; }


        #endregion
    }
}
