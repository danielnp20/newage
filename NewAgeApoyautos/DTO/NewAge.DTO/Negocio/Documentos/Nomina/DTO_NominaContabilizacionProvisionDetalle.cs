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
    /// Models DTO_NominaContabilizacionPlanillaDetalle
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_NominaContabilizacionProvisionDetalle
    {
        #region DTO_NominaContabilizacion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacionProvisionDetalle(IDataReader dr)
        {
            this.InitCols();
            this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
            this.Empleado.Value = dr["Empleado"].ToString();
            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
            this.ProyectoID.Value = dr["ProyectoID"].ToString();
            this.ValorProvision.Value = Convert.ToDecimal(dr["ValorProvision"]);
         }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacionProvisionDetalle()
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
            this.ValorProvision = new UDT_Valor();            
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
        public UDT_Valor ValorProvision { get; set; }
       
        #endregion
    }
}
