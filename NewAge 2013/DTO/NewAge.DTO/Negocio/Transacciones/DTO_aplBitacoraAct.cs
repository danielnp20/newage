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
    /// Models DTO_aplBitacoraAct
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_aplBitacoraAct
    {
        #region Contructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_aplBitacoraAct(IDataReader dr)
        {
            this.InitCols();
            this.BitacoraID.Value = Convert.ToInt32(dr["BitacoraID"]);
            this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
            this.NombreCampo.Value = dr["NombreCampo"].ToString();
            this.Valor.Value = dr["Valor"].ToString();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplBitacoraAct()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.BitacoraID = new UDT_BitacoraID();
            this.DocumentoID = new UDT_DocumentoID();
            this.NombreCampo = new UDTSQL_varchar(50);
            this.Valor = new UDTSQL_varchar(2024);
        }

        #endregion

        #region propiedades

        /// <summary>
        /// Gets or sets the BitacoraID
        /// </summary>
        [DataMember]
        public UDT_BitacoraID BitacoraID { get; set; }

        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the NombreCampo
        /// </summary>
        [DataMember]
        public UDTSQL_varchar NombreCampo { get; set; }

        /// <summary>
        /// Gets or sets the Valor
        /// </summary>
        [DataMember]
        public UDTSQL_varchar Valor { get; set; }

        #endregion
    }
}
