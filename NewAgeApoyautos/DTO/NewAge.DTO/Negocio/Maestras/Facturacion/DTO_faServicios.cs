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
    /// Models DTO_faServicios
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faServicios : DTO_MasterHierarchyBasic 
    {
        #region DTO_faServicios
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faServicios(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoIngresoDesc.Value = dr["ConceptoIngresoDesc"].ToString();
                    this.CodigoBSDesc.Value = dr["CodigoBSDesc"].ToString();
                    this.ClaseServicioDesc.Value = dr["ClaseServicioDesc"].ToString();
                }

                this.ConceptoIngresoID.Value = dr["ConceptoIngresoID"].ToString();
                this.DescrVariableInd.Value = Convert.ToBoolean(dr["DescrVariableInd"]);
                this.DetallaInventariosInd.Value = Convert.ToBoolean(dr["DetallaInventariosInd"]);
                this.DescVariable.Value = dr["DescVariable"].ToString();
                if (!string.IsNullOrEmpty(dr["CodigoBSID"].ToString()))
                    this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                if (!string.IsNullOrEmpty(dr["ClaseServicioID"].ToString()))
                    this.ClaseServicioID.Value = dr["ClaseServicioID"].ToString();
           }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faServicios() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConceptoIngresoID = new UDT_BasicID();
            this.ConceptoIngresoDesc = new UDT_Descriptivo();
            this.DescrVariableInd = new UDT_SiNo();
            this.DetallaInventariosInd = new UDT_SiNo();
            this.DescVariable = new UDT_DescripTBase();
            this.CodigoBSID = new UDT_BasicID();
            this.CodigoBSDesc = new UDT_Descriptivo();
            this.ClaseServicioID = new UDT_BasicID();
            this.ClaseServicioDesc = new UDT_Descriptivo();
        }

        public DTO_faServicios(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faServicios(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID ConceptoIngresoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoIngresoDesc { get; set; }

        [DataMember]
        public UDT_SiNo DescrVariableInd { get; set; }

        [DataMember]
        public UDT_SiNo DetallaInventariosInd { get; set; }

        [DataMember]
        public UDT_DescripTBase DescVariable { get; set; }

        [DataMember]
        public UDT_BasicID CodigoBSID { get; set; }

        [DataMember]
        public UDT_Descriptivo CodigoBSDesc { get; set; }

        [DataMember]
        public UDT_BasicID ClaseServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseServicioDesc { get; set; }
    }

}
