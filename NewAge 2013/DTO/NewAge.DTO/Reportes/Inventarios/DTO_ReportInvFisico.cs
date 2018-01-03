using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    // <summary>
    // Clase del reporte Transaccion Manual
    // </summary>
    public class DTO_ReportInvFisico : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportInvFisico()
        {
            this.Header = new DTO_InvFisicoHeader();
            this.Footer = new List<DTO_InvFisicoFooter>();
            this.Detail = new List<DTO_InvFisicoDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_InvFisicoHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_InvFisicoFooter> Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_InvFisicoDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }

        [DataMember]
        public InventarioFisicoReportType TipoReport{ get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_InvFisicoHeader
    {
        #region Propiedades

        /// <summary>
        /// Solicitud
        /// </summary>
        [DataMember]
        public string Bodega { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_InvFisicoHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Footer del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_InvFisicoFooter
    {
        #region Propiedades
        /// <summary>
        /// Referencia
        /// </summary>
        [DataMember]
        public string TotalBodega { get; set; }

        [DataMember]
        public string GrandTotal { get; set; }
        #endregion
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_InvFisicoFooter()
        {

        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_InvFisicoDetail
    {
        #region Propiedades
        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public decimal Kardex { get; set; }

        public string Cantidades { get; set; }

        public decimal Fisico { get; set; }

        public decimal Diferencia { get; set; }

        public decimal UnidadLoc { get; set; }

        public decimal TotalLoc { get; set; }

        public decimal UnidadExt { get; set; }

        public decimal TotalExt { get; set; }

        #endregion
        ///<summary>
        ///Constructor por defecto
        ///</summary>
        public DTO_InvFisicoDetail()
        {
        }
    }
}

