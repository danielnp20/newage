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
    public class DTO_noAportesVoluntariosPension
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noAportesVoluntariosPension(IDataReader dr)
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
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noAportesVoluntariosPension()
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
            this.Valor = new UDT_Valor();
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
        public UDT_Valor Valor { get; set; }
    }
}
