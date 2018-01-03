using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del documento Pago Facturas
    /// </summary>
    public class DTO_ReportPagoFacturas : DTO_BasicReport
    {
        public DTO_ReportPagoFacturas(IDataReader dr)
        {
            NumeroDoc = Convert.ToInt32(dr["NumeroDoc"]);
        }

        public DTO_ReportPagoFacturas()
        {
            this.PagoFacturasDetail = new List<DTO_PagoFacturasDetail>();
        }

        #region Propiedades

        /// <summary>
        /// Interno ID del documento
        /// </summary>
        [DataMember]
        public int NumeroDoc { get; set; }

        /// <summary>
        /// Concepto del documento
        /// </summary>
        [DataMember]
        public string Documento { get; set; }

        /// <summary>
        /// Ciudad
        /// </summary>
        [DataMember]
        public string Ciudad { get; set; }

        /// <summary>
        /// Fecha del Documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Fecha del Documento
        /// </summary>
        [DataMember]
        public DateTime Periodo { get; set; }

        /// <summary>
        /// Beneficiario Id
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Beneficiario Nombre
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Numero del Cheque
        /// </summary>
        [DataMember]
        public string ChequeNro { get; set; }

        /// <summary>
        /// Banco
        /// </summary>
        [DataMember]
        public string BancoDesc { get; set; }

        /// <summary>
        /// Banco Cuenta
        /// </summary>
        [DataMember]
        public string BancoCuentaID { get; set; }
        
        /// <summary>
        /// Comprobante ID
        /// </summary>
        [DataMember]
        public string ComprobanteID{ get; set; }

        /// <summary>
        /// Comprobante Numero
        /// </summary>
        [DataMember]
        public string ComprobanteNro { get; set; }

        /// <summary>
        /// Valor Cheque
        /// </summary>
        [DataMember]
        public decimal ValorCheque { get; set; }

        /// <summary>
        /// Valor del Cheque (en letras)
        /// </summary>
        [DataMember]
        public string ValorCheque_letters { get; set; }

        /// <summary>
        /// Moneda Cheque
        /// </summary>
        [DataMember]
        public string MonedaCheque { get; set; }

        /// <summary>
        /// Indicador del Estado del documento (aprobado - false)
        /// </summary>
        //[DataMember]
        //public bool EstadoInd { get; set; }

        /// <summary>
        /// Detalle del Recibo
        /// </summary>
        [DataMember]
        public List<DTO_PagoFacturasDetail> PagoFacturasDetail { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    
    /// <summary>
    /// Clase del Detalle de Pago Facturas
    /// </summary>
    public class DTO_PagoFacturasDetail
    {

        #region Propiedades

        /// <summary>
        /// Cuenta ID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// Numero de la factura
        /// </summary>
        [DataMember]
        public string FacturaID { get; set; }
        
        /// <summary>
        /// Concepto de la factura
        /// </summary>
        [DataMember]
        public string FacturaDesc { get; set; }

        /// <summary>
        /// Valor de la Factura
        /// </summary>
        [DataMember]
        public decimal ValorFactura { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PagoFacturasDetail()
        {
        }
    }

}
