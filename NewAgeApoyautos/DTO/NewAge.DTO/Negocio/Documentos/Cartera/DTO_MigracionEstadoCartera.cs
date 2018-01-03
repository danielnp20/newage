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
    /// Clase para migracon de comprobantes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionEstadoCartera
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigracionEstadoCartera()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_LibranzaID();
            this.CobranzaEstadoID = new UDT_CodigoGrl10();
            this.CobranzaGestionID = new UDT_CodigoGrl10();
            this.NovedadIncorporaID = new UDT_CodigoGrl5();
            this.SiniestroEstadoID = new UDT_CodigoGrl5();
        }

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }
      
        [DataMember]
        public UDT_CodigoGrl10 CobranzaEstadoID   { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 CobranzaGestionID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 NovedadIncorporaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 SiniestroEstadoID { get; set; }


        #endregion
    }
}
