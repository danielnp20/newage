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
    /// Models DTO_noAportesPorcentaje
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noAportesPorcentaje : DTO_MasterComplex 
    {
        #region DTO_noAportesPorcentaje
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noAportesPorcentaje(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ConvencionNODesc.Value = dr["ConvencionNODesc"].ToString();                    
                    this.ConceptoNODesc.Value = dr["ConceptoNODesc"].ToString();
                }
                this.ConvencionNOID.Value = dr["ConvencionNOID"].ToString();
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.EmpresaPorcentaje.Value = Convert.ToDecimal(dr["EmpresaPorcentaje"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noAportesPorcentaje() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConvencionNOID = new UDT_BasicID();
            this.ConvencionNODesc = new UDT_Descriptivo();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoNODesc = new UDT_Descriptivo();
            this.EmpresaPorcentaje = new UDT_PorcentajeID();           
        }

        public DTO_noAportesPorcentaje(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noAportesPorcentaje(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID ConvencionNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConvencionNODesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID EmpresaPorcentaje { get; set; }
    }

}
