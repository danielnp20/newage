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
    /// Clase del Reporte Saldos Documentos
    /// </summary>
    public class DTO_ReportSaldosDocumentos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportSaldosDocumentos(IDataReader dr)
        {
            Fecha = Convert.ToDateTime(dr["Fecha"]);
            DocSaldoControl = Convert.ToInt32(dr["DocSaldoControl"]);
            DocumentoPrefijo = dr["DocumentoPrefijo"].ToString();
            DocumentoNumero_order = Convert.ToInt32(dr["DocumentoNumero"]);
            DocumentoNumero = dr["DocumentoNumero"].ToString();
            DocumentoTercero = dr["DocumentoTercero"].ToString();
            DocumentoDesc = dr["DocumentoDesc"].ToString();           
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();  
            FinalML = Convert.ToDecimal(dr["FinalML"]);
            FinalME = Convert.ToDecimal(dr["FinalME"]);
        }

        #region Propiedades
        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }
        
        /// <summary>
        /// Concepto Saldo del documento
        /// </summary>
        [DataMember]
        public int DocSaldoControl { get; set; }

        /// <summary>
        /// Prefijo del documento
        /// </summary>
        [DataMember]
        public string DocumentoPrefijo { get; set; }

        /// <summary>
        /// Numero del documento (int)
        /// </summary>
        [DataMember]
        public int DocumentoNumero_order { get; set; }

        /// <summary>
        /// Numero del documento (String)
        /// </summary>
        [DataMember]
        public string DocumentoNumero { get; set; }

        /// <summary>
        /// Documento Tercero
        /// </summary>
        [DataMember]
        public string DocumentoTercero { get; set; }

        /// <summary>
        /// Descripcion del Documento
        /// </summary>
        [DataMember]
        public string DocumentoDesc { get; set; }

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
        /// Saldo Final (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal FinalML { get; set; }

        /// <summary>
        /// Saldo Final (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal FinalME { get; set; }

        #endregion
    }
}