using System;
using System.Collections;
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
    public class DTO_ReportNominaDetalle 
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNominaDetalle()
        {
            this.Header = new DTO_noReportDetalleNominaHeader();
            this.Detail = new List<DTO_noReportDetalleNominaDetalle>();
            this.Footer = new List<DTO_ReportNominaDetalleFooter>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_noReportDetalleNominaHeader Header { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_noReportDetalleNominaDetalle> Detail { get; set; }

        [DataMember]
        public List<DTO_ReportNominaDetalleFooter> Footer { get; set; }
        
        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    ////[DataContract]
    ////[Serializable]
    ////<summary>
    ////Clase del Header del Reporte de LegalizacionGastos
    ////</summary>
    ////public class DTO_ReportNominaDetalleDetail
    ////{
    ////    #region Propiedades

    ////    [DataMember]
    ////    public DateTime FechaIni { get; set; }

    ////    [DataMember]
    ////    public DateTime FechaFin { get; set; }

    ////    [DataMember]
    ////    public string Localidad { get; set; }
        
    ////    [DataMember]
    ////    public string Operacion { get; set; }

    ////    [DataMember]
    ////    public string Cargo { get; set; }
        
    ////    [DataMember]
    ////    public string Brigada { get; set; }

    ////    [DataMember]
    ////    public string CentroCosto { get; set; }
        
    ////    #endregion

    ////    /// <summary>
    ////    /// Constructor por defecto
    ////    /// </summary>
    ////    public DTO_ReportNominaDetalleDetail()
    ////    {

    ////    }
    ////}

    ////<summary>
    ////Clase del Detalle del Reporte de LegalizacionGastos
    ////</summary>
    //[DataContract]
    //[Serializable]
    //public class DTO_noReportDetalleNominaDetalle
    //{
    //    #region Propiedades
    //    [DataMember]
    //    public string ClienteID { get; set; }

    //    [DataMember]
    //    public string ClienteDesc { get; set; }

    //    [DataMember]
    //    public DateTime FechaIngreso { get; set; }

    //    [DataMember]
    //    public DateTime FechaRetiro { get; set; }

    //    [DataMember]
    //    public string Salario { get; set; }

    //    [DataMember]
    //    public string DiasDescanso { get; set; }

    //    [DataMember]
    //    public string Proyecto { get; set; }

    //    [DataMember]
    //    public string Concepto { get; set; }

    //    [DataMember]
    //    public int Base { get; set; }

    //    [DataMember]
    //    public decimal Devengos { get; set; }

    //    [DataMember]
    //    public decimal Deducciones { get; set; }
        
    //    #endregion

    //    ///<summary>
    //    ///Constructor por defecto
    //    ///</summary>
    //    public DTO_ReportNominaDetalleDetail()
    //    {
    //    }
    //}

    [DataContract]
    [Serializable]
    public class DTO_ReportNominaDetalleFooter
    {
        #region Propiedades
        [DataMember]
        public string CuotaID_Footer { get; set; }

        [DataMember]
        public DateTime Venc_Cta_Footer { get; set; }

        [DataMember]
        public decimal VlrCuota_Footer { get; set; }

        [DataMember]
        public decimal Capital_Footer { get; set; }

        [DataMember]
        public decimal Seguro_Footer { get; set; }

        [DataMember]
        public decimal Interes_Footer { get; set; }

        [DataMember]
        public decimal VlrOtros_Footer { get; set; }

        [DataMember]
        public decimal ComponentesSuma_Footer { get; set; }

        [DataMember]
        public decimal SaldoInteres_Footer { get; set; }

        [DataMember]
        public decimal SaldoCapital_Footer { get; set; }

        [DataMember]
        public decimal Otros1_Footer { get; set; }
        #endregion
    }
}

