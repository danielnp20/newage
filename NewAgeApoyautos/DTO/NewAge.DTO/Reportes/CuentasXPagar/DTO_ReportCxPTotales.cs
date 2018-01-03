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
    public class DTO_ReportCxPTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCxPTotales()
        {
            this.ValorTotalDeta = new UDT_Valor();
        }

        #region Propiedades Genericas

        /// <summary>
        /// Fecha Inicial para reportes
        /// </summary>
        [DataMember]
        public DateTime FechaIni { get; set; }

        /// <summary>
        /// Fecha Final para reportes
        /// </summary>
        [DataMember]
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Adicional
        /// </summary>
        [DataMember]
        public UDT_Valor ValorTotalDeta { get; set; }

        #endregion

        #region Propiades Para reportes

        /// <summary>
        /// Listado de Detalle para el reporte de de Cuentas por Pagar por edades Resumido
        /// </summary>
        [DataMember]
        public List<DTO_ReportCxPPorEdades> DetallesPorEdades { get; set; }

        /// <summary>
        /// Listado de detalle de radicaciones de cuentas por pagar
        /// </summary>
        [DataMember]
        public List<DTO_ReportRadicaciones> DetalleRadicaciones { get; set; }

        /// <summary>
        /// Listado con la causacion de la factura
        /// </summary>
        [DataMember]
        public List<DTO_ReportCausacionFacturas> DetalleCausacionFactura { get; set; }

        /// <summary>
        /// Listado con los anticipos
        /// </summary>
        [DataMember]
        public List<DTO_ReportAnticiposDetallado> DetalleAnticipos { get; set; }

        /// <summary>
        /// Listado con los anticipos
        /// </summary>
        [DataMember]
        public List<DTO_ReportAnticiposViaje> DetalleAnticiposViaje { get; set; }

        /// <summary>
        /// Carga los detalles del libro de compras
        /// </summary>
        [DataMember]
        public List<DTO_ReportLibroCompras> DetalleLibroCompras { get; set; }

        /// <summary>
        /// Listado con el flujo semanal
        /// </summary>
        [DataMember]
        public List<DTO_ReportCxPFlujoSemanalDetallado> DetalleFlujoSemanal { get; set; }
        
        
        #endregion

    }
}
