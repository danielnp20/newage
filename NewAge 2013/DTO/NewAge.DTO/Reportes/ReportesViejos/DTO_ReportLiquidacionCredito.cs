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
    // <summary>
    // Clase del reporte Transaccion Manual
    // </summary>
    public class DTO_ReportLiquidacionCredito2 : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLiquidacionCredito2()
        {
            this.Header = new DTO_LiquidacionHeader();
            this.Detail = new List<DTO_LiquidacionDetail>();
            this.Footer = new List<DTO_LiquidacionFooter>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_LiquidacionHeader Header { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_LiquidacionDetail> Detail { get; set; }

        [DataMember]
        public IList<DTO_LiquidacionFooter> Footer { get; set; }
        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_LiquidacionHeader
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
        public decimal Aportes { get; set; }

        [DataMember]
        public decimal Afiliacion { get; set; }
        
        [DataMember]
        public decimal AportesMes { get; set; }

        [DataMember]
        public decimal AfiliacionMes { get; set; }

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

        [DataMember]
        public decimal VrSeguro { get; set; }

        [DataMember]
        public decimal VrCuota { get; set; }

        [DataMember]
        public decimal VrGiro { get; set; }

        [DataMember]
        public decimal xInteres { get; set; }

        [DataMember]
        public decimal xSeguro { get; set; }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_LiquidacionHeader()
        {
        }
    }

    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    [DataContract]
    [Serializable]
    public class DTO_LiquidacionDetail
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
        public DTO_LiquidacionDetail()
        {
        }
    }

    [DataContract]
    [Serializable]
    public class DTO_LiquidacionFooter
    {
        public DTO_LiquidacionFooter()
        {

        }
    }

}

