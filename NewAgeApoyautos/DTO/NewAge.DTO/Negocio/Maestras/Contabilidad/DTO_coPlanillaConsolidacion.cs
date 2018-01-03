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
    /// Models DTO_coPlanillaConsolidacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coPlanillaConsolidacion : DTO_MasterComplex
    {
        #region DTO_coPlanillaConsolidacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coPlanillaConsolidacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.EmpresaDesc.Value = dr["EmpresaDesc"].ToString();
                }
                
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coPlanillaConsolidacion() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.EmpresaID = new UDT_BasicID();
            this.EmpresaDesc = new UDT_Descriptivo();
        }

        public DTO_coPlanillaConsolidacion(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coPlanillaConsolidacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpresaDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

    }
}
