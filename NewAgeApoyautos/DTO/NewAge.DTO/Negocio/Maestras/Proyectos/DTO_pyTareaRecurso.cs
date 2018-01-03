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
    /// Models DTO_pyTareaRecurso
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTareaRecurso : DTO_MasterComplex
    {
        #region pyTrabajoRecurso
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareaRecurso(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.TareaDesc.Value = Convert.ToString(dr["TareaDesc"]);
                    this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                    //this.CodigoBSDesc.Value = Convert.ToString(dr["CodigoBSDesc"]);
                    //this.CargoEmpDesc.Value = Convert.ToString(dr["CargoEmpDesc"]);
                }
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                if (!string.IsNullOrEmpty(dr["RecursoID"].ToString()))
                    this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                //this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                //if (!string.IsNullOrEmpty(dr["CargoEmpID"].ToString()))
                //    this.CargoEmpID.Value = Convert.ToString(dr["CargoEmpID"]);
                this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareaRecurso(IDataReader dr)
        {
            InitCols();
            try
            {
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                this.TareaID.Value = Convert.ToString(dr["TrabajoID"]);
                this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
                this.CostoBase.Value = Convert.ToDecimal(dr["CostoBase"]);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyTareaRecurso() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TareaID = new UDT_BasicID();
            this.TareaDesc = new UDT_Descriptivo();
            this.RecursoID = new UDT_BasicID();
            this.RecursoDesc = new UDT_Descriptivo();
            //this.CodigoBSID = new UDT_BasicID();
            //this.CodigoBSDesc = new UDT_Descriptivo();
            //this.CargoEmpID = new UDT_BasicID();
            //this.CargoEmpDesc = new UDT_Descriptivo();
            this.FactorID = new UDT_FactorID();
            this.CostoBase = new UDT_Valor();
        }

        public DTO_pyTareaRecurso(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTareaRecurso(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID TareaID { get; set; }
        
        [DataMember]
        public UDT_Descriptivo TareaDesc { get; set; }

        [DataMember]
        public UDT_BasicID RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }
        
        //[DataMember]
        //public UDT_BasicID CodigoBSID { get; set; }
        
        //[DataMember]
        //public UDT_Descriptivo CodigoBSDesc { get; set; }
        
        //[DataMember]
        //public UDT_BasicID CargoEmpID { get; set; }
        
        //[DataMember]
        //public UDT_Descriptivo CargoEmpDesc { get; set; }
        
        [DataMember]
        public UDT_FactorID FactorID { get; set; }

        [DataMember]
        public UDT_Valor CostoBase { get; set; }


    }

}
