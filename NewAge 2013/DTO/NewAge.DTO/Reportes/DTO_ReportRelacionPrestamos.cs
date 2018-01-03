using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    // <summary>
    // Clase del reporte Transaccion Manual
    // </summary>
    public class DTO_ReportRelacionPrestamos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportRelacionPrestamos()
        {
            this.Detail = new List<DTO_InvFisicoDetail>();
        }
        #region Propiedades
        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_InvFisicoDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_InvFisicoDetail
    {
        #region Propiedades

        [DataMember]
        public int Numero { get; set; }

        [DataMember]
        public int Cedula { get; set; }

        [DataMember]
        public string NombreEmpleado { get; set; }

        [DataMember]
        public decimal VrlTotal { get; set; }

        [DataMember]
        public decimal VlrCuota { get; set; }

        [DataMember]
        public decimal VlrAbono { get; set; }

        [DataMember]
        public decimal Saldo { get; set; }

        #endregion
        ///<summary>
        ///Constructor por defecto
        ///</summary>
        public DTO_InvFisicoDetail()
        {
        }
    }
}

