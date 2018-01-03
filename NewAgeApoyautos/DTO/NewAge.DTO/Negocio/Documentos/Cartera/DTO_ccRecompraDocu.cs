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
    /// Models DTO_ccRecompraDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccRecompraDocu
    {
        #region DTO_ccRecompraDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccRecompraDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["DocCXP"].ToString()))
                    this.DocCXP.Value = Convert.ToInt32(dr["DocCXP"]);
                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoRecompra"].ToString()))
                    this.TipoRecompra.Value = Convert.ToByte(dr["TipoRecompra"]);
                this.DocRecompra.Value = dr["DocRecompra"].ToString();
                this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FactorRecompra"].ToString()))
                    this.FactorRecompra.Value = Convert.ToInt32(dr["FactorRecompra"]);
                this.NoComercialInd.Value = Convert.ToBoolean(dr["NoComercialInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCXP"].ToString()))
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
        public DTO_ccRecompraDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocCXP = new UDT_Consecutivo();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.TipoRecompra = new UDTSQL_tinyint();
            this.DocRecompra = new UDT_DocTerceroID();
            this.Observacion = new UDT_DescripTBase();
            this.FactorRecompra = new UDT_TasaID();
            this.NoComercialInd = new UDT_SiNo();
            this.NumeroDocCXP = new UDT_Consecutivo();
            //Campos Adicionales
            this.TerceroID = new UDT_TerceroID();
            this.FechaCorte = new UDTSQL_datetime();
            this.ValorRecompra = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo DocCXP { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoRecompra { get; set; }

        [DataMember]
        public UDT_DocTerceroID DocRecompra { get; set; }

        [DataMember]
        public UDT_DescripTBase Observacion { get; set; }

        [DataMember]
        public UDT_TasaID FactorRecompra { get; set; }

        [DataMember]
        public UDT_SiNo NoComercialInd { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

        #endregion

        #region Campos Adicionales

        [DataMember]
        public UDTSQL_datetime FechaCorte { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Valor ValorRecompra { get; set; }

        #endregion

    }
}
