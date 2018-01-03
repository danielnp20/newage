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
    /// Models DTO_coActEconomica
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coActEconomica : DTO_MasterBasic
    {
        #region DTO_coActEconomica
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coActEconomica(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                this.ServicioTipo.Value = Convert.ToByte(dr["ServicioTipo"]);
                if(!string.IsNullOrWhiteSpace(dr["ConceptoCargoID"].ToString()))
                    this.ConceptoCargoID.Value=dr["ConceptoCargoID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coActEconomica()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ServicioTipo = new UDTSQL_tinyint();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
        }

        public DTO_coActEconomica(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coActEconomica(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint ServicioTipo { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

    }

}
