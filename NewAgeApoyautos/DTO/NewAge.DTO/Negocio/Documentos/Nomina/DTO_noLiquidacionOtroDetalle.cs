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
    public class DTO_noLiquidacionOtroDetalle
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_noLiquidacionOtroDetalle()
        {
            this.InitColums();  
        }

        public void InitColums()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Estado = new UDTSQL_smallint();
            this.TerceroID = new UDT_TerceroID();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.ConceptoNODesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
        }

        #region Propiedades

        public DTO_noLiquidacionPreliminar Preliminar { get; set; }

        public UDT_Consecutivo NumeroDoc { get; set; }

        public UDTSQL_smallint Estado { get; set; }

        public UDT_TerceroID TerceroID { get; set; }

        public UDT_EmpleadoID EmpleadoID { get; set; }

        public UDT_Descriptivo EmpleadoDesc { get; set; }

        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        public UDT_Descriptivo ConceptoNODesc { get; set; }

        public UDT_Valor Valor { get; set; }

        public UDT_BasicID DocumentoID { get; set; }

        public UDT_Descriptivo DocumentoDesc { get; set; }

        #endregion
    }
}
