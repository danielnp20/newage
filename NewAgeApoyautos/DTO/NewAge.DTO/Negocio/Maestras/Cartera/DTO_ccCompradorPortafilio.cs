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
    public class DTO_ccCompradorPortafilio : DTO_MasterComplex
    {
        #region DTO_ccCompradorPortafilio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorPortafilio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
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
                this.Portafolio.Value = dr["Portafolio"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorPortafilio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CompradorCarteraID = new UDT_BasicID();
            this.CompradorCarteraDesc = new UDT_Descriptivo();
            this.Portafolio = new UDTSQL_char(5);
            this.Descriptivo = new UDT_DescripTBase();
            
        }

        public DTO_ccCompradorPortafilio(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCompradorPortafilio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo CompradorCarteraDesc { get; set; }

        [DataMember]
        public UDTSQL_char Portafolio { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo{ get; set; }

    }

}
