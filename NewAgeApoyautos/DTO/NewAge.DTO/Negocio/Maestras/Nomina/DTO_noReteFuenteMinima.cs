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
    /// Models DTO_noReteFuenteMinima
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReteFuenteMinima : DTO_MasterComplex
    {
        #region DTO_noReteFuenteMinima
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReteFuenteMinima(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.BaseUVTID.Value = Convert.ToDecimal(dr["BaseUVTID"]);
                this.BaseUVT.Value = Convert.ToDecimal(dr["BaseUVT"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.ValorUVT.Value = Convert.ToDecimal(dr["ValorUVT"]);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReteFuenteMinima() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BaseUVTID = new UDT_BaseUVT();
            this.BaseUVT = new UDT_BaseUVT();
            this.Descriptivo = new UDT_DescripTBase();
            this.ValorUVT = new UDT_Valor(); 
        }

        public DTO_noReteFuenteMinima(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noReteFuenteMinima(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BaseUVT BaseUVTID { get; set; }

        [DataMember]
        public UDT_BaseUVT BaseUVT { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor ValorUVT { get; set; }
          
    }
}

