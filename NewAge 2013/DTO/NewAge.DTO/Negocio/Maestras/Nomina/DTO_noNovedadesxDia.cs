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
    /// Models DTO_noNovedadesxDia
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noNovedadesxDia : DTO_MasterComplex 
    {
        #region DTO_noNovedadesxDia
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noNovedadesxDia(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.RolNODesc.Value = dr["RolNODesc"].ToString(); 
                    this.ConvencionNODesc.Value = dr["ConvencionNODesc"].ToString(); 
                    this.ConceptoNODesc.Value = dr["ConceptoNODesc"].ToString(); 
                }
                this.RolNOID.Value = dr["RolNOID"].ToString();
                this.ConvencionNOID.Value = dr["ConvencionNOID"].ToString();
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
        public DTO_noNovedadesxDia() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.RolNOID = new UDT_BasicID();
            this.RolNODesc = new UDT_Descriptivo();
            this.ConvencionNOID = new UDT_BasicID();
            this.ConvencionNODesc = new UDT_Descriptivo();
            this.ConceptoNOID = new UDT_BasicID();
            this.ConceptoNODesc = new UDT_Descriptivo();
            this.Valor = new UDT_Valor();           
        }

        public DTO_noNovedadesxDia(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noNovedadesxDia(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID RolNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo RolNODesc { get; set; }
    
        [DataMember]
        public UDT_BasicID ConvencionNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConvencionNODesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoNODesc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }
    }

}

