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
    /// Clase del documento Recibo Caja
    /// </summary>
    public class DTO_ReportReciboCaja : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportReciboCaja(IDataReader dr)
        {
            try
            {
                CajaID = dr["CajaID"].ToString();
                CajaDesc = dr["CajaDesc"].ToString();
                ReciboNro = dr["ReciboNro"].ToString().Trim();
                DocumentoDesc = dr["DocumentoDesc"].ToString().Trim();
                TerceroID = dr["TerceroID"].ToString().Trim();
                TerceroDesc = dr["TerceroDesc"].ToString().Trim();
                ComprobanteID = dr["ComprobanteID"].ToString().Trim();
                ComprobanteNro = dr["ComprobanteNro"].ToString().Trim();
                Fecha = Convert.ToDateTime(dr["Fecha"]);
                Valor = Convert.ToDecimal(dr["Valor"]);
                MonedaID = dr["MonedaID"].ToString();
                Usuario = dr["Usuario"] == null ? string.Empty : dr["Usuario"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DTO_ReportReciboCaja()
        {
            this.ReciboDetail = new List<DTO_ReciboDetail>();
        }

        #region Propiedades

        /// <summary>
        /// CajaID
        /// </summary>
        [DataMember]
        public string CajaID { get; set; }

        /// <summary>
        /// Caja Desccriptivo
        /// </summary>
        [DataMember]
        public string CajaDesc { get; set; }

        /// <summary>
        /// Numero del Recibo
        /// </summary>
        [DataMember]
        public string ReciboNro { get; set; }

        /// <summary>
        /// Descripcion del Documento
        /// </summary>
        [DataMember]
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Nit del Tercero (TerceroID)
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string ComprobanteID { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string ComprobanteNro { get; set; }

        /// <summary>
        /// Fecha del Documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Fecha de  consignacion
        /// </summary>
        [DataMember]
        public DateTime FechaConsigna { get; set; }

        /// <summary>
        /// Valor del Documento
        /// </summary>
        [DataMember]
        public decimal Valor { get; set; }

        /// <summary>
        /// Valor del Documento (en letras)
        /// </summary>
        [DataMember]
        public string Valor_letters { get; set; }

        /// <summary>
        /// Moneda ID
        /// </summary>
        [DataMember]
        public string MonedaID { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [DataMember]
        public string Usuario { get; set; }

        /// <summary>
        /// Detalle del Recibo
        /// </summary>
        [DataMember]
        public List<DTO_ReciboDetail> ReciboDetail { get; set; }

        /// <summary>
        /// Footer del Recibo
        /// </summary>
        //[DataMember]
        //public List<DTO_ReciboFooter> ReciboFooter { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    
    /// <summary>
    /// Clase del Detalle del Recibo
    /// </summary>
    public class DTO_ReciboDetail
    {

        #region Propiedades
        /// <summary>
        /// Cuenta ID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// Descripcion Cuenta
        /// </summary>
        [DataMember]
        public string CuentaDesc { get; set; }

        /// <summary>
        /// Tercero ID
        /// </summary>
        [DataMember]
        public string TerceroID_cuenta { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Descripcion del documento
        /// </summary>
        [DataMember]
        public string DocumentoCOM { get; set; }

        /// <summary>
        /// Moneda Transacc
        /// </summary>
        [DataMember]
        public decimal ValorML_cuenta { get; set; }
        #endregion

        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReciboDetail(IDataReader dr)
        {
            CuentaID = dr["CuentaID"].ToString().Trim();
            CuentaDesc = dr["CuentaDesc"].ToString().Trim();
            TerceroID_cuenta = dr["TerceroID_cuenta"].ToString().Trim();
            ValorML_cuenta = Convert.ToDecimal(dr["ValorML_cuenta"]);            
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReciboDetail()
        {
        }

    }

    /* ReciboCaja Footer
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Footer del Recibo
    /// </summary>
    public class DTO_ReciboFooter
    {

        #region Propiedades
        /// <summary>
        /// Tipo del pago
        /// </summary>
        [DataMember]
        public string TipoPago { get; set; }

        /// <summary>
        /// Banco del pago
        /// </summary>
        [DataMember]
        public string BancoPago { get; set; }

        /// <summary>
        /// Documento del pago
        /// </summary>
        [DataMember]
        public string DocPAgo { get; set; }

        /// <summary>
        /// Codigo del pago
        /// </summary>
        [DataMember]
        public decimal CodigoPago { get; set; }
        #endregion

        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReciboFooter(IDataReader dr)
        {
            TipoPago = dr["TipoPago"].ToString().Trim();
            BancoPago = dr["BancoPago"].ToString().Trim();
            DocPAgo = dr["DocPAgo"].ToString().Trim();
            CodigoPago = Convert.ToDecimal(dr["CodigoPago"]);
          }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReciboFooter()
        {
        }    
    }
     */
}
