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
    /// Models DTO_coTercero
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glLugarGeografico : DTO_MasterHierarchyBasic
    {
        #region DTO_glLugarGeografico
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glLugarGeografico(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.PaisDesc.Value = dr["PaisDesc"].ToString();
                }

                this.PrefijoTel.Value = dr["PrefijoTel"].ToString();
                this.DistribuyeInd.Value = Convert.ToBoolean(dr["DistribuyeInd"]);
                this.PaisID.Value = dr["PaisID"].ToString();
                this.CiudadCD.Value = dr["CiudadCD"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glLugarGeografico() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PrefijoTel = new UDTSQL_char(7);
            this.DistribuyeInd = new UDT_SiNo();  
            this.PaisID = new UDT_PaisID();
            this.PaisDesc = new UDT_Descriptivo();
            this.CiudadCD = new UDTSQL_char(10);
        }

        public DTO_glLugarGeografico(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glLugarGeografico(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
                
        [DataMember]
        public UDT_SiNo DistribuyeInd { get; set; }

        [DataMember]
        public UDTSQL_char PrefijoTel { get; set; }

        [DataMember]
        public UDT_PaisID PaisID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo PaisDesc { get; set; }

        [DataMember]
        public UDTSQL_char CiudadCD { get; set; }

    }
}
