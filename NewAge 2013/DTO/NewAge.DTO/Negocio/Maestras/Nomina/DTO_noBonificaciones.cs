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
    /// Models DTO_noBonificaciones
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noBonificaciones : DTO_MasterComplex 
    {
        #region DTO_noBonificaciones
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noBonificaciones(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.RolNODesc.Value = dr["RolNODesc"].ToString(); 
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.ConceptoNOPlanillaDesc.Value = dr["ConceptoNOPlanillaDesc"].ToString(); 
                    this.ConceptoNODesc.Value = dr["ConceptoNODesc"].ToString(); 
                }
                this.RolNOID.Value = dr["RolNOID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.ConceptoNOPlanillaID.Value = dr["ConceptoNOPlanillaID"].ToString();
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noBonificaciones() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.RolNOID = new UDT_BasicID();
            this.RolNODesc = new UDT_Descriptivo();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.ConceptoNOPlanillaID = new UDT_BasicID();
            this.ConceptoNOPlanillaDesc = new UDT_Descriptivo();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoNODesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();           
        }

        public DTO_noBonificaciones(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noBonificaciones(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID RolNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo RolNODesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOPlanillaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNOPlanillaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }
    }

}
