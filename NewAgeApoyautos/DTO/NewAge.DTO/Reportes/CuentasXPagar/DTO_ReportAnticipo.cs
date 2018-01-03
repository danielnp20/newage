using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Drawing;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]
    
    /// <summary>
    /// Clase del documetno Anticipo
    /// </summary>
    public class DTO_ReportAnticipo : DTO_BasicReport
    {
        #region Constructores
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportAnticipo(IDataReader dr) 
        {
            MonedaID = dr["MonedaID"].ToString();
            AnticipoTipoID = dr["AnticipoTipoID"].ToString();
            AnticipoTipoDesc = dr["AnticipoTipoDesc"].ToString();
            Documento = dr["DocumentoTercero"].ToString();
            Observacion = dr["Observacion"].ToString();
            Fecha = Convert.ToDateTime(dr["Fecha"]);
            Valor = Convert.ToDecimal(dr["Valor"]);
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();
            TasaCambio = (!string.IsNullOrEmpty(dr["TasaCambio"].ToString()))? Convert.ToDecimal(dr["TasaCambio"]) : 0;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportAnticipo() { }
        #endregion

        #region Propiedades

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public decimal TasaCambio { get; set; }

        /// <summary>
        /// Tasa Cambio
        /// </summary>
        [DataMember]
        public string MonedaID { get; set; }

        /// <summary>
        /// Tipo del anticipo
        /// </summary>
        [DataMember]
        public string AnticipoTipoID { get; set; }

        /// <summary>
        /// Descripcion del tipo del anticipo
        /// </summary>
        [DataMember]
        public string AnticipoTipoDesc { get; set; }

        /// <summary>
        /// Numero del documento 
        /// </summary>
        [DataMember]
        public string Documento { get; set; }

        /// <summary>
        /// Observacion del documento
        /// </summary>
        [DataMember]
        public string Observacion { get; set; }

        /// <summary>
        /// Fecha del documetno 
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// ID del Beneficiario del anticipo
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Beneficiario del anticipo
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// Valor del anticipo
        /// </summary>
        [DataMember]
        public decimal Valor { get; set; }
        /// <summary>
        /// Valor del anticipo
        /// </summary>
        [DataMember]
        public decimal ValorML { get; set; }

        /// <summary>
        /// Valor del anticipo
        /// </summary>
        [DataMember]
        public decimal ValorME { get; set; }

        /// <summary>
        /// Indicador del Estado del documento (sin aprobar - true)
        /// </summary>
        [DataMember]
        public bool EstadoInd { get; set; }

        //[DataMember]
        //public string Observaciones { get; set; }

        /// <summary>
        /// La persona responsable para quien firma el anticipo 
        /// </summary>
        [DataMember]
        public string Jefe { get; set; }

        [DataMember]
        public string Gerente { get; set; }

        #endregion
    }
}
