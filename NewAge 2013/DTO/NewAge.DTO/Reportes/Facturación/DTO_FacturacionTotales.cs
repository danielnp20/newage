using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Drawing;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_FacturacionTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_FacturacionTotales()
        {
            this.ValorTotalDeta = new UDT_Valor();
        }

        #region Propiades Genericas

        /// <summary>
        /// Adicional
        /// </summary>
        [DataMember]
        public UDT_Valor ValorTotalDeta { get; set; }

        #endregion

        #region Propiedades para los Reportes

        /// <summary>
        /// Lista de datos para la factura de venta
        /// </summary>
        [DataMember]
        public List<DTO_ReportFacturaVenta> DetalleFacturaVenta { get; set; }

        /// <summary>
        /// Carga la lista de datos para el reporte de cuenta por cobrar por edades detallado y resumido
        /// </summary>
        [DataMember]
        public List<DTO_ReportCxCPorEdades> DetalleCxCPorEdades { get; set; }

        /// <summary>
        /// Carga la lista para el libro de venta
        /// </summary>
        public List<DTO_ReportLibroVentas> DetalleLibroVentas { get; set; }

        /// <summary>
        /// Carga la lista para las Cuentas Por Cobrar
        /// </summary>
        public List<DTO_ReportCuentasPorCobrar> DetalleCuentasPorCobrar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Image Imagen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LetrasValor { get; set; }

        #endregion

    }
}
