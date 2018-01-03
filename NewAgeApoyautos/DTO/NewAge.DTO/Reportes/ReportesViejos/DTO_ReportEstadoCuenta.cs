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
    public class DTO_ReportEstadoCuenta : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportEstadoCuenta()
        {
            this.Header = new DTO_ReportEstadoCuentaHeader();
            this.Detail = new List<DTO_ReportEstadoCuentaDetail>();
            this.Footer = new List<DTO_ReportEstadoCuentaFooter>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportEstadoCuentaHeader Header { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportEstadoCuentaDetail> Detail { get; set; }

        [DataMember]
        public IList<DTO_ReportEstadoCuentaFooter> Footer { get; set; }
        
        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportEstadoCuentaHeader
    {
        #region Propiedades

        [DataMember]
        public int Libranza { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Cliente { get; set; }

        [DataMember]
        public short? NumeroCuotas { get; set; }

        [DataMember]
        public string Pagaduria { get; set; }

        [DataMember]
        public decimal Interes { get; set; }
        
        [DataMember]
        public decimal Otros1 { get; set; }

        [DataMember]
        public decimal Otros2 { get; set; }

        [DataMember]
        public decimal Otros3 { get; set; }

        [DataMember]
        public decimal VrOtros1 { get; set; }

        [DataMember]
        public decimal VrOtros2 { get; set; }

        [DataMember]
        public decimal VrOtros3 { get; set; }

        [DataMember]
        public decimal VrOtrosMes1 { get; set; }

        [DataMember]
        public decimal VrOtrosMes2 { get; set; }

        [DataMember]
        public decimal VrOtrosMes3 { get; set; }

        [DataMember]
        public decimal TotalOtrosMes { get; set; }

        [DataMember]
        public decimal TotalOtros { get; set; }

        [DataMember]
        public decimal? VrLibranza { get; set; }

        [DataMember]
        public decimal VrOtroTotalHeader { get; set; }

        [DataMember]
        public decimal? VrCredito { get; set; }

        [DataMember]
        public decimal? VrSolicitado { get; set; }

        [DataMember]
        public decimal? VrAdicional { get; set; }

        [DataMember]
        public decimal VrDescuento { get; set; }

        [DataMember]
        public decimal? VrCompra { get; set; }

        [DataMember]
        public decimal VrAfiliacionMes { get; set; }

        [DataMember]
        public decimal VrAfiliacionTotal { get; set; }
        
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportEstadoCuentaHeader()
        {
        }
    }

    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    [DataContract]
    [Serializable]
    public class DTO_ReportEstadoCuentaDetail
    {
        #region Propiedades
        [DataMember]
        public string CuotaID { get; set; }

        [DataMember]
        public DateTime Venc_Cta { get; set; }

        [DataMember]
        public decimal VlrCuota { get; set; }

        [DataMember]
        public decimal Capital { get; set; }

        [DataMember]
        public decimal Seguro { get; set; }

        [DataMember]
        public decimal Interes { get; set; }

        [DataMember]
        public decimal VlrOtros { get; set; }

        [DataMember]
        public decimal ComponentesSuma { get; set; }

        [DataMember]
        public decimal SaldoInteres { get; set; }

        [DataMember]
        public decimal SaldoCapital { get; set; }

        [DataMember]
        public decimal Otros1 { get; set; }
        #endregion
        ///<summary>
        ///Constructor por defecto
        ///</summary>
        public DTO_ReportEstadoCuentaDetail()
        {
        }
    }

    [DataContract]
    [Serializable]
    public class DTO_ReportEstadoCuentaFooter
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
        
        public DTO_ReportEstadoCuentaFooter()
        {

        }
    }
}

