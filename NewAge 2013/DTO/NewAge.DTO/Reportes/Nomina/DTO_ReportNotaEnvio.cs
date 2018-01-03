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
    // Clase del reporte Nota Envío
    // </summary>
    public class DTO_ReportNotaEnvio : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNotaEnvio()
        {
            this.Header = new DTO_ReportNotaEnvioHeader();
            this.Footer = new List<DTO_ReportNotaEnvioFooter>();
            this.Detail = new DTO_ReportNotaEnvioDetail();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNotaEnvioHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportNotaEnvioFooter> Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNotaEnvioDetail Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNotaEnvioHeader
    {
        #region Propiedades

        /// <summary>
        /// Employee's NIT
        /// </summary>
        [DataMember]
        public string Cliente { get; set; }

        /// <summary>
        /// CentroCosto ID
        /// </summary>
        [DataMember]
        public string TipoVehiculo { get; set; }

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
        /// Observacion
        /// </summary>
        [DataMember]
        public string Observacion { get; set; }

        /// <summary>
        /// Conductor
        /// </summary>
        [DataMember]
        public string Conductor { get; set; }

        /// <summary>
        /// Cedula
        /// </summary>
        [DataMember]
        public string cedula { get; set; }

        [DataMember]
        public string Placa { get; set; }
        #endregion

        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Prefijo
        /// </summary>
        [DataMember]
        public string Prefijo { get; set; }

        /// <summary>
        /// NumeroDoc
        /// </summary>
        [DataMember]
        public string Documento { get; set; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNotaEnvioHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Footer del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNotaEnvioFooter
    {
        #region Propiedades
        /// <summary>
        /// Referencia
        /// </summary>
        [DataMember]
        public string inReferenciaID { get; set; }

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
        public DTO_ReportNotaEnvioFooter()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNotaEnvioDetail
    {
        #region Propiedades
        [DataMember]
        public string EnviadoPor { get; set; }

        [DataMember]
        public string RecibidoPor { get; set; }

        [DataMember]
        public string DireccionBase { get; set; }

        [DataMember]
        public DateTime FechaRecepcion { get; set; }
        
        #endregion

        //<summary>
        //Constructor por defecto
        //</summary>
        public DTO_ReportNotaEnvioDetail()
        {
        }
    }

}

