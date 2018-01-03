using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pago Facturas:
    /// Models DTO_RegistroPagoFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_RegistroPagoFactura
    {
        #region DTO_RegistroPagoFactura

        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public DTO_RegistroPagoFactura()
        {
            this.DocumentoControl = new DTO_glDocumentoControl();
            this.BancosDocu = new DTO_tsBancosDocu();
        }

        /// <summary>
        /// Constructor con información
        /// </summary>
        /// <param name="documentoControl">Documento Control</param>
        /// <param name="bancosDocu">Bancos Docu</param>
        public void AddData(DTO_glDocumentoControl documentoControl, DTO_tsBancosDocu bancosDocu)
        {
            this.DocumentoControl = documentoControl;
            this.BancosDocu = bancosDocu;
        }

        #endregion

        #region Propiedades

        [DataMember]
        public DTO_glDocumentoControl DocumentoControl { get; set; }

        [DataMember]
        public DTO_tsBancosDocu BancosDocu { get; set; }

        #endregion
    }
}
