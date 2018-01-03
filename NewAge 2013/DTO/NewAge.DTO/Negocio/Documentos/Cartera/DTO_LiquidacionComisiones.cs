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
    /// Models DTO_LiquidacionComisiones
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_LiquidacionComisiones
    {
        public DTO_LiquidacionComisiones()
        {
            this.ComisionDeta = new List<DTO_ccComisionDeta>();
            this.ComisionDocu = new DTO_ccComisionDocu();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccComisionDeta> cde, DTO_ccComisionDocu cdo)
        {
            this.ComisionDeta = cde;
            this.ComisionDocu = cdo;
        }

        #region Propiedades

        [DataMember]
        public List<DTO_ccComisionDeta> ComisionDeta
        {
            get;
            set;
        }

        [DataMember]
        public DTO_ccComisionDocu ComisionDocu
        {
            get;
            set;
        }

        #endregion
    }
}
