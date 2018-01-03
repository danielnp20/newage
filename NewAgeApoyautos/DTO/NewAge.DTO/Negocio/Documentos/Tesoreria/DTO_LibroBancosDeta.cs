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
    public class DTO_LibroBancosDeta
    {
        #region DTO_DetalleFactura

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_LibroBancosDeta(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Comprobante"].ToString()))
                    this.Comprobante.Value = (dr["Comprobante"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoCom"].ToString()))
                    this.DocumentoCom.Value = (dr["DocumentoCom"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = (dr["Descriptivo"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["vlrMdaLoc"].ToString()))
                    this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                if (!string.IsNullOrWhiteSpace(dr["vlrMdaExt"].ToString()))
                    this.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_LibroBancosDeta()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Comprobante = new UDTSQL_char(50);
            this.Fecha = new UDTSQL_datetime();
            this.DocumentoCom = new UDTSQL_char(20);
            this.Descriptivo = new UDT_Descriptivo();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoCom { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaExt { get; set; }

        #endregion
    }
}
