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
    /// Models DTO_ocContratoCampo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocContratoCampo : DTO_MasterComplex
    {
        #region ocContratoCampo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocContratoCampo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ContratoDesc.Value = dr["ContratoDesc"].ToString();
                    this.CampoDesc.Value = dr["CampoDesc"].ToString();
                }
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.Campo.Value = dr["Campo"].ToString();
                this.TipoParticionCapex.Value = Convert.ToByte(dr["TipoParticionCapex"]);
                this.TipoParticionOpex.Value = Convert.ToByte(dr["TipoParticionOpex"]);
                this.TipoParticionInversion.Value = Convert.ToByte(dr["TipoParticionInversion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocContratoCampo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ContratoID = new UDT_BasicID();
            this.ContratoDesc = new UDT_Descriptivo();
            this.Campo = new UDT_BasicID();
            this.CampoDesc = new UDT_Descriptivo();
            this.TipoParticionCapex = new UDTSQL_tinyint();
            this.TipoParticionOpex = new UDTSQL_tinyint();
            this.TipoParticionInversion = new UDTSQL_tinyint();
        }

        public DTO_ocContratoCampo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocContratoCampo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ContratoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ContratoDesc { get; set; }

        [DataMember]
        public UDT_BasicID Campo { get; set; }

        [DataMember]
        public UDT_Descriptivo CampoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoParticionCapex { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoParticionOpex { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoParticionInversion { get; set; }
    }
}
