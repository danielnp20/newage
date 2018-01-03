using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_Presupuesto
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Presupuesto() { this.NumeroDocPresup = new UDT_Consecutivo(); }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Presupuesto(DTO_glDocumentoControl ctrl, List<DTO_plPresupuestoDeta> det) 
        {
            this.DocCtrl = ctrl;
            this.Detalles = det;
            this.NumeroDocPresup = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }

        [DataMember]
        public List<DTO_plPresupuestoDeta> Detalles { get; set; }

        [DataMember]
        public List<DTO_plPresupuestoPxQDeta> DetallesPxQ { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocPresup { get; set; }

        #endregion
    }
}
