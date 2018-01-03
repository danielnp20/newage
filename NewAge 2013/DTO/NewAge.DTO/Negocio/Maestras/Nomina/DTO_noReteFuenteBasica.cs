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
    /// Models DTO_noReteFuenteBasica
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReteFuenteBasica : DTO_MasterComplex
    {
        #region DTO_noReteFuenteBasica
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReteFuenteBasica(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.BaseUVTID.Value = Convert.ToDecimal(dr["BaseUVTID"]);
                this.BaseUVT.Value = Convert.ToDecimal(dr["BaseUVT"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.AdicionUVT.Value = Convert.ToInt32(dr["AdicionUVT"]);
                this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReteFuenteBasica()
            : base()
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
            this.AdicionUVT = new UDTSQL_int();
            this.PorcentajeID = new UDT_PorcentajeID();
        }

        public DTO_noReteFuenteBasica(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noReteFuenteBasica(DTO_aplMaestraPropiedades masterProperties)
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
        public UDTSQL_int AdicionUVT { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeID { get; set; }
    }
}

