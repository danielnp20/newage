using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Programacion Pagos
    /// </summary>
    public class DTO_ReportProgramacionPagos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportProgramacionPagos(IDataReader dr)
        {
            BancoCuentaID = dr["BancoCuentaID"].ToString();
            BancoCuentaDesc = dr["BancoCuentaDesc"].ToString();
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();
            Factura = dr["Factura"].ToString();
            Concepto = dr["Concepto"].ToString();
            if (!string.IsNullOrEmpty(dr["FacturaFecha"].ToString()))
                FacturaFecha = Convert.ToDateTime(dr["FacturaFecha"]);
            MonedaID = dr["MonedaID"].ToString();
            if (!string.IsNullOrEmpty(dr["ValorPago"].ToString()))
                ValorPago = Convert.ToDecimal(dr["ValorPago"]);
        }
        
        #region Propiedades
       
        /// <summary>
        /// BancoCuenta ID
        /// </summary>
        [DataMember]
        public string BancoCuentaID { get; set; }

        /// <summary>
        /// Descriptivo de BancoCuenta
        /// </summary>
        [DataMember]
        public string BancoCuentaDesc { get; set; }

        /// <summary>
        /// Proveedor ID
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Nombre de Proveedor
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Numero de la Factura
        /// </summary>
        [DataMember]
        public string Factura { get; set; }

        /// <summary>
        /// Concepto de la Factura
        /// </summary>
        [DataMember]
        public string Concepto { get; set; }

        /// <summary>
        /// Fecha de la Factura
        /// </summary>
        [DataMember]
        public DateTime FacturaFecha { get; set; }

        /// <summary>
        /// Moneda codigo de la Factura
        /// </summary>
        [DataMember]
        public string MonedaID { get; set; }

        /// <summary>
        /// Valor de pago
        /// </summary>
        [DataMember]
        public decimal ValorPago { get; set; }

        #endregion

    }
}
