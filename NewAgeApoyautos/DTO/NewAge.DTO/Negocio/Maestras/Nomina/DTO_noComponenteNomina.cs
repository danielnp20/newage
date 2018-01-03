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
    /// Models DTO_noConceptoNOM
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noComponenteNomina : DTO_MasterBasic
    {
        #region DTO_noComponenteNomina
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noComponenteNomina(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.CtaAnticipoDesc.Value = dr["CtaAnticipoDesc"].ToString();
                    this.ConceptoDesc.Value = dr["ConceptoDesc"].ToString();
                }
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.CtaAnticipo.Value = dr["CtaAnticipo"].ToString();
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
        public DTO_noComponenteNomina() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.CtaAnticipo = new UDT_BasicID();
            this.CtaAnticipoDesc = new UDT_Descriptivo();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoDesc = new UDT_Descriptivo();
        }

        public DTO_noComponenteNomina(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noComponenteNomina(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CtaAnticipo { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CtaAnticipoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoDesc { get; set; }
       
    }
}

