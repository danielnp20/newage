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
    /// Models DTO_DetalleFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ChequesGirados
    {
        #region DTO_DetalleFactura

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ChequesGirados(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Nit"].ToString()))
                    this.Nit.Value = (dr["Nit"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Nombre"].ToString()))
                    this.Nombre.Value = (dr["Nombre"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGirado"].ToString()))
                    this.VlrGirado.Value = Convert.ToDecimal(dr["VlrGirado"]);
                if (!string.IsNullOrWhiteSpace(dr["numeroDoc"].ToString()))
                    this.NumDoc.Value = Convert.ToInt32(dr["numeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NumCheque"].ToString()))
                    this.NumCheque.Value = Convert.ToInt32(dr["NumCheque"]);
                if (!string.IsNullOrWhiteSpace(dr["BancoCuentaId"].ToString()))
                    this.BancoCuentaId.Value = (dr["BancoCuentaId"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["LugarGeograficoId"].ToString()))
                    this.LugarGeograficoId.Value = (dr["LugarGeograficoId"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = (dr["ComprobanteID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteIDNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteIDNro"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ChequesGirados()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Nit = new UDTSQL_char(50);
            this.Nombre = new UDT_Descriptivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.VlrGirado = new UDT_Valor();
            this.NumDoc = new UDTSQL_int();
            this.NumCheque = new UDTSQL_int();
            this.NumFactura = new UDTSQL_int();
            this.CuentaNumero = new UDTSQL_char(20);
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
        public UDTSQL_int NumCheque { get; set; }

        [DataMember]
        public UDTSQL_int NumFactura { get; set; }

        [DataMember]
        public UDTSQL_char Nit { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Valor VlrGirado { get; set; }

        [DataMember]
        public List<DTO_ChequesGiradosDeta> Detalle { get; set; }

        [DataMember]
        public UDTSQL_int NumDoc { get; set; }

        [DataMember]
        public UDTSQL_char CuentaNumero { get; set; }

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
