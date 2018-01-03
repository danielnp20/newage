using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    public class DTO_ReportNominaInfoEmpleado_Totales 
    {
       
        /// <summary>
        /// Constructor x Defecto
        /// </summary>
        public DTO_ReportNominaInfoEmpleado_Totales()
        {
            this.VlrTotal = new UDT_Valor();
            this.Detalles = new List<DTO_noReportDetalleNominaXConcepto>();
        }

        #region Propiedades
   
       
        [DataMember]
        public List<DTO_noReportDetalleNominaXConcepto> Detalles { get; set; }

        [DataMember]
        public DateTime FechaInicial { get; set; }

        [DataMember]
        public DateTime FechaFinal { get; set; }

        [DataMember]
        public UDT_Valor VlrTotal { get; set; }
        #endregion
    }
}

