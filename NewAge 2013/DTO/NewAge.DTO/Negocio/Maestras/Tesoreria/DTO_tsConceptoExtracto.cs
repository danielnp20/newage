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
    /// Models DTO_tsConceptoExtracto
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_tsConceptoExtracto : DTO_MasterComplex
    {
        #region DTO_tsConceptoExtracto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsConceptoExtracto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica) 
            : base(dr,mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.BancoCuentaDesc.Value = dr["BancoCuentaDesc"].ToString();
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                }

                this.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();
                this.ConceptoBanco.Value = dr["ConceptoBanco"].ToString();
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
            }
             catch (Exception e)
             {
                throw e;
             }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsConceptoExtracto()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BancoCuentaID = new UDT_BasicID();
            this.BancoCuentaDesc = new UDT_Descriptivo();
            this.ConceptoBanco = new UDTSQL_char(20);
            this.ComprobanteID = new UDT_BasicID();
            this.ComprobanteDesc = new UDT_Descriptivo();
        }

        public DTO_tsConceptoExtracto(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsConceptoExtracto(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID BancoCuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoCuentaDesc { get; set; }

        [DataMember]
        public UDTSQL_char ConceptoBanco { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }

    }
}
