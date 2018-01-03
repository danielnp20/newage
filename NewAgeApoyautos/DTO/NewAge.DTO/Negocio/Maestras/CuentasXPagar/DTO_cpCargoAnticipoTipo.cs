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
    /// Models DTO_cpDistribuyeImpLocal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpCargoAnticipoTipo : DTO_MasterComplex
    {
        #region DTO_cpCargoAnticipoTipo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpCargoAnticipoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.AnticipoTipoDesc.Value = dr["AnticipoTipoDesc"].ToString();
                    this.CargoEspecialDesc.Value = dr["CargoEspecialDesc"].ToString();
                }

                this.AnticipoTipoID.Value = dr["AnticipoTipoID"].ToString();
                this.CargoEspecialID.Value = dr["CargoEspecialID"].ToString();

            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpCargoAnticipoTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.AnticipoTipoID = new UDT_BasicID();
            this.AnticipoTipoDesc = new UDT_Descriptivo();
            this.CargoEspecialID = new UDT_BasicID();
            this.CargoEspecialDesc = new UDT_Descriptivo();
   
        }

        public DTO_cpCargoAnticipoTipo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpCargoAnticipoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID AnticipoTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo AnticipoTipoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CargoEspecialID { get; set; }

        [DataMember]
        public UDT_Descriptivo CargoEspecialDesc { get; set; }

    }

}
