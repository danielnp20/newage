using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportResumidoXEmpleado
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportResumidoXEmpleado
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReportResumidoXEmpleado(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoID"].ToString()))
                    { this.EmpleadoID = dr["EmpleadoID"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["CuentaAbono"].ToString()))
                    { this.CuentaAbono = dr["CuentaAbono"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoDesc"].ToString()))
                    { this.EmpleadoDesc = dr["EmpleadoDesc"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    { this.Valor.Value = Convert.ToDecimal(dr["Valor"]); }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_noReportResumidoXEmpleado(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {

            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReportResumidoXEmpleado()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Valor = new UDT_Valor();
        }
        #endregion
        #region Propiedades

        [DataMember]
        public string EmpleadoID { get; set; }

        [DataMember]
        public string CuentaAbono { get; set; }

        [DataMember]
        public string EmpleadoDesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
        #endregion
    }
}
