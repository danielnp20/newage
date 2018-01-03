using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_aplModulo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_aplModulo
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_aplModulo(IDataReader dr)
        {
            this.InitCols();
            this.ActivoInd.Value = Convert.ToBoolean(dr["ActivoInd"]);
            this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
            this.ContabilidadInd.Value = Convert.ToBoolean(dr["ContabilidadInd"]);
            this.ModuloID.Value = Convert.ToString(dr["ModuloID"]);
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplModulo()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ActivoInd = new UDT_SiNo();
            this.Descriptivo = new UDT_DescripTBase();
            this.ContabilidadInd = new UDT_SiNo();
            this.ModuloID = new UDTSQL_char(3);
        }

        #endregion

        /// <summary>
        /// Gets or sets the ModuloID
        /// </summary>
        [DataMember]
        public UDTSQL_char ModuloID { get; set; }

        /// <summary>
        /// Gets or sets the Descriptivo
        /// </summary>
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        /// <summary>
        /// Gets or sets the ContabilidadInd
        /// </summary>
        [DataMember]
        public UDT_SiNo ContabilidadInd { get; set; }

        /// <summary>
        /// Gets or sets the ActivoInd
        /// </summary>
        [DataMember]
        public UDT_SiNo ActivoInd { get; set; }
    }
}
