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
    /// Models DTO_coValIVA
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coValIVA : DTO_MasterComplex 
    {
        #region DTO_coValIVA
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coValIVA(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.RegFisEmpresaDesc.Value = dr["RegFisEmpresaDesc"].ToString();
                    this.RegFisTerceroDesc.Value = dr["RegFisTerceroDesc"].ToString();
                    this.OperacionDesc.Value = dr["OperacionDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.CuentaCostoIVADesc.Value = dr["CuentaCostoIVADesc"].ToString();
                }

                this.RegimenFiscalEmpresaID.Value = dr["RegimenFiscalEmpresaID"].ToString();
                this.RegimenFiscalTerceroID.Value = dr["RegimenFiscalTerceroID"].ToString();
                this.OperacionID.Value = dr["OperacionID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.CuentaCostoIVA.Value = dr["CuentaCostoIVA"].ToString(); 
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coValIVA() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.RegimenFiscalEmpresaID = new UDT_BasicID();
            this.RegFisEmpresaDesc = new UDT_Descriptivo();
            this.RegimenFiscalTerceroID = new UDT_BasicID();
            this.RegFisTerceroDesc = new UDT_Descriptivo();
            this.OperacionID = new UDT_BasicID();
            this.OperacionDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.CuentaCostoIVA = new UDT_BasicID();
            this.CuentaCostoIVADesc = new UDT_Descriptivo();
        }

        public DTO_coValIVA(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coValIVA(DTO_aplMaestraPropiedades masterProperties)
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
        public UDT_BasicID OperacionID { get; set; }

        [DataMember]
        public UDT_Descriptivo OperacionDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaCostoIVA { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaCostoIVADesc { get; set; }



    }

}
