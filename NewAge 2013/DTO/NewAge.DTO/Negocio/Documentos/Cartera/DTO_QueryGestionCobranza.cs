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
    /// Class Models DTO_prOrdenCompraAprobDet
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryGestionCobranza
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryGestionCobranza(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"].ToString());
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuotaID"].ToString()))
                    this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaCuota"].ToString()))
                    this.FechaCuota.Value = Convert.ToDateTime(dr["FechaCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAct"].ToString()))
                    this.FechaAct.Value = Convert.ToDateTime(dr["FechaAct"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSaldoCuota"].ToString()))
                    this.VlrSaldoCuota.Value = Convert.ToDecimal(dr["VlrSaldoCuota"]);               
                this.EtapaID.Value = dr["EtapaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoEstado"].ToString()))
                    this.TipoEstado.Value = Convert.ToByte(dr["TipoEstado"]);
                this.CobranzaGestionCierre.Value = dr["CobranzaGestionCierre"].ToString();
     
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryGestionCobranza()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.CuotaID = new UDT_CuotaID();
            this.FechaCuota = new UDTSQL_smalldatetime();
            this.FechaAct = new UDTSQL_smalldatetime();
            this.VlrSaldoCuota = new UDT_Valor();
            this.EtapaID = new UDT_CodigoGrl10();
            this.EtapaDesc = new UDT_Descriptivo();
            this.TipoEstado = new UDTSQL_tinyint();
            this.CobranzaGestionCierre = new UDT_CodigoGrl10();
        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAct { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoCuota { get; set; }  

        [DataMember]
        public UDT_CodigoGrl10 EtapaID { get; set; }

        [DataMember]
        public UDT_Descriptivo EtapaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestionCierre { get; set; }

        #endregion
    }
}
