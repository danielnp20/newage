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
    /// Models DTO_noConceptoPlaTra
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noConceptoPlaTra : DTO_MasterBasic
    {
        #region DTO_noConceptoPlaTra
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noConceptoPlaTra(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                this.HD.Value = Convert.ToByte(dr["HD"]);
                this.HED.Value = Convert.ToByte(dr["HED"]);
                this.HRN.Value = Convert.ToByte(dr["HRN"]);
                this.HEN.Value = Convert.ToByte(dr["HEN"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noConceptoPlaTra() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Tipo = new UDTSQL_tinyint();
            this.HD = new UDTSQL_tinyint();
            this.HED = new UDTSQL_tinyint();
            this.HRN = new UDTSQL_tinyint();
            this.HEN = new UDTSQL_tinyint();
        }

        public DTO_noConceptoPlaTra(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noConceptoPlaTra(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        public UDTSQL_tinyint HD { get; set; }

        [DataMember]
        public UDTSQL_tinyint HED { get; set; }

        [DataMember]
        public UDTSQL_tinyint HRN { get; set; }

        [DataMember]
        public UDTSQL_tinyint HEN { get; set; }

    }
}


