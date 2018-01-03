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
    /// Models DTO_glDocMigracionCampo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocMigracionCampo : DTO_MasterComplex
    {
        #region glDocMigracionCampo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocMigracionCampo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.CodigoDoc.Value = Convert.ToString(dr["CodigoDoc"]);
                this.TipoRegistro.Value = Convert.ToByte(dr["TipoRegistro"]);
                this.DetalleNumero.Value = Convert.ToByte(dr["DetalleNumero"]);
                this.Titulo.Value = dr["Titulo"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.CodigoCpo.Value = dr["CodigoCpo"].ToString();
                this.TipoCpo.Value = Convert.ToByte(dr["TipoCpo"]);
                if (!string.IsNullOrEmpty(dr["LongitudCpo"].ToString()))
                    this.LongitudCpo.Value = Convert.ToDecimal(dr["LongitudCpo"]);
                this.EliminaCeroIzqInd.Value = Convert.ToBoolean(dr["EliminaCeroIzqInd"]);
                this.NoUtilizadoInd.Value = Convert.ToBoolean(dr["NoUtilizadoInd"]);  
            }
            catch (Exception e)
            {
               throw e;
            }         
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDocMigracionCampo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CodigoDoc = new UDTSQL_varchar(5);
            this.TipoRegistro = new UDTSQL_tinyint();
            this.DetalleNumero = new UDTSQL_int();
            this.Descriptivo = new UDT_DescripTBase();
            this.Titulo = new UDTSQL_varchar(50);
            this.CodigoCpo = new UDTSQL_varchar(20);
            this.TipoCpo = new UDTSQL_tinyint();
            this.LongitudCpo = new UDTSQL_numeric();
            this.EliminaCeroIzqInd = new UDT_SiNo();
            this.NoUtilizadoInd = new UDT_SiNo();
        }

        public DTO_glDocMigracionCampo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDocMigracionCampo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
                
        [DataMember]
        public UDTSQL_varchar CodigoDoc { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoRegistro { get; set; }
        
        [DataMember]
        public UDTSQL_int DetalleNumero { get; set; }
        
        [DataMember]
        public UDTSQL_varchar Titulo { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }
        
        [DataMember]
        public UDTSQL_varchar CodigoCpo { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoCpo { get; set; }
        
        [DataMember]
        public UDTSQL_numeric LongitudCpo { get; set; }

        [DataMember]
        public UDT_SiNo EliminaCeroIzqInd { get; set; }

        [DataMember]
        public UDT_SiNo NoUtilizadoInd { get; set; }
    }

}
