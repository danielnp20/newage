using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noEstadoLiquidaciones
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noEstadoLiquidaciones(IDataReader dr)
        {
            this.InitCols();
            this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
            this.EmpleadoDesc.Value = dr["NombreEmpleado"].ToString();
            this.FechaIniLiq.Value = Convert.ToDateTime(dr["FechaIniLiquidacion"].ToString());
            this.FechaFinLiq.Value = Convert.ToDateTime(dr["FechaFinLiquidacion"].ToString());
            this.EnVacaciones.Value = Convert.ToByte(dr["Vacaciones"].ToString());
            this.EstadoLiqNomina.Value = Convert.ToByte(dr["EstadoNomina"].ToString());
            this.EstadoLiqVacaciones.Value = Convert.ToByte(dr["EstadoVacaciones"].ToString());
            this.EstadoLiqPrima.Value = Convert.ToByte(dr["EstadoPrima"].ToString());
            this.EstadoLiqCesantias.Value = Convert.ToByte(dr["EstadoCesantias"].ToString());
            this.EstadoLiqProvisiones.Value = Convert.ToByte(dr["EstadoProvisiones"].ToString());
            this.EstadoLiqContrato.Value = Convert.ToByte(dr["EstadoContrato"].ToString());
            this.EstadoLiqPlanilla.Value = Convert.ToByte(dr["EstadoPlanilla"].ToString());
            this.EstadoLiqPrestamo.Value = Convert.ToByte(dr["EstadoPrestamo"].ToString());
            this.EstadoEmpleado.Value = Convert.ToByte(dr["EstadoEmpleado"].ToString());
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noEstadoLiquidaciones()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpleadoID = new UDT_EmpleadoID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.EnVacaciones = new UDTSQL_tinyint();
            this.FechaIniLiq = new UDTSQL_smalldatetime();
            this.FechaFinLiq = new UDTSQL_smalldatetime();
            this.EstadoLiqNomina = new UDTSQL_tinyint();
            this.EstadoLiqVacaciones = new UDTSQL_tinyint();
            this.EstadoLiqPrima = new UDTSQL_tinyint();
            this.EstadoLiqCesantias = new UDTSQL_tinyint();
            this.EstadoLiqProvisiones = new UDTSQL_tinyint();
            this.EstadoLiqContrato = new UDTSQL_tinyint();
            this.EstadoLiqPlanilla = new UDTSQL_tinyint();
            this.EstadoLiqPrestamo = new UDTSQL_tinyint();
            this.EstadoEmpleado = new UDTSQL_tinyint();
        }

        #endregion

        #region Members

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaIniLiq { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFinLiq { get; set; }       
        
        [DataMember]
        public UDTSQL_tinyint EstadoEmpleado { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint EnVacaciones { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqNomina { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqVacaciones { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqPrima { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqContrato { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqCesantias { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqProvisiones { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqPlanilla { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoLiqPrestamo { get; set; }


        #endregion
    }
}
