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
    /// Clase del documento Pago Electronico
    /// </summary>
    public class DTO_ReportPagoElectr : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportPagoElectr()
        {
            this.PagoElectrDetail = new List<DTO_PagoElectrDetail>();
        }

        #region Propiedades

        /// <summary>
        /// Interno ID del documento
        /// </summary>
        [DataMember]
        public int NumeroDoc { get; set; }

        /// <summary>
        /// Fecha del pago
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Numero del pago
        /// </summary>
        [DataMember]
        public string PagoNro { get; set; }
        
        /// <summary>
        /// Detalle del pago (la lista de las facturas)
        /// </summary>
        [DataMember]
        public List<DTO_PagoElectrDetail> PagoElectrDetail { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    
    /// <summary>
    /// Clase del Detalle del Pago Electronico
    /// </summary>
    public class DTO_PagoElectrDetail
    {
        #region Propiedades


        /// <summary>
        /// NIT
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Nombre
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
        //[DataMember]
        //public string BancoCuentaID { get; set; }

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
        public DTO_PagoElectrDetail()
        {
        }
    }

}
