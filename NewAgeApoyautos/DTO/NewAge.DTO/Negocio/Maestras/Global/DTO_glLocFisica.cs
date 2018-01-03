using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_LocFisica
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glLocFisica : DTO_MasterHierarchyBasic
    {
        #region DTO_LocFisica
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glLocFisica(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    if (!string.IsNullOrEmpty(dr["LugarGeoDesc"].ToString())) ;
                    this.LugarGeoDesc.Value = dr["LugarGeoDesc"].ToString();
                    this.AreaFisicaDesc.Value = dr["AreaFisicaDesc"].ToString();
                    this.UnidadGenEfectivoDesc.Value = dr["UnidadGenEfectivoDesc"].ToString();
                }
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                this.Direccion.Value = dr["Direccion"].ToString();
                this.Telefono.Value = dr["Telefono"].ToString();
                this.Encargado.Value = dr["Encargado"].ToString();
                this.RecibidosInd.Value = Convert.ToBoolean(dr["RecibidosInd"]);
                if (!string.IsNullOrEmpty(dr["AreaFisica"].ToString())) 
                    this.AreaFisica.Value = dr["AreaFisica"].ToString();
                if (!string.IsNullOrEmpty(dr["UnidGenEfectID"].ToString()))
                    this.UnidGenEfectID.Value = dr["UnidGenEfectID"].ToString();
                this.TipoLocalidad.Value = Convert.ToByte(dr["TipoLocalidad"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glLocFisica() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeoDesc = new UDT_Descriptivo();
            this.AreaFisica = new UDT_BasicID();
            this.AreaFisicaDesc = new UDT_Descriptivo();
            this.UnidGenEfectID = new UDT_BasicID();
            this.UnidadGenEfectivoDesc = new UDT_Descriptivo();
            this.TipoLocalidad = new UDTSQL_tinyint();
            this.Direccion = new UDT_DescripTExt();
            this.Telefono = new UDT_DescripTBase();
            this.Encargado = new UDT_DescripTBase();
            this.RecibidosInd = new UDT_SiNo();
        }

        public DTO_glLocFisica(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glLocFisica(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeoDesc { get; set; }

        [DataMember]
        public UDT_BasicID AreaFisica { get; set; }

        [DataMember]
        public UDT_Descriptivo AreaFisicaDesc { get; set; }

        [DataMember]
        public UDT_BasicID UnidGenEfectID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadGenEfectivoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoLocalidad { get; set; }

        [DataMember]
        public UDT_DescripTExt Direccion { get; set; }

        [DataMember]
        public UDT_DescripTBase Telefono { get; set; }

        [DataMember]
        public UDT_DescripTBase Encargado { get; set; }

        [DataMember]
        public UDT_SiNo RecibidosInd { get; set; }

    }

}
