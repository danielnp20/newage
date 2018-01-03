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
    /// Models DTO_rhCompetenciasxCargo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_rhCompetenciasxCargo : DTO_MasterComplex
    {
        #region DTO_rhCompetenciasxCargo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_rhCompetenciasxCargo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CargoDesc.Value = dr["CargoDesc"].ToString();
                    this.CompetenciaDesc.Value = dr["CompetenciaDesc"].ToString();
                }
                
                this.CargoEmpID.Value = dr["CargoEmpID"].ToString();
                this.CompetenciaID.Value = dr["CompetenciaID"].ToString();
                this.Nivel.Value = Convert.ToByte(dr["Nivel"]);
                this.Necesidad.Value = Convert.ToByte(dr["Necesidad"]);
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_rhCompetenciasxCargo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.CargoEmpID = new UDT_BasicID();
            this.CargoDesc = new UDT_Descriptivo();
            this.CompetenciaID = new UDT_BasicID();
            this.CompetenciaDesc = new UDT_Descriptivo();
            this.Nivel = new UDTSQL_tinyint();
            this.Necesidad = new UDTSQL_tinyint();               
        }

        public DTO_rhCompetenciasxCargo(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_rhCompetenciasxCargo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID CargoEmpID { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CompetenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CompetenciaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Nivel { get; set; }

        [DataMember]
        public UDTSQL_tinyint Necesidad { get; set; }            
        
    }
}
