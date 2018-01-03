using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    // <summary>
    // Clase del reporte Transaccion Manual
    // </summary>
    public class DTO_ReportNoPreNomina : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNoPreNomina()
        {
            this.Header = new DTO_ReportNoPreNominaHeader();
            this.Footer = new DTO_ReportNoPreNominaFooter();
            this.Detail = new List<DTO_ReportNoPreNominaDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNoPreNominaHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNoPreNominaFooter Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportNoPreNominaDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNoPreNominaHeader
    {
        #region Propiedades

        [DataMember]
        public string CedulaEmpleado { get; set; }

        [DataMember]
        public string Empleado { get; set; }

        [DataMember]
        public decimal ProcReteFte { get; set; }

        [DataMember]
        public decimal Porcentaje { get; set; }

        [DataMember]
        public decimal DDRete { get; set; }

        [DataMember]
        public string CentroCosto { get; set; }

        [DataMember]
        public string LocFisica { get; set; }

        [DataMember]
        public string IDEmpleado { get; set; }

        [DataMember]
        public string Cargo { get; set; }

        [DataMember]
        public string Brigada { get; set; }

        [DataMember]
        public string Operacion { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNoPreNominaHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Footer del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNoPreNominaFooter
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
        public DTO_ReportNoPreNominaFooter()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNoPreNominaDetail
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
        public DTO_ReportNoPreNominaDetail()
        {
        }
    }
}

