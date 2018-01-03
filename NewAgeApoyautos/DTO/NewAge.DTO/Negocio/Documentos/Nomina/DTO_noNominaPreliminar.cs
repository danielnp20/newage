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
    public class DTO_noNominaPreliminar : DTO_NominaAprobacion
    {
        /// <summary>
        /// Constructor de la aplicación
        /// </summary>
        public DTO_noNominaPreliminar(DTO_glDocumentoControl doc)
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
            this.FechaCorteCesantias = new UDTSQL_smalldatetime();
            this.FechaPagoCesantias = new UDTSQL_smalldatetime();
            this.ValorCesantias = new UDT_Valor();
            this.ValorInteresesCesantias = new UDT_Valor();
            this.Resolucion = new UDTSQL_char(20);
            this.Estado = new UDTSQL_smallint();
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

        [DataMember]
        public List<DTO_noLiquidacionPreliminar> Detalle
        {
            get;
            set;
        }

        [DataMember]
        public UDTSQL_smallint Estado { get; set; }
                
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

        #region Campos de Documento de Liquidacion

        [DataMember]
        public UDTSQL_smalldatetime FechaCorteCesantias { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaPagoCesantias { get; set; }
        
        [DataMember]
        public UDT_Valor ValorCesantias { get; set; }

        [DataMember]
        public UDT_Valor ValorInteresesCesantias { get; set; }

        [DataMember]
        public UDTSQL_char Resolucion { get; set; }
        
        #endregion


        #endregion
    }
}
