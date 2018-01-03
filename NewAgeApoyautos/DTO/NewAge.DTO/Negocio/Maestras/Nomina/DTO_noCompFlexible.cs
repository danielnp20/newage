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
    /// Models DTO_noCompFlexible
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noCompFlexible : DTO_MasterBasic
    {
        #region DTO_noCompFlexible
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noCompFlexible(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoNODesc.Value = dr["ConceptoNODesc"].ToString();
                }
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                this.GravadoInd.Value = Convert.ToBoolean(dr["GravadoInd"]);
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noCompFlexible() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.PeriodoPago = new UDTSQL_tinyint();
            this.GravadoInd = new UDT_SiNo();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoNODesc = new UDT_Descriptivo();
        }

        public DTO_noCompFlexible(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noCompFlexible(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoPago { get; set; }

        [DataMember]
        public UDT_SiNo GravadoInd { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; } 
    }
}

