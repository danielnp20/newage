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
    public class DTO_aplIdiomaTraduccion
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_aplIdiomaTraduccion(IDataReader dr)
        {
            this.InitCols();
            this.Dato.Value = Convert.ToString(dr["Dato"]);
            this.IdiomaID.Value = Convert.ToString(dr["IdiomaID"]);
            this.Llave.Value = Convert.ToString(dr["Llave"]);
            this.TipoID.Value = Convert.ToInt16(dr["TipoID"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplIdiomaTraduccion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Dato = new UDTSQL_varchar(2024);
            this.IdiomaID = new UDT_IdiomaID();
            this.Llave = new UDTSQL_varchar(200);
            this.TipoID = new UDTSQL_smallint();
        }

        #endregion
        
        /// <summary>
        /// Gets or sets the Llave
        /// </summary>
        [DataMember]
        public UDTSQL_varchar Llave { get; set; }

        /// <summary>
        /// Gets or sets the IdiomaID
        /// </summary>
        [DataMember]
        public UDT_IdiomaID IdiomaID { get; set; }

        /// <summary>
        /// Gets or sets the TipoID
        /// </summary>
        [DataMember]
        public UDTSQL_smallint TipoID { get; set; }

        /// <summary>
        /// Gets or sets the Dato
        /// </summary>
        [DataMember]
        public UDTSQL_varchar Dato { get; set; }
    }
}
