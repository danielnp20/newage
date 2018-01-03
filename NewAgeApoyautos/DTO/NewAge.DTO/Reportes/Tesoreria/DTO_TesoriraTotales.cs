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
    public class DTO_TesoriraTotales
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_TesoriraTotales()
        {
        }

        #region Propiades Genericas
        #endregion

        #region Propiedades para los Reportes

        /// <summary>
        /// Carga el DTO del pago de factura
        /// </summary>
        [DataMember]
        public List<DTO_ReportPagoFactura> DetallePagosFactura { get; set; }
       
        #endregion

    }
}
