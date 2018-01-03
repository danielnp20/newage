using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    // <summary>
    // Clase del reporte Transaccion Manual
    // </summary>
    public class DTO_ReportNoVacaciones : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNoVacaciones()
        {
            this.Header = new DTO_ReportNoVacacionesHeader();
            this.Footer = new DTO_ReportNoVacacionesFooter();
            this.Detail = new List<DTO_ReportNoVacacionesDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNoVacacionesHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNoVacacionesFooter Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportNoVacacionesDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNoVacacionesHeader
    {
        #region Propiedades

        [DataMember]
        public string CedulaEmpleado { get; set; }

        [DataMember]
        public string Empleado { get; set; }

        [DataMember]
        public string Periodo { get; set; }

        [DataMember]
        public string FechaIngreso { get; set; }

        [DataMember]
        public string PeriodoPago { get; set; }

        [DataMember]
        public string PeriodoDescanso { get; set; }

        [DataMember]
        public string FechaReIntegro { get; set; }

        [DataMember]
        public int DiasTomados { get; set; }

        [DataMember]
        public int DiasPagados { get; set; }

        [DataMember]
        public string Resolucion { get; set; }

        [DataMember]
        public decimal Salario { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNoVacacionesHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Footer del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNoVacacionesFooter
    {
        #region Propiedades
        /// <summary>
        /// Referencia
        /// </summary>
        [DataMember]
        public string TotalDevengado { get; set; }

        [DataMember]
        public decimal TotalDeducido { get; set; }

        [DataMember]
        public decimal NetoPagar { get; set; }
        #endregion
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNoVacacionesFooter()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNoVacacionesDetail
    {
        #region Propiedades

        [DataMember]
        public string CodigoDevengos { get; set; }

        [DataMember]
        public string DescripcionDevengos { get; set; }

        [DataMember]
        public decimal BaseDevengos { get; set; }

        [DataMember]
        public decimal ValorDevengos { get; set; }

        [DataMember]
        public string CodigoDeducciones { get; set; }

        [DataMember]
        public string DescripcionDeducciones { get; set; }

        [DataMember]
        public decimal BaseDeducciones { get; set; }

        [DataMember]
        public decimal ValorDeducciones { get; set; }

        #endregion
        ///<summary>
        ///Constructor por defecto
        ///</summary>
        public DTO_ReportNoVacacionesDetail()
        {
        }
    }
}

