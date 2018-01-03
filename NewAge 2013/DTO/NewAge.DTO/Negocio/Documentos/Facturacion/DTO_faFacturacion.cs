using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_Facturacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faFacturacion : DTO_BasicReport
    {
        #region Propriedades
        [DataMember]
        public DTO_faFacturaDocu Header { get; set; }

        [DataMember]
        public List<DTO_faFacturacionFooter> Footer { get; set; }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faFacturacion()
        {
            this.Header = new DTO_faFacturaDocu();
            this.Footer = new List<DTO_faFacturacionFooter>();
            this.DocCtrl = new DTO_glDocumentoControl();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crea el header de la facturacion
        /// </summary>
        /// <param h>Header de la facturacion</param>
        /// <param f>Detalle de la facturacion</param>
        /// <param dc>Documento Control asociado</param>
        public void AddData(DTO_faFacturaDocu h, List<DTO_faFacturacionFooter> f, DTO_glDocumentoControl dc)
        {
            this.Header = h;
            this.Footer = f;
            this.DocCtrl = dc;
        }
        #endregion
    }

}
