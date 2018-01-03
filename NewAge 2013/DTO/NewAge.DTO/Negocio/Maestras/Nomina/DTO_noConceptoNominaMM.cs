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
    /// Models DTO_noConceptoNominaMM
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noConceptoNominaMM : DTO_MasterComplex 
    {
        #region DTO_noConceptoNominaMM
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noConceptoNominaMM(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ConceptoNODesc.Value = dr["ConceptoNODesc"].ToString();
                    this.ConceptoMMDesc.Value = dr["ConceptoMMDesc"].ToString();
                }
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.ConceptoMM.Value = dr["ConceptoMM"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noConceptoNominaMM() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConceptoMM = new UDT_BasicID();
            this.ConceptoMMDesc = new UDT_Descriptivo();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoNODesc = new UDT_Descriptivo();
                      
        }

        public DTO_noConceptoNominaMM(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noConceptoNominaMM(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID ConceptoMM { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoMMDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; }
    }

}
