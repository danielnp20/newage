using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noAportesPension
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noAportesPension(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["FondoNOID"].ToString()))
                    this.FondoNOID.Value =dr["FondoNOID"].ToString();
                if (!string.IsNullOrEmpty(dr["FondoDesc"].ToString()))
                    this.FondoDesc.Value = dr["FondoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Cedula"].ToString()))
                    this.Cedula.Value = dr["Cedula"].ToString();
                if (!string.IsNullOrEmpty(dr["EmpleadoDesc"].ToString()))
                    this.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Base"].ToString()))
                    this.Base.Value = Convert.ToDecimal(dr["Base"]);
                if (!string.IsNullOrEmpty(dr["AporteEmpresa"].ToString()))
                    this.AporteEmpresa.Value = Convert.ToDecimal(dr["AporteEmpresa"]);
                if (!string.IsNullOrEmpty(dr["AporteEmpleado"].ToString()))
                    this.AporteEmpleado.Value = Convert.ToDecimal(dr["AporteEmpleado"]);
                if (!string.IsNullOrEmpty(dr["AporteSolidaridad"].ToString()))
                    this.AporteSolidaridad.Value = Convert.ToDecimal(dr["AporteSolidaridad"]);
                if (!string.IsNullOrEmpty(dr["TotalAporte"].ToString()))
                    this.TotalAporte.Value = Convert.ToDecimal(dr["TotalAporte"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noAportesPension()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FondoNOID = new UDT_FondoNOID();
            this.FondoDesc = new UDT_Descriptivo();
            this.Cedula = new UDT_EmpleadoID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.Base = new UDT_Valor();
            this.AporteEmpresa = new UDT_Valor();
            this.AporteEmpleado = new UDT_Valor();
            this.AporteSolidaridad = new UDT_Valor();
            this.TotalAporte = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_FondoNOID FondoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo FondoDesc { get; set; }

        [DataMember]
        public UDT_EmpleadoID Cedula { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public UDT_Valor AporteEmpresa { get; set; }

        [DataMember]
        public UDT_Valor AporteEmpleado { get; set; }

        [DataMember]
        public UDT_Valor AporteSolidaridad { get; set; }

        [DataMember]
        public UDT_Valor TotalAporte { get; set; }
    }
}
