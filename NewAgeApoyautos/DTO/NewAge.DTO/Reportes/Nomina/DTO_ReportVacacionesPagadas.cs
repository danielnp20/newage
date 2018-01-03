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
    public class DTO_ReportVacacionesPagadas : DTO_BasicReport
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportVacacionesPagadas()
        {
            this.Detail = new List<ReportVacacionesPagadasDetails>();
        }

        #region Propiedades
        /// <summary>
        /// Detalle del Reporte de LegalizacionGastos
        /// </summary>
        [DataMember]
        public List<ReportVacacionesPagadasDetails> Detail { get; set; }

        [DataMember]
        public bool isApro { get; set; }
        #endregion
    }

    [DataContract]
    [Serializable]
    //<summary>
    //Clase del Detalle del Reporte de Vacaciones Pagadas

    //</summary>
    public class ReportVacacionesPagadasDetails
    {
        [DataMember]
        public int EmpleadoID { get; set; }

        [DataMember]
        public DateTime FechaIni1 { get; set; }

        [DataMember]
        public DateTime FechaFin1 { get; set; }

        [DataMember]
        public DateTime FechaIni2 { get; set; }

        [DataMember]
        public DateTime FechaFin2 { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int Dias1 { get; set; }

        [DataMember]
        public int Dias2 { get; set; }


        //<summary>
        //Constructor por defecto
        //</summary>
        public ReportVacacionesPagadasDetails()
        {
        }
    }

}

