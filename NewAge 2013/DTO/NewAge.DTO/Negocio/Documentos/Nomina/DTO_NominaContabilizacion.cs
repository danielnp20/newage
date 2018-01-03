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
    /// Class Nomina para aprobacion:
    /// Models DTO_NominaAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NominaContabilizacion
    {
        #region DTO_NominaContabilizacion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacion(IDataReader dr)
        {
            this.InitCols();
            this.Liquidacion.Value = Convert.ToByte(dr["Liquidacion"]);
            this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
            this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacion()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Liquidacion = new UDTSQL_tinyint();
            this.Periodo = new UDT_PeriodoID();         
            this.Valor = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_tinyint Liquidacion { get; set; }

        [DataMember]
        public UDT_PeriodoID Periodo { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public List<DTO_NominaContabilizacionDetalle> Detalle { get; set; }

        [DataMember]
        public List<DTO_NominaContabilizacionPlanillaDetalle> Aportes { get; set; }

        [DataMember]
        public List<DTO_NominaContabilizacionProvisionDetalle> Provisiones { get; set; }

        #endregion
    }
}
