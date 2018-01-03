using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_CarteraTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CarteraTotales()
        {
        }

        #region Propiades Genericas

        /// <summary>
        /// Carga el Perido Inicial de la consulta
        /// </summary>
        [DataMember]
        public DateTime PeriodoIncial { get; set; }

        /// <summary>
        /// Carga el PeriodoFinal de Consulta
        /// </summary>
        [DataMember]
        public DateTime PeriodoFinal { get; set; }

        #endregion

        #region Propiedades para los Reportes

        /// <summary>
        /// Carga el DTO de los Saldos
        /// </summary>
        [DataMember]
        public List<DTO_ccSaldos> DetallesSaldos { get; set; }

        /// <summary>
        /// Carga el DTO de los Saldos En Mora
        /// </summary>
        [DataMember]
        public List<DTO_ccSaldosMora> DetalleSaldoMora { get; set; }

        /// <summary>
        /// Carga la lista del los credito liquidados
        /// </summary>
        [DataMember]
        public List<DTO_ReportLiquidacionCredito> DetalleLiquidaCredito { get; set; }

        /// <summary>
        /// Carga la lista con las solicitudes de credito
        /// </summary>
        [DataMember]
        public List<DTO_ccSolicitudes> DetalleSolicitudes { get; set; }

        /// <summary>
        /// Carga la lista con la venta de la cartera
        /// </summary>
        [DataMember]
        public List<DTO_ccVentaCartera> DetalleVentaCartera { get; set; }

        /// <summary>
        /// Carga la lista de las libranzas que ya estan referenciadas
        /// </summary>
        [DataMember]
        public List<DTO_ccReferenciacion> DetalleRefenciacion { get; set; }

        /// <summary>
        /// Carga el Listado de las los clientes que tienen incoporaciones en la pagaduria
        /// </summary>
        public List<DTO_ccPagaduriaIncoporacion> DetallePagaduriaIncorporacion { get; set; }

        /// <summary>
        /// Carga la lista de las libranzas
        /// </summary>
        [DataMember]
        public List<DTO_ReportLibranzas> DetalleLibranzas { get; set; }

        #endregion

    }
}
