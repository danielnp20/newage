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
    public class DTO_ReportNominaDetalleCxE : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNominaDetalleCxE()
        {
            this.Header = new DTO_ReportNominaDetalleCxEHeader();
            this.Detail = new List<ReportNominaDetalleCxEDetail>();
        }

        #region Propiedades
        /// <summary>
        /// Header del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public DTO_ReportNominaDetalleCxEHeader Header { get; set; }
        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<ReportNominaDetalleCxEDetail> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Header del Reporte de LegalizacionGastos
    //</summary>
    public class DTO_ReportNominaDetalleCxEHeader
    {
        #region Propiedades

        /// <summary>
        /// Employee's NIT
        /// </summary>
        [DataMember]
        public DateTime FechaInicial { get; set; }

        /// <summary>
        /// CentroCosto ID
        /// </summary>
        [DataMember]
        public DateTime FechaFinal { get; set; }

        /// <summary>
        /// Fecha del documento
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string Localidad { get; set; }

        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string Operacion { get; set; }
        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string Cargo { get; set; }
        /// <summary>
        /// Origen
        /// </summary>
        [DataMember]
        public string Brigada { get; set; }

        [DataMember]
        public string CentroCosto { get; set; }
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportNominaDetalleCxEHeader()
        {
        }
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de LegalizacionGastos
    //</summary>
    public class ReportNominaDetalleCxEDetail
    {
        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Cuenta { get; set; }

        [DataMember]
        public decimal Valor { get; set; }


        //<summary>
        //Constructor por defecto
        //</summary>
        public ReportNominaDetalleCxEDetail()
        {
        }
    }

}

