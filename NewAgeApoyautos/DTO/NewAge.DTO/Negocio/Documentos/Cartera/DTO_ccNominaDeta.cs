using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccNominaDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccNominaDeta
    {
        #region DTO_ccNominaDeta

        public DTO_ccNominaDeta(IDataReader dr, bool isCoperativa=true)
        {
            InitCols();
            try
            {
                this.NumDocNomina.Value = Convert.ToInt32(dr["NumDocNomina"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                this.ValorNomina.Value = Convert.ToDecimal(dr["ValorNomina"]);
                this.ValorCuota.Value = Convert.ToDecimal(dr["ValorCuota"]);
                if(isCoperativa)
                {
                if(!string.IsNullOrWhiteSpace(dr["CentroPagoID"].ToString()))
                    this.CentroPagoID.Value = dr["CentroPagoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumDocIncorpora"].ToString()))
                    this.NumDocIncorpora.Value = Convert.ToInt32(dr["NumDocIncorpora"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIncorpora"].ToString()))
                    this.FechaIncorpora.Value = Convert.ToDateTime(dr["FechaIncorpora"]);
                }
                if (!string.IsNullOrWhiteSpace(dr["FechaNomina"].ToString()))
                    this.FechaNomina.Value = Convert.ToDateTime(dr["FechaNomina"]);
                if (!string.IsNullOrWhiteSpace(dr["EstadoCruce"].ToString()))
                    this.EstadoCruce.Value = Convert.ToByte(dr["EstadoCruce"]);
                if (!string.IsNullOrWhiteSpace(dr["InconsistenciaIncID"].ToString()))
                    this.InconsistenciaIncID.Value = dr["InconsistenciaIncID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["IndInconsistencia"].ToString()))
                    this.IndInconsistencia.Value = Convert.ToBoolean(dr["IndInconsistencia"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccNominaDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumDocNomina = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.CentroPagoID = new UDT_CentroPagoID();
            this.FechaIncorpora = new UDTSQL_smalldatetime();
            this.FechaNomina = new UDTSQL_smalldatetime();
            this.ValorNomina = new UDT_Valor();
            this.ValorCuota = new UDT_Valor();
            this.IndInconsistencia = new UDT_SiNo();
            this.NumDocIncorpora = new UDT_Consecutivo();
            this.EstadoCruce = new UDTSQL_tinyint();
            this.InconsistenciaIncID = new UDT_InconsistenciaIncID();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumDocNomina { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPagoID{ get; set; }

        [DataMember]
        public UDT_Valor ValorNomina { get; set; }

        [DataMember]
        public UDT_Valor ValorCuota { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIncorpora { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaNomina { get; set; }
              
        [DataMember]
        public UDT_Consecutivo NumDocIncorpora{ get; set; }

        [DataMember]
        public UDT_SiNo IndInconsistencia{ get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoCruce{ get; set; }

        [DataMember]
        public UDT_InconsistenciaIncID InconsistenciaIncID{ get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        #endregion
    }
}
