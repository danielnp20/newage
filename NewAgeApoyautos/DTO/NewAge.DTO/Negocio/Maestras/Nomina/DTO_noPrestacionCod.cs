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
    /// Models DTO_noPrestacionCod
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noPrestacionCod : DTO_MasterBasic
    {
        #region DTO_noPrestacionCod
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noPrestacionCod(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoNODesc.Value = Convert.ToString(dr["ConceptoNODesc"]);    
                }
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                this.ConceptoNOID.Value = Convert.ToString(dr["ConceptoNOID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noPrestacionCod()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoNODesc = new UDT_Descriptivo();
        }

        public DTO_noPrestacionCod(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noPrestacionCod(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }
        
        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; }
        
        #endregion
    }
}

