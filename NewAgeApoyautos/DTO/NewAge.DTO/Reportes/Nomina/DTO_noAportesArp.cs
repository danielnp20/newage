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
    public class DTO_noAportesArp
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noAportesArp(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Cedula"].ToString()))
                    this.Cedula.Value = dr["Cedula"].ToString();
                if (!string.IsNullOrEmpty(dr["EmpleadoDesc"].ToString()))
                    this.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Base"].ToString()))
                    this.Base.Value = Convert.ToDecimal(dr["Base"]);
                if (!string.IsNullOrEmpty(dr["ValorArp"].ToString()))
                    this.ValorArp.Value = Convert.ToDecimal(dr["ValorArp"]);
                if (!string.IsNullOrEmpty(dr["Tarifa"].ToString()))
                    this.Tarifa.Value = Convert.ToDecimal(dr["Tarifa"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noAportesArp()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Cedula = new UDT_EmpleadoID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.Base = new UDT_Valor();
            this.ValorArp = new UDT_Valor();
            this.Tarifa = new UDT_PorcentajeID();
            this.FechaIni = new DateTime();
            this.FechaFin = new DateTime();
        }
        #endregion

        [DataMember]
        public UDT_EmpleadoID Cedula { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public UDT_Valor ValorArp { get; set; }

        [DataMember]
        public UDT_PorcentajeID Tarifa { get; set; }

        //Campos Extra
        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }
    }
}
