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
    /// Class Nomina para el detalle de la Contabilización
    /// Models DTO_NominaContabilizacionDetalle
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NominaContabilizacionDetalle
    {
        #region DTO_NominaContabilizacion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacionDetalle(IDataReader dr)
        {
            this.InitCols();
            this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
            this.Empleado.Value = dr["Empleado"].ToString();
            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
            this.ProyectoID.Value = dr["ProyectoID"].ToString();
            this.ValorDetalle.Value = Convert.ToDecimal(dr["ValorDetalle"]);
            this.Liquidacion.Value = Convert.ToByte(dr["Liquidacion"]);
            
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacionDetalle()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.EmpleadoID = new UDT_EmpleadoID();
            this.Empleado = new UDT_DescripTBase();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ValorDetalle = new UDT_Valor();
            this.Liquidacion = new UDTSQL_tinyint();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Empleado { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }
        
        [DataMember]
        public UDT_Valor ValorDetalle { get; set; }

        [DataMember]
        public UDTSQL_tinyint Liquidacion { get; set; }

        #endregion
    }
}
