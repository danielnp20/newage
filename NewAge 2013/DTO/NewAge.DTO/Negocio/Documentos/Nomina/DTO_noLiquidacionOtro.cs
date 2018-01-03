using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.UDT;
using System.Runtime.Serialization;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noLiquidacionOtro : DTO_NominaAprobacion
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_noLiquidacionOtro()
        {
            this.InitColums();
            this.Valor.Value = 0;
        }


        public void InitColums()
        {
            this.Tercero = new DTO_coTercero();
            this.Preliminar = new List<DTO_noLiquidacionOtroDetalle>();
            this.TerceroID = new UDT_TerceroID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();
        }

        #region Propiedades

        public DTO_coTercero Tercero { get; set; }
        
        public UDT_TerceroID TerceroID { get; set; }

        public UDT_Descriptivo TerceroDesc { get; set; }

        public List<DTO_noLiquidacionOtroDetalle> Preliminar { get; set; }

        #endregion
    }

}
