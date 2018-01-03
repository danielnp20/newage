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
    /// Clase del reporte Libro Mayor
    /// </summary>
    public class DTO_ContabilidadTotal 
    {
        #region Propiedades Genericas

        /// <summary>
        /// Carga el periodo Inicial
        /// </summary>
        [DataMember]
        public DateTime PeriodoInicial { get; set; }

        /// <summary>
        /// Carga el periodo Final
        /// </summary>
        [DataMember]
        public DateTime PeriodoFinal { get; set; }

        #endregion
    
        #region Propiedades de reportes

        /// <summary>
        /// Carga el DTO de Inventarios de Balance
        /// </summary>
        [DataMember]
        public List<DTO_ReportInventariosBalance> DetallesInventarioBalance { get; set; }

        /// <summary>
        /// Carga el DTO con los balances comparaitivos
        /// </summary>
        [DataMember]
        public List<DTO_ReportBalanceComparativo> DetallesBalancesComparativo { get; set; }

        /// <summary>
        /// Listado para Generar el Certificado de Rete Fuente
        /// </summary>
        public List<DTO_Certificates> DetalleCertificadoReteFuente { get; set; }

        /// <summary>
        /// Carga el listado para el reporte de Tasas
        /// </summary>
        public List<DTO_ReportTasas> DetallesTasas { get; set; }

        /// <summary>
        /// Carga el listado para el Documento Comprobante Manual
        /// </summary>
        public List<DTO_ReportComprobanteManual> DetallesComprobanteManual { get; set; }
                
        #endregion

    }
}
