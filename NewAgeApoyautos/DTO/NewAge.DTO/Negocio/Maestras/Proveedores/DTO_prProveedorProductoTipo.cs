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
    /// Models DTO_prProveedorProductoTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prProveedorProductoTipo : DTO_MasterComplex
    {
        #region DTO_prProveedorProductoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prProveedorProductoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                    this.TipoInvDesc.Value = dr["TipoInvDesc"].ToString();
                }
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.TipoInvID.Value = dr["TipoInvID"].ToString();
                if (!string.IsNullOrEmpty(dr["Garantia"].ToString()))
                    this.Garantia.Value = dr["Garantia"].ToString();
                this.FabricaInd.Value = Convert.ToBoolean(dr["FabricaInd"]);
                this.ImportaInd.Value = Convert.ToBoolean(dr["ImportaInd"]);
                this.DistribuyeInd.Value = Convert.ToBoolean(dr["DistribuyeInd"]);
                this.RepresentaInd.Value = Convert.ToBoolean(dr["RepresentaInd"]);
                if (!string.IsNullOrEmpty(dr["NivelRiesgo"].ToString()))
                    this.NivelRiesgo.Value = Convert.ToByte(dr["NivelRiesgo"]);
                this.Observaciones.Value = dr["Observaciones"].ToString();
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prProveedorProductoTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.TipoInvID = new UDT_BasicID();
            this.TipoInvDesc = new UDT_Descriptivo();
            this.Garantia = new UDTSQL_char(30);
            this.FabricaInd = new UDT_SiNo();
            this.ImportaInd = new UDT_SiNo();
            this.DistribuyeInd = new UDT_SiNo();
            this.RepresentaInd = new UDT_SiNo();
            this.NivelRiesgo = new UDTSQL_tinyint();
            this.Observaciones = new UDT_DescripTExt();
        }

        public DTO_prProveedorProductoTipo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prProveedorProductoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_BasicID TipoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoInvDesc { get; set; }

        [DataMember]
        public UDTSQL_char Garantia { get; set; }

        [DataMember]
        public UDT_SiNo FabricaInd { get; set; }

        [DataMember]
        public UDT_SiNo ImportaInd { get; set; }

        [DataMember]
        public UDT_SiNo DistribuyeInd { get; set; }

        [DataMember]
        public UDT_SiNo RepresentaInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint NivelRiesgo { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }
    }
}
