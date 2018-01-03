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
    /// Models DTO_LiquidacionImportacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_LiquidacionImportacion : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_LiquidacionImportacion()
        {
            this.Header = new DTO_inImportacionDocu();
            this.Footer = new List<DTO_inImportacionDeta>();
            this.DocCtrl = new DTO_glDocumentoControl();
        }

        #region Propiedades

        [DataMember]
        public DTO_inImportacionDocu Header
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_inImportacionDeta> Footer
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
