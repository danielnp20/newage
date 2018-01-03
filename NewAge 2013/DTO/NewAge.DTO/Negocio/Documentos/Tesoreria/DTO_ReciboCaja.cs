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
    /// Models DTO_ReciboCaja
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ReciboCaja : DTO_BasicReport
    {
        [DataMember]
        public DTO_tsReciboCajaDocu ReciboCajaDoc;

        [DataMember]
        public DTO_glDocumentoControl DocControl;

        [DataMember]
        public DTO_Comprobante Comp;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReciboCaja()
        {
            this.ReciboCajaDoc = new DTO_tsReciboCajaDocu();
            this.DocControl = new DTO_glDocumentoControl();
            this.Comp = new DTO_Comprobante();
        }

        /// <summary>
        /// Crea documento de Recibo de Caja 
        /// </summary>
        /// <param ajuste>Documento Recibo de Caja</param>
        /// <param ctrl>Documento Control relacionado</param>
        /// <param comp>Comprobante relacionado</param>
        public void AddData(DTO_tsReciboCajaDocu recibo, DTO_glDocumentoControl ctrl, DTO_Comprobante comp)
        {
            this.ReciboCajaDoc = recibo;
            this.DocControl = ctrl;
            this.Comp = comp;
        }
    }
}
