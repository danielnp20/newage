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
    public class DTO_ReportNominaDetalleExE : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNominaDetalleExE()
        {
            this.Header = new List<DTO_ReportNominaDetalleExEHeader>();
            this.Detail = new List<DTO_ReportNominaDetalleExEDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportNominaDetalleExEHeader> Header { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportNominaDetalleExEDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }


        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNominaDetalleExEHeader
    {
        #region Propiedades

        /// <summary>
        /// Solicitud
        /// </summary>
        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public string EmpleadoDesc { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime FechaIng { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }

        [DataMember]
        public string Cargo { get; set; }

        [DataMember]
        public decimal Salario { get; set; }

        [DataMember]
        public string DiasDescanso { get; set; }

        [DataMember]
        public string CentroCosto { get; set; }

        [DataMember]
        public string Proyecto { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNominaDetalleExEHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNominaDetalleExEDetail
    {
        #region Propiedades
        [DataMember]
        public string Concepto { get; set; }

        [DataMember]
        public string Base { get; set; }

        [DataMember]
        public string Devengos { get; set; }

        [DataMember]
        public string Deducciones { get; set; }

        #endregion
        ///<summary>
        ///Constructor por defecto
        ///</summary>
        public DTO_ReportNominaDetalleExEDetail()
        {
        }
    }
}

