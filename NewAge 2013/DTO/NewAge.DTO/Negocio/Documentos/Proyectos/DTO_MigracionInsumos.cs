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
    /// Models DTO_MigracionInsumos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigracionInsumos
    {
        #region DTO_MigracionInsumos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_MigracionInsumos()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.LugarGeograficoID = new UDT_Consecutivo();
            this.Codigo = new UDT_CodigoGrl20();
            this.Nombre = new UDT_DescripTBase();
            this.Observacion = new UDT_DescripTExt();
            this.Grupo = new UDT_CodigoGrl10();
            this.TipoInv = new UDT_CodigoGrl5();
            this.Proveedor = new UDT_DescripTBase();
            this.Proveedor2 = new UDT_DescripTBase();
            this.Medida = new UDT_DescripTBase();
            this.Precio = new UDT_Valor();
            this.Tipo_Grupo = new UDTSQL_tinyint();
            this.MonedaID = new UDT_DescripTBase();
            this.Precio2 = new UDT_Valor();
            this.MarcaInvID = new UDT_CodigoGrl20();
            this.RefProveedor = new UDT_CodigoGrl20();
        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_Consecutivo LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 Codigo { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 Grupo { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 TipoInv { get; set; }

        [DataMember]
        public UDT_DescripTBase Proveedor { get; set; }

        [DataMember]
        public UDT_DescripTBase Proveedor2 { get; set; }

        [DataMember]
        public UDT_DescripTBase Medida { get; set; }

        [DataMember]
        public UDT_Valor Precio { get; set; }

        [DataMember]
        public UDTSQL_tinyint Tipo_Grupo { get; set; }

        [DataMember]
        public UDT_DescripTBase MonedaID { get; set; }

        [DataMember]
        public UDT_Valor Precio2 { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        #endregion
   }
}
