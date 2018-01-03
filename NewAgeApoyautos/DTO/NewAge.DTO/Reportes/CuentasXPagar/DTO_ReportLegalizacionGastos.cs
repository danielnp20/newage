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
    /// <summary>
    /// Clase del reporte LegalizacionGastos
    /// </summary>
    public class DTO_ReportLegalizacionGastos : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLegalizacionGastos()
        {
            this.Header = new DTO_ReportLegaHeader();
            this.Footer = new List<DTO_ReportLegaFooter>();
            this.Detail = new List<DTO_ReportLegaDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportLegaHeader Header { get; set; }

        /// <summary>
        /// Footer del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportLegaFooter> Footer { get; set; }

        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<DTO_ReportLegaDetail> Detail { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    /// <summary>
    /// Clase del Header del Reporte de LegalizacionGastos
    /// </summary>
    public class DTO_ReportLegaHeader
    {
        #region Propiedades
        /// <summary>
        /// Documento Numero (internal ID)
        /// </summary>
        [DataMember]
        public int NumeroDoc { get; set; }
        
        /// <summary>
        /// Prefijo
        /// </summary>
        [DataMember]
        public string Prefijo { get; set; }

        /// <summary>
        /// Documento Numero 
        /// </summary>
        [DataMember]
        public int DocumentoNro { get; set; }

        /// <summary>
        /// Brief explanation of expenses reason
        /// </summary>
        [DataMember]
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Employee's NIT
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Employee's name
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public string MonedaID { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember]
        public string LugarGeograficoID { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [DataMember]
        public string LugarGeograficoDesc { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public bool EstadoInd { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLegaHeader()
        {
        }
    }
    
    [DataContract]
    [Serializable]
    /// <summary>
    /// Clase del Footer del Reporte de LegalizacionGastos
    /// </summary>
    public class DTO_ReportLegaFooter
    {
        #region Propiedades
        /// <summary>
        /// Fecha del gasto
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Reason of expenditures
        /// </summary>
        [DataMember]
        public string Observacion { get; set; }

        /// <summary>
        /// Valor de Alojamiento
        /// </summary>
        [DataMember]
        public decimal ValorAlojamiento { get; set; }
        
        /// <summary>
        /// Valor de Alimentacion
        /// </summary>
        [DataMember]
        public decimal ValorAlimentacion { get; set; }
        
        /// <summary>
        /// Valor de Transporte Aereo
        /// </summary>
        [DataMember]
        public decimal ValorTranspAer { get; set; }
        
        /// <summary>
        /// Valor de Transporte Terrestreo
        /// </summary>
        [DataMember]
        public decimal ValorTranspTer { get; set; }
        
        /// <summary>
        /// Valor de Viaticos
        /// </summary>
        [DataMember]
        public decimal ValorViaticos { get; set; }

        /// <summary>
        /// Valor de Impuestos
        /// </summary>
        [DataMember]
        public decimal ValorImpuestos { get; set; }
        
        /// <summary>
        /// Valor de Otros
        /// </summary>
        [DataMember]
        public decimal ValorOtros { get; set; }

        /// <summary>
        /// Valor de Total
        /// </summary>
        [DataMember]
        public decimal ValorTotal { get; set; }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLegaFooter()
        {
        }
    }
    
    [DataContract]
    [Serializable]
    /// <summary>
    /// Clase del Detalle del Reporte de LegalizacionGastos
    /// </summary>
    public class DTO_ReportLegaDetail
    {
        #region Propiedades
        /// <summary>
        /// CargoEspecial ID
        /// </summary>
        [DataMember]
        public int TipoCargoID { get; set; }

        /// <summary>
        /// CargoEspecial Descriptivo
        /// </summary>
        [DataMember]
        public string TipoCargoDesc { get; set; }

        /// <summary>
        /// Cuenta ID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        ///// <summary>
        ///// Descripcion de la Cuent
        ///// </summary>
        //[DataMember]
        //public string CuentaDesc { get; set; }

        /// <summary>
        /// Proyecto ID
        /// </summary>
        [DataMember]
        public string ProyectoID { get; set; }

        /// <summary>
        /// CentroCosto ID
        /// </summary>
        [DataMember]
        public string CentroCostoID { get; set; }

        /// <summary>
        /// LugarGeo ID
        /// </summary>
        [DataMember]
        public string LugarGeoID { get; set; }

        /// <summary>
        /// Valor de ReteIva
        /// </summary>
        [DataMember]
        public decimal Valor { get; set; }
        #endregion
      
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLegaDetail()
        {
        }
    }

}

