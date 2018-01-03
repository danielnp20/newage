using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO;
using System.Runtime.Serialization;
using System.Data;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_cpCausalesDev : DTO_MasterBasic
    {
        #region DTO_cpCausalesDev

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>        
        public DTO_cpCausalesDev(IDataReader dr, DTO_aplMaestraPropiedades mp)
            : base(dr, mp)
        {
            InitCols();

            this.EmpresaGrupoID.Value = dr["EmpresaGrupoID"].ToString();
            this.CausalDevID.Value = dr["CausalDevID"].ToString();
            this.ActivoInd.Value = Convert.ToBoolean(dr["ActivoInd"]);
            this.Descriptivo.Value = dr["Descriptivo"].ToString();
            this.CtrlVersion.Value = Convert.ToInt16(dr["CtrlVersion"]);
            this.ReplicaID.Value = Convert.ToInt16(dr["ReplicaID"].ToString());
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpCausalesDev() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaGrupoID = new UDT_EmpresaGrupoID();
            this.CausalDevID = new UDTSQL_char(3);
            this.ActivoInd = new UDT_SiNo();
            this.Descriptivo = new UDT_DescripTBase();
            this.CtrlVersion = new UDT_CtrlVersion();
            this.ReplicaID = new UDT_ReplicaID();
        }

        /// <summary>
        /// Constructor Master Basic
        /// </summary>
        /// <param name="basic"></param>
        public DTO_cpCausalesDev(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpCausalesDev(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        } 
 

        #endregion 

        #region Propiedades

        [DataMember]
        public UDT_EmpresaGrupoID EmpresaGrupoID { get; set; }
        [DataMember]
        public UDTSQL_char CausalDevID { get; set; }
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }
        [DataMember]
        public UDT_SiNo ActivoInd { get; set; }
        [DataMember]
        public UDT_CtrlVersion CtrlVersion { get; set; }
        [DataMember]
        public UDT_ReplicaID ReplicaID { get; set; }

        #endregion
    }
}
