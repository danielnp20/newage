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
    /// Models DTO_coImpuestoFiltro
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpuestoFiltro : DTO_MasterComplex
    {
        #region DTO_coImpuestoFiltro
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpuestoFiltro(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.FormatoImpDesc.Value = dr["FormatoImpDesc"].ToString();
                    this.ConceptoImpDesc.Value = dr["ConceptoImpDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                }

                this.FormatoImpID.Value = dr["FormatoImpID"].ToString();
                this.ConceptoImpID.Value = dr["ConceptoImpID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Columna.Value = Convert.ToBoolean(dr["Columna"]);
                this.ExcluyeInd.Value = Convert.ToBoolean(dr["ExcluyeInd"]);
                this.DebitoInd.Value = Convert.ToBoolean(dr["DebitoInd"]);
                this.CreditoInd.Value = Convert.ToBoolean(dr["CreditoInd"]); 
               
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpuestoFiltro() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.FormatoImpID = new UDT_BasicID();
            this.FormatoImpDesc = new UDT_Descriptivo();
            this.ConceptoImpID = new UDT_BasicID();
            this.ConceptoImpDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.ComprobanteID = new UDT_BasicID();
            this.ComprobanteDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.Columna = new UDT_SiNo();
            this.ExcluyeInd = new UDT_SiNo();
            this.DebitoInd = new UDT_SiNo();
            this.CreditoInd = new UDT_SiNo();

          
           
        }

        public DTO_coImpuestoFiltro(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_coImpuestoFiltro(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID FormatoImpID { get; set; }

        [DataMember]
        public UDT_Descriptivo FormatoImpDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID ConceptoImpID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoImpDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_SiNo Columna { get; set; }
        [DataMember]

        public UDT_SiNo ExcluyeInd { get; set; }
        [DataMember]

        public UDT_SiNo DebitoInd { get; set; }
        [DataMember]

        public UDT_SiNo CreditoInd { get; set; }
        
    }
}
