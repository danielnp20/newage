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
    public class DTO_ReportFacturasPagadas
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportFacturasPagadas(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Factura"].ToString()))
                    this.Factura.Value = dr["Factura"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaPago"].ToString()))
                    this.FechaPago.Value = Convert.ToDateTime(dr["FechaPago"]);
                this.Comprobante.Value = dr["Comprobante"].ToString();
                this.Banco.Value = dr["Banco"].ToString();
                this.CuentaBanco.Value = dr["CuentaBanco"].ToString();
                this.ChequeNro.Value = dr["ChequeNro"].ToString();
                if (!string.IsNullOrEmpty(dr["ValorFactura"].ToString()))
                    this.ValorFactura.Value = Convert.ToDecimal(dr["ValorFactura"]);
                if (!string.IsNullOrEmpty(dr["ValorPago"].ToString()))
                    this.ValorPago.Value = Convert.ToDecimal(dr["ValorPago"]);
                if (!string.IsNullOrEmpty(dr["SaldoTotal"].ToString()))
                    this.SaldoTotal.Value = Convert.ToDecimal(dr["SaldoTotal"]);
                this.TerceroId.Value = dr["TerceroId"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportFacturasPagadas()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Factura = new UDTSQL_char(20);
            this.FechaPago = new UDTSQL_datetime();
            this.Comprobante = new UDTSQL_char(15);
            this.Banco = new UDT_BancoCuentaID();
            this.ChequeNro = new UDTSQL_char(20);
            this.CuentaBanco = new UDT_CuentaID();
            this.ValorFactura = new UDT_Valor();
            this.ValorPago = new UDT_Valor();
            this.SaldoTotal = new UDT_Valor();
            this.TerceroId = new UDT_TerceroID();
            this.Descriptivo = new UDT_Descriptivo();
        }
        #endregion

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaPago { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDT_BancoCuentaID Banco { get; set; }

        [DataMember]
        public UDTSQL_char ChequeNro { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaBanco { get; set; }

        [DataMember]
        public UDT_Valor ValorFactura { get; set; }

        [DataMember]
        public UDT_Valor ValorPago { get; set; }

        [DataMember]
        public UDT_Valor SaldoTotal { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroId { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }
    }
}
