using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ReintegrosCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ReintegrosCartera : DTO_SerializedObject
    {
        #region DTO_ccReintegroClienteDeta

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReintegrosCartera()
        {
            this.InitCols();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccReintegroClienteDeta> reintegroDeta)
        {
            this.DetalleReintegros = reintegroDeta;
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(List<DTO_ccReintegroClienteDeta> reintegros, List<DTO_ccReintegroClienteDeta> reintegroDeta, List<DTO_coTercero> coTerceros)
        {
            this.Reintegros = reintegros;
            this.DetalleReintegros = reintegroDeta;
            this.TercerosReintegro = coTerceros;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Reintegros = new List<DTO_ccReintegroClienteDeta>();
            this.DetalleReintegros = new List<DTO_ccReintegroClienteDeta>();
            this.TercerosReintegro = new List<DTO_coTercero>();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        public List<DTO_ccReintegroClienteDeta> Reintegros { get; set; }

        [DataMember]
        public List<DTO_ccReintegroClienteDeta> DetalleReintegros { get; set; }

        [DataMember]
        public List<DTO_coTercero> TercerosReintegro { get; set; }

        #endregion
    }
}
