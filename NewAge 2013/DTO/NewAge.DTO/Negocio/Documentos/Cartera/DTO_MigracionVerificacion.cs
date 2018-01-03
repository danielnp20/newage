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
    /// Clase para migracon 
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionVerificacion
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigracionVerificacion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.Observacion = new UDT_DescripTExt();
        }

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        #endregion
    }
}
