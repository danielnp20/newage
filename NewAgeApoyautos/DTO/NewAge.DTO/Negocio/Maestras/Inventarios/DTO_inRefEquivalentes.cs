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
    /// Models DTO_inRefEquivalentes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inRefEquivalentes : DTO_MasterComplex
    {
        #region DTO_inRefEquivalentes
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inRefEquivalentes(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                    this.ReferenciaEquDesc.Value = dr["ReferenciaEquDesc"].ToString();
                }

                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.ReferenciaEqu.Value = dr["ReferenciaEqu"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inRefEquivalentes() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas          
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.ReferenciaEqu = new UDT_BasicID();
            this.ReferenciaEquDesc = new UDT_Descriptivo();      
        }

        public DTO_inRefEquivalentes(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_inRefEquivalentes(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ReferenciaEqu { get; set; }

        [DataMember]
        public UDT_Descriptivo ReferenciaEquDesc { get; set; }

    }
}
