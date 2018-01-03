using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Reporte Facturas Por Pagar
    /// </summary>
    public class DTO_ReportFacturasPorPagar : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportFacturasPorPagar(IDataReader dr)
        {
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();
            Documento = dr["Documento"].ToString();
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();
            FechaDoc = Convert.ToDateTime(dr["FechaDoc"]);
            FechaVto = (string.IsNullOrEmpty(dr["VtoFecha"].ToString().Trim()))? new DateTime(): Convert.ToDateTime(dr["VtoFecha"]);
            ComprobanteID = dr["ComprobanteID"].ToString();
            ComprobanteNro = dr["ComprobanteIDNro"].ToString();
            MonedaID = dr["MonedaID"].ToString().Trim();
            TasaCambio = (dr["TasaCambio"] == null) ? 0 : Convert.ToDecimal(dr["TasaCambio"]);
            VrBrutoML = Convert.ToDecimal(dr["VrBrutoML"]);
            VrNetoML = (!string.IsNullOrEmpty(dr["VrNetoML"].ToString())) ? Convert.ToDecimal(dr["VrNetoML"]) : 0;
            VrNetoME = (!string.IsNullOrEmpty(dr["VrNetoME"].ToString())) ? Convert.ToDecimal(dr["VrNetoME"]) : 0;
            SaldoTotalML = Convert.ToDecimal(dr["SaldoTotalML"]);
            SaldoTotalME = Convert.ToDecimal(dr["SaldoTotalME"]);
            SaldoTotalML_sinSigno = Convert.ToDecimal(dr["SaldoTotalML_sinSigno"]);
            SaldoTotalME_sinSigno = Convert.ToDecimal(dr["SaldoTotalME_sinSigno"]);
            AbonosML = VrNetoML - SaldoTotalML;
            AbonosME = VrNetoME - SaldoTotalME;
        }

        #region Propiedades

        /// <summary>
        /// Tercero ID
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }
        
        /// <summary>
        /// Documento ID
        /// </summary>
        [DataMember]
        public string Documento { get; set; }

        /// <summary>
        /// Cuenta ID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// Descripcion de la Cuenta
        /// </summary>
        [DataMember]
        public string CuentaDesc { get; set; }

        /// <summary>
        /// Factura Fecha
        /// </summary>
        [DataMember]
        public DateTime FechaDoc { get; set; }
        
        /// <summary>
        /// Factura Fecha
        /// </summary>
        [DataMember]
        public DateTime FechaVto { get; set; }

        /// <summary>
        /// Comprobante ID
        /// </summary>
        [DataMember]
        public string ComprobanteID { get; set; }

        /// <summary>
        /// Numero del Comprobante
        /// </summary>
        [DataMember]
        public string ComprobanteNro { get; set; }

        /// <summary>
        /// Numero del Comprobante
        /// </summary>
        [DataMember]
        public string MonedaID { get; set; }

        /// <summary>
        /// Tasa de Cambio
        /// </summary>
        [DataMember]
        public decimal TasaCambio { get; set; }

        /// <summary>
        /// Saldo Inicial
        /// </summary>
        [DataMember]
        public decimal VrBrutoML { get; set; }

        /// <summary>
        /// Saldo Inicial (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal VrNetoML { get; set; }

        /// <summary>
        /// Abonos (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal AbonosML { get; set; }
        
        /// <summary>
        /// Saldo Actual (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal SaldoTotalML { get; set; }

        /// <summary>
        /// Saldo Actual (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal SaldoTotalML_sinSigno { get; set; }

        /// <summary>
        /// Saldo InicialML (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal VrNetoME { get; set; }

        /// <summary>
        /// Abonos (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal AbonosME { get; set; }
        
        /// <summary>
        /// Saldo Actual (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal SaldoTotalME { get; set; }

        /// <summary>
        /// Saldo Actual (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal SaldoTotalME_sinSigno { get; set; }

        ///// <summary>
        ///// Saldo Inicial Moneda Local) - string
        ///// </summary>
        //[DataMember]
        //public string SaldoInicialML_str { get; set; }

        ///// <summary>
        ///// Abonos (Moneda Local) - string
        ///// </summary>
        //[DataMember]
        //public string AbonosML_str { get; set; }

        ///// <summary>
        ///// Saldo InicialML (Moneda Extranjera) - string
        ///// </summary>
        //[DataMember]
        //public string SaldoInicialME_str { get; set; }

        ///// <summary>
        ///// Abonos (Moneda Extranjera) - string
        ///// </summary>
        //[DataMember]
        //public string AbonosME_str { get; set; }

        #endregion
    }
}
