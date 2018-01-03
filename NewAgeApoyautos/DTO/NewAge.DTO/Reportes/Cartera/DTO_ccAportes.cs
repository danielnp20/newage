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
    public class DTO_ccAportes
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccAportes(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["ClienteId"].ToString()))
                    this.ClienteId.Value = dr["ClienteId"].ToString();
                if (!string.IsNullOrEmpty(dr["EmpleadoDesc"].ToString()))
                    this.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["ResidenciaDir"].ToString()))
                    this.ResidenciaDir.Value = dr["ResidenciaDir"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaIngreso"].ToString()))
                    this.FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]);
                if (!string.IsNullOrEmpty(dr["LugarGeograficoId"].ToString()))
                    this.LugarGeograficoId.Value = dr["LugarGeograficoId"].ToString();
                if (!string.IsNullOrEmpty(dr["Pagaduria"].ToString()))
                    this.Pagaduria.Value = dr["Pagaduria"].ToString();
                if (!string.IsNullOrEmpty(dr["CuotaValor"].ToString()))
                    this.CuotaValor.Value = Convert.ToDecimal(dr["CuotaValor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccAportes()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClienteId = new UDT_ClienteID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.ResidenciaDir = new UDTSQL_char(50);
            this.FechaIngreso = new DateTime();
            this.LugarGeograficoId = new UDT_LugarGeograficoID();
            this.Pagaduria = new UDT_Descriptivo();
            this.CuotaValor = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_ClienteID ClienteId { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDTSQL_char ResidenciaDir { get; set; }

        [DataMember]
        public DateTime FechaIngreso { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID LugarGeograficoId { get; set; }

        [DataMember]
        public UDT_Descriptivo Pagaduria { get; set; }

        [DataMember]
        public UDT_Valor CuotaValor { get; set; }
    }
}
