using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_ccCompradorPagaduria
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompradorMonto : DTO_MasterComplex
    {
        #region DTO_ccCompradorMonto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorMonto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CompradorCarteraDesc.Value = dr["CompradorCarteraDesc"].ToString();
                   
                }

                this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if(!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                if (!string.IsNullOrEmpty(dr["FactorRecompra"].ToString()))
                    this.FactorRecompra.Value = Convert.ToDecimal(dr["FactorRecompra"]);
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorMonto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CompradorCarteraID = new UDT_BasicID();
            this.CompradorCarteraDesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();
            this.FactorCesion = new UDT_TasaID();
            this.FactorRecompra = new UDT_TasaID();
            
        }

        public DTO_ccCompradorMonto(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCompradorMonto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo CompradorCarteraDesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_TasaID FactorCesion { get; set; }

        [DataMember]
        public UDT_TasaID FactorRecompra { get; set; }
    }

}
