using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase de los Formularios (Formato declaracion)
    /// </summary>
    public class DTO_ReportNominaDetalleRC : DTO_BasicReport
    {
        #region Propiedades
        [DataMember]
        public DTO_ReportNominaDetalleRCHeader FormDecHeader { get; set; }

        [DataMember]
        public List<DTO_ReportNominaDetalleRCDetail> FormDecDetail { get; set; }
        #endregion
    }

    /// <summary>
    /// Clase del cabezote del Formulario (Formato declaracion)
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ReportNominaDetalleRCHeader
    {
        #region Propriedades
       

        /// <summary>
        /// Codigo Direccion seccional
        /// </summary>
        [DataMember]
        public DateTime FechaInicial { get; set; }

        [DataMember]
        public DateTime FechaFinal { get; set; } 
        #endregion
    }

    /// <summary>
    /// Clase del detalle del Formulario (Formato declaracion)
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ReportNominaDetalleRCDetail
    {
        #region Propriedades
        /// <summary>
        /// Tipo de la declaracion (IVA,ICA,Retefuent etc.)
        /// </summary>
        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Concepto { get; set; }

        [DataMember]
        public string Base { get; set; }

        [DataMember]
        public decimal Valor { get; set; }


        public DTO_ReportNominaDetalleRCDetail()
        {
            
        }
        #endregion
    }
}
