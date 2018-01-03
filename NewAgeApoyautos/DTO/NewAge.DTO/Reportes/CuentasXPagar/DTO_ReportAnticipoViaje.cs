using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del documento Anticipo del Viaje
    /// </summary>
    public class DTO_ReportAnticipoViaje : DTO_BasicReport
    {
        #region Propiedades
        /// <summary>
        /// Numero del Anticipo
        /// </summary>
        [DataMember]
        public string No { get; set; }

        /// <summary>
        /// Indicador del Estado del documento (sin aprobar - true)
        /// </summary>
        [DataMember]
        public bool EstadoInd { get; set; }

        /// <summary>
        /// Fecha del anticipo
        /// </summary>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// ID de la Empresa
        /// </summary>
        [DataMember]
        public string EmpresaID { get; set; }

        ///// <summary>
        ///// Descripcion de la Empresa
        ///// </summary>
        //[DataMember]
        //public string EmpresaDesc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Area { get; set; }

        /// <summary>
        /// Nombres y Apellidos del Funcionario (Descripcion del Tercero)
        /// </summary>
        [DataMember]
        public string Nombres { get; set; }

        /// <summary>
        /// ID del Tercero
        /// </summary>
        [DataMember]
        public string DocumentoIdent { get; set; }

        /// <summary>
        /// Motivo del Viaje
        /// </summary>
        [DataMember]
        public string MotivoViaje { get; set; }

        /// <summary>
        /// Destino del Viaje
        /// </summary>
        [DataMember]
        public string Destino { get; set; }

        /// <summary>
        /// Dias del Alojamiento
        /// </summary>
        [DataMember]
        public decimal DiasAlojamiento { get; set; }

        /// <summary>
        /// Valor del Alojamiento
        /// </summary>
        [DataMember]
        public decimal ValorAlojamiento { get; set; }

        /// <summary>
        /// Total Valor del Alojamiento
        /// </summary>
        [DataMember]
        public decimal TotalAlojamiento { get; set; }

        /// <summary>
        /// Dias de la Alimentacion
        /// </summary>
        [DataMember]
        public decimal DiasAlimentacion { get; set; }

        /// <summary>
        /// Valor de la Alimentacion
        /// </summary>
        [DataMember]
        public decimal ValorAlimentacion { get; set; }

        /// <summary>
        /// Total Valor de la Alimentacion
        /// </summary>
        [DataMember]
        public decimal TotalAlimentacion { get; set; }

        /// <summary>
        /// Dias del Transporte
        /// </summary>
        [DataMember]
        public decimal DiasTransporte { get; set; }

        /// <summary>
        /// Valor del Transporte
        /// </summary>
        [DataMember]
        public decimal ValorTransporte { get; set; }

        /// <summary>
        /// Total Valor del Transporte
        /// </summary>
        [DataMember]
        public decimal TotalTransporte { get; set; }

        /// <summary>
        /// Dias del Otros Gastos
        /// </summary>
        [DataMember]
        public decimal DiasOtrosGastos { get; set; }

        /// <summary>
        /// Valor del Otros Gastos
        /// </summary>
        [DataMember]
        public decimal ValorOtrosGastos { get; set; }

        /// <summary>
        /// Total Valor del Otros Gastos
        /// </summary>
        [DataMember]
        public decimal TotalOtrosGastos { get; set; }
        
        /// <summary>
        /// Total Anticipo
        /// </summary>
        [DataMember]
        public decimal TotalAnticipo { get; set; }

        [DataMember]
        public string Funcionario { get; set; }

        [DataMember]
        public string Autorizado { get; set; }

        #endregion

    }
}
