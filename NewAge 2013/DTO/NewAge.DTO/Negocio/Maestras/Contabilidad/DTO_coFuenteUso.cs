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
    /// Models DTO_coFuenteUso
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coFuenteUso : DTO_MasterBasic
    {
        #region DTO_coFuenteUso
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coFuenteUso(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                this.Actividad.Value = Convert.ToByte(dr["Actividad"]);
                this.TipoFlujo.Value = Convert.ToByte(dr["TipoFlujo"]);
            }
            catch (Exception e)
            {               
                throw e;
            }            
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_coFuenteUso() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.Actividad = new UDTSQL_tinyint();
            this.TipoFlujo = new UDTSQL_tinyint();
            
        }

        public DTO_coFuenteUso(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coFuenteUso(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDTSQL_tinyint Tipo{ get; set; }

        [DataMember]
        public UDTSQL_tinyint Actividad{ get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoFlujo { get; set; }

    }

}
