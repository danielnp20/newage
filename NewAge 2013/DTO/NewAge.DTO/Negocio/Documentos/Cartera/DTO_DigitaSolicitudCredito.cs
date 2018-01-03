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
    /// Models DTO_DigitaSolicitudCredito
    [Serializable]
    [DataContract]
    public class DTO_DigitaSolicitudCredito
    /// </summary>
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_DigitaSolicitudCredito()
        {
            this.SolicituDocu = new DTO_ccSolicitudDocu();
            this.DocCtrl = new DTO_glDocumentoControl();
            this.DatosPersonales = new List<DTO_ccSolicitudDatosPersonales>();            
        }

        #region Propiedades

        [DataMember]
        public DTO_ccSolicitudDocu SolicituDocu
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

        [DataMember]
        public List<DTO_ccSolicitudDatosPersonales> DatosPersonales
        {
            get;
            set;
        }

        
        #endregion
    }
}