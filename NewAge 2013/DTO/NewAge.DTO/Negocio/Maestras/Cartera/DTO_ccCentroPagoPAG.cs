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
    /// Models DTO_ccCentroPagoPAG
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCentroPagoPAG : DTO_MasterBasic
    {
        #region DTO_ccCentroPagoPAG
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCentroPagoPAG(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.PagaduriaDesc.Value = dr["PagaduriaDesc"].ToString();
                }

                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                if (!string.IsNullOrEmpty(dr["PeriodoINC"].ToString()))
                    this.PeriodoINC.Value = Convert.ToByte(dr["PeriodoINC"]);
                if (!string.IsNullOrEmpty(dr["FechaultimaINC"].ToString()))
                    this.FechaultimaINC.Value = Convert.ToDateTime(dr["FechaultimaINC"]);
                if (!string.IsNullOrEmpty(dr["FechaProximaINC"].ToString()))
                    this.FechaProximaINC.Value = Convert.ToDateTime(dr["FechaProximaINC"]);
                if (!string.IsNullOrEmpty(dr["Dia1Inc"].ToString()))
                    this.Dia1Inc.Value = Convert.ToByte(dr["Dia1Inc"]);
                if (!string.IsNullOrEmpty(dr["Dia2Inc"].ToString()))
                    this.Dia2Inc.Value = Convert.ToByte(dr["Dia2Inc"]);
                if (!string.IsNullOrEmpty(dr["CitaInd"].ToString()))
                    this.CitaInd.Value = Convert.ToBoolean(dr["CitaInd"]);
                if (!string.IsNullOrEmpty(dr["DigitoReincorporaIND"].ToString()))
                    this.DigitoReincorporaIND.Value = Convert.ToBoolean(dr["DigitoReincorporaIND"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCentroPagoPAG() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PagaduriaDesc = new UDT_Descriptivo();
            this.PagaduriaID = new UDT_BasicID();
            this.PeriodoINC = new UDTSQL_tinyint();
            this.FechaultimaINC = new UDTSQL_smalldatetime();
            this.FechaProximaINC = new UDTSQL_smalldatetime();
            this.Dia1Inc = new UDTSQL_tinyint();
            this.Dia2Inc = new UDTSQL_tinyint();
            this.CitaInd = new UDT_SiNo();
            this.DigitoReincorporaIND = new UDT_SiNo();
        }

        public DTO_ccCentroPagoPAG(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCentroPagoPAG(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoINC { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaultimaINC { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaProximaINC { get; set; }

        [DataMember]
        public UDTSQL_tinyint Dia1Inc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Dia2Inc { get; set; }

        [DataMember]
        public UDT_SiNo CitaInd { get; set; }

        [DataMember]
        public UDT_SiNo DigitoReincorporaIND { get; set; }

    }
}
