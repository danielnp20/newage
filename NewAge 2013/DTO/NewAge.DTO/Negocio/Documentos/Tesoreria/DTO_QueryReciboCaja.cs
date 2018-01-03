using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pago Facturas:
    /// Models DTO_QueryReciboCaja
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryReciboCaja
    {
        #region DTO_DetalleFactura

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_QueryReciboCaja(IDataReader dr)
        {
            this.InitCols();
            try
            {

                if (!string.IsNullOrWhiteSpace(dr["CajaID"].ToString()))
                    this.CajaID.Value = (dr["CajaID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["PrefDoc"].ToString()))
                    this.PrefDoc.Value = (dr["PrefDoc"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = (dr["TerceroID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Nombre"].ToString()))
                    this.Nombre.Value = (dr["Nombre"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NumReciboCaja"].ToString()))
                    this.NumReciboCaja.Value = Convert.ToInt32(dr["NumReciboCaja"]);
                if (!string.IsNullOrWhiteSpace(dr["BancoCuentaId"].ToString()))
                    this.BancoCuentaId.Value = (dr["BancoCuentaId"]).ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryReciboCaja()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CajaID = new UDT_CajaID();
            this.TerceroID = new UDTSQL_char(50);
            this.Nombre = new UDT_Descriptivo();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.NumeroDoc = new UDTSQL_int();
            this.NumReciboCaja = new UDTSQL_int();
            this.PrefDoc = new UDT_DescripTBase ();
            this.Descriptivo = new UDT_Descriptivo();
            this.LugarGeografico = new UDT_Descriptivo();
            this.Direccion = new UDT_Descriptivo();
            this.BancoCuentaId = new UDT_BancoCuentaID();
            this.LugarGeograficoId = new UDT_LugarGeograficoID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();

            this.FechaIni = new DateTime();
            this.FechaFin = new DateTime();
        }

        #endregion

        #region Propiedades

        //Grilla
        [DataMember]
        public UDTSQL_int NumReciboCaja { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDTSQL_char TerceroID { get; set; }

        [DataMember]
        public UDT_CajaID CajaID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_int NumeroDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaId { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID LugarGeograficoId { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        //Campos Extra

        [DataMember]
        public UDT_Descriptivo LugarGeografico { get; set; }
        
        [DataMember]
        public UDT_Descriptivo Direccion { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }

        #endregion
    }
}
