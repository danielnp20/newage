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
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_PagoFlujos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_PagoFlujos
    {
        public DTO_PagoFlujos()
        {
            this.FlujoCesionDeta = new List<DTO_ccFlujoCesionDeta>();
            this.FlujoCesionDocu = new List<DTO_ccFlujoCesionDocu>();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccFlujoCesionDeta> fcDeta, List<DTO_ccFlujoCesionDocu> fcDocu)
        {
            this.FlujoCesionDeta = fcDeta;
            this.FlujoCesionDocu = fcDocu;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccFlujoCesionDeta> FlujoCesionDeta
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccFlujoCesionDocu> FlujoCesionDocu
        {
            get;
            set;
        }

        #endregion
    }
}
