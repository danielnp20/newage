using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_AumentoSalarial
    {             
   
         /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_AumentoSalarial()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpleadoID = new UDT_EmpleadoID();
            this.NombreEmpleado = new UDT_Descriptivo();
            this.Sueldo = new UDT_Valor();
            this.NuevoSueldo = new UDT_Valor();
            this.Aumento = new UDT_Valor();
            this.Dias = new UDT_Consecutivo();
            this.Ajuste = new UDT_PorcentajeID();
            this.FechaAumento = new UDTSQL_datetime();
        }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo NombreEmpleado { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAumento { get; set; } 

        [DataMember]
        public UDT_Valor Sueldo { get; set; }

        [DataMember]
        public UDT_Valor NuevoSueldo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor Aumento { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Dias { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PorcentajeID Ajuste { get; set; }
       
    }
}
