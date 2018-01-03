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
    /// Models DTO_ccFinanciera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccFinanciera : DTO_MasterBasic
    {
        #region DTO_ccFinanciera
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccFinanciera(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica) this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                if (!isReplica) this.CooperativaDesc.Value = dr["CooperativaDesc"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Cooperativa.Value = dr["Cooperativa"].ToString();
                this.TipoEmpresa.Value = Convert.ToByte(dr["TipoEmpresa"]);
                this.PazySalvoInd.Value = Convert.ToBoolean(dr["PazySalvoInd"]);

            }
            catch (Exception e)
            {

                throw e;
            }


        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccFinanciera()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.Cooperativa = new UDT_BasicID();
            this.CooperativaDesc = new UDT_Descriptivo();
            this.TipoEmpresa = new UDTSQL_tinyint();
            this.PazySalvoInd = new UDT_SiNo();
        }

        public DTO_ccFinanciera(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccFinanciera(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID Cooperativa { get; set; }

        [DataMember]
        public UDT_Descriptivo CooperativaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEmpresa { get; set; }

        [DataMember]
        public UDT_SiNo PazySalvoInd { get; set; }
    }
}

