using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_inReferenciaCod
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inReferenciaCod : DTO_MasterBasic
    {
        #region DTO_inReferenciaCod

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inReferenciaCod(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.inReferenciaDesc.Value = dr["inReferenciaDesc"].ToString();
                    this.Parametro1Desc.Value = dr["Parametro1Desc"].ToString();
                    this.Parametro2Desc.Value = dr["Parametro2Desc"].ToString();
                    this.ProveedorDesc.Value = dr["ProveedorDesc"].ToString();
                }
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Parametro1ID.Value = dr["Parametro1ID"].ToString();
                this.Parametro2ID.Value = dr["Parametro2ID"].ToString();
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inReferenciaCod()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.inReferenciaID = new UDT_BasicID();
            this.inReferenciaDesc = new UDT_Descriptivo();
            this.Parametro1ID = new UDT_BasicID();
            this.Parametro1Desc = new UDT_Descriptivo();
            this.Parametro2Desc = new UDT_Descriptivo();
            this.Parametro2ID = new UDT_BasicID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.ProveedorID = new UDT_BasicID();
            this.ProveedorDesc = new UDT_Descriptivo();
        }

        public DTO_inReferenciaCod(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inReferenciaCod(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }

        [DataMember]
        public UDT_BasicID Parametro1ID { get; set; }

        [DataMember]
        public UDT_Descriptivo Parametro1Desc { get; set; }

        [DataMember]
        public UDT_BasicID Parametro2ID { get; set; }

        [DataMember]
        public UDT_Descriptivo Parametro2Desc { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_BasicID ProveedorID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorDesc { get; set; }
    }
}
