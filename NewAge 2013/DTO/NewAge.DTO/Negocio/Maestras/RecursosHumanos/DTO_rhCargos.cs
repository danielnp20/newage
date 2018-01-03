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
    /// Models DTO_rhCargos
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_rhCargos : DTO_MasterBasic
    {
        #region DTO_rhCargos
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_rhCargos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica) 
            : base(dr,mp)
        {
             InitCols();
             try
             {
                if (!isReplica)
                {
                    this.NivelSalarialDesc.Value = dr["NivelSalarialDesc"].ToString();
                    this.NivelRespDesc.Value = dr["NivelRespDesc"].ToString();
                }
                 
                this.Detalle.Value = dr["Detalle"].ToString();
                this.NivelSalarialID.Value = dr["NivelSalarialID"].ToString();
                this.NivelResponsabilidadID.Value = dr["NivelResponsabilidadID"].ToString();
                this.ControlHorarioInd.Value = Convert.ToBoolean(dr["ControlHorarioInd"]);
                this.RespFinancieraInd.Value = Convert.ToBoolean(dr["RespFinancieraInd"]);
             }
             catch (Exception e)
             {
                 throw e;
             }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_rhCargos()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Detalle = new UDT_DescripTExt();
            this.NivelSalarialID = new UDT_BasicID();
            this.NivelSalarialDesc = new UDT_Descriptivo();
            this.NivelResponsabilidadID = new UDT_BasicID();
            this.NivelRespDesc = new UDT_Descriptivo();
            this.ControlHorarioInd = new UDT_SiNo();
            this.RespFinancieraInd = new UDT_SiNo();
        }

        public DTO_rhCargos(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_rhCargos(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_DescripTExt Detalle { get; set; }

        [DataMember]
        public UDT_BasicID NivelSalarialID { get; set; }

        [DataMember]
        public UDT_Descriptivo NivelSalarialDesc { get; set; }

        [DataMember]
        public UDT_BasicID NivelResponsabilidadID { get; set; }

        [DataMember]
        public UDT_Descriptivo NivelRespDesc { get; set; }

        [DataMember]
        public UDT_SiNo ControlHorarioInd { get; set; }

        [DataMember]
        public UDT_SiNo RespFinancieraInd { get; set; }
    }
}
