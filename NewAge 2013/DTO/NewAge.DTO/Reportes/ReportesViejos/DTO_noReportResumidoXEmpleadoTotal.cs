using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportResumidoXEmpleadoTotal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportResumidoXEmpleadoTotal
    {
        
        #region Propiedades

        [DataMember]
        public List<DTO_noReportResumidoXEmpleado> Detalles { get; set; }
        #endregion
    }
}
