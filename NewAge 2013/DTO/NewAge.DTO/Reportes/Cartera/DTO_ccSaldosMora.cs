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
    public class DTO_ccSaldosMora
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSaldosMora(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["NroCuotasVencidas"].ToString()))
                    this.NroCuotasVencidas.Value = Convert.ToDecimal(dr["NroCuotasVencidas"]);
                if (!string.IsNullOrEmpty(dr["SaldoMoraTotal"].ToString()))
                    this.SaldoMoraTotal.Value = Convert.ToDecimal(dr["SaldoMoraTotal"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSaldosMora()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.CompradorCarteraID = new UDTSQL_char(5);
            this.moraComponente = new UDT_Valor();
            this.NroCuotasVencidas = new UDT_Valor();
            this.SaldoMoraTotal = new UDT_Valor();
         }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDTSQL_char CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Valor moraComponente { get; set; }

        [DataMember]
        public UDT_Valor NroCuotasVencidas { get; set; }

        [DataMember]
        public UDT_Valor SaldoMoraTotal { get; set; }

        #endregion
    }
}
