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
    /// Models DTO_coConceptoCargo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coConceptoCargo : DTO_MasterHierarchyBasic
    {
        #region DTO_coConceptoCargo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coConceptoCargo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ConceptoImpDesc.Value = dr["ConceptoImpDesc"].ToString();   
                }
                if (!string.IsNullOrWhiteSpace(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoConcepto"].ToString()))
                    this.TipoConcepto.Value = Convert.ToByte(dr["TipoConcepto"]);
                if (!string.IsNullOrWhiteSpace(dr["BienServicio"].ToString()))
                    this.BienServicio.Value = Convert.ToByte(dr["BienServicio"]);
                if (!string.IsNullOrWhiteSpace(dr["ConceptoImpID"].ToString()))
                    this.ConceptoImpID.Value = dr["ConceptoImpID"].ToString();
               
                this.PresMensualVariableInd.Value = Convert.ToBoolean(dr["PresMensualVariableInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coConceptoCargo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.TipoConcepto = new UDTSQL_tinyint();
            this.BienServicio = new UDTSQL_tinyint();
            this.PresMensualVariableInd = new UDT_SiNo();
            this.ConceptoImpID = new UDT_BasicID();
            this.ConceptoImpDesc = new UDT_Descriptivo();
        }

        public DTO_coConceptoCargo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coConceptoCargo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoConcepto { get; set; }

        [DataMember]
        public UDTSQL_tinyint BienServicio { get; set; }

        [DataMember]
        public UDT_SiNo PresMensualVariableInd { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoImpID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo ConceptoImpDesc { get; set; }
    }
}
