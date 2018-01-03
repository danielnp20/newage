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
    /// Models DTO_ccPagaduriaPrograma
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccPagaduriaPrograma : DTO_MasterComplex
    {
        #region DTO_ccPagaduriaPrograma
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPagaduriaPrograma(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.PagaduriaDesc.Value = dr["PagaduriaDesc"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["PagaduriaID"].ToString()))
                    this.PagaduriaID.Value = (dr["PagaduriaID"].ToString());
                if (!string.IsNullOrEmpty(dr["PeriodoDoc"].ToString()))
                    this.PeriodoDoc.Value = Convert.ToDateTime(dr["PeriodoDoc"]);
                if (!string.IsNullOrEmpty(dr["FechaEnvioInc"].ToString()))
                    this.FechaEnvioInc.Value = Convert.ToDateTime(dr["FechaEnvioInc"]);
                if (!string.IsNullOrEmpty(dr["EnviadoInd"].ToString()))
                    this.EnviadoInd.Value = Convert.ToBoolean(dr["EnviadoInd"]);
                if (!string.IsNullOrEmpty(dr["FechaHojaVida"].ToString()))
                    this.FechaHojaVida.Value = Convert.ToDateTime(dr["FechaHojaVida"]);
                if (!string.IsNullOrEmpty(dr["HojaVidaIND"].ToString()))
                    this.HojaVidaIND.Value = Convert.ToBoolean(dr["HojaVidaIND"]);
                if (!string.IsNullOrEmpty(dr["FechaInconsistencias"].ToString()))
                    this.FechaInconsistencias.Value = Convert.ToDateTime(dr["FechaInconsistencias"]);
                if (!string.IsNullOrEmpty(dr["InconsistenciasND"].ToString()))
                    this.InconsistenciasND.Value = Convert.ToBoolean(dr["InconsistenciasND"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPagaduriaPrograma() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PagaduriaID = new UDT_BasicID();
            this.PagaduriaDesc = new UDT_Descriptivo();
            this.PeriodoDoc = new UDT_PeriodoID();
            this.FechaEnvioInc = new UDTSQL_smalldatetime();
            this.EnviadoInd = new UDT_SiNo();
            this.FechaHojaVida = new UDTSQL_smalldatetime();
            this.HojaVidaIND = new UDT_SiNo();
            this.FechaInconsistencias = new UDTSQL_smalldatetime();
            this.InconsistenciasND = new UDT_SiNo();
        }

        public DTO_ccPagaduriaPrograma(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccPagaduriaPrograma(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaduriaDesc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaEnvioInc { get; set; }

        [DataMember]
        public UDT_SiNo EnviadoInd { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaHojaVida { get; set; }

        [DataMember]
        public UDT_SiNo HojaVidaIND { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInconsistencias { get; set; }

        [DataMember]
        public UDT_SiNo InconsistenciasND { get; set; }
    }

}
