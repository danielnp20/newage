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
    /// Models DTO_coImpuesto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpuesto : DTO_MasterComplex
    {
        #region DTO_coImpuesto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpuesto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.RegFisEmpresaDesc.Value = dr["RegFisEmpresaDesc"].ToString();
                    this.RegFisTerceroDesc.Value = dr["RegFisTerceroDesc"].ToString();
                    this.ImpuestoDesc.Value = dr["ImpuestoDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.LugarGeoDesc.Value = dr["LugarGeoDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                }

                this.RegimenFiscalEmpresaID.Value = dr["RegimenFiscalEmpresaID"].ToString();
                this.RegimenFiscalTerceroID.Value = dr["RegimenFiscalTerceroID"].ToString();
                this.ImpuestoTipoID.Value = dr["ImpuestoTipoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString(); 
               
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpuesto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.RegimenFiscalEmpresaID = new UDT_BasicID();
            this.RegFisEmpresaDesc = new UDT_Descriptivo();
            this.RegimenFiscalTerceroID = new UDT_BasicID();
            this.RegFisTerceroDesc = new UDT_Descriptivo();
            this.ImpuestoTipoID = new UDT_BasicID();
            this.ImpuestoDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeoDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
           
        }

        public DTO_coImpuesto(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_coImpuesto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID RegimenFiscalEmpresaID { get; set; }

        [DataMember]
        public UDT_Descriptivo RegFisEmpresaDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID RegimenFiscalTerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo RegFisTerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID ImpuestoTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

       
        
    }
}
