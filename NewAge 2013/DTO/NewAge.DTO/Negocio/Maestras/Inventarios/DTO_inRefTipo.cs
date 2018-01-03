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
    /// Models DTO_inRefTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inRefTipo : DTO_MasterBasic
    {
        #region DTO_inRefTipo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inRefTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.RefEmpaqueDesc.Value = dr["RefEmpaqueDesc"].ToString();
                }

                this.TipoInv.Value = Convert.ToByte(dr["TipoInv"]);
                this.ControlEspecial.Value =  Convert.ToByte(dr["ControlEspecial"]);
                this.MarcaInd.Value = Convert.ToBoolean(dr["MarcaInd"]);
                this.SerializadoInd.Value = Convert.ToBoolean(dr["SerializadoInd"]);
                this.RefEmpaque.Value = dr["RefEmpaque"].ToString();
                this.ControlImportacion.Value = Convert.ToByte(dr["ControlImportacion"]);
                this.Parametro1Ind.Value = Convert.ToBoolean(dr["Parametro1Ind"]);
                this.Parametro2Ind.Value = Convert.ToBoolean(dr["Parametro2Ind"]);
                this.DiasCompra.Value = Convert.ToInt32(dr["DiasCompra"]);
                this.GarantiaInd.Value = Convert.ToBoolean(dr["GarantiaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inRefTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoInv = new UDTSQL_tinyint();
            this.ControlEspecial = new UDTSQL_tinyint();
            this.MarcaInd = new UDT_SiNo();
            this.SerializadoInd = new UDT_SiNo();
            this.RefEmpaque = new UDT_BasicID();
            this.RefEmpaqueDesc = new UDT_Descriptivo();
            this.ControlImportacion = new UDTSQL_tinyint();
            this.Parametro1Ind = new UDT_SiNo();
            this.Parametro2Ind = new UDT_SiNo();
            this.DiasCompra = new UDTSQL_int();
            this.GarantiaInd = new UDT_SiNo();
        }

        public DTO_inRefTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inRefTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint TipoInv { get; set; }

        [DataMember]
        public UDTSQL_tinyint ControlEspecial { get; set; }

        [DataMember]
        public UDT_SiNo MarcaInd  { get; set; }

        [DataMember]
        public UDT_SiNo SerializadoInd { get; set; }

        [DataMember]
        public UDT_BasicID RefEmpaque { get; set; }

        [DataMember]
        public UDT_Descriptivo RefEmpaqueDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ControlImportacion { get; set; }

        [DataMember]
        public UDT_SiNo Parametro1Ind { get; set; }

        [DataMember]
        public UDT_SiNo Parametro2Ind { get; set; }

        [DataMember]
        public UDTSQL_int DiasCompra { get; set; }

        [DataMember]
        public UDT_SiNo GarantiaInd { get; set; }
    }

}
