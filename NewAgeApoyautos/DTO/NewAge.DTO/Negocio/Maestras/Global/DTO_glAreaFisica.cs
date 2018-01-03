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
    /// Models DTO_glConceptoSaldo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glAreaFisica : DTO_MasterBasic
    {
        #region DTO_glAreaFisica

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glAreaFisica(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.UnidadGenEfectivoDesc.Value = dr["UnidadGenEfectivoDesc"].ToString();
                    this.BloqueDesc.Value = Convert.ToString(dr["BloqueDesc"]);
                    this.CentroCostoDesc.Value = Convert.ToString(dr["CentroCostoDesc"]);
                    this.ContratoDesc.Value = Convert.ToString(dr["ContratoDesc"]);
                }

                this.UnidGenEfectID.Value = dr["UnidGenEfectID"].ToString();
                this.TipoArea.Value = Convert.ToByte(dr["TipoArea"]);
                this.BloqueID.Value = dr["BloqueID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.Sigla.Value = dr["Sigla"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glAreaFisica()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.UnidGenEfectID = new UDT_BasicID();
            this.UnidadGenEfectivoDesc = new UDT_Descriptivo();
            this.TipoArea = new UDTSQL_smallint();
            this.BloqueID = new UDT_BasicID();
            this.BloqueDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.ContratoID = new UDT_BasicID();
            this.ContratoDesc = new UDT_Descriptivo();
            this.Sigla = new UDT_CodigoGrl5();
        }

        public DTO_glAreaFisica(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glAreaFisica(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        [DataMember]
        public UDT_BasicID UnidGenEfectID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadGenEfectivoDesc { get; set; }

        [DataMember]
        public UDTSQL_smallint TipoArea { get; set; }

        [DataMember]
        public UDT_BasicID BloqueID { get; set; }

        [DataMember]
        public UDT_Descriptivo BloqueDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ContratoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ContratoDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 Sigla { get; set; }
    }
}
