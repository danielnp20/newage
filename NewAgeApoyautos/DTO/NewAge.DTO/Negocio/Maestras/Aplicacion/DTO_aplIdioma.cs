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
    public class DTO_aplIdioma
    {
        #region Contructora

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_aplIdioma(IDataReader dr)
        {
            this.InitCols();
            this.IdiomaID.Value = dr["IdiomaID"].ToString();
            this.Descriptivo.Value = dr["Descriptivo"].ToString();
            this.Version.Value = Convert.ToInt16(dr["Version"]);
            this.VersionAyuda.Value = dr["VersionAyuda"].ToString();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplIdioma()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.IdiomaID = new UDT_IdiomaID();
            this.Descriptivo = new UDT_DescripTBase();
            this.Version = new UDTSQL_smallint();
            this.VersionAyuda = new UDTSQL_varchar(20);
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the IdiomaID
        /// </summary>
        [DataMember]
        public UDT_IdiomaID IdiomaID { get; set; }

        /// <summary>
        /// Gets or sets the Descriptivo
        /// </summary>
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        /// <summary>
        /// Gets or sets the Version
        /// </summary>
        [DataMember]
        public UDTSQL_smallint Version { get; set; }

        /// <summary>
        /// Gets or sets the VersionAyuda
        /// </summary>
        [DataMember]
        public UDTSQL_varchar VersionAyuda { get; set; }

        #endregion
    }
}
