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
    /// Models DTO_inCosteoGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inCosteoGrupo : DTO_MasterBasic
    {
        #region DTO_inCosteoGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inCosteoGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.CosteoTipo.Value = Convert.ToByte(dr["CosteoTipo"]);                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inCosteoGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CosteoTipo = new UDTSQL_tinyint();
        }

        public DTO_inCosteoGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inCosteoGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint CosteoTipo  { get; set; }

    }

}
