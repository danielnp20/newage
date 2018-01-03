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
    /// Models DTO_acActivoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_DeterioroActivo
    {
        #region Constructor
                
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_DeterioroActivo()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PlaquetaID = new UDT_PlaquetaID();
            this.SerialID = new UDTSQL_char(25);
            this.LocFisicaID = new UDT_LocFisicaID();
            this.CostoLOC = new UDT_Valor();
            this.CostoEXT = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }
       
        [DataMember]
        [AllowNull]
        public UDTSQL_char SerialID { get; set; }     
        
              
        [DataMember]
        [AllowNull]
        public UDT_LocFisicaID LocFisicaID { get; set; }   
     
        
        #region Campos de Extras  Valor

        [DataMember]
        public UDT_Valor CostoLOC { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoEXT { get; set; }

        #endregion

       
        #endregion

    }   
}
