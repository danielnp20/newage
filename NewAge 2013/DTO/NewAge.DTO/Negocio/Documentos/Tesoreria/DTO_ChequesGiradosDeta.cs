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
    public class DTO_ChequesGiradosDeta
    {
        #region DTO_ChequesGiradosDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ChequesGiradosDeta(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["NroFactura"].ToString()))
                    this.NumeroFactura.Value = (dr["NroFactura"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGirado"].ToString()))
                    this.VlrPagado.Value = Convert.ToDecimal(dr["VlrGirado"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = (dr["Observacion"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = (dr["ComprobanteID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ChequesGiradosDeta()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroFactura = new UDTSQL_char(50);
            this.Fecha = new UDTSQL_datetime();
            this.VlrPagado = new UDT_Valor();
            this.Observacion = new UDT_Descriptivo();
            this.Descriptivo = new UDT_Descriptivo();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_char NumeroFactura { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }
        
        [DataMember]
        public UDT_Valor VlrPagado { get; set; }

        [DataMember]
        public UDT_Descriptivo Observacion { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        #endregion
    }
}
