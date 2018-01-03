using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_ComprobanteConsolidacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ComprobanteConsolidacion
    {
        #region Contructor

        public DTO_ComprobanteConsolidacion()
        {
            this.InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Consolidar = false;
            this.EmpresaID = new UDT_EmpresaID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.Procesado = false;
        }
        #endregion

        #region Propiedades

        [DataMember]
        public bool Consolidar { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public bool Procesado { get; set; }

        #endregion
    }
}
