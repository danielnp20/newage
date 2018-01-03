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
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_OrdenSalida
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_OrdenSalida : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_OrdenSalida()
        {
            this.Header = new DTO_inOrdenSalidaDocu();
            this.Footer = new List<DTO_inOrdenSalidaDeta>();
            this.DocCtrl = new DTO_glDocumentoControl();
        }

        #region Propiedades

        [DataMember]
        public DTO_inOrdenSalidaDocu Header
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_inOrdenSalidaDeta> Footer
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
