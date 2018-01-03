using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class  DTO_ProveedoresTotal
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ProveedoresTotal()
        {
        }

        #region Propiedades Genericas

        /// <summary>
        /// Propiedad para manejo de Fecha
        /// </summary>
        [DataMember]
        public DateTime PeriodoInicial { get; set; }

        /// <summary>
        /// Propiedad para manejo de Fecha
        /// </summary>
        [DataMember]
        public DateTime PeriodoFinal { get; set; }

        #endregion

        #region Propiades para reportes

        /// <summary>
        /// Carga la lista con los compromisos VS Facturas
        /// </summary>
        [DataMember]
        public List<DTO_ReportCompromisoVSFacturas> DetalleCompromisosVSFacturas { get; set; }

        /// <summary>
        /// Reportes de Orden de compra
        /// </summary>
        [DataMember]
        public List<DTO_ReportOrdenCompra> DetallesOrdenCompra { get; set; }

        /// <summary>
        /// Carga la Lista para el reporte de Solicitudes
        /// </summary>
        [DataMember]
        public List<DTO_ReportProveedoresSolicitudes> DetallesSolicitudes { get; set; }

        /// <summary>
        /// Carga la Lista para el reporte de Solicitudes
        /// </summary>
        [DataMember]
        public List<DTO_ReportOrdenCompraDoc> DetallesOrdenCompraDoc { get; set; }

        /// <summary>
        /// Carga la Lista para el reporte de Recibidos
        /// </summary>
        [DataMember]
        public List<DTO_ReportProveedoresRecibidos> DetallesRecibidos { get; set; }

        #endregion
    }
}
