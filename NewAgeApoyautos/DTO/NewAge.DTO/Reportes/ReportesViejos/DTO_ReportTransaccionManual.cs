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
    // <summary>
    // Clase del reporte Transaccion Manual
    // </summary>
    public class DTO_ReportTransaccionManual : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportTransaccionManual()
        {
            this.Header = new DTO_ReportTransacHeader();
            this.Footer = new List<DTO_ReportTransacFooter>();
            this.Detail = new List<DTO_ReportTransacDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportTransacHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportTransacFooter> Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportTransacDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportTransacHeader
    {
        #region Propiedades

        /// <summary>
        /// Employee's NIT
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// CentroCosto ID
        /// </summary>
        [DataMember]
        public string CentroCostoID { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string BodegaOrigen { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string BodegaDestino { get; set; }

        /// <summary>
        /// Movimiento Tipo
        /// </summary>
        [DataMember]
        public string MvtoTipoInvID { get; set; }
        
        /// <summary>
        /// Descriptivo
        /// </summary>
        [DataMember]
        public string Descriptivo { get; set; }

        /// <summary>
        /// EmpresaID
        /// </summary>
        [DataMember]
        public string EmpresaID { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportTransacHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Footer del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportTransacFooter
    {
        #region Propiedades
        /// <summary>
        /// Referencia
        /// </summary>
        [DataMember]
        public string inReferenciaID { get; set; }

        /// <summary>
        /// Documento de Soporte
        /// </summary>
        [DataMember]
        public int DocSoporte { get; set; }

        /// <summary>
        /// Descripfion de a referencia
        /// </summary>
        [DataMember]
        public string DescripReferencia { get; set; }

        /// <summary>
        /// Cantidad Unitaria
        /// </summary>
        [DataMember]
        public Decimal CantidadUni { get; set; }

        /// <summary>
        /// Serial
        /// </summary>
        [DataMember]
        public string Serial { get; set; }

        #endregion
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportTransacFooter()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportTransacDetail
    {
       

        //<summary>
        //Constructor por defecto
        //</summary>
        public DTO_ReportTransacDetail()
        {
        }
    }

}

