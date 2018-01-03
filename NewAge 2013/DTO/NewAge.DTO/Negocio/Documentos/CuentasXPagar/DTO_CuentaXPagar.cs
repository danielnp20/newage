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
    /// Models DTO_CuentaXPagar
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CuentaXPagar : DTO_BasicReport
    {
        [DataMember]
        public DTO_glDocumentoControl DocControl;

        [DataMember]
        public DTO_cpCuentaXPagar CxP;

        [DataMember]
        public DTO_Comprobante Comp;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CuentaXPagar()
        {
            this.DocControl = new DTO_glDocumentoControl();
            this.CxP = new DTO_cpCuentaXPagar();
            this.Comp = new DTO_Comprobante();
        }

        /// <summary>
        /// Crea la cxp
        /// </summary>
        /// <param name="ctrl">Documento Control Relacionado</param>
        /// <param name="cxp">CxP relacionada</param>
        /// <param f>Detalle del comprobante</param>
        public void AddData(DTO_glDocumentoControl ctrl, DTO_cpCuentaXPagar cxp, List<DTO_ComprobanteFooter> f, DTO_Comprobante comp)
        {
            this.DocControl = ctrl;
            this.CxP = cxp;
            this.Comp = comp;
        }
    }

}
