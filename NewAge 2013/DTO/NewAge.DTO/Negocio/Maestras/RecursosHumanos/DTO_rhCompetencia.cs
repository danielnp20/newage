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
    /// Models DTO_rhCompetencia
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_rhCompetencia : DTO_MasterBasic
    {
        #region DTO_rhCompetencia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_rhCompetencia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica) 
            : base(dr,mp)
        {
            InitCols();
            try
            {
                this.Detalle.Value = dr["Detalle"].ToString();
                this.TipoCompetencia.Value = Convert.ToByte(dr["TipoCompetencia"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_rhCompetencia()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Detalle = new UDT_DescripTExt();
            this.TipoCompetencia = new UDTSQL_tinyint();
        }

        public DTO_rhCompetencia(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_rhCompetencia(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_DescripTExt Detalle { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCompetencia { get; set; }
    }
}
