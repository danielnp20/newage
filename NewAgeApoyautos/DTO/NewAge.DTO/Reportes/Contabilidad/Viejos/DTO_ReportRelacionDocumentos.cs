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
    /// Clase del Reporte Relacion Documentos
    /// </summary>
    public class DTO_ReportRelacionDocumentos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportRelacionDocumentos(IDataReader dr)
        {
            Fecha = Convert.ToDateTime(dr["Fecha"]);
            DocSaldoControl = Convert.ToInt32(dr["DocSaldoControl"]);
            DocumentoPrefijo = dr["DocumentoPrefijo"].ToString();
            DocumentoPrefijoDesc = dr["DocumentoPrefijoDesc"].ToString();
            DocumentoNumero_order = Convert.ToInt32(dr["DocumentoNumero"]);
            DocumentoNumero = dr["DocumentoNumero"].ToString();
            DocumentoTercero = dr["DocumentoTercero"].ToString();
            DocumentoDesc = dr["DocumentoDesc"].ToString(); 
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();
            MonedaCodigo = dr["MonedaCodigo"].ToString();
            DocumentoEstado = ((EstadoDocControl)Convert.ToInt32(dr["DocumentoEstado"])).ToString();
            PorMeses = new DateTime(Convert.ToDateTime(dr["Fecha"]).Year,Convert.ToDateTime(dr["Fecha"]).Month,1);
            //CuentaID = dr["CuentaID"].ToString();
            //CuentaDesc = dr["CuentaDesc"].ToString();
        }

        #region Propiedades
        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Define si documento esta interno (0) o externo (1)
        /// </summary>
        [DataMember]
        public int DocSaldoControl { get; set; }

        /// <summary>
        /// Prefijo del documento
        /// </summary>
        [DataMember]
        public string DocumentoPrefijo { get; set; }

        /// <summary>
        /// Descripcion del Prefijo
        /// </summary>
        [DataMember]
        public string DocumentoPrefijoDesc { get; set; }

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
        /// Codigo de la Moneda del Documento
        /// </summary>
        [DataMember]
        public string MonedaCodigo { get; set; }

        /// <summary>
        /// Estado del Documento
        /// </summary>
        [DataMember]
        public string DocumentoEstado { get; set; }

        /// <summary>
        /// Fecha del documento (año y mes)
        /// </summary>
        [DataMember]
        public DateTime PorMeses { get; set; }


        //[DataMember]
        //public string CuentaID { get; set; }

        //[DataMember]
        //public string CuentaDesc { get; set; }
        #endregion
    }
}