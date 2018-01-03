using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_MigracionMvtos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionMvtos
    {
        #region DTO_MigracionMvtos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_MigracionMvtos()
        {
            InitCols();
        }

        public void InitCols()
        {        
            this.inReferenciaID = new UDT_inReferenciaID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.SerialID = new UDT_SerialID();
            this.DocSoporte = new UDT_Consecutivo();
            this.CantidadEMP = new UDT_Cantidad();
            this.EmpaqueInvID = new UDT_EmpaqueInvID(); 
            this.ValorUNI = new UDT_Valor();
        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }
       
        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocSoporte { get; set; }      
      
        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadEMP { get; set; }        

        [DataMember]
        public UDT_Valor ValorUNI { get; set; }

        #endregion
   }
}
