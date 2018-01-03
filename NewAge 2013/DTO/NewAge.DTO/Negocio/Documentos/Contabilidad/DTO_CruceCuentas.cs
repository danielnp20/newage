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
    /// Models DTO_CruceCuentas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CruceCuentas : DTO_BasicReport
    {
        [DataMember]
        public DTO_coDocumentoAjuste AjusteDoc;

        [DataMember]
        public DTO_glDocumentoControl DocControl;

        [DataMember]
        public DTO_Comprobante Comp;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CruceCuentas()
        {
            this.AjusteDoc = new DTO_coDocumentoAjuste();
            this.DocControl = new DTO_glDocumentoControl();
            this.Comp = new DTO_Comprobante();
        }

        /// <summary>
        /// Crea documento de Ajuste de saldos 
        /// </summary>
        /// <param ajuste>Documento Ajuste relacionado</param>
        /// <param ctrl>Documento Control relacionado</param>
        /// <param comp>Comprobante relacionado</param>
        public void AddData(DTO_coDocumentoAjuste ajuste, DTO_glDocumentoControl ctrl, DTO_Comprobante comp)
        {
            this.AjusteDoc = ajuste;
            this.DocControl = ctrl;
            this.Comp = comp;
        }
    }
}
