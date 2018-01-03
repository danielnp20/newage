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
    /// Models DTO_inRefClase
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inRefClase : DTO_MasterHierarchyBasic
    {
        #region DTO_inRefClase
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inRefClase(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ActivoClaseDesc.Value = dr["ActivoClaseDesc"].ToString();
                }

                this.InventarioInd.Value = Convert.ToBoolean(dr["InventarioInd"]);
                this.SuministroInd.Value = Convert.ToBoolean(dr["SuministroInd"]);
                this.ActivoFijoInd.Value = Convert.ToBoolean(dr["ActivoFijoInd"]);
                this.ActivoClaseID.Value = dr["ActivoClaseID"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inRefClase() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.InventarioInd = new UDT_SiNo();
            this.SuministroInd = new UDT_SiNo();
            this.ActivoFijoInd = new UDT_SiNo();

            this.ActivoClaseID = new UDT_BasicID();
            this.ActivoClaseDesc = new UDT_Descriptivo();
        }

        public DTO_inRefClase(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inRefClase(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_SiNo InventarioInd { get; set; }

        [DataMember]
        public UDT_SiNo SuministroInd { get; set; }

        [DataMember]
        public UDT_SiNo ActivoFijoInd { get; set; }

        [DataMember]
        public UDT_BasicID ActivoClaseID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActivoClaseDesc { get; set; }
   
    }

}
