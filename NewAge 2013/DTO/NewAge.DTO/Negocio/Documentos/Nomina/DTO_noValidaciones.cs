using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noValidaciones
    {
        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public DTO_noValidaciones()
        {
            this.InitColums();
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las propiedades
        /// </summary>
        private void InitColums()
        {
            this.EmpleadoID = new UDT_EmpleadoID();
            this.EmpleadoDesc = new UDT_Descriptivo();
            this.Estado = new UDT_DescripTExt();
            this.Descripcion = new UDT_DescripTExt();
            this.Accion = new UDT_SiNo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_Descriptivo EmpleadoDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Estado { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_SiNo Accion { get; set; }
        
  
        #endregion
    }
}
