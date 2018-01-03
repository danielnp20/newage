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
    public class DTO_ReportSolicitud : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportSolicitud()
        {
            this.Header = new DTO_ReportSolicHeader();
            this.Footer = new List<DTO_ReportSolicFooter>();
            this.Detail = new List<DTO_ReportSolicDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportSolicHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportSolicFooter> Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportSolicDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportSolicHeader
    {
        #region Propiedades

        /// <summary>
        /// Solicitud
        /// </summary>
        [DataMember]
        public string Solicitud { get; set; }

        /// <summary>
        /// Digitador
        /// </summary>
        [DataMember]
        public string Digitador { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Solicitante
        /// </summary>
        [DataMember]
        public string Solicitante { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public short Estado { get; set; }
        
        /// <summary>
        /// Documento
        /// </summary>
        [DataMember]
        public string Documento { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportSolicHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Footer del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportSolicFooter
    {
        #region Propiedades
        /// <summary>
        /// Referencia
        /// </summary>
        [DataMember]
        public string Observacion { get; set; }
        #endregion
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportSolicFooter()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportSolicDetail
    {
        #region Propiedades
        public string CodigoBSID { get; set; }

        public string inReferenciaID { get; set; }

        public string Descripcion { get; set; }

        public string UnidadInv { get; set; }

        public string CantidadSol { get; set; }

        public string Proyecto { get; set; }

        public string CentroCosto { get; set; }
        #endregion
        ///<summary>
        ///Constructor por defecto
        ///</summary>
        public DTO_ReportSolicDetail()
        {
        }
    }
}

