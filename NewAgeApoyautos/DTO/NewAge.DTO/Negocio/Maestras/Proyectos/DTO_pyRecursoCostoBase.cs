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
    /// Models DTO_pyRecursoCostoBase
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyRecursoCostoBase : DTO_MasterComplex
    {
        #region pyRecursoCostoBase
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyRecursoCostoBase(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                //this.CodigoBSDesc.Value = Convert.ToString(dr["CodigoBSDesc"]);
                this.LugarGeograficoDesc.Value = Convert.ToString(dr["LugarGeograficoDesc"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);                }
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.LugarGeograficoID.Value = Convert.ToString(dr["LugarGeograficoID"]);
                //this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                this.ProveedorEMP.Value = Convert.ToString(dr["ProveedorEMP"]);
                this.ProveedorEXT.Value = Convert.ToString(dr["ProveedorEXT"]);
                this.CostoLocalEMP.Value = Convert.ToDecimal(dr["CostoLocalEMP"]);
                this.CostoExtraEMP.Value = Convert.ToDecimal(dr["CostoExtraEMP"]);
                this.CostoLocalEXT.Value = Convert.ToDecimal(dr["CostoLocalEXT"]);
                this.CostoExtraEXT.Value = Convert.ToDecimal(dr["CostoExtraEXT"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyRecursoCostoBase() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeograficoDesc = new UDT_Descriptivo();
            this.RecursoID = new UDT_BasicID();
            this.RecursoDesc = new UDT_Descriptivo();
            //this.CodigoBSID = new UDT_BasicID();
            //this.CodigoBSDesc = new UDT_Descriptivo();
            this.ProveedorEMP = new UDT_BasicID();
            this.ProveedorEMPDesc = new UDT_Descriptivo();
            this.ProveedorEXT = new UDT_BasicID();
            this.ProveedorEXTDesc = new UDT_Descriptivo();
            this.CostoLocalEMP = new UDT_Valor();
            this.CostoExtraEMP = new UDT_Valor();
            this.CostoLocalEXT = new UDT_Valor();
            this.CostoExtraEXT = new UDT_Valor();
        }

        public DTO_pyRecursoCostoBase(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyRecursoCostoBase(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeograficoDesc { get; set; }

        [DataMember]
        public UDT_BasicID RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        //[DataMember]
        //public UDT_BasicID CodigoBSID { get; set; }
        
        //[DataMember]
        //public UDT_Descriptivo CodigoBSDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProveedorEMP { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorEMPDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProveedorEXT { get; set; }

        [DataMember]
        public UDT_Descriptivo ProveedorEXTDesc { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocalEMP { get; set; }
        
        [DataMember]
        public UDT_Valor CostoExtraEMP { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalEXT { get; set; }

        [DataMember]
        public UDT_Valor CostoExtraEXT { get; set; }

    }

}
