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
    /// Models DTO_MigracionProveedor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionProveedor
    {
        #region DTO_MigracionProveedor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_MigracionProveedor()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.Codigo = new UDT_Consecutivo();
            this.Nombre = new UDT_DescripTBase();
            this.Direccion = new UDT_DescripTBase();
            this.Telefono = new UDT_DescripTBase();
            this.Ciudad = new UDT_DescripTBase();
            this.Email = new UDT_DescripTBase();
            this.Web = new UDT_DescripTBase();
            this.Identificacion = new UDT_DescripTBase();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Codigo { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        public UDT_DescripTBase Telefono { get; set; }

        [DataMember]
        public UDT_DescripTBase Ciudad { get; set; }

        [DataMember]
        public UDT_DescripTBase Email { get; set; }

        [DataMember]
        public UDT_DescripTBase Web { get; set; }

        [DataMember]
        public UDT_DescripTBase Identificacion { get; set; }

        #endregion
   }
}
