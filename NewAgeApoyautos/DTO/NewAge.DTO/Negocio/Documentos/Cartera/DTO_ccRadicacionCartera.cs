using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_ccRadicacionCartera
    [Serializable]
    [DataContract]
    public class DTO_ccRadicacionCartera : DTO_BasicReport
    /// </summary>
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccRadicacionCartera()
        {
            this.Header = new DTO_ccSolicitudDocu();
            this.Footer = new List<DTO_ccSolicitudAnexo>();
            this.DocCtrl = new DTO_glDocumentoControl();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(DTO_glDocumentoControl dc,DTO_ccSolicitudDocu h, List<DTO_ccSolicitudAnexo> f)
        {
            this.Header = h;
            this.Footer = f;
            this.DocCtrl = dc;
        }

        #region Propiedades

        [DataMember]
        public DTO_ccSolicitudDocu Header
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudAnexo> Footer
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl
        {
            get;
            set;
        }

        #endregion
    }
}