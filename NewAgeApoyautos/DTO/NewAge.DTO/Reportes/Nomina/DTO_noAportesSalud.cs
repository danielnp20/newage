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
    public class DTO_noAportesSalud
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noAportesSalud(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["FondoNOID"].ToString()))
                    this.FondoNOID.Value = dr["FondoNOID"].ToString();
                if (!string.IsNullOrEmpty(dr["FondoDesc"].ToString()))
                    this.FondoDesc.Value = dr["FondoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Cedula"].ToString()))
                    this.Cedula.Value = dr["Cedula"].ToString();
                if (!string.IsNullOrEmpty(dr["EmpleadoDesc"].ToString()))
                    this.EmpleadoDesc.Value = dr["EmpleadoDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Base"].ToString()))
                    this.Base.Value = Convert.ToDecimal(dr["Base"]);
                if (!string.IsNullOrEmpty(dr["Empresa"].ToString()))
                    this.Empresa.Value = Convert.ToDecimal(dr["Empresa"]);
                if (!string.IsNullOrEmpty(dr["Trabajador"].ToString()))
                    this.Trabajador.Value = Convert.ToDecimal(dr["Trabajador"]);
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
        public DTO_noAportesSalud()
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
            this.Trabajador = new UDT_Valor();
            this.Empresa = new UDT_Valor();
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
        public UDT_Valor Trabajador { get; set; }

        [DataMember]
        public UDT_Valor Empresa { get; set; }

        [DataMember]
        public UDT_Valor TotalAporte { get; set; }
    }
}
