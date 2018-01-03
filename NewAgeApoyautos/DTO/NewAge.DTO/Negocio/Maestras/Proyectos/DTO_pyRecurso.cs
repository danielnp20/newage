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
    /// Models DTO_pyRecurso
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyRecurso : DTO_MasterBasic
    {
        #region pyRecurso
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyRecurso(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.UnidadInvDesc.Value = Convert.ToString(dr["UnidadInvDesc"]);
                    this.inReferenciaDesc.Value = Convert.ToString(dr["inReferenciaDesc"]);
                    this.CodigoBSDesc.Value = Convert.ToString(dr["CodigoBSDesc"]);
                    this.CargoEmpDesc.Value = Convert.ToString(dr["CargoEmpDesc"]);
                }
                this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                if (!string.IsNullOrEmpty(dr["UnidadInvID"].ToString()))
                    this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);

                if (!string.IsNullOrEmpty(dr["TiempoRealInd"].ToString()))
                    this.TiempoRealInd.Value = Convert.ToBoolean(dr["TiempoRealInd"]);

                if (!string.IsNullOrEmpty(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);

                this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                if (!string.IsNullOrEmpty(dr["CargoEmpID"].ToString()))
                    this.CargoEmpID.Value = Convert.ToString(dr["CargoEmpID"]);

                if (!string.IsNullOrEmpty(dr["CostoBaseLocal"].ToString()))
                    this.CostoBaseLocal.Value = Convert.ToDecimal(dr["CostoBaseLocal"]);

                if (!string.IsNullOrEmpty(dr["CostoBaseExtra"].ToString()))
                    this.CostoBaseExtra.Value = Convert.ToDecimal(dr["CostoBaseExtra"]);

                if (!string.IsNullOrEmpty(dr["FactorID"].ToString()))
                    this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);

                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);

                if (!string.IsNullOrEmpty(dr["TipoCalculo"].ToString()))
                    this.TipoCalculo.Value = Convert.ToByte(dr["TipoCalculo"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyRecurso() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoRecurso = new UDTSQL_tinyint();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_BasicID();
            this.TiempoRealInd = new UDT_SiNo();
            this.inReferenciaDesc = new UDT_Descriptivo();           
            this.CodigoBSID = new UDT_BasicID();
            this.CodigoBSDesc = new UDT_Descriptivo();
            this.CargoEmpID = new UDT_BasicID();
            this.CargoEmpDesc = new UDT_Descriptivo();
            this.CostoBaseLocal = new UDT_Valor();
            this.CostoBaseExtra = new UDT_Valor();
            this.FactorID = new UDT_FactorID();
            this.Observacion = new UDT_DescripTExt();
            this.TipoCalculo = new UDTSQL_tinyint();
        }

        public DTO_pyRecurso(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyRecurso(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDTSQL_tinyint TipoRecurso { get; set; }
        
        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDT_SiNo TiempoRealInd { get; set; }

        [DataMember]
        public UDT_BasicID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo inReferenciaDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CodigoBSID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CodigoBSDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CargoEmpID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CargoEmpDesc { get; set; }

        [DataMember]
        public UDT_Valor CostoBaseLocal { get; set; }

        [DataMember]
        public UDT_Valor CostoBaseExtra { get; set; }

        [DataMember]
        public UDT_FactorID FactorID { get; set; }
        
        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
        
        [DataMember]
        public UDTSQL_tinyint TipoCalculo { get; set; }
    }

}
