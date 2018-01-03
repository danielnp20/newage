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
    public class DTO_Formularios : DTO_BasicReport
    {
        #region Propiedades
        [DataMember]
        public DTO_FormDecHeader FormDecHeader { get; set; }

        [DataMember]
        public List<DTO_FormDecDetail> FormDecDetail { get; set; }
        #endregion
    }

    /// <summary>
    /// Clase del cabezote del Formulario (Formato declaracion)
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_FormDecHeader
    {
        #region Propriedades
        /// <summary>
        /// NIT
        /// </summary>
        [DataMember]
        public string Nit { get; set; }

        /// <summary>
        /// Tipo det documento de identificacion
        /// </summary>
        [DataMember]
        public string NitTipoDoc { get; set; }

        /// <summary>
        /// Digito verificacion
        /// </summary>
        [DataMember]
        public string DV { get; set; }

        /// <summary>
        /// Apellido primero
        /// </summary>
        [DataMember]
        public string ApellidoPri { get; set; }

        /// <summary>
        /// Apellido segundo
        /// </summary>
        [DataMember]
        public string ApellidoSdo { get; set; }

        /// <summary>
        /// Nombre primero
        /// </summary>
        [DataMember]
        public string NombrePri { get; set; }

        /// <summary>
        /// Otros nombres 
        /// </summary>
        [DataMember]
        public string NombreOtr { get; set; }

        /// <summary>
        /// Razon Social 
        /// </summary>
        [DataMember]
        public string RazonSoc { get; set; }

        /// <summary>
        /// Codigo Direccion seccional
        /// </summary>
        [DataMember]
        public string CodigoDir { get; set; } 
        #endregion
    }

    /// <summary>
    /// Clase del detalle del Formulario (Formato declaracion)
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_FormDecDetail
    {
        #region Propriedades
        /// <summary>
        /// Tipo de la declaracion (IVA,ICA,Retefuent etc.)
        /// </summary>
        [DataMember]
        public string Declaracion { get; set; }

        /// <summary>
        /// Renglon del formulario
        /// </summary>
        [DataMember]
        public string Renglon { get; set; }

        /// <summary>
        /// Valor por Renglon (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal ValorML { get; set; }
        #endregion
    }
}
