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
    /// Models DTO_EstadoCuenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_EstadoCuenta
    {
        public DTO_EstadoCuenta()
        {
            this.EstadoCuentaHistoria = new DTO_ccEstadoCuentaHistoria();
            this.EstadoCuentaComponentes = new List<DTO_ccEstadoCuentaComponentes>();
            this.EstadoCuentaCuotas = new List<DTO_ccEstadoCuentaCuotas>();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(DTO_ccEstadoCuentaHistoria ech, List<DTO_ccEstadoCuentaComponentes> ecc)
        {
            this.EstadoCuentaHistoria = ech;
            this.EstadoCuentaComponentes = ecc;
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(DTO_ccEstadoCuentaHistoria ech, List<DTO_ccEstadoCuentaComponentes> ecc, List<DTO_ccEstadoCuentaCuotas> ecctas)
        {
            this.EstadoCuentaHistoria = ech;
            this.EstadoCuentaComponentes = ecc;
            this.EstadoCuentaCuotas = ecctas;
        }

        #region Propiedades

        [DataMember]
        public DTO_ccEstadoCuentaHistoria EstadoCuentaHistoria
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccEstadoCuentaComponentes> EstadoCuentaComponentes
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccEstadoCuentaCuotas> EstadoCuentaCuotas
        {
            get;
            set;
        }

        #endregion
    }
}
