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
    /// Models DTO_prProveedorMarca
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prProveedorMarca : DTO_MasterComplex
    {
        #region DTO_prProveedorMarca
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prProveedorMarca(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                    this.MarcaInvDesc.Value = dr["MarcaInvDesc"].ToString();
                }
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                this.FabricaInd.Value = Convert.ToBoolean(dr["FabricaInd"]);
                this.ImportaInd.Value = Convert.ToBoolean(dr["ImportaInd"]);
                this.DistribuyeInd.Value = Convert.ToBoolean(dr["DistribuyeInd"]);
                this.RepresentaInd.Value = Convert.ToBoolean(dr["RepresentaInd"]);
                if (!string.IsNullOrEmpty(dr["NivelRiesgo"].ToString()))
                    this.NivelRiesgo.Value = Convert.ToByte(dr["NivelRiesgo"]);
                this.Observacion.Value = dr["Observacion"].ToString();
            }
            catch (Exception e)
            {
              throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prProveedorMarca() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.MarcaInvID = new UDT_BasicID();
            this.MarcaInvDesc = new UDT_Descriptivo();
            this.FabricaInd = new UDT_SiNo();
            this.ImportaInd = new UDT_SiNo();
            this.DistribuyeInd = new UDT_SiNo();
            this.RepresentaInd = new UDT_SiNo();
            this.NivelRiesgo = new UDTSQL_tinyint();
            this.Observacion = new UDT_DescripTExt();
        }

        public DTO_prProveedorMarca(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_prProveedorMarca(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        public UDT_BasicID MarcaInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaInvDesc { get; set; }

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
        public UDT_DescripTExt Observacion { get; set; }
    }
}
