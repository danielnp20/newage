using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noPagoLiquidaciones : DTO_NominaAprobacion
    {
        /// <summary>
        /// Constructor de la aplicación
        /// </summary>
        public DTO_noPagoLiquidaciones(DTO_glDocumentoControl doc)
            : base(doc)
        {
            this.DocControl = doc;
            this.EmpleadoID = new UDT_EmpleadoID();
            this.NombreEmpleado = new UDT_Descriptivo();
            this.TerceroID = new UDT_EmpleadoID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.BancoID = new UDT_BancoID();
            this.BancoDesc = new UDT_Descriptivo();
            this.TipoCuenta = new UDTSQL_tinyint();
            this.CuentaAbono = new UDTSQL_char(20);
        }

        #region Propiedades

        [DataMember]
        public DTO_noEmpleado Empleado
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocControl
        {
            get;
            set;
        }

        [DataMember]
        public DTO_noLiquidacionesDocu DocLiquidacion
        {
            get;
            set;
        }     
        
        #region Campos para Aprobación

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreEmpleado { get; set; }

        [DataMember]
        public UDT_EmpleadoID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BancoID BancoID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCuenta { get; set; }

        [DataMember]
        public UDTSQL_char CuentaAbono { get; set; }

        #endregion


        #endregion
    }
}
