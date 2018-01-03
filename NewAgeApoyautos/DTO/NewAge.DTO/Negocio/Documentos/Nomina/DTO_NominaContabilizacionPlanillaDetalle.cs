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
    public class DTO_NominaContabilizacionPlanillaDetalle
    {
        #region DTO_NominaContabilizacion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacionPlanillaDetalle(IDataReader dr)
        {
            this.InitCols();
            this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
            this.Empleado.Value = dr["Empleado"].ToString();
            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
            this.ProyectoID.Value = dr["ProyectoID"].ToString();
            this.VlrARP.Value = Convert.ToDecimal(dr["VlrARP"]);
            this.VlrCCF.Value = Convert.ToDecimal(dr["VlrCCF"]);
            this.VlrEmpresaPEN.Value = Convert.ToDecimal(dr["VlrEmpresaPEN"]);
            this.VlrEmpresaSLD.Value = Convert.ToDecimal(dr["VlrEmpresaSLD"]);
            this.VlrIBF.Value = Convert.ToDecimal(dr["VlrIBF"]);
            this.VlrSEN.Value = Convert.ToDecimal(dr["VlrSEN"]);
            this.VlrSolidaridad.Value = Convert.ToDecimal(dr["VlrSolidaridad"]);
            this.VlrSubsistencia.Value = Convert.ToDecimal(dr["VlrSubsistencia"]);
            this.VlrTrabajadorPEN.Value = Convert.ToDecimal(dr["VlrTrabajadorPEN"]);
            this.VlrTrabajadorSLD.Value = Convert.ToDecimal(dr["VlrTrabajadorSLD"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_NominaContabilizacionPlanillaDetalle()
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
            this.VlrARP = new UDT_Valor();
            this.VlrCCF = new UDT_Valor();
            this.VlrEmpresaPEN = new UDT_Valor();
            this.VlrEmpresaSLD = new UDT_Valor();
            this.VlrIBF = new UDT_Valor();
            this.VlrSEN = new UDT_Valor();
            this.VlrSolidaridad = new UDT_Valor();
            this.VlrSubsistencia = new UDT_Valor();
            this.VlrTrabajadorPEN = new UDT_Valor();
            this.VlrTrabajadorSLD = new UDT_Valor();
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
        public UDT_Valor VlrEmpresaPEN { get; set; }

        [DataMember]
        public UDT_Valor VlrTrabajadorPEN { get; set; }

        [DataMember]
        public UDT_Valor VlrSolidaridad { get; set; }

        [DataMember]
        public UDT_Valor VlrSubsistencia { get; set; }

        [DataMember]
        public UDT_Valor VlrEmpresaSLD { get; set; }

        [DataMember]
        public UDT_Valor VlrTrabajadorSLD { get; set; }

        [DataMember]
        public UDT_Valor VlrARP { get; set; }

        [DataMember]
        public UDT_Valor VlrCCF { get; set; }

        [DataMember]
        public UDT_Valor VlrSEN { get; set; }

        [DataMember]
        public UDT_Valor VlrIBF { get; set; }

        #endregion
    }
}
