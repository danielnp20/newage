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
    public class DTO_ccAportesCliente
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccAportesCliente(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["ComponenteCarteraID"].ToString()))
                    this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoMLoc"].ToString()))
                    this.SaldoMLoc.Value = Convert.ToDecimal(dr["SaldoMLoc"]);
                if (!string.IsNullOrEmpty(dr["Ingreso"].ToString()))
                    this.Ingreso.Value = Convert.ToDecimal(dr["Ingreso"]);
                if (!string.IsNullOrEmpty(dr["Retiro"].ToString()))
                    this.Retiro.Value = Convert.ToDecimal(dr["Retiro"]);
                if (!string.IsNullOrEmpty(dr["NuevoSaldo"].ToString()))
                    this.NuevoSaldo.Value = Convert.ToDecimal(dr["NuevoSaldo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccAportesCliente()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.ClienteID = new UDT_ClienteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.SaldoMLoc = new UDT_Valor();
            this.Ingreso = new UDT_Valor();
            this.Retiro = new UDT_Valor();
            this.NuevoSaldo = new UDT_Valor();
        }
        #endregion
        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor SaldoMLoc { get; set; }

        [DataMember]
        public UDT_Valor Ingreso { get; set; }

        [DataMember]
        public UDT_Valor Retiro { get; set; }

        [DataMember]
        public UDT_Valor NuevoSaldo { get; set; }
    }
}
